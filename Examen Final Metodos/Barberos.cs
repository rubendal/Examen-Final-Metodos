using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examen_Final_Metodos
{
    public class Barberos : List<Barbero>
    {
        public Barberos(int count,Distribucion distribucion, double media, double varianza)
        {
            for(int i = 0; i< count; i++)
            {
                Add(new Barbero(distribucion, media, varianza));
            }
        }

        public bool Atender(Cliente cliente)
        {
            foreach(Barbero barbero in this)
            {
                if (barbero.Atender(cliente))
                {
                    return true;
                }
            }
            return false;
        }

        public List<Cliente> Terminar(double tiempo)
        {
            List<Cliente> clientes = new List<Cliente>();
            foreach (Barbero barbero in this)
            {
                Cliente activo = barbero.Terminar(tiempo);
                if(activo == null)
                {

                }else
                {
                    clientes.Add(activo);
                }
            }
            return clientes;
        }

        public List<Estado> posibleEstado(double tiempo, Cola cola)
        {
            List<Estado> estados = new List<Estado>();
            bool hecho = false;
            foreach (Barbero barbero in this)
            {
                if (cola.Size(tiempo) > 0)
                {
                    if (barbero.getEstado(tiempo) == Estado.INACTIVO && !hecho)
                    {
                        estados.Add(Estado.ACTIVO);
                        hecho = true;
                    }
                    else
                    {
                        estados.Add(barbero.estado);
                    }
                }else
                {
                    estados.Add(barbero.estado);
                }
                
            }
            return estados;
        }

        public List<Estado> getEstados(double tiempo)
        {
            List<Estado> estados = new List<Estado>();
            foreach (Barbero barbero in this)
            {
                estados.Add(barbero.getEstado(tiempo));
            }
            return estados;
        }
    }
}
