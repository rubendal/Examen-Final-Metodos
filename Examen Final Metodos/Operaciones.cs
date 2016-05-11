using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Numerics;

namespace Examen_Final_Metodos
{
    public static class Operaciones
    {

        public static string ToTiempo(this double t)
        {
            int h = (int)(t / 3600);
            int m = (int)((t - (h * 3600)) / 60);
            int s = (int)(t - (h * 3600) - (m * 60));
            string r = "";
            if (h > 0)
            {
                r += $"{h} horas,";
            }
            if (m > 0)
            {
                r += $"{m} min,";
            }
            if (s >= 0)
            {
                r += $"{s} segundos,";
            }
            r = r.TrimEnd(',');
            return r;
        }

        public static string IntList(this List<int> list)
        {
            string s = "";
            foreach(int i in list)
            {
                s += i.ToString() + ",";
            }
            return s.TrimEnd(',');
        }

        public static void BoxMuller(double rand1, double rand2, double med, double des, out double x, out double y, int round = 0)
        {
            x = Math.Round(Math.Sqrt(-2 * Math.Log(rand1)) * Math.Cos(2 * Math.PI * rand2) * des, round) + med;
            y = Math.Round(Math.Sqrt(-2 * Math.Log(rand1)) * Math.Sin(2 * Math.PI * rand2) * des, round) + med;
        }

        public static double Exponencial(double x, double med = 0, double des = 1, int round = 0)
        {
            return Math.Round((-med * Math.Log(1-x)), round);
        }

        public static double Normal(double x, double med = 0, double des = 1, int round = 0)
        {
            return Math.Round((med + (des * Math.Sqrt(2) * SpecialFunctions.ErfInv(-1 + (2 * x)))), round);
        }

        public static double Media(List<double> list, int count)
        {
            if(count == 0)
            {
                count = list.Count;
            }
            double x = 0;
            for(int i=0;i<count; i++)
            {
                x += list[i];
            }
            return x / count;
        }

        public static double Varianza(List<double> list, int count = 0)
        {
            if(count == 0)
            {
                count = list.Count;
            }
            double x = 0;
            double med = Media(list, count);
            for (int i = 0; i < count; i++)
            {
                x += (list[i] * list[i]);
            }
            return (x / count) - (med * med);
        }

        public static double DesviacionEstandar(List<double> list, int round = 3, int count = 0)
        {
            return Math.Round(Math.Sqrt(Varianza(list, count)), round);
        }

        public static Dictionary<T,int> GenerarHistograma<T>(List<T> list)
        {
            Dictionary<T, int> dic = new Dictionary<T, int>();
            for(int i = 0; i < list.Count; i++)
            {
                if (dic.ContainsKey(list[i]))
                {
                    dic[list[i]]++;
                }
                else
                {
                    dic.Add(list[i], 1);
                }
            }
            return dic;
        }

        public static void Graficar<T>(DataPointCollection chart, List<T> list, bool repeat = false)
        {
            List<T> l = list;
            l.Sort();
            List<T> repeated = new List<T>();
            Dictionary<T, int> dic = GenerarHistograma(l);
            foreach(T t in l)
            {
                if (repeat)
                {
                    chart.AddXY(t, dic[t]);
                }
                else
                {
                    if (!repeated.Contains(t))
                    {
                        chart.AddXY(t, dic[t]);
                        repeated.Add(t);
                    }
                }
            }
        }

        public static void Graficar(DataPointCollection chart, Dictionary<double, double> dic)
        {
            List<double> l = dic.Keys.ToList();
            l.Sort();
            foreach (double i in l)
            {
                chart.AddXY(i, dic[i]);
            }
        }

        public static void GraficarPoisson(DataPointCollection chart, Dictionary<int,double> dic)
        {
            List<int> l = dic.Keys.ToList();
            l.Sort();
            double n = 0;
            foreach (int i in l){
                n += dic[i];
                chart.AddXY(n, i);
            }
        }

        //public static void GraficarPoisson3D(ChartArea chart, Dictionary<int, double> dic, Dictionary<int, double> dic2)
        //{

        //    List<int> l = dic.Keys.ToList();
        //    l.Sort();
        //    double n = 0;
        //    double n2 = 0;

        //    List<double> k1 = new List<double>();
        //    List<double> k2 = new List<double>();
        //    List<double> i1 = new List<double>();
        //    int c = 0;

