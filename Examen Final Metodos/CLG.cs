using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Examen_Final_Metodos
{
    public class CLG
    {
        private BigInteger m;
        private BigInteger a;
        private BigInteger c;
        private BigInteger x0;
        public int index = 0;

        public List<BigInteger> x { get; private set; }
        public List<double> normalized { get; private set; }

        public CLG(BigInteger m, BigInteger x0)
        {
            this.m = m;
            this.x0 = x0 % m;
            x = new List<BigInteger>();
            List<BigInteger> p = primos();
            List<BigInteger> f = factores(p);
            c = calcC(p, f);
            a = f.Aggregate((x, y) => x * y);
            //Console.WriteLine("a= " + a);
            if(m % 4 == 0)
            {
                if(a%4 == 0)
                {
                    a++;
                }
                else
                {
                    a = (a * 4) + 1;
                }
            }
            else
            {
                a++;
            }

            index = 0;
            x.Add(x0);
            for(BigInteger i = 1; i< m; i++)
            {
                x.Add(((a * x[(int)i - 1]) + c) % m);
            }
            normalized = normalize();
        }

        public void setIndex(int i)
        {
            index = i;
        }

        public BigInteger get(int i)
        {
            return x[i];
        }

        public BigInteger next()
        {
            BigInteger r = x[index];
            index++;
            if (index > x.Count)
            {
                index = 0;
            }
            return r;
        }

        public double nextNormalized()
        {
            double r = normalized[index];
            index++;
            if (index > x.Count-1)
            {
                index = 0;
            }
            return r;
        }

        public List<double> getList()
        {
            List<double> list = new List<double>();
            for (BigInteger i = 0; i < m; i++)
            {
                list.Add((double)x[(int)i]);
            }
            return list;
        }

        public List<double> normalize()
        {
            List<double> list = new List<double>();
            for (BigInteger i = 0; i < m; i++)
            {
                double n = ((double)x[(int)i] / (int)(m - 1));
                if (n != 0)
                {
                    if (n != 1)
                    {
                        list.Add(n);
                    }
                    else {
                        list.Add(0.999999999999);
                    }
                    
                }
                else
                {
                    list.Add(0.0000000001);
                }
                //Console.WriteLine(((double)x[(int)i] / (int)(m - 1)));
            }
            return list;
        }

        private BigInteger calcC(List<BigInteger> primo, List<BigInteger> factor)
        {

            List<BigInteger> l = new List<BigInteger>();

            l.AddRange(primo);
            for(int i = 0; i < factor.Count; i++)
            {
                l.Remove(factor[i]);
            }

            if(l.Count > 0)
            {
                return l[new Random().Next(0, l.Count)];
            }
            
            return 1;
        }

        private List<BigInteger> factores(List<BigInteger> primo)
        {
            List<BigInteger> factores = new List<BigInteger>();
            foreach(BigInteger p in primo)
            {
                if(m % p == 0)
                {
                    factores.Add(p);
                }
            }
            return factores;
        }


        private List<BigInteger> primos()
        {
            List<BigInteger> primos = new List<BigInteger>();
            primos = Prime.getPrimes(m);
            primos.Add(1);

            return primos;

        }




    }
}
