using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examen_Final_Metodos
{
    public class Imagen : IComparable<Imagen>
    {
        public double tiempo { get; set; }
        public int cola { get; set; }
        public List<int> uso { get; set; }

        public Imagen(double tiempo,int cola, List<int> uso)
        {
            this.tiempo = tiempo;
            this.cola = cola;
            this.uso = uso;
        }

        public int CompareTo(Imagen other)
        {
            return tiempo.CompareTo(other.tiempo);
        }
    }
}
