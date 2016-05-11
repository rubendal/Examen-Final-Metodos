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
        private Barberia barberia = new Barberia(3,Distribucion.EXPONENCIAL, 15, 1, 5, Distribucion.UNIFORME, 10, 5);

        public Form1()
        {
            InitializeComponent();
            barberia.procesarClientes();
            barberia.imagenes.Sort();
            string s = "";
            foreach(Cliente cliente in barberia.atendidos)
            {
                s+= $"Tiempo llegada: {cliente.llegada.ToTiempo()}, Tiempo salida: {cliente.salida.ToTiempo()}" + "\r\n";
            }
            foreach(Imagen i in barberia.imagenes)
            {
                s += $"Tiempo imagen: {i.tiempo.ToTiempo()}, Tamaño : {i.cola}, En uso : {i.uso.IntList()}" + "\r\n";
            }
            textBox1.Text = s;
        }
    }
}
