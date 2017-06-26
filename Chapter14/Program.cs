using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Chapter14.MyExtensions;
using ThreadState = System.Threading.ThreadState;

namespace Chapter14
{
    class Program
    {
        static bool _done;
        static readonly object Locker = new object();
        static void Main(string[] args)
        {
            var p = new Program();
            p.Test9();

            Console.ReadKey();
        }
        //dla sklepu Windows Store nie możne aroibć wątków ale można zadania

        private void Test1()
        {

            var t = new Thread(WriteY);
            t.Start();
            for (var i = 0; i < 1000; i++)
            {
                Console.Write("X");
                //Thread.Sleep(1);
            }
        }

        private static void WriteY()
        {
            Thread.CurrentThread.Name = "wow";//można tylko raz zmienic nazwe watku
            //Console.WriteLine(Thread.CurrentThread.Name);
            for (var i = 0; i < 1000; i++)
            {
                Console.Write("Y");
                //Thread.Sleep(1);
            }
        }

        private void Test2()
        {
            var t = new Thread(WriteY);
            t.Start();
            //t.Join();
            if (t.Join(20))//join i czas oznajmia ze daje watkowi 20ms na wykonywanie a póżniej powraca do glownego
            {
                Console.WriteLine("zakonczyl juz");
                Thread.Yield();
                //jesli spowoduje awarie to oznadcza istnienie bledu w kodzie
                //Thread.Yield oddaje wątek z powrotem do CPU. Wywołanie mówi, że nie mam nic więcej do roboty i
                //jeśli CPU ma coś lepszego do zrobienia to niech to zrobi a po tym dopiero wątek macierzysty zostanie wznowiony
                //sleep(0) i Yield  znacz to samo ale yield dotyczy watków w tym samym processorze/rdzeniu,sleep(0) wolnioejszy
                //yield zwraca true jesli znaleziono kandytata któremu trzeba zrobic miejsce w procesorze
            }
            else
            {
                Console.WriteLine("nie zakonczył jeszcze");
            }

            Console.WriteLine("Watek t został zakończony");

        }

        private void Test3()
        {
            bool blocked = (Thread.CurrentThread.ThreadState & ThreadState.WaitSleepJoin) != 0;//sprawdzenie czy wątek jest zablokowany
                                                                                               //można stosowac w celach diagnostycnych nie zaleca sie tego wykorzystywac do kodu if...


            new Thread(this.Go2).Start();
            Go2();//tutaj wspodziela zmienna done
            //statyczne zmienne też są wpołdzielone tak samo jak zmienne lokalne przechwycone przez wyrażenie lambda lub dlegaty
            //nei stosowac tego , lepiej używac blokady a jeszcze lepiej wzorce asynchroniczne

            Thread t = new Thread(()=>Print("Hi from t!"));
            t.Start();


            for (int i = 0 ; i < 10; i++)
            {
                new Thread(()=>Console.Write(i)).Start();
            }//nie derministyczne bo w każdym wypadku odwołuje sie do tego samego miejsca w pamięci
            //dlateo lepiej zastosowac zmienna lokalna
            Thread.Sleep(1);
            Console.WriteLine(Environment.NewLine);
            for (int i = 0; i < 10; i++)
            {
                int temp = i;
                new Thread(() => Console.Write(temp)).Start();
            } //liczby tylko po razie sie wyswietla ale nie wiadomo czy w dobrej kolejnosci bo watki uruchamikane są w roznym czasie

        }

        private void Test4()
        {
            //obsługa błedów do danych wejsciowych wątku musi być obsłuzona w wątku głównym

            //takie podniesienie prioytetu warto robic dla watku z obliczeniami
            //using (Process p = Process.GetCurrentProcess())
            //    p.PriorityClass = ProcessPriorityClass.High;

            var signal = new ManualResetEvent(false);
            new Thread(() =>
            {
                Console.WriteLine("Oczekiwanie na sygnał...");
                signal.WaitOne();
                signal.Dispose();
                Console.WriteLine("Otrzymano sygnał");

            }).Start();
            Thread.Sleep(2000);
            signal.Set();//sygnał otwierajacy
            //sygnal mozna zamknac rpzez polecenie Reset()

        }

        SynchronizationContext _synchronizationContext;
        private void Test5()
        {
            _synchronizationContext = SynchronizationContext.Current;
            //poczytac o SynchronizationContext
            new Thread(Work).Start();
            


            //pula wątków
            //nie można definiowac nazwy wątku
            //są zawsze wątkami działającymi w tle
            //bolkujące wątki z puli moga zmniejszyć wydajnosć
            Console.WriteLine(Thread.CurrentThread.IsThreadPoolThread);
            Task.Run(() => Console.WriteLine("Welcome in ThreadPool"));
        }

        void Work()
        {
            Thread.Sleep(4000);
            //wywyolaj cos na interfejsie jako result
            Print("Odpowiedz");
        }

        void Go()
        {
            if (!_done)
            {
                _done = true;
                Console.WriteLine("Gotowe");
            }
        }

