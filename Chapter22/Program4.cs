using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace Chapter22
{
    class Program4
    {
        public static void Test()
        {
            //System.Threading
            //Timer tmr = new Timer(Tick, "Tik-tak", 5000, 1000);
            ////jesli ma byc jednorazowo to jako ostatni parametr Timeout.Infinite
            //Console.ReadKey();
            //tmr.Dispose();

            //System.Timers, The best
            Timer tmr = new Timer();
            tmr.Interval = 500;
            tmr.Elapsed += tmr_Elapsed;
            tmr.Start();
            Console.ReadLine();
            tmr.Stop();
            Console.ReadLine();
            tmr.Start();
            Console.ReadLine();
            tmr.Dispose();

        }

        //pamięc lokalna watku zle działa z asynchronicznymi operacjami
        class Foo
        {
            //pamięc lokalna wątku dla pól statycznych i egzemplarzowych
            static ThreadLocal<int> _x = new ThreadLocal<int>(()=>3);


            //osobna kopia dla kazdego wątku i bezpieczne losowanie z innymi ziarnami
            ThreadLocal<Random> localRandom = new ThreadLocal<Random>(()=>new Random(Guid.NewGuid().GetHashCode())); 

            static void Print()
            {
                Console.WriteLine(_x.Value);
            }
        }
        class Test2
        {
            private LocalDataStoreSlot _secSlot = Thread.GetNamedDataSlot("securityLevel");
            //ta własnosć w każdym wątku ma inną wartośc
            private int SecurityLevel
            {
                get
                {
                    object data = Thread.GetData(_secSlot);
                    return data == null ? 0 : (int) data;
                }
                set
                {
                    Thread.SetData(_secSlot,value);
                }
            }
        }

        static void Tick(object data)
        {
            //działa w wątku z puli
            Console.WriteLine(data);
        }

        static void tmr_Elapsed(object sender, EventArgs e)
        {
            Console.WriteLine("Tik");
        }
    }
}
