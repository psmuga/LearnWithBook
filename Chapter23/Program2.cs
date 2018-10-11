using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter23
{
    class Program2
    {
        public static void Test()
        {
            //metody Paraller.* zoptymalizowane są pod kątem obliczeń, a nie operacji wejścia wyjścia, ale np nadaje się na pobranie dwóch stron internetowcyh
            //Parallel.Invoke(
            //    ()=>new WebClient().DownloadFile("http://www.linqpad.net","lp.html"),
            //    ()=>new WebClient().DownloadFile("http://www.jaoo.dk","jaoo.html")
            //    );
            //przyjmuje tablice delegatów action do wykonania
            //dziąła efektywnie nawet przy milinione delegatów, lepiej niż taski bo nie tworzy osobnego zadania dla kazdego delegatu

            //var kePairs = new string[6];
            //Parallel.For(0, kePairs.Length, i => kePairs[i] = RSA.Create().ToXmlString(true));
            ////lub w tym przypadku tez moze byc za pomcoą plinq
            //string[] keyPairs = ParallelEnumerable.Range(0, 6).Select(i => RSA.Create().ToXmlString(true)).ToArray();

            ////gdy mamy pętle wewnętrzne i zewnętrzne lepiej zrównoleglić tylko zewnętrzne

            //Parallel.ForEach("Hello World", (c, state, i) =>
            //{
            //    Console.WriteLine(c.ToString() + i);
            //});

            //Parallel.ForEach("Hello World", (c, state) =>
            //{
            //    if (c==' ')
            //    {
            //        state.Break();
            //    }
            //    else
            //    {
            //        Console.Write(c);
            //    }
            //});
            //można tęż state.Stop gdy dostalismy to czego chcielismy lubgdyby coś nie wyszło i nie interesują nas wyniki
            //foreach i for zwracaja obiekt ParallelLoopResult


            //suma 10 000 000 pierwiastków równolegle
            object locker = new object();
            double grandTotal = 0;
            Parallel.For(1, 10000000,
                () => 0.0,//inicjalizacja wartosci lokalnej
                (i, state, localTotal) => localTotal + Math.Sqrt(i),//zwwraca nową sumę lokalną
                localTotal =>
                {
                    lock (locker)
                    {
                        grandTotal += localTotal;//dodwanaie wartosci lokalnej do wartosci głównej
                    }
                }
            );
            Console.WriteLine(grandTotal);
            //można też za pomocą plinq
            var wynik = ParallelEnumerable.Range(1, 10000000-1).Sum(i => Math.Sqrt(i));//zwróc uwage na -1
            Console.WriteLine(wynik);

            double grand2 = 0;
            for (int i = 1; i < 10000000; i++)
            {
                grand2 += Math.Sqrt(i);
            }
            Console.WriteLine($"Sekwencyjnie: {grand2}");

        }
    }
}
