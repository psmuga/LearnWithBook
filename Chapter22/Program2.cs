using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter22
{
    class Program2
    {
        static EventWaitHandle _waitHandle = new AutoResetEvent(false);
        static EventWaitHandle _go = new AutoResetEvent(false);
        static readonly object _locker = new object();
        private static string _message;

        static CountdownEvent _countdown = new CountdownEvent(3);

        public static void Test()
        {
            ////kosntrukcje sygnalizujące
            //new Thread(Waiter).Start();
            //Thread.Sleep(5000);
            //_waitHandle.Set();//wysłąnie sygnału

            ////sygnały dwustronne
            //new Thread(Work).Start();
            //_waitHandle.WaitOne();
            //lock (_locker)
            //{
            //    _message = "ooo";
            //}
            //_go.Set();

            //_waitHandle.WaitOne();
            //lock (_locker)
            //{
            //    _message = "aah";
            //}
            //_go.Set();

            //_waitHandle.WaitOne();
            //lock (_locker)
            //{
            //    _message = null;//sygnał zamkniecia wątku robocego
            //}
            //_go.Set();


            //manualresetEvent chapter14

            ////coutdownevent, czeka na wiecej niż jeden wątek
            //new Thread(SaySomething).Start("jestem wątek 1");
            //new Thread(SaySomething).Start("jestem wątek 2");
            //new Thread(SaySomething).Start("jestem wątek 3");
            //_countdown.Wait();
            //Console.WriteLine("Wszystkie wątki zakończyły działanie");

            //uchwyty oczekiwania i kontynuacja
            //dzięki metodzie rozszerzajacej możemy wykonac coś takiego:
            //myWaitHandle.ToTask().ContinueWith(...) lub na niego poczekac
            //await myWaitHandle.ToTask()
            //albo jeszcze okreslic limit czasu if(!await(myWaitHandle.ToTask(5000)))Console.WriteLine("Upłynął limit");


            //WaitHandle.WaitAll();
            //WaitHandle.WaitAny();
            //WaitHandle.SignalAndWait();

            //oraz jesli trzeba atomowości to wait i pulse z klasy Monitor
            //http://www.albahari.com/threading/ 


        }

        static void Waiter()
        {
            Console.WriteLine("Czekanie ...");
            _waitHandle.WaitOne();//czekanie na sygnał
            //_waitHandle.WaitOne(3000);//czekanie na sygnał przez 3 sekundy, jesli nie dostanie sygnalu zwróci false
            Console.WriteLine("Jest sygnał");

            //_waitHandle.Reset();//zamknięcie "bramki"
            //_waitHandle.Close();//zwolnienie zasobów gdy uchwyt nie jest już potrzebny
        }

        static void Work()
        {
            while (true)
            {
                _waitHandle.Set();
                _go.WaitOne();
                lock (_locker)
                {
                    if(_message == null) return;
                    Console.WriteLine(_message);
                }
            }
        }

        static void SaySomething(object thing)
        {
            Thread.Sleep(3000);
            Console.WriteLine(thing);
            _countdown.Signal();//zmniejszenie licznika o 1
        }


    }
}
