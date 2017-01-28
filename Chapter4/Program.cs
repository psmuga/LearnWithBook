using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter4
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] val = {1, 2, 3};
            Util.Transform(val,Square);
            //foreach (var i in val)
            //{
            //    Console.WriteLine(i);
            //}

            ProgressReporter p = WriteProgressToConsole;
            Util.DoHardWork(p);


            Console.ReadKey();
        }

        static void WriteProgressToConsole(int percentComplete) => Console.WriteLine(percentComplete);
        static int Square(int x) => x * x;
    }
}
