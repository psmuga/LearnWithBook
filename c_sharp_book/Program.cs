using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_book
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(int.MaxValue);

            //Console.Write(Environment.NewLine);

            //Console.WriteLine(default(int));

            //Console.WriteLine('\u00A9');

            //int a = int.MaxValue;
            //int b = 1;
            //Console.WriteLine(checked(a + b));

            //int c = 5;
            ////drugi parametr określa jakiego typu ma być podstawa liczby
            //string s = Convert.ToString(c, 2);
            //Console.WriteLine("string s = {0}", s);

            //x określa ile miejsc zajmie liczba szesnastkowo
            //Console.WriteLine(byte.MaxValue.ToString("X4"));

            //StringBuilder stb = new StringBuilder();
            //stb.Append("Ala ma kota");

            //string a = "co: ";
            //int b = 0;
            //Foo test = new Foo();
            //test.first(ref a, out b);
            //Console.WriteLine(a + ' ' + b);
            //int c = test.suma_ile_chcesz(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            //Console.WriteLine("Suma wynosi: {0}", c);
            ////dopasowuje wartości do odpowiednich argumentów
            ////test.dopasuj(y: 2, x: 1);


            //Console.WriteLine("\a Alert");

            //string a2 = @"\\C:\cos\cos\al.cs";
            //Console.WriteLine(a2);
            //int b = 4;
            //string a3 = $" Kot ma {b} łapy";

            //string s1 = "a";
            ////jeśli s1 jest null to do s2 przypisane będzie przypisana "wartosć nowa"
            //string s2 = s1 ?? "wartosc nowa";
            //Console.WriteLine(s2);


            ////?. jesli s jest nullem wyrażenie ewaluuje do null zamiast rzucać wyjątek
            //StringBuilder s = null;
            //string s2 = s?.ToString();
            //Console.WriteLine(s2);

            //char wybor = Convert.ToChar(Console.Read());
            //switch (wybor)
            //{
            //    case '1':
            //        Console.WriteLine("1");
            //        break;
            //    case '2':
            //        goto case '1';
            //    default:
            //        break;
            //}

            //Console.WriteLine(typeof(Wine));
            //Wine wino = new Wine(100m);
            //Console.WriteLine(wino.GetType());

            //Point pkt = new Point();
            //Console.WriteLine(pkt.X);

            //Console.WriteLine(Side.left.ToString());
            //Console.WriteLine(Side2.left.ToString());

            //Side s = unchecked((Side)5);
            //Console.WriteLine(Enum.IsDefined(typeof(Side), s));

            //Stack<Bear> bears = new Stack<Bear>();
            //Bear b = new Bear();
            //bears.Push(b);
            //bears.Push(b);
            //Camel c = new Camel();
            //Stack<Animal> an = new Stack<Animal>();
            //an.Push(c);
            //an.Push(b);
            //ZooCleaner.Wash(an);

            //Version versja = new Version(1, 0, 0);
            //Console.WriteLine(versja);
            //Console.Write(Environment.NewLine);
            //OperatingSystem os = Environment.OSVersion;
            //Version ver = os.Version;
            //Console.WriteLine("Operating System: {0} ({1})", os.VersionString, ver.ToString());
            //Console.Write(Environment.NewLine);

            ////przypisuje nazwę zmiennej
            //int zmienna = 1;
            //string c = nameof(zmienna);
            //Console.WriteLine(c);

            //Animal an = new Animal();
            //if (an as Bear == null)
            //{
            //    //as - downcast przypisuje null zamiast rzucić wyjątek, dobre do testowania wyniku czy jest zero, nie można używać do typów wbudowanych
            //    Console.WriteLine("Jest nullem");
            //}

            //Animal an = new Animal();
            //if (an is Bear)
            //{
            //    Bear n = (Bear)an;
            //}

            //int x = 2;
            //object y = x;
            //int z = (int)y;
            //long l = (long)(int)y;

            //w klasie może istnieć pole o typie tej klasy

            //Bunny b1 = new Bunny { Name = "Bo", LikesCarrots = true, LikesHumans = false };
            //Bunny b2 = new Bunny("Bo") { LikesHumans = true, LikesCarrots = false };

            //Sentence s = new Sentence();
            //Console.WriteLine(s[3]);
            //s[3] = "kangoo";

            //string s = null;
            //Console.WriteLine(s?[2]);

            string names = nameof(StringBuilder) + "." + nameof(StringBuilder.Length); //"StringBuilder.Length"
            Console.WriteLine(names);
            //sealed uniemożliwia nadpisanie funkcji lub klasy



            //Test first = new Test();
            //Test drugi = first + 1;
            //drugi += 2;//możemy bo przeładowalismy + i automatycznie operator += jest przełądowany

            Console.ReadKey();
        }




    }

    //indexer
    class Sentence
    {
        private string[] _words = "The quick brown fox".Split();
        public string this[int wordNum]
        {                                 
            get { return _words[wordNum]; }
            set { _words[wordNum] = value; }
        }

    }

    public class Bunny
    {
        public string Name;
        public bool LikesCarrots;
        public bool LikesHumans;
        public Bunny() { }
        public Bunny(string n) { Name = n; }
    }


    //public class klasa
    //{
    //    klasa() { }
    //    public static klasa create(...)
    //    {
    //        //perform custom logic here to return an instance of klasa
    //    }
    //}



    //public class A { public int Counter = 1; }
    //public class B : A { public new int Counter = 2; }
    ////operator new informuje kompilator że nie przez przypadek zasłaniamy Counter

    class X
    {
        protected virtual void F() { Console.WriteLine("X.F"); }
        protected virtual void F2() { Console.WriteLine("X.F2"); }
    }
    class Y : X
    {
        sealed protected override void F() { Console.WriteLine("Y.F"); }
        protected override void F2() { Console.WriteLine("Y.F2"); }
    }
    class Z : Y
    {
        // Attempting to override F causes compiler error CS0239.
        // protected override void F() { Console.WriteLine("C.F"); }
        // Overriding F2 is allowed.
        protected override void F2() { Console.WriteLine("Z.F2"); }
    }




    class Animal
    {
        public virtual void Info()
        {
            Console.WriteLine("Animal");
        }


    }
    class Bear : Animal
    {
        public override void Info()
        {
            Console.WriteLine("Bear");
        }
    }
    class Camel : Animal
    {
        public override void Info()
        {
            Console.WriteLine("Camel");
        }
    }

    class ZooCleaner
    {
        //generic types
        public static void Wash<T>(Stack<T> animals) where T : Animal
        {
            foreach (var item in animals)
            {
                item.Info();
            }
        }
    }



    //domyślnie jest int zamiast byte
    public enum Side : byte
    {
        left,
        right,
        top = 5,
        bottom
    }
    [Flags]
    public enum Side2 : int
    {
        left,
        right,
        top,
        bottom,
        leftright = left | right
    }


    struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    //typ referencyjny
    public class Wine
    {
        public decimal Price;
        public int Year;
        public Wine(decimal price)
        {
            Price = price;
        }
        public Wine(decimal price, int year) : this(price)
        {
            Year = year;
        }
    }



    partial class Foo
    {
        //definicje
        partial void Cena(decimal amount);


    }
    partial class Foo
    {
        //implementacja
        partial void Cena(decimal amount)
        {
            if (amount > 0)
            {
                Console.WriteLine("Success");
            }
        }

        public void first(ref string a, out int b)
        {
            //ref in
            //out out
            a += "ref";
            b = 1;
            return;
        }
        public int suma_ile_chcesz(params int[] ints)
        {
            int suma = default(int);
            foreach (var item in ints)
            {
                suma += item;
            }
            return suma;
        }
        public void Dopasuj(int x, int y)
        {
            Console.WriteLine("X = {0}\nY = {1}", x, y);

        }

        // public int mnozenie_razy_2(int x){return x*2;}
        public int mnozenie_razy_2(int x) => x * 2;


    }
}