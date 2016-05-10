using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examen_Final_Metodos
{
    public class Cola : List<Cliente>
    {
        public int Size(double tiempo_actual)
        {
            int i = 0;
            foreach(Cliente cliente in this)
            {
                if(cliente.llegada <= tiempo_actual)
                {
                    i++;
                }
            }
            return i;
        }
    }
}
