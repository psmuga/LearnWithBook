using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter22
{
    class Program
    {
        private static int _val1 = 1;
        private static int _val2 = 1;
        private static readonly object _locker = new object();


        static List<string> _list = new List<string>();

        static SemaphoreSlim _sem = new SemaphoreSlim(3);//maks 3 wątki mogą wejsc
        //semaphor jeśli ma nazwe to działa jak mutex prawie czyli obejmuje cały komputer


        static ReaderWriterLockSlim _rw = new ReaderWriterLockSlim();
        static Random _rand = new Random();
        static List<int> _items = new List<int>();

        static void Main(string[] args)
        {
            using (var mutex = new Mutex(true,"Piotr OneAtATimeDemo"))
            {
                //nadanie nazwy mutexowi sprawia że staje się dostępny w całym komputerze
                if (!mutex.WaitOne(TimeSpan.FromSeconds(1), false))
                {
                    Console.WriteLine("Egzemlarz tego programu jest juz uruchomiony. Zegnaj!");
                    Console.ReadKey();
                    return;
                }
                try
                {
                    RunProgram();
                }
                finally { mutex.ReleaseMutex();}
            }

            
        }

        static void Go()
        {
            lock (_locker)
            {
                if(_val2!=0 ) Console.WriteLine(_val1 / _val2);
                _val2 = 0;
            }    
        }

        static void RunProgram()
        {
            //new Thread(AddItem).Start();
            //new Thread(AddItem).Start();
            //new Thread(AddItem).Start();
            //new Thread(AddItem).Start();
            
            //for(int i =1; i<=5;i++) new Thread(Enter).Start(i);

            //new Thread(Read).Start();
            //new Thread(Read).Start();
            //new Thread(Read).Start();
            //new Thread(Write2).Start("A");
            //new Thread(Write2).Start("B");
            

            Program4.Test();

            Console.ReadKey();
        }

        static void AddItem()
        {
            lock (_list)
            {
                _list.Add("Item " + _list.Count);
            }
            string[] items;
            lock (_list)
            {
                items = _list.ToArray();
            }
            foreach (var s in items)
            {
                Console.WriteLine(s);
            }
        }

        //blokowanie bez wykluczeń

        static void Enter(object id)
        {
            Console.WriteLine(id + " chce wejść");
            _sem.Wait();

            Console.WriteLine(id + " jest w środku!");
            Thread.Sleep(1000 * (int) id);
            Console.WriteLine(id + " wychodzi");

            _sem.Release();
        }


        //blokady zapisu i odczytu stosowany gdy czesciej odczytujemy niż zapisujemy np w serwerach
        static void Read()
        {
            while (true)
            {
                _rw.EnterReadLock();
                foreach (var i in _items)
                {
                    Thread.Sleep(10);
                }
                _rw.ExitReadLock();
            }
        }
        static void Write(object threadId)
        {
            int i = 10;
            while (i-- > 0)
            {
                int newNumber = GetRandNum(100);
                _rw.EnterWriteLock();
                _items.Add(newNumber);
                _rw.ExitWriteLock();
                Console.WriteLine($"Wątek {threadId} dodał {newNumber}");
                Thread.Sleep(100);
            }
        }
        static int GetRandNum(int max) {
            lock (_rand)
            {
                return _rand.Next(max);
            }
        }

        //blokada z możliwością uaktualnienia
        //np że dodaje tylko wtedy gdy tego elementu nie ma
        static void Write2(object threadId)
        {
            int i = 10;
            while (i-- > 0)
            {
                int newNumber = GetRandNum(100);
                _rw.EnterUpgradeableReadLock();
                if (!_items.Contains(newNumber))
                {
                    _rw.EnterWriteLock();
                    _items.Add(newNumber);
                    _rw.ExitWriteLock();
                    Console.WriteLine($"Wątek {threadId} dodał {newNumber}");
                }
                _rw.ExitUpgradeableReadLock();
                Thread.Sleep(100);
            }
        }
    }
}
