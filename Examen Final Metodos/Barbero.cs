﻿using System;
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
        public List<Cliente> cliente { get; set; }
        private double fin { get; set; }
        private double tiempo_ultimo { get; set; }
        public double tiempo_trabajo { get; set; }
        public List<double> tiempos = new List<double>();

        public Barbero(Distribucion distribucion, double media, double desviacion)
        {
            estado = Estado.INACTIVO;
            this.distribucion = distribucion;
            this.media = media;
            this.desviacion = desviacion;
            clg = new CLG(1000, new Random().Next(0, 1000));
            cliente = new List<Cliente>();
        }

        public Estado getEstado(double tiempo)
        {
            foreach(Cliente c in cliente)
            {
                if(tiempo >= c.llegada && tiempo < c.salida)
                {
                    return Estado.ACTIVO;
                }
                
            }
            return Estado.INACTIVO;

        }

        public bool Atender(Cliente cliente)
        {
            if (estado == Estado.INACTIVO)
            {
                this.cliente.Add(cliente);
                estado = Estado.ACTIVO;
                if (cliente.llegada < tiempo_ultimo)
                {
                    fin = generarFin(tiempo_ultimo);
                }
                else
                {
                    fin = generarFin(cliente.llegada);
                }
                tiempo_trabajo += fin - cliente.llegada;
                this.cliente[this.cliente.Count-1].salida = fin;
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
                if(tiempo >= cliente[cliente.Count - 1].salida)
                {
                    estado = Estado.INACTIVO;
                    tiempo_ultimo = cliente[cliente.Count - 1].salida;
                    return cliente[cliente.Count - 1];
                }
            }
            return null;
        }

        private double generarFin(double inicio)
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
            tiempos.Add(v);
            return n;
        }

        public double TiempoPromedio(double max)
        {
            return tiempo_trabajo / max;
        }

        public List<double> histograma()
        {
            List<double> histograma = new List<double>();
            foreach (double v in clg.normalized)
            {
                if (distribucion != Distribucion.UNIFORME)
                {
                    histograma.Add(Math.Round(generarFin(v), 0));
                }
                else
                {
                    histograma.Add(generarFin(v));
                }
            }
            return histograma;
        }

    }
}
