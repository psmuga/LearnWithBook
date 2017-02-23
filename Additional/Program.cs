using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Additional
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.SortbyYourWish();





            Console.ReadKey();
        }

        public void SortbyYourWish()
        {
            var list = new List<string> { "Dog", "Cat", "Mouse", "Elephant", "Lion", "Rabbit" };


            list = list.OrderByDescending(item => item == "Lion")
            .ThenByDescending(item => item == "Cat")
            .ThenBy(item => item)
            .ToList();

            foreach (var i in list)
            {
                Console.WriteLine(i);
            }
        }
    }
}
