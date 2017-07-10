using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chapter18
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(System.Threading.Thread.CurrentThread.CurrentCulture);
            Console.WriteLine(System.Threading.Thread.CurrentThread.CurrentUICulture);

            //aktualnie wczytane w pamięci podzespoły
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Console.WriteLine(assembly.Location);
                Console.WriteLine(assembly.CodeBase);
                Console.WriteLine(assembly.GetName().Name);
            }

            //Assembly a = Assembly.LoadFrom(@"c:\temp\lib.dll");

            //http://www.albahari.com/nutshell/cs4ch17.aspx
            Console.ReadKey();
        }
    }
}
