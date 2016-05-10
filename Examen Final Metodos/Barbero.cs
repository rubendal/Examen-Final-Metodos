using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examen_Final_Metodos
{
    public class Barbero
    {
        public Estado estado { get; set; }
        private Distribucion distribucion { get; set; }
        private double media { get; set; }
        private double desviacion { get; set; }
        private CLG clg; //Genera tiempos de salida
        public Cliente cliente { get; set; }
        private double fin { get; set; }
        private double tiempo_ultimo { get; set; }

        public Barbero(Distribucion distribucion, double media, double desviacion)
        {
            estado = Estado.INACTIVO;
            this.distribucion = distribucion;
            this.media = media;
            this.desviacion = desviacion;
            clg = new CLG(1000, new Random().Next(0, 1000));
        }

        public bool Atender(Cliente cliente)
        {
            if (estado == Estado.INACTIVO)
            {
                this.cliente = cliente;
                estado = Estado.ACTIVO;
                if (cliente.llegada < tiempo_ultimo)
                {
                    fin = generarFin(tiempo_ultimo);
                }
                else
                {
                    fin = generarFin(cliente.llegada);
                }
                
                this.cliente.salida = fin;
                return true;
            }else
            {
                return false;
            }
        }

        public Cliente Terminar(double tiempo)
        {
            if(estado == Estado.ACTIVO)
            {
                if(tiempo >= cliente.salida)
                {
                    estado = Estado.INACTIVO;
                    tiempo_ultimo = cliente.salida;
                    return cliente;
                }
            }
            return null;
        }

        private double generarFin(double inicio)
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
