using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chapter4.Event;

namespace Chapter4
{
    //Func<int,out>
    //Action //void
    //Action<in> //mnic nie zwraca
    //Preicate<in> zwraca bool i musi miec parametr chociaz lepiej, jak ma wiecej lepiej uzyc func
    //nei warto stosowac kiedys były uzywane teraz lepiej func
    //Converter <> nie uzywane
    //Comparison<> też nie używane

    class Program
    {
        static void Main(string[] args)
        {
            //var program = new Program();
            //int[] val = {1, 2, 3};
            //Util.Transform(val,Square);
            //foreach (var i in val)
            //{
            //    Console.WriteLine(i);
            //}

            //ProgressReporter p = WriteProgressToConsole;
            //Util.DoHardWork(p);


            //var stock = new Stock("Tel") {Price = 10};
            //Console.WriteLine(stock.Price);

            Stock stock = new Stock("Cos");
            stock.Price = 27.1M;

            //rejestracja w zdarzeniu PriceChanged
            stock.PriceChanged += stock_PriceChanged;
            stock.Price = 31.59M;

            Console.ReadKey();
        }
        static void WriteProgressToConsole(int percentComplete) => Console.WriteLine(percentComplete);
        static int Square(int x) => x * x;

        static void stock_PriceChanged(object sender, PriceChangedEventArgs e)
        {
            if((e.NewPrice-e.LastPrice)/e.LastPrice>0.1M)
                Console.WriteLine("Alert, 10% stock price increase!");
        }
    }
}