        void Go2()
        {
            //bezpieczeństwo watków
            lock (Locker)
            {
                if (!_done)
                {
                    _done = true;
                    Console.WriteLine("Gotowe");
                }
            }
        }

        void Print(string msg)
        {
            //Console.WriteLine(msg);
            //dlat test5
            Action action = () => Console.WriteLine(msg);
            action.Invoke();
            //Dispatcher.BeginInvoke(action);//Wpf
            //this.BeginInvoke(action);//winforms

        }

        void Test6()
        {
            Task task = Task.Run(() =>
            {
                Thread.Sleep(2000);
                Console.WriteLine("Foo");

            });
            Console.WriteLine(task.IsCompleted);
            task.Wait();//podowbne do join w watku możńa podać czas 
           //taski sa dobre dla nie długim obliczeń
            Task task2 = Task.Factory.StartNew(() => Console.WriteLine("Hi"), TaskCreationOptions.LongRunning);//dla jakiegoś jednego taska który dłuzej może podziałas

            Task<int> task3 = Task.Run(() =>
            {
                Console.WriteLine("Hi 3");
                return 3;
            });
            int result = task3.Result;//blokuje watek jeśli zadanie nie zostało wykonane

            Task<int> primeNumberTask = Task.Run(() =>
            {
                return Enumerable.Range(2, 3000000)
                    .Count(n => Enumerable.Range(2, (int) Math.Sqrt(n) - 1).All(i => n % i > 0));
            });
            Console.WriteLine("Zadanie jest wykonywane...");
            Console.WriteLine("Liczb pierwszych w przedziale od 2 do 300000 jest: " + primeNumberTask.Result);
            //w taskach wyjatki sa propagowane i opakowywane jako AggregatException
            //jesli isCacellerd ==true to rzuca wyjątek OperationCanceledExeption

            Task<int> primeNumberTask2 = Task.Run(() =>
            {
                return Enumerable.Range(2, 3000000)
                    .Count(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0));
            });
            var awaiter = primeNumberTask2.GetAwaiter();//tutaj moze rzucic wyjątek ale i tak wynik zostanie pobrany z ostaniego zadania
            awaiter.OnCompleted(() =>
            {
                int result2 = awaiter.GetResult();
                Console.WriteLine(result2);
            });

            //kontynuacja

            //tutaj nie rzuca wyjatkami 
            var awaiter2 = primeNumberTask2.ConfigureAwait(false).GetAwaiter();//kontynuacja będzie wykonywana w tym samym watku co poprzednie zadanie co unika niepotrzebnego obiciazenia

            
            primeNumberTask2.ContinueWith(x =>
            {
                int result3 = x.Result;
                Console.WriteLine(result3);
                
            });


        }

        async void Test7()
        {
            //ta klasa dobra ejst do operacji wejscia wyjścia
            //ręcznie można wskazac kiedy operacja sie koćńzy lub ulega awarii
            var tcs = new TaskCompletionSource<int>();
            new Thread(() =>
            {
                Thread.Sleep(5000);
                tcs.SetResult(42);
                
            })
            { IsBackground = true}.Start();

            //metoda Task.Deley to asynchroonicznyodpowiednik Thread.sleep()
            //Task.Delay(5000).ContinueWith((x) => Console.WriteLine(42));

            Task<int> task = tcs.Task;//zadanie'podległe'
                                      //Console.WriteLine(task.Result);

            await DisplayPrimeCountsAsync();

        }

        Task<int> GetPrimesCountAsync(int start, int count)
        {
            return Task.Run(() => ParallelEnumerable.Range(start, count)
                .Count(n => Enumerable.Range(2, (int) Math.Sqrt(n) - 1).All(i => n % i > 0)));
        }

        void DisplayPrimeCount()
        {
            for (int i = 0; i < 10; i++)
            {
                var awaiter = GetPrimesCountAsync(i * 1000000 + 2, 1000000).GetAwaiter();
                awaiter.OnCompleted(()=>Console.WriteLine(awaiter.GetResult()+" liczb pierwszych między ..."));
            }
            Console.WriteLine("Gotowe");
        }

