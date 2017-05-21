using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter14
{
    public static class MyExtensions
    {
      
        ////własne łączniki przykaldy
        //Pozwala oczekiwac przez okreslona ilosc czasu na wykonanie
        public static async Task<TResult> WithTimeOut<TResult>(this Task<TResult> task, int timeout)
        {
            Task winner = await Task.WhenAny(task, Task.Delay(timeout));
            if (winner != task) throw new TimeoutException();
            return await task;
        }
        //porzucić  używajac cancellationtoken
        public static Task<TResult> WithCancellation<TResult>(this Task<TResult> task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<TResult>();
            var reg = cancellationToken.Register(() => tcs.TrySetCanceled());
            task.ContinueWith(ant =>
            {
                reg.Dispose();
                if (ant.IsCanceled)
                    tcs.TrySetCanceled();
                else if (ant.IsFaulted)
                    tcs.TrySetException(ant.Exception.InnerExceptions);
                else
                    tcs.TrySetResult(ant.Result);
            });
            return tcs.Task;
        }

       
        public static async Task<TResult[]> WhenAllOrError<TResult>(params Task<TResult>[] tasks)
        {
            var killJoy = new TaskCompletionSource<TResult[]>();
            foreach (var task in tasks)
            {
                await task.ContinueWith(ant =>
                {
                    if (ant.IsCanceled)
                        killJoy.TrySetCanceled();
                    else if (ant.IsFaulted)
                        killJoy.TrySetException(ant.Exception.InnerExceptions);
                });

            }
            return await await Task.WhenAny(killJoy.Task, Task.WhenAll(tasks));
        }
    }
}
