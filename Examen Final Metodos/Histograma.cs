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
    public partial class Histograma : Form
    {

        public Histograma(List<double> datos)
        {
            InitializeComponent();
            chart1.Series.Add("Frecuencia");
            chart1.Series["Frecuencia"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            chart1.ChartAreas.Add("Area");
            Operaciones.Graficar(chart1.Series["Frecuencia"].Points, datos);
        }
    }
}
