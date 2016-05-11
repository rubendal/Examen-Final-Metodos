using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examen_Final_Metodos
{
    public class Cliente
    {
        public double llegada { get; set; }
        public double atendido { get; set; }
        public double salida { get; set; }

        public Cliente()
        {

        }

        public Cliente(double llegada)
        {
            this.llegada = llegada;
        }

        public double TiempoDeEspera()
        {
            return atendido - llegada;
        }
    }
}
