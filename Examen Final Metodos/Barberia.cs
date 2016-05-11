using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examen_Final_Metodos
{
    public class Barberia
    {
        public Barberos barberos;
        public Cola espera = new Cola();
        public Cola atendidos = new Cola();
        private CLG clg; //Genera tiempos de llegada
        public List<Imagen> imagenes = new List<Imagen>();
        private int no_clientes;
        private Distribucion distribucion { get; set; }
        private double media { get; set; }
        private double desviacion { get; set; }
        private double tiempo_anterior { get; set; }
        private double tiempo_a_analizar { get; set; }
        private int generados = 0;
        private int procesados = 0;
        private List<double> tiempos = new List<double>();
        public List<double> tiempos_cliente = new List<double>();


        public Barberia(int barberos, Distribucion distribucion, double media, double desviacion, int no_clientes, Distribucion distribucion_barbero, double media_barbero, double desviacion_barbero)
        {
            this.barberos = new Barberos(barberos, distribucion_barbero, media_barbero, desviacion_barbero);
            this.distribucion = distribucion;
            this.media = media;
            this.desviacion = desviacion;
            this.no_clientes = no_clientes;
            tiempo_anterior = 0;
            tiempo_a_analizar = 0;
            
            clg = new CLG(1000, new Random().Next(0, 1000));
            generados++;
            Cliente cliente = new Cliente(0);
            if (!this.barberos.Atender(cliente))
            {
                espera.Add(cliente);
            }
            List<int> estados = new List<int>();
            this.barberos.getEstados(0).ForEach(e => estados.Add(e == Estado.ACTIVO ? 1 : 0));
            tiempo_anterior = 0;
            //tiempos_cliente.Add(0);
            for(generados = 1; generados < no_clientes; generados++)
            {
                double tiempo = generarTiempo(tiempo_anterior);
                Cliente c = new Cliente(tiempo);
                espera.Add(c);
                tiempo_anterior = tiempo;
                //tiempos_cliente.Add(tiempo);
            }
            clg.setIndex(0);
            tiempo_anterior = 0;
            tiempos.Add(0);
        }

        private void procesar(double tiempo)
        {
            List<Cliente> activo = barberos.Terminar(tiempo);
            foreach (Cliente c in activo)
            {
                if (c == null)
                {

                }
                else
                {
                    procesarTiempo(c.salida);
                    procesados++;
                }
            }
        }

        public double tiempo_max
        {
            get
            {
                double max = 0;
                foreach(Cliente c in atendidos)
                {
                    if(max < c.salida)
                    {
                        max = c.salida;
                    }
                }
                return max;
                
            }
        }

        public void procesarClientes()
        {
            double tiempo = generarTiempo(tiempo_anterior);
            tiempo_anterior = tiempo;
            if (procesados < no_clientes)
            {
                List<Cliente> activo = barberos.Terminar(tiempo);
                foreach (Cliente c in activo)
                {
                    if (c == null)
                    {

                    }
                    else
                    {
                        tiempos.Add(c.salida);
                        procesados++;
                    }
                }
                if (espera.Count > 0)
                {
                    if (tiempo >= espera[0].llegada)
                    {
                        espera[0].atendido = tiempo;
                        if (barberos.Atender(espera[0]))
                        {
                            espera.RemoveAt(0);
                        }
                    }
                }
                procesarClientes();
            }
            else
            {
                List<Cliente> activo = barberos.Terminar(tiempo);
                foreach (Cliente c in activo)
                {

                    if (c == null)
                    {

                    }
                    else
                    {
                        tiempos.Add(c.salida);
                        procesados++;
                    }
                }
                List<int> estados = new List<int>();
                barberos.getEstados(tiempo).ForEach(e => estados.Add(e == Estado.ACTIVO ? 1 : 0));
                imagenes.Add(new Imagen(tiempo, espera.Size(tiempo), estados));
                tiempos.Add(tiempo);
                foreach (Barbero barbero in barberos)
                {
                    atendidos.AddRange(barbero.cliente);
                }
                foreach(double t in tiempos)
                {
                    procesarTiempo(t);
                }
            }
            

        }

        private void procesarTiempo(double tiempo)
        {
            List<int> estados = new List<int>();
            int i = atendidos.SizeInTime(tiempo);
            barberos.getEstados(tiempo).ForEach(e => estados.Add(e == Estado.ACTIVO ? 1 : 0));
            estados.ForEach(estado =>
            {
                if (estado == 1)
                {
                    i--;
                }
            });
           
            imagenes.Add(new Imagen(tiempo, i, estados));
            //if(espera.Count > 0)
            //{
            //    if (espera[0].llegada <= tiempo)
            //    {
            //        imagenes.Add(new Imagen(tiempo, espera.Size(tiempo), 1));
            //    }
            //    else
            //    {
            //        imagenes.Add(new Imagen(tiempo, espera.Size(tiempo), 0));
            //    }
            //}else
            //{
            //    imagenes.Add(new Imagen(tiempo, espera.Size(tiempo), 0));
            //}
        }

        public List<double> histograma()
        {
            List<double> histograma = new List<double>();
            foreach(double v in clg.normalized)
            {
                if (distribucion != Distribucion.UNIFORME)
                {
                    histograma.Add(Math.Round(generarTiempo(v), 0));
                }
                else
                {
                    histograma.Add(generarTiempo(v));
                }
            }
            return histograma;
        }

        private double generarTiempo(double inicio)
        {
            double n = 0;
            double v = 0;
            switch (distribucion)
            {
                case Distribucion.UNIFORME:
                    v = (media + (desviacion * Math.Round(clg.nextNormalized(), 2)));
                    n = inicio + v;
                    break;
                case Distribucion.NORMAL:
                    v = Operaciones.Normal(clg.nextNormalized(), media, desviacion, 2);
                    n = inicio + v;
                    break;
                case Distribucion.EXPONENCIAL:
                    v = Operaciones.Exponencial(clg.nextNormalized(), media, desviacion, 2);
                    n = inicio + v;
                    break;
            }
            tiempos_cliente.Add(v);
            return n;
        }
    }
}
