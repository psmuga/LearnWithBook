using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;


//Automapper do mapowania danych z DTO
namespace Additional
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            //p.SortbyYourWish();



            var fooModel = new Foo(){IsExist = true, Name = "Pixel"};//source
            var config  = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Foo, FooView>();
            } );
            IMapper mapper = config.CreateMapper();
            FooView test =new FooView();//dest
            test = mapper.Map<Foo, FooView>(fooModel);
            Console.WriteLine(test.ToString());



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

    public class Foo
    {
        public string Name { get; set; }
        public bool IsExist { get; set; }
        
    }

    public class FooView
    {
        public string Name { get; set; }
        public bool IsExist { get; set; }
        public override string ToString()
        {
            return Name + " " + IsExist;
        }
    }
}
