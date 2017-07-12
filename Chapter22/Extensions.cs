using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter22
{
    public static class Extensions
    {
        public static Task<bool> ToTask(this WaitHandle waitHandle, int timeout = -1)
        {
            //-1 oznacza brak limitu czasu
            var tcs = new TaskCompletionSource<bool>();
            RegisteredWaitHandle token = null;
            var tokenReady = new ManualResetEventSlim();
            token = ThreadPool.RegisterWaitForSingleObject(waitHandle, (state, timeOut) =>
            {
                tokenReady.Wait();
                tokenReady.Dispose();
                token.Unregister(waitHandle);
                tcs.SetResult(!timeOut);
            }, null, timeout, true);
            tokenReady.Set();
            return tcs.Task;
        }
    }
}