        //    string dllPath = @"C:\Program Files\R\R-3.2.4revised\bin\x64\";
        //    REngine.SetEnvironmentVariables(dllPath);

        //    REngine engine = REngine.GetInstance();
        //    engine.Initialize();

        //    foreach (int i in l)
        //    {
        //        n += dic[i];
        //        n2 += dic2[i];
        //        k1.Add(n);
        //        k2.Add(n2);
        //        i1.Add(i);
        //        c++;
        //    }

        //    engine.Evaluate("library(\"plot3D\")");
        //    NumericVector x = engine.CreateNumericVector(k1);
        //    engine.SetSymbol("x", x);
        //    NumericVector y = engine.CreateNumericVector(k2);
        //    engine.SetSymbol("y", y);
        //    NumericVector z = engine.CreateNumericVector(i1);
        //    engine.SetSymbol("z", z);

        //    engine.Evaluate("plot3D::lines3D(x,y,z)");

        //    engine.Dispose();

        //}

        public static List<double> GenerarNormalizado(BigInteger m)
        {
            CLG clg = new CLG(m, new Random().Next(0, (int)m) * new Random().Next(1, (int)m));
            return clg.normalize();
        }

        public static double Integrar(Func<double, double> f, double a, double b)
        {
            
            return Integrate.OnClosedInterval(f, a, b);
        }

        public static List<List<double>> Estocastico(BigInteger x, BigInteger y)
        {
            List<List<double>> e = new List<List<double>>();
            for(BigInteger i = 0; i < x; i++)
            {
                CLG clg = new CLG(y, new Random().Next(0, (int)y));
                List<double> t = clg.normalize();
                //List<double> t = new List<double>();
                //for (BigInteger j = 0; j < y; j++)
                //{
                //    t.Add()
                //}
                e.Add(t);
            }

            return e;
        }

        public static Dictionary<int,double> Poisson(double x, int m, double lambda)
        {
            Dictionary<int, double> p = new Dictionary<int, double>();
            double t = 0;
            double u = 0;
            double T = x;
            CLG clg = new CLG(m, new Random().Next(0, m));
            List<double> rand = clg.normalize();
            int i = 0;
            int c = 0;
            while(T>0)
            {
                if(c > m-1)
                {
                    clg = new CLG(m, new Random().Next(0, m));
                    rand = clg.normalize();
                    c = 0;
                }
                u = rand[c];
                t = Math.Round(-(Math.Log(u) / lambda),3);
                //T -= t;
                T--;
                p.Add(i,t);
                c++;
                i++;
            }
            return p;
        }

        public static Dictionary<int, double> PoissonNoHomogeneo(double x, int m, double lambda, Func<double,double> F) //F = p(t)
        {
            Dictionary<int, double> p = new Dictionary<int, double>();
            double t = 0;
            double u = 0;
            double u2 = 0;
            double T = x;
            CLG clg = new CLG(m, new Random().Next(0, m));
            CLG clg2 = new CLG(m, new Random().Next(0, m));
            List<double> rand = clg.normalize();
            List<double> rand2 = clg2.normalize();
            int i = 0;
            int c = 0;
            int c2 = 0;
            while (T > 0)
            {
                if (c > m - 1)
                {
                    clg = new CLG(m, new Random().Next(0, m));
                    rand = clg.normalize();
                    c = 0;
                }
                if (i == 0)
                {
                    u = rand[c];
                    t = Math.Round(-(Math.Log(u) / lambda), 3);
                    //T -= t;
                    T--;
                    p.Add(i, t);
                    c++;
                    i++;
                }else
                {
                    do
                    {
                        if (c2 > m - 1)
                        {
                            clg2 = new CLG(m, new Random().Next(0, m));
                            rand2 = clg2.normalize();
                            c2 = 0;
                        }
                        u2 = rand2[c2];
                        c2++;
                        if (c > m - 1)
                        {
                            clg = new CLG(m, new Random().Next(0, m));
                            rand = clg.normalize();
                            c = 0;
                        }
                        u = rand[c];
                        t = Math.Round(-(Math.Log(u) / lambda), 3);
                        //T -= t;
                        T--;
                        c++;
                    } while (u2 <= F.Invoke(t));
                    p.Add(i, t);
                    c++;
                    i++;
                }
            }
            return p;
        }


    }
}
