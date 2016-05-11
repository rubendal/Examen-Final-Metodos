using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examen_Final_Metodos
{
    public class Barberia
    {
        public Barbero barbero;
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


        public Barberia(Distribucion distribucion, double media, double desviacion, int no_clientes, Distribucion distribucion_barbero, double media_barbero, double desviacion_barbero)
        {
            barbero = new Barbero(distribucion_barbero, media_barbero, desviacion_barbero);
            this.distribucion = distribucion;
            this.media = media;
            this.desviacion = this.desviacion;
            this.no_clientes = no_clientes;
            tiempo_anterior = 0;
            tiempo_a_analizar = 0;
            imagenes.Add(new Imagen(0, 0, 0));
            clg = new CLG(1000, new Random().Next(0, 1000));
        }

        public void generarCliente()
        {
            double tiempo = generarTiempo(tiempo_anterior);

            if (generados < no_clientes)
            {
                Cliente cliente = new Cliente(tiempo);
                generados++;
                if (!barbero.Atender(cliente))
                {
                    espera.Add(cliente);
                }
            }
            if (procesados < no_clientes)
            {
                Cliente activo = barbero.Terminar(tiempo);
                if (activo == null)
                {

                }
                else
                {
                    procesarTiempo(activo.salida);
                    atendidos.Add(activo);
                    procesados++;
                }
            }
            tiempo_anterior = tiempo;
            if (espera.Count > 0 || generados < no_clientes)
            {
                if (espera.Count > 0)
                {
                    if (barbero.Atender(espera[0]))
                    {
                        espera.RemoveAt(0);
                    }
                }
                Estado estado = barbero.estado;
                imagenes.Add(new Imagen(tiempo, espera.Size(tiempo), estado == Estado.ACTIVO ? 1 : 0));
                generarCliente();
            }else
            {
                Cliente activo = barbero.Terminar(tiempo);
                if (activo == null)
                {

                }
                else
                {
                    procesarTiempo(activo.salida);
                    atendidos.Add(activo);
                    procesados++;
                }
                Estado estado = barbero.estado;
                imagenes.Add(new Imagen(tiempo, espera.Size(tiempo), estado == Estado.ACTIVO ? 1 : 0));
            }

        }

        private void procesarTiempo(double tiempo)
        {
            if(espera.Count > 0)
            {
                if (espera[0].llegada <= tiempo)
                {
                    imagenes.Add(new Imagen(tiempo, espera.Size(tiempo), 1));
                }
                else
                {
                    imagenes.Add(new Imagen(tiempo, espera.Size(tiempo), 0));
                }
            }else
            {
                imagenes.Add(new Imagen(tiempo, espera.Size(tiempo), 0));
            }
        }

        private double generarTiempo(double inicio)
        {
            double n = 0;
            switch (distribucion)
            {
                case Distribucion.UNIFORME:
                    n = inicio + (media + (desviacion * Math.Round(clg.nextNormalized(), 2)));
                    break;
                case Distribucion.NORMAL:
                    n = inicio + Operaciones.Normal(clg.nextNormalized(), media, desviacion, 2);
                    break;
                case Distribucion.EXPONENCIAL:
                    n = inicio + Operaciones.Exponencial(clg.nextNormalized(), media, desviacion, 2);
                    break;
            }
            return n;
        }
    }
}
