using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Examen_Final_Metodos
{
    public partial class Grafica : Form
    {

        public Grafica(Dictionary<double,double> datos)
        {
            InitializeComponent();
            chart1.Series.Add("Valor");
            chart1.Series["Valor"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            chart1.ChartAreas.Add("Area");
            foreach (ChartArea c in chart1.ChartAreas)
            {
                c.AxisX.MajorGrid.LineWidth = 0;
                c.AxisY.MajorGrid.LineWidth = 0;

            }
            chart1.Series["Valor"].BorderWidth = 3;
            Operaciones.Graficar(chart1.Series["Valor"].Points, datos);
        }

        public Grafica(List<Imagen> datos)
        {
            InitializeComponent();
            chart1.Series.Add("Valor");
            chart1.Series["Valor"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            chart1.Series["Valor"].BorderWidth = 3;
            chart1.ChartAreas.Add("Area");
            foreach (ChartArea c in chart1.ChartAreas)
            {
                c.AxisX.MajorGrid.LineWidth = 0;
                c.AxisY.MajorGrid.LineWidth = 0;

            }
            Operaciones.GraficarImagenCola(chart1.Series["Valor"].Points, datos);
        }

        public Grafica(List<Imagen> datos, int barberos)
        {
            InitializeComponent();
            graficarBarberos(datos, barberos);
            
            
        }

        private void graficarBarberos(List<Imagen> datos, int barberos)
        {
            chart1.ChartAreas.Add("Area");
            foreach (ChartArea c in chart1.ChartAreas)
            {
                c.AxisX.MajorGrid.LineWidth = 0;
                c.AxisY.MajorGrid.LineWidth = 0;

            }
            for (int i = 0; i < barberos; i++)
            {
                chart1.Series.Add("Barbero " + (i + 1));
                chart1.Series["Barbero " + (i + 1)].ChartType = SeriesChartType.StepLine;
                chart1.Series["Barbero " + (i + 1)].BorderWidth = 3-i;
                Operaciones.GraficarImagenUso(chart1.Series["Barbero " + (i + 1)].Points, datos, i);
            }
        }

        
    }
}
