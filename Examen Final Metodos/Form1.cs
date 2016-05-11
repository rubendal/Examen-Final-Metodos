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
        private int barberos = 3;
        private Barberia barberia;

        public Form1()
        {
            InitializeComponent();
            barberia = new Barberia(barberos, Distribucion.EXPONENCIAL, 15, 1, 5, Distribucion.UNIFORME, 10, 5);
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
            Console.WriteLine(s);
            double tiempoespera = 0;
            barberia.atendidos.ForEach(cliente => tiempoespera += cliente.TiempoDeEspera());
            double tiempoespera_max = tiempoespera / barberia.tiempo_max; //Longitud promedio de la cola
            string sb = "";
            int ib = 0;
            foreach(Barbero b in barberia.barberos)
            {
                sb += $"Barbero {ib + 1}: {b.TiempoPromedio(barberia.tiempo_max)}\n";
                ib++;
            }
            sb.TrimEnd('\n');
            double tiempoespera_promedio = tiempoespera / barberia.atendidos.Count;
            label1.Text = $"Longitud promedio de cola: {tiempoespera_max}";
            label2.Text = $"Tiempo espera promedio: {tiempoespera_promedio}";
            label3.Text = sb;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Histograma h = new Histograma(barberia.histograma());
            h.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Histograma h = new Histograma(barberia.barberos[0].histograma());
            h.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Histograma h = new Histograma(barberia.tiempos_cliente);
            h.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Grafica g = new Grafica(barberia.imagenes, barberos);
            g.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Grafica g = new Grafica(barberia.imagenes);
            g.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Histograma h = new Histograma(barberia.barberos[0].tiempos);
            h.ShowDialog();
        }
    }
}
