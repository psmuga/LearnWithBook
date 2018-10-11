using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter23
{
    public static class Extensions
    {
        public static void IgnoreExceptions(this Task task)
        {
            //można udoskonalić poprzez zapis np informacji o błezie w logu
            task.ContinueWith(t =>
            {
                foreach (var exception in t.Exception.InnerExceptions)
                {
                    Console.WriteLine(exception.Message);
                }
            }, TaskContinuationOptions.OnlyOnFaulted );
        }
    }
}