        async Task DisplayPrimeCountsAsync()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(await GetPrimesCountAsync(i * 1000000 + 2, 1000000) +
                                  $" liczb pierwszych miedzy {1 * 1000000} i {(i + 1) * 1000000 - 1}");
            }
            Console.WriteLine("Gotowe");
        }


        //Task gdy normalnie funkcja zwracałaby void
        async Task Test8()
        {
            //await Task.Delay(5000);
            //Console.WriteLine("Upłynęło 5 sekund");

            //w np wpf funkcja po nacisnieciu przycisku, najpierw zablokowac button wykonac await i odblkowac button: button.isEnabled =true;
            string[] urls = "www.albahari.com www.oreilly.com www.linqpad.net www.google.com www.formula1.pl".Split();
            int totalLength = 0;
            try
            {
                foreach (var url in urls)
                {
                    var uri = new Uri("http://" + url);
                    byte[] data = await new WebClient().DownloadDataTaskAsync(uri);
                    totalLength += data.Length;
                    Console.WriteLine($"Wielkość adresu {url} wynosi {data.Length}");
                }
                Console.WriteLine($"Całkowita wielkosć: {totalLength}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                //tutaj button isEnabled = true
            }

            //funkcja anonimowa lub wyrażenie lambda
            Func<Task> unnamed = async () =>
            {
                await Task.Delay(1000);
                Console.WriteLine("Foo");
            };
            await unnamed();
            //może rówież zwracać wartośc
            Func<Task<int>> unnamed2 = async () =>
            {
                await Task.Delay(1000);
                return 123;
            };
            int wynik = await unnamed2();


            //asynchroniczne wyrażenia lambda mogą być  używane podczas dołączania procedur obsługi zdarzeń
            //button.Click += async (Sender, args) =>
            //{
            //    await Task.Delay(1000);
            //    button.Content = "Gotowe";
            //}

            //Ukończenie synchroniczne
            //await GetWebPageAsync("http://google.pl");

        }
        static Dictionary<string,string> _cache = new Dictionary<string, string>();

        async Task<string> GetWebPageAsync(string uri)
        {
            string html;
            if (_cache.TryGetValue(uri, out html)) return html;
            return _cache[uri] = await new WebClient().DownloadStringTaskAsync(uri);
        }

        async Task Test9()
        {
            // WZORCE ASYNCHRONICZNOSCI
            //var cancelationTokenSource = new CancellationTokenSource();
            //var foo = Foo(cancelationTokenSource.Token);

            //jakis czas pozniej
            //Task.Delay(5000).ContinueWith(ant => cancelationTokenSource.Cancel());

            //var cancel = new CancellationTokenSource(5000);//czas po którym zadanie zostanie anulowane
            //try
            //{
            //    await Foo(cancel.Token);
            //}
            //catch (OperationCanceledException e)
            //{
            //    Console.WriteLine("Anulowano!");
            //}
            ////cancel.Token.Register(/* Action który wykona sie po anulowaniu */ );

            ////Progress<T>
            //var progress = new Progress<int>(i => Console.WriteLine($"{i} %"));
            //await Foo2(progress);

            //Task<int> winningTask = await Task.WhenAny(delay1(), delay2(), delay3());//whenany zwraca zadanei zakończone jako pierwsze
            //Console.WriteLine("Gotowe");
            ////Console.WriteLine(winningTask.Result);
            //Console.WriteLine(await winningTask); //lepsze rozwiazanie niz .Result
            ////to samo  w jednej lini
            //Console.WriteLine(await await Task.WhenAny(delay1(), delay2()));


            //Task<int> task = delay3();
            //Task winner = await Task.WhenAny(task, Task.Delay(5000));//zwraca Task gdy taski o róznych typach, zamaist task<int>
            //if(winner!=task ) Console.WriteLine("Zglosc wyjatek TimeoutException");
            //int result = await task;

            //Task task1 = delay1();
            //Task task2 = delay2();
            //Task all = Task.WhenAll(task1, task2);//zwraca zadanie które zostanie uznane za ukończone po zakończeniu wszystkich zadań do wykonania 
            //try
            //{
            //    await all;
            //}
            //catch 
            //{
            //    Console.WriteLine(all.Exception.InnerExceptions.Count);
            //    //jak jedne z task1 lub task2 rzuci wyjatek to tuaj wyswietli ile dokładnie ich był
            //    throw;
            //}

            int[] res = await Task.WhenAll(delay1(), delay2());//zwaraca sume z wszystkich wywolanych taskó
            Task<int> test = delay1().WithTimeOut(500);
            try
            {
                await test;

            }
            catch 
            {
                Console.WriteLine("timeout");
            }
        }


        async Task Foo(CancellationToken cancellationToken)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(i);
                await Task.Delay(1000, cancellationToken);//nie ma potrzeby ThrowIFCancel.. bo delay robi to za nas
                //cancellationToken.ThrowIfCancellationRequested();
            }
        }
        async Task Foo2(IProgress<int> onProgressPercentageChanged)
        {
            for (int i = 0; i <= 1000; i++)
            {
                //Console.WriteLine(i);
                if(i%10 ==0) onProgressPercentageChanged.Report(i/10);
                await Task.Delay(1);
                
            }
        }
        async Task<int> delay1()
        {
            await Task.Delay(1000);
            //ten throw do Test9 Whenall
            //throw null;
            return 1;
        }
        async Task<int> delay2()
        {
            await Task.Delay(2000);
            return 2;
        }
        async Task<int> delay3()
        {
            await Task.Delay(3000);
            return 3;
        }

        //praktyczny przykłąd z  użyciem WhenAll
        async Task<int> GetTotalSize(string[] uris)
        {
            //IEnumerable<Task<byte[]>> downloadTasks = uris.Select(uri => new WebClient().DownloadDataTaskAsync(uri));
            //byte[][] contents = await Task.WhenAll(downloadTasks);
            //return contents.Sum(c => c.Length);
            IEnumerable<Task<int>> downloadTasks =
                uris.Select(async uri => (await new WebClient().DownloadDataTaskAsync(uri)).Length);
            int[] content = await Task.WhenAll(downloadTasks);
            return content.Sum();
        }
        
        
    }
}
