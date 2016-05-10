using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Numerics;
using System.IO;

namespace Examen_Final_Metodos
{
    public static class Prime
    {
        private static List<BigInteger> primeList = new List<BigInteger>();
        private static BigInteger index = 0;
        private static Thread thread;
        private static readonly string file = "primes.bin";
        private static StringBuilder sbuilder = new StringBuilder();
        private static int intent = 0;
        private static readonly int maxIntent = 300;


        public static void Initialize()
        {
            primeList = new List<BigInteger>();

            InitializeUsingFiles();

            thread = new Thread(generate);
            thread.Start();
        }

        private static void InitializeUsingFiles()
        {
            if (!File.Exists(file))
            {
                primeList.Add(2);
                primeList.Add(3);
                index = 5;
                File.WriteAllText(file, "2\r\n3\r\n");
            }
            else
            {
                foreach (string s in File.ReadAllLines(file))
                {
                    BigInteger b = 0;
                    if (BigInteger.TryParse(s.TrimEnd('\r', '\n'), out b))
                    {
                        primeList.Add(b);
                    }
                    else
                    {
                        primeList.Clear();
                        primeList.Add(2);
                        primeList.Add(3);
                        index = 5;
                        return;
                    }
                }
                index = primeList[primeList.Count - 1];
            }
        }

        private static void generate()
        {
            for (BigInteger i = index; i < new BigInteger(100000); i += 2)
            {
                bool e = true;
                if (i % 2 == 0 || i % 3 == 0)
                {
                    e = false;
                }
                for (BigInteger n = 5; n * n <= i; n += 6)
                {
                    if (i % n == 0 || i % (n + 2) == 0)
                    {
                        e = false;
                    }
                }
                if (e)
                {
                    lock (primeList)
                    {
                        primeList.Add(i);
                        sbuilder.Append(i.ToString() + "\r\n");
                        intent++;
                        if (intent > maxIntent)
                        {
                            try
                            {
                                File.AppendAllText(file, sbuilder.ToString());
                                intent = 0;
                                sbuilder.Clear();
                                sbuilder = new StringBuilder();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
                        //Console.WriteLine("Prime: " + i);
                    }
                }
                index += 2;
            }
        }

        public static List<BigInteger> getPrimes(BigInteger m)
        {
            while (index < m)
            {
                Thread.Sleep(1000);
            }
            List<BigInteger> list = new List<BigInteger>();
            lock (primeList)
            {
                list = primeList.Where(n => n <= m).ToList();
            }

            return list;
        }

        public static void Finish()
        {
            try
            {
                if (thread != null)
                {
                    thread.Abort();
                }
            }
            catch (Exception e)
            {

            }
            try
            {
                if (!string.IsNullOrWhiteSpace(sbuilder.ToString()))
                {
                    File.AppendAllText(file, sbuilder.ToString());
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
