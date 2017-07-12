using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter22
{
    class Program3
    {
        //static Barrier _barrier = new Barrier(3);//jest szybka , doprowadza do spotkania w czasie wątków
        static Barrier _barrier = new Barrier(3,x=>Console.WriteLine());//definuje w postaci delegatu czynnosć do wykonania po każdej fazie


        public static void Test()
        {
            //każdy z 3 wątków drukuje liczby od 0 do 4  pilnująca aby cały czas być na równi z pozostałymi wątkami
            new Thread(Speak).Start();
            new Thread(Speak).Start();
            new Thread(Speak).Start();

            //http://www.albahari.com/threading/part4.aspx#_Nonblocking_Synchronization -> warto poczytac
        }

        static void Speak()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.Write($"{i} ");
                _barrier.SignalAndWait();
            }
        }
    }

    class Expensive
    {
        //powiedzmy że utworzenie tego obiwektu jest kosztowne
    }

    class Foo
    {
        private Expensive _expensive;
        readonly object _expenseLock = new object();

        public Expensive Expensive
        {
            get
            {
                lock (_expenseLock)
                {
                    if(_expensive==null) _expensive = new Expensive();//leniwa inicjalizacja
                    return _expensive;
                }        
            }
        }
    }

    class FooBetter
    {
        //jak da sie true to bedzie bezpieczne wątkowo 
        //gdy bedzie false to będzie dobry dla jednowątkowcyh apek
        Lazy<Expensive> _expensive = new Lazy<Expensive>(() => new Expensive(), true);

        public Expensive Expensive
        {
            get
            {
                return _expensive.Value;
            }
        }
    }

    class Foo3
    {
        private Expensive _expensive;

        public Expensive Expensive
        {
            get
            {
                //troche lepsza wydajnosć niż Lazy<>
                //zapewnia dodatkowy tryb inicjalizacji, w którym kilka wątków może się scigać do inicjalizacji
                LazyInitializer.EnsureInitialized(ref _expensive, () => new Expensive());
                return _expensive;
            }
        }
    }
}
