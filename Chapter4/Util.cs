using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter4
{
    //public delegate int Transformer(int x);

    public delegate void ProgressReporter(int percentComplete);

    public delegate T Transformer<T>(T arg);
    class Util
    {
        //public static void Transform(int[] values, Transformer t)
        //{
        //    for (var i = 0; i < values.Length; i++)
        //    {
        //        values[i] = t(values[i]);
        //    }
        //}

        public static void DoHardWork(ProgressReporter p)
        {
            for (int i = 0; i <= 10; i++)
            {
                p(i * 10);//wywołanie delegatu
                System.Threading.Thread.Sleep(100);
            }
        }
        public static void Transform<T>(T[] values, Transformer<T> t)
        {
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = t(values[i]);
            }
        }
    }
}
