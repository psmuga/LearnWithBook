using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Chapter23
{
    class Program3
    {
        public static void Test()
        {
            //var task = Task.Factory.StartNew(state=>Greet("Witaj!"),"Powitanie");//gdzies w oknie debugowania ta nazwa Powitanie bedzie widoczna jako nazwa taska
            //Console.WriteLine(task.AsyncState);
            //task.Wait();

            //Task parent = Task.Factory.StartNew(() =>
            //{
            //    Console.WriteLine("Jestem rodzicem");
            //    Task.Factory.StartNew(() =>
            //    {
            //        Console.WriteLine("Nie jestem połączony");
            //    });
            //    Task.Factory.StartNew(() =>
            //    {
            //        Console.WriteLine("Jestem dzieckiem");
            //    }, TaskCreationOptions.AttachedToParent);
            //});


            ////następuje propagacja wyjatków
            //TaskCreationOptions atp = TaskCreationOptions.AttachedToParent;
            //var parent2 = Task.Factory.StartNew(() =>
            //{
            //    Task.Factory.StartNew(() =>
            //    {
            //        Task.Factory.StartNew(() =>
            //        {
            //            throw null;
            //        }, atp);//wnuk
            //    }, atp);//dziecko
            //});



            //var  cts = new CancellationTokenSource();
            //CancellationToken token = cts.Token;

            //Task task = Task.Factory.StartNew(() =>
            //{

            //    Thread.Sleep(1000);
            //    if(cts.IsCancellationRequested)
            //        token.ThrowIfCancellationRequested();
            //}, token);

            //task.ContinueWith(ant => Console.WriteLine("... kontynuacja"));//ant jest referencja do task 
            //task.ContinueWith(ant => Console.WriteLine("... kontynuacja 2"),
            //    TaskContinuationOptions.ExecuteSynchronously);//gdy mała akcja kontynuacji, to może poprawic wydajnosć bo działa na tym samym zadaniu 

            //cts.Cancel();
            //try
            //{

            //    task.Wait();
            //}
            //catch (AggregateException e)
            //{
            //    Console.WriteLine(e.InnerException is TaskCanceledException);
            //    Console.WriteLine(task.IsCanceled);
            //    Console.WriteLine(task.Status);
            //}

            ////w rzecywistości każda kontynuacja obejmowałaby duże oblcizenia
            //Task.Factory.StartNew<int>(() => 8)
            //    .ContinueWith(ant => ant.Result * 2)
            //    .ContinueWith(ant => Math.Sqrt(ant.Result))
            //    .ContinueWith(ant => Console.WriteLine(ant.Result));


            //ponizsze sposoby coś kurwa nie działaja 4
            ////sp1
            //Task continuation = Task.Factory.StartNew(() => throw null).ContinueWith(ant =>
            //{
            //    try
            //    {
            //        ant.Wait();

            //    }
            //    catch (AggregateException e)
            //    {
            //        foreach (var ex in e.InnerExceptions)
            //        {
            //            Console.WriteLine(ex.Message);
            //        }
            //    }   
            //    Console.WriteLine("jestem tutaj");
            //      //kontynuuj przetwarzanie
            //});
            //continuation.Wait();//wyjatek nie zostanie zgłoszony do wywołujacego

            //sp2
            //Task task1 = Task.Factory.StartNew(() => { throw null; });
            //Task error = task1.ContinueWith(ant =>
            //{
            //    foreach (var exception in ant.Exception.InnerExceptions)
            //    {
            //        Console.WriteLine(exception.Message);
            //    }
            //}, TaskContinuationOptions.OnlyOnFaulted);
            //Task ok = task1.ContinueWith(ant => Console.Write("Sukcess"), TaskContinuationOptions.NotOnFaulted);

            //sp3
            //Task.Factory.StartNew(() => { throw new Exception(); }).IgnoreExceptions();
            Console.WriteLine("here");

            ////sp4 z neta
            //int i = 1, j = 0;
            //Task.Factory.StartNew<int>(() =>
            //{
            //    return i/j;
            //}).ContinueWith((t) =>
            //{
            //    if (t.IsFaulted)
            //    {
            //        // faulted with exception
            //        Exception ex = t.Exception;
            //        while (ex is AggregateException && ex.InnerException != null)
            //            ex = ex.InnerException;
            //        Console.WriteLine("Error: " + ex.Message);
            //    }
            //    else if (t.IsCanceled)
            //    {
            //        // this should not happen 
            //        // as you don't pass a CancellationToken into your task
            //        Console.WriteLine("Cancelled.");
            //    }
            //    else
            //    {
            //        // completed successfully
            //        Console.WriteLine("Result: " + t.Result);
            //    }
            //});

            //kolejne nei działa
            //TaskCreationOptions atp = TaskCreationOptions.AttachedToParent;
            //Task.Factory.StartNew(() =>
            //{
            //    Task.Factory.StartNew(() => { throw null; }, atp);
            //    Task.Factory.StartNew(() => { throw null; }, atp);
            //    Task.Factory.StartNew(() => { throw null; }, atp);
            //}).ContinueWith(p => Console.WriteLine(p.Exception), TaskContinuationOptions.OnlyOnFaulted);


            //ConcurrentBag<>

            //BlockingCollenction

            var pcQ = new PCQueue(1);
            Task task = pcQ.EnqueueTask(() => Console.WriteLine("Easy!"));

        }

    

        static void Greet(string message)
        {
            Console.WriteLine(message);
        }
    }

    class PCQueue1:IDisposable
    {
        BlockingCollection<Action> _taskQ = new BlockingCollection<Action>();//domyślnie jest wybierana kolejka ale można do konstruktora podać obiekt np COncurrentStack, otrzyamlibyśmy stos

        public PCQueue1(int workerCount)
        {
            //utworzenie i uruchomienie osobnego zadania dla każdego konsumenta
            for (int index = 0; index < workerCount; index++)
            {
                Task.Factory.StartNew(Consume);
            }
        }

        public void Enqueue(Action action)
        {
            _taskQ.Add(action);
        }

        void Consume()
        {
            //sekwencja którą przeglądamy zablokuje się jesni nie będzie dostępnych elementów i skończy sięw chwili, gdy wywołamy metodę CompleteAdding
            foreach (var action in _taskQ.GetConsumingEnumerable())
            {
                action();
            }
        }
        public void Dispose()
        {
            _taskQ.CompleteAdding();
        }
    }

    public class PCQueue : IDisposable
    {
        class WorkItem
        {
            public readonly TaskCompletionSource<object> TaskSource;
            public readonly Action Action;
            public readonly CancellationToken? CancelToken;

            public WorkItem(
                TaskCompletionSource<object> taskSource,
                Action action,
                CancellationToken? cancelToken)
            {
                TaskSource = taskSource;
                Action = action;
                CancelToken = cancelToken;
            }
        }

        BlockingCollection<WorkItem> _taskQ = new BlockingCollection<WorkItem>();

        public PCQueue(int workerCount)
        {
            // Create and start a separate Task for each consumer:
            for (int i = 0; i < workerCount; i++)
                Task.Factory.StartNew(Consume);
        }

        public void Dispose() { _taskQ.CompleteAdding(); }

        public Task EnqueueTask(Action action)
        {
            return EnqueueTask(action, null);
        }

        public Task EnqueueTask(Action action, CancellationToken? cancelToken)
        {
            var tcs = new TaskCompletionSource<object>();
            _taskQ.Add(new WorkItem(tcs, action, cancelToken));
            return tcs.Task;
        }

        void Consume()
        {
            foreach (WorkItem workItem in _taskQ.GetConsumingEnumerable())
                if (workItem.CancelToken.HasValue &&
                    workItem.CancelToken.Value.IsCancellationRequested)
                {
                    workItem.TaskSource.SetCanceled();
                }
                else
                    try
                    {
                        workItem.Action();
                        workItem.TaskSource.SetResult(null);   // Indicate completion
                    }
                    catch (OperationCanceledException ex)
                    {
                        if (ex.CancellationToken == workItem.CancelToken)
                            workItem.TaskSource.SetCanceled();
                        else
                            workItem.TaskSource.SetException(ex);
                    }
                    catch (Exception ex)
                    {
                        workItem.TaskSource.SetException(ex);
                    }
        }
    }
}
