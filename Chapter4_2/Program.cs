#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Chapter4_2
{
    class Program
    {
        public int x = 2;
        delegate int Dziel(int i);
        static unsafe void Main(string[] args)
        {

            // try
            // {
            //     Dziel abc = x => 10 / x;
            //     abc(0);
            //     Console.WriteLine("Linia nastepna po  błędzie"); // nie  wyswiwtli 
            // }
            // catch (DivideByZeroException e) //when (e.    ) filtr wyjatku
            // {
            //     Console.WriteLine("blad ");
            //     //throw;
            // }
            // catch 
            // {
            //     Console.WriteLine("every other exceptions");
            // }


            //// File.Create("file.txt");
            // StreamReader reader = null;
            // try
            // {
            //     reader = File.OpenText("file.txt");
            //     if (reader.EndOfStream) return;
            //     Console.WriteLine(reader.ReadToEnd());
            // }
            // finally
            // {
            //     //if (reader != null) reader.Dispose();
            //     reader?.Dispose(); //to samo
            // }

            // using (var cosStreamReader = File.OpenText("File.txt")) //to jest równoważne z całym blokkiem try wyzej razem z stworzeniem readera
            // {
            //     Console.WriteLine(cosStreamReader.ReadToEnd());
            // }

            // //wzorzec metod tryXXX str 165

            // var dic = new Dictionary<int, string>()
            // {
            //     {1,"ala" },
            //     {2,"ma" }
            // };
            // var dict = new Dictionary<int,string>()
            // {
            //     [5] = "five",
            //     [10] = "ten"
            // };



            // foreach (var x in Fibs(8))
            // {
            //     Console.Write(x + " ");
            // }
            // Console.WriteLine();  
            // foreach (var x in ParzysteFib(Fibs(8)))
            // {
            //     Console.Write(x + " ");
            // }
            // Console.WriteLine();


            // foreach (var x in GetSequence(4,10))
            // {
            //     Console.Write(x + " ");
            // }
            // Console.WriteLine();
            // var cos = (IEnumerator<int>) GetSequence(2, 8).GetEnumerator();
            // while (cos.MoveNext())
            // {
            //     Console.Write(cos.Current + " ");
            // }
            // Console.WriteLine();

            // var test = new Word();
            // Console.WriteLine(test[1]);
            // foreach (var VARIABLE in Word.Zdanie())
            // {
            //     Console.Write(VARIABLE + " ");
            // }
            // Console.WriteLine();


            int? zmienna = null; // dopuszczajaca null
            zmienna = 1;
            var y = (int)zmienna;
            Console.WriteLine(zmienna);

            // object o = "cosik";
            // int? a = o as int?; // null gdyz nie da sie przekonwertowac stringa na int
            // Console.WriteLine(a);


            //Console.WriteLine("Hello".IsCapitalized()); // z klasy Klasa metoda rozszerzajaca



            ////typy anonimowe
            //var dude = new {Name="Piotr",Age = 23};
            //Console.WriteLine(dude.Age);


            //Foo();
            //#region moj

            //Program abcd = new Program();   
            //unsafe //properties -> debug ->allow unsafe code
            //{
            //    fixed (int* p = &abcd.x)
            //    {
            //        *p = 9;
            //    }
            //    Console.WriteLine(abcd.x);
            //}


            //int* a = stackalloc int[10]; // alokacja na stosie i ogranicozne do istnienia np w funkcji
            //for (int i = 0; i < 10; i++)
            //{
            //    Console.Write(a[i] + " ");
            //}
            //#endregion







            Console.ReadKey();
        }


        //atrybuty
        static void Foo([CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            Console.WriteLine(memberName);
            Console.WriteLine(filePath);
            Console.WriteLine(lineNumber);
        }



        [Conditional("DEBUG")]//generuje ejsli jest DEBUG -> pierwsza linai
        class Word: Attribute
        {
            private static string[] _w = "Ala ma kota a kot ma ale".Split();

            public string this[int nr]
            {
                get { return _w[nr]; }
                set { _w[nr] = value; }
            }

            public static IEnumerable<string> Zdanie()
            {
                foreach (var VARIABLE in _w)
                {
                    yield return VARIABLE;
                }
            }
        }





        /// <summary>
        /// Wylicza kolejne wartosci ciagu fibonacciego
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static IEnumerable<int> Fibs(int x)
        {
            for (int i = 0, prev =1,cur =1; i < x; i++)
            {
                yield return prev;
                int newFib = prev + cur;
                prev = cur;
                cur = newFib;
            }
        }

        static IEnumerable<int> ParzysteFib(IEnumerable<int> sequence) //komponowaaanie iteratorów
        {
            foreach (var VARIABLE in sequence)
            {
                if ((VARIABLE % 2) == 0) yield return VARIABLE;
            }
        }



        static IEnumerable<int> GetSequence(int fromValue, int toValue)
        {
            if (toValue >= fromValue)
            {
                for (int i = fromValue; i <= toValue; i++)
                {
                    yield return i;
                }
            }
            else
            {
                for (int i = fromValue; i >= toValue; i--)
                {
                    yield return i;
                }
            }
        }
    }
}
