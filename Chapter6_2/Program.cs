using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Numerics;
using System.Security.Cryptography;

namespace Chapter6_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            //double x = double.Parse("1.123", CultureInfo.InvariantCulture);//kultura niezmienna
            //string s = 1.123.ToString(CultureInfo.InvariantCulture);

            //NumberFormatInfo f = new NumberFormatInfo();
            //f.CurrencySymbol = "$$";
            //Console.WriteLine(3.ToString("C", f)); //C oznacza walute
            ////jeśli zamiast f damy null to wczytane zostanie domyslne ustawineie CurrentCulture
            //Console.WriteLine(10.3.ToString("F4"));//dokąłdnosc do 4 miejsc po przecinku

            //CultureInfo uk = CultureInfo.GetCultureInfo("en-GB");
            //Console.WriteLine(3.ToString("C", uk));

            //NumberFormatInfo f = new NumberFormatInfo();
            //f.NumberGroupSeparator = " ";
            //Console.WriteLine(122345.6789.ToString("N3", f));//uzycie separatora i dokładnosc do 3 miejsc po przecinku

            //string composite = "Credit={0:C}";
            //Console.WriteLine(string.Format(composite, 500));
            //Console.WriteLine(5.2.ToString("C",CultureInfo.GetCultureInfo("pl-PL")));

            //Standardowe numeryczne łańcuchy formatu str 251
            //int a = int.Parse("3E8", NumberStyles.HexNumber);
            //Console.WriteLine(a);
            //Console.WriteLine(DateTime.Now.ToString("U"));

            //Console.WriteLine(ConsoleColor.Red.ToString("X"));

            //double d = 3.59;
            //int i = Convert.ToInt32(d);//zaokrągla, a jawne rzutowanie odcina
            ////Convert stosuje zaokrąglenie bnakierskie
            ////można do zaokrąglenia uzyc Math.Round() gdzie można 
            //uint b = Convert.ToUInt32("101010", 2);
            //Console.WriteLine(b);

            //Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("pl-PL"); //zmiana kultury na polska

            //BigInteger googol = BigInteger.Pow(10, 100);
            //BigInteger bi = BigInteger.Parse("1".PadRight(100, '0'));
            //Console.WriteLine(googol.ToString());

            //RandomNumberGenerator rand = RandomNumberGenerator.Create();//duzo beziecziejszy ale  brak elastyccnzosci mzona tylko uzupełnic tablice losowymi lcizbami
            //byte[] bytes = new byte[1028];
            //rand.GetBytes(bytes);
            //var BigRandomNumber = new BigInteger(bytes);
            //Console.WriteLine(BigRandomNumber);

            //Random r1 = new Random();//bez ziarna , wykorzystywany czas systemowy
            //Console.WriteLine(r1.Next());
            //Console.WriteLine(r1.Next(100));//0-99
            //Console.WriteLine(r1.NextDouble());//0-1 

            //foreach (Enum i in Enum.GetValues(typeof(BorderSides)))
            //{
            //    Console.WriteLine(i);
            //}

            //Tuple<int, string> t = Tuple.Create(123, "Hello");
            //var tt = new Tuple<int, string>(123, "Hello");
            //Console.WriteLine(t.Item1 + " " + t.Item2.ToUpperInvariant());
            //Console.WriteLine(t == tt);//fałsz
            //Console.WriteLine(t.Equals(tt));//prawda

            //WriteLine(g.ToString());

            //.Equals() porównuje według zasad równosci wartościowej/ jesli pierwszy argument ma wartos null zgłasza wyjatek
            //== według równości wartoscioweej dla liczb i referencyjnie dla klas/nie zgłasza wyjatku

            //Console.WriteLine(object.ReferenceEquals(cos, cos2));//porównuje referencyjnie

            //Area a1 = new Area(1, 2);
            //Area a2 = new Area(2, 1);
            //Console.WriteLine(a1.Equals(a2));//true
            //Console.WriteLine(a1 == a2);//true po przeładowaniu operatora



            //Console.WindowHeight = Console.LargestWindowHeight;
            //Console.Write("test... 50%");
            //Console.CursorLeft -= 3;
            //Console.WriteLine("90%");

            //System.IO.TextWriter oldOut = Console.Out;
            //using (System.IO.TextWriter w = System.IO.File.CreateText("output.txt"))
            //{
            //    Console.SetOut(w);
            //    Console.WriteLine("Witaj, świecie");
            //}
            //Console.SetOut(oldOut);
            //System.Diagnostics.Process.Start("output.txt");

            Console.Title = "Hello";

            //statyczna klasa System.Envionment
            //Console.WriteLine(Environment.UserName);

            //Klasa Process
            //System.Diagnostics.Process.Start("plik.txt");






            Console.ReadKey();
        }
        [Flags]
        public enum BorderSides { Left = 1, Right = 2, Top = 4, Bottom = 8 }

    }
}
