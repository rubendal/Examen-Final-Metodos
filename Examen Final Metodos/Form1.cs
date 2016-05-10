using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Examen_Final_Metodos
{
    public partial class Form1 : Form
    {
        private Barberia barberia = new Barberia(Distribucion.EXPONENCIAL, 15, 1, 5, Distribucion.UNIFORME, 10, 5);

        public Form1()
        {
            InitializeComponent();
            barberia.generarCliente();
            barberia.imagenes.Sort();
            foreach(Cliente cliente in barberia.atendidos)
            {
                Console.WriteLine($"Tiempo llegada: {cliente.llegada.ToTiempo()}, Tiempo salida: {cliente.salida.ToTiempo()}");
            }
            foreach(Imagen i in barberia.imagenes)
            {
                Console.WriteLine($"Tiempo imagen: {i.tiempo.ToTiempo()}, Tamaño : {i.cola}, En uso : {i.uso}");
            }
        }
    }
}
