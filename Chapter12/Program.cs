using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter12
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var p = new Program();


            //żużycie pamięci przez ten process
            var procName = Process.GetCurrentProcess().ProcessName;
            using (var pc = new PerformanceCounter("Process", "Private Bytes", procName))
            {
                Console.WriteLine(pc.NextValue());
            }
            GCSettings.LatencyMode = GCLatencyMode.LowLatency;
    
            p.funkcja();
            GC.Collect();
            Thread.Sleep(1000);
            foreach (var VARIABLE in TempFileRef._failedDeletions)
                Console.WriteLine(VARIABLE.DeletionError);


            //słabe odwołania str 507



            Console.ReadKey();
        }

        private void funkcja()
        {
            var a = new TempFileRef("ala.txt");
            var b = new TempFileRef("ala.txt");
            Console.WriteLine(a.FilePath);
            Console.WriteLine(b.FilePath);
            GC.Collect();
        }


        //w zdarzeniach  można uzyc IDisposable w celu usunięcia subskrybcji aby uniknąć wycieku pamięci
    }
}
