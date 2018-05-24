using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opportion
{
    class Program
    {
        static void Main(string[] args)
        {
            string t = "00,01,02,03,04,05,06,07,08,09";
            string a = "10";

            if (t.Contains(a))
            {
                Console.WriteLine("ok");
            }
            Program.chia(12);
            Console.ReadLine();
        }
        public static int phannguyen, phandu;
        public static void chia(int sochia)
        {
            phannguyen = 100 / sochia;
            phandu = 100 - phannguyen * sochia;
            Console.WriteLine("phan nguyen:  {0}, phan du:  {1}", phannguyen, phandu);


            int k = 0;
            for (int i = 0; i < 100; i += phannguyen)
            {
                k++;
                if (k <= sochia)
                {
                    Console.Write("track :" + k + " gồm: ");
                    for (int j = 0; j < phannguyen; j++)
                    {
                        Console.Write(string.Format("{0:00}", i + j) + ",");
                    }

                    Console.WriteLine();

                }

                else
                {
                    int track2 =0;
                    for (int j = 0; j < phandu; j++)
                    {
                        track2++;
                        Console.WriteLine("track {0} so {1} ", track2,  i+j);
                    }
                    Console.WriteLine();
                }
            }

        }
    }
}
