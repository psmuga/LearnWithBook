#define LOGGINGMODE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Chapter13
{
    class Program
    {
        private static bool EnableLogging=false;
        static void Main(string[] args)
        {

#if TESTMODE
            Console.WriteLine("Inside Test mode");
#endif

            //lepsze rozwiązanie z użyciem zmiennej bool
            //Log(()=>"informacja"); jesli Enablelogging jest false to metoda nie zostanie uruchomiona

            //Debug.WriteLine("Write in debug display");
            // Debug.Fail("brak pliku");//wypisanie w output i w konsoli//wyrzuca wyjatek w oknie dialogowym

            //alternatywa do [Conditional] jest z użyciem zmiennej EnableLogging





            //Debug.Assert(File.Exists("plik.xml"),"brak pliku plik.xml");


            //KlasaTraceListener();

            //Debug.Fail("brak");

            //LogStatus("dupa");

            //SetProgress("dupa",100);

            //InvariantClass test = new InvariantClass();
            //test.Test();






            ////Klasa Process jest niedostępna dla aplikacji do sklepu Windows Store
            //foreach (var p in Process.GetProcesses() )
            //{
            //    using (p)
            //    {
            //        Console.WriteLine(p.ProcessName + "\tPID: "+p.Id + "\tPamięć: " + p.WorkingSet64 + "\tWątki: " + p.Threads.Count);
            //    }
            //}
            ////wywołanie mmetody kill() zakończy process

            //EnumerateThreads(Process.GetCurrentProcess());

            //str 540
            //dla systemów stacjonarnych
            //StackTrace s = new StackTrace(true);
            //Console.WriteLine(s.ToString());
            //A();

            //Liczniki wydajności str 544
            //LW_SprawdzanieDostepnychLicznikow();
            //LW_LicznikWydajnosciNET();


            ////wyswietlneie poziomu wykorzystania wszystkicj procesorów
            //using (PerformanceCounter pc = new PerformanceCounter("Processor", "% Processor Time", "_Total"))
            //    Console.WriteLine(pc.NextValue());

            ////wyswietlenie rzeczywisty prywatny poziom użycia pamięci dla biezacego processu
            //string procName = Process.GetCurrentProcess().ProcessName;
            //using (PerformanceCounter pc = new PerformanceCounter("Process","Private Bytes","_Total"))
            //{
            //    Console.WriteLine(pc.NextValue());
            //}


            //EventWaitHandle stopper = new ManualResetEvent(false);
            //new Thread(()=>
            //    Monitor("Processor","% Processor Time","_Total",stopper)
            //).Start();
            //new Thread(() =>
            //    Monitor("LogicalDisk","% Idle Time","C:",stopper)
            //).Start();
            //Console.WriteLine("Monitorowanie - nacisnij dowolny klawisz aby zakonczyc ...");
            //Console.ReadKey();
            //stopper.Set();



            Console.ReadKey();
            Stopwatch s = Stopwatch.StartNew();
            Console.ReadKey();
            Console.WriteLine(s.Elapsed);
            Console.WriteLine(s.ElapsedMilliseconds);




            Console.ReadKey();
        }

        static void Monitor(string category, string counter, string instance, EventWaitHandle stopper)
        {
            if(!PerformanceCounterCategory.Exists(category))
                throw new InvalidOperationException("Kategoria nie istnieje");
            if(!PerformanceCounterCategory.CounterExists(counter,category))
                throw new InvalidOperationException("Licznik nie istnieje");
            if (instance == null) instance = "";
            if(instance!= "" && !PerformanceCounterCategory.InstanceExists(instance,category))
                throw new InvalidOperationException("Egzemplarz nie istnieje.");
            float lastValue = 0f;
            using (PerformanceCounter pc = new PerformanceCounter(category,counter,instance))
            {
                while (!stopper.WaitOne(200,false))
                {
                    float value = pc.NextValue();
                    if (!value.Equals(lastValue))
                    {
                        Console.WriteLine(category +": " +value);
                        lastValue = value;
                    }
                }
            }
        }
        static void LW_LicznikWydajnosciNET()
        {
            var x = new XElement("counters", 
                from PerformanceCounterCategory cat in PerformanceCounterCategory.GetCategories()
                where cat.CategoryName.StartsWith(".NET")
                let instances = cat.GetInstanceNames()
                select new XElement("category",
                    new XAttribute("name",cat.CategoryName),
                        instances.Length==0
                        ?
                        from c in cat.GetCounters()
                        select new XElement("counter",
                             new XAttribute("name",c.CounterName))
                        :
                        from i in instances
                        select     new XElement("instance", new XAttribute("name", i ),
                            !cat.InstanceExists(i)
                            ?
                            null
                            :
                            from c in cat.GetCounters(i)
                            select new XElement("counter",
                            new XAttribute("name",c.CounterName))
                    )
                )
            );
            Console.WriteLine(x);
        }
        static void LW_SprawdzanieDostepnychLicznikow()
        {
            PerformanceCounterCategory[] cats = PerformanceCounterCategory.GetCategories();
            foreach (var cat in cats)
            {
                Console.WriteLine("Kategoria: " + cat.CategoryName);
                string[] instances = cat.GetInstanceNames();
                if (instances.Length == 0)
                {
                    foreach (var ctr in cat.GetCounters())
                    {
                        Console.WriteLine(" Licznik: " + ctr.CounterName);
                    }
                }
                else
                {
                    foreach (var instance in instances)
                    {
                        Console.WriteLine(" Egzemplarz: " + instance);
                        if(cat.InstanceExists(instance))
                            foreach (var ctr in cat.GetCounters(instance))
                            {
                                Console.WriteLine("   Licznik: " + ctr.CounterName);
                            }
                    }
                }
            }
        }
        static void A()
        {
            B();
        }

        static void B()
        {
            C();
        }

        static void C()
        {
            StackTrace s = new StackTrace(true);
            Console.WriteLine("Całkowita liczba ramek: " + s.FrameCount);
            Console.WriteLine("Bieżąca metoda: " + s.GetFrame(0).GetMethod().Name);
            Console.WriteLine("Metoda wywołująca: " + s.GetFrame(1).GetMethod().Name);
            Console.WriteLine("Metoda wejściowa: " +  s.GetFrame(s.FrameCount-1).GetMethod().Name);
            Console.WriteLine("Stos wywołań: ");
            foreach (var frame in s.GetFrames())
            {
                Console.WriteLine("Plik: " + frame.GetFileName() + " Wiersz: " + frame.GetFileLineNumber() + " Kolumna: " + frame.GetFileColumnNumber() + " Przesunięcie: " + frame.GetILOffset() + " Metoda: " + frame.GetMethod().Name);
            }
        }

        public static void EnumerateThreads(Process p)
        {
            foreach (ProcessThread pt in p.Threads)
            {
                Console.WriteLine(pt.Id);
                Console.WriteLine("\tState:    " + pt.ThreadState);
                Console.WriteLine("\tPriority: " + pt.PriorityLevel);
                Console.WriteLine("\tStarted:  " + pt.StartTime);
                Console.WriteLine("\tCPU time: " + pt.TotalProcessorTime);
            }
        }

        //będzie może wywolać tylko w trybie LOGGINGMODE
        [Conditional("LOGGINGMODE")]
        static void LogStatus(string msg)
        {
            Console.WriteLine("Write to file msg");
            File.AppendAllText("text.txt","tett");
        }

        static void Log(Func<string> message)
        {
            if (EnableLogging)
            {
                //do sth, for example write to file
            }
        }

        public static void KlasaTraceListener()
        {
            //default listeners wyrzuca wyjatki w oknie dialogowym
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new TextWriterTraceListener("trace.txt"));//do pliku
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));//na konsole

            
            Trace.AutoFlush = true;
            Debug.AutoFlush = true;

        }

        //do łapania kontraktów potrzeba zainstalwoac programy odpowiednie
        //https://visualstudiogallery.msdn.microsoft.com/1ec7db13-3363-46c9-851f-1ce455f66970
        //wszysstkei kontrakty na początku funkcji
        //requires warunki początkowe - nie mogą zmieniac nic w zmiennych lokalnych funkcji
        //ensures warunki koncowe - mogą odpwoływac się do zmiennych w funkcji bo wykonuja się na koniec
        //assert moze być wywoływane w całej funkcji
        [Pure]
        public static bool AddIfNotPresent<T>(IList<T> list, T item )
        {
            //kontrakty kodu
            Contract.Requires(list!=null);
            Contract.Requires(!list.IsReadOnly);
            Contract.Requires(item!=null);
            Contract.Ensures(list.Contains(item));//warunek końcowy
            if (list.Contains(item)) return false;
            list.Add(item);
            return true;
        }

        //Contract.ValueAtReturn<T>
        //Contract.OldValue<T>
        
        static void SetProgress(string message, int percentage)
        {

            Contract.ContractFailed += Anonimowa;
            //Contract.ContractFailed += (sender, args) =>
            //{
                
            //    string messgae = args.FailureKind + ": " + args.Message;
            //    Console.WriteLine(messgae);
            //    args.SetUnwind();
            //};
            Contract.Requires(message!=null);
            Contract.Requires(percentage>=0 && percentage <= 100);
            
            //do sth with data
        }
        [Pure]
        private static void Anonimowa(object sender, ContractFailedEventArgs e)
        {
            string messgae = e.FailureKind + ": " + e.Message;
            Console.WriteLine(messgae);
            e.SetUnwind();
        }

        static void Foo(string name)
        {
            //Contract.Requires<ArgumentNullException>(name!=null);
            //if(name == null) throw new ArgumentNullException(nameof(name));
            Contract.Requires(name.Length>0); // kontrakty po rzucaniu wyjątków lepiej jest dawć
            //do sth with data

            Contract.Assume(name.Equals("Piotr"));//niepowodzenie jeśli name nie równa się Piotr
            Contract.Assert(name.Equals("Piotr"),"name musi wynosić Piotr");


        }

    }
}
