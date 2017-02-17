using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Chapter8
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            //string[] names = { "Jan", "Olga", "Daria" };
            ////IEnumerable<string> filteredNames = System.Linq.Enumerable.Where(names, n => n.Length >= 4);
            //IEnumerable<string> filteredNames = names.Where(n => n.Length >= 4); //to samo co wyzej bo to metody rozszerzeń
            //foreach (string i in filteredNames)
            //    Console.WriteLine(i); //olga,daria
            //IEnumerable<string> ContainsG = names.Where(n => n.Contains("g"));

            //IEnumerable<string> cong = from n in names where n.Contains("g") select n;//query expression

            //string[] names = { "Jan", "Olga", "Daria", "Robert", "Zenon" };
            //IEnumerable<string> query = names
            //    .Where(n => n.Contains("a"))
            //    .OrderBy(n => n.Length)
            //    .Select(n => n.ToUpper());
            //foreach (string name in query)
            //    Console.WriteLine(name);
            //IEnumerable<int> query2 = names.Select(n => n.Length);
            //foreach (int i in query2)
            //    Console.WriteLine(i + " | "); //3,4,5,6,5
            //IEnumerable<string> query3 = names.OrderBy(n => n);//alfabetycznie
            //foreach (string i in query3)
            //    Console.WriteLine(i + " | ");

            //int[] numbers = { 10, 9, 8, 7, 6 };
            //IEnumerable<int> firstThree = numbers.Take(3);//10,9,8
            //IEnumerable<int> lastTwo = numbers.Skip(3);//7,6
            //IEnumerable<int> reversed = numbers.Reverse();//6,7,8,9,10

            //int firstNumber = numbers.First();
            //int lastNumber = numbers.Last();
            //int secondNumber = numbers.ElementAt(1);
            //int secondLowest = numbers.OrderBy(n => n).Skip(1).First();
            //int count = numbers.Count();//5
            //int min = numbers.Min();
            //bool hasTheNumberNine = numbers.Contains(9);
            //bool hasMoreThanZeroElements = numbers.Any();
            //bool hasAnOddElement = numbers.Any(n => n % 2 != 0);//czy ma nieparzysta liczbe

            //int[] numbers2 = { 6, 5, 4 };
            //IEnumerable<int> concat = numbers.Concat(numbers2);//dodaj dwie sekwencje
            //IEnumerable<int> union = numbers.Union(numbers2);//eliminuje dupliikaty, dodaje dwie sekwencje

            ////notacja mieszana (zapytaniowa i płynna)
            //string[] names = { "Tomasz", "Dariusz", "Hubert", "Maria", "Jacek" };
            //int matches = (from n in names where n.Contains("a") select n).Count();//4
            //string first = (from n in names orderby n select n).First();

            //var numbers = new List<int>();
            //numbers.Add(1);
            //IEnumerable<int> query = numbers.Select(n => n * 10);
            //numbers.Add(2);
            //foreach (int i in query)
            //    Console.Write(i + " | ");
            //Console.Write(Environment.NewLine);//10 | 20 wykonanie opóźnione
            ////wykonanie opóznione występuje tez w rzypadku delegatów
            ////wykonuje się dopiero w momwncie wywołania a nie stworzenia

            //var numerki = new List<int>() { 1, 2, 3 };
            //IEnumerable<int> que = numerki.Select(n => n * 10);
            //foreach(int i in que)
            //    Console.Write(i + " | ");//10|20|30
            //Console.Write(Environment.NewLine);
            //numerki.Clear();
            //foreach (int i in que)
            //    Console.Write(i + " | ");//nic
            ////czasami wykonanie zapytania jest niekorzystne np w połaczeniu z bazda danych, logowanie itd
            ////mozna wylłaczyc przez wywołanie operatora konwesji np ToArray lub ToList
            //var numer = new List<int>() { 1, 2 };
            //List<int> TimesTen = numer.Select(n => n * 10).ToList();
            //numer.Clear();
            //Console.WriteLine(numer.Count());//2

            //int[] numbers = { 1, 2 };
            //int factor = 10;
            //IEnumerable<int> query = numbers.Select(n => n * factor);
            //factor = 20;
            //foreach (int i in query)
            //    Console.Write(i + " | ");//20 | 40
            //Console.WriteLine();

            //IEnumerable<char> query = "Nie tego sie spodziewaliśmy";
            //string vowels = "aeiouyó";
            ////for (int i = 0; i < vowels.Length; i++)
            ////    query = query.Where(c => c != vowels[i]);//wyjatek IndexOutOfException
            //for (int i = 0; i < vowels.Length; i++)
            //{
            //    char vowel = vowels[i];
            //    query = query.Where(c => c != vowel);
            //}
            //foreach (char c in query)
            //    Console.Write(c);

            ////PODZAPYTANIA
            //string[] musos = { "David Gilmour", "Roger Waters", "Rick Wright", "Nick Mason" };
            //IEnumerable<string> query = musos.OrderBy(m => m.Split().Last());
            //string[] musos2 = { "David", "Roger", "Rick", "Nick" };
            ////IEnumerable<string> query2 = musos2.Where(n => n.Length == musos2.OrderBy(n2 => n2.Length).Select(n2 => n2.Length).First());
            ////IEnumerable<String> query2 = from n in musos2
            //                               where n.Length == (from n2 in musos2 orderby n2.Length select n2.Length).First()
            //                               select n;//to samo co wyżej
            //IEnumerable<string> query2 = from n in musos2
            //                             where n.Length == musos2.Min(n2 => n2.Length)
            //                             select n;//to samo
            //int shortest = musos2.Min(n => n.Length);
            //IEnumerable<string> query3 = from n in musos2
            //                             where n.Length == shortest
            //                             select n;//to samo co wyzej ale wyodrębnione, dobre do kolekji lokkalnych ale niezbyt gdy odnosi się do zewnętrznej zmiennej zakresowej
            //foreach (string i in query2)
            //    Console.Write(i + " | "); // Rick | Nick

            //string[] names = { "Jan", "Olga", "Daria", "Robert", "Zenon" };
            //IEnumerable<string> query = names.Select(n => Regex.Replace(n, "[aeiouy]", "")).Where(n => n.Length > 2).OrderBy(n => n);
            //foreach (string i in query)
            //    Console.Write(i + " | ");
            //Console.WriteLine();


            //TWORZENIE ZAPYTAŃ ZŁOŻONYCH
            ////metoda progresywna
            //string[] names = { "Jan", "Olga", "Daria", "Robert", "Zenon" };
            //var filtered = names.Where(n => n.Contains("a"));
            //var sorted = filtered.OrderBy(n => n);
            //var query = sorted.Select(n => n.ToUpper());

            ////metoda z słowem into
            //string[] names = { "Jan", "Olga", "Daria", "Robert", "Zenon" };
            //IEnumerable<string> query = from n in names
            //                            select Regex.Replace(n, "[aeiouy]", "")
            //                            into noVowel
            //                            where noVowel.Length > 2
            //                            orderby noVowel
            //                            select noVowel;
            //foreach (string i in query)
            //    Console.Write(i + " | ");

            ////opakowywanie zapytań
            //string[] names = { "Jan", "Olga", "Daria", "Robert", "Zenon" };
            //IEnumerable<string> query = from n in
            //                                (
            //                                from n2 in names
            //                                select Regex.Replace(n2, "[aeiouy]", "")
            //                                )
            //                            where n.Length > 2
            //                            orderby n
            //                            select n;

            //p.StrategieProjekcji();
            //p.ZapytaniaInterpretowane();
            p.LinqiEF();







            Console.ReadKey();    
        }
        public void StrategieProjekcji()
        {
            ////INICJALIZATORY OBIEKTÓW
            //string[] names = { "Tomasz", "Dariusz", "Hubert", "Maria", "Jacek" };
            //IEnumerable<TempProjctionItem> temp = from n in names
            //                                      select new TempProjctionItem
            //                                      {
            //                                          Original = n,
            //                                          VowelLess = Regex.Replace(n, "[aeiouy]", "")
            //                                      };
            //IEnumerable<string> query = from n in temp
            //                            where n.VowelLess.Length > 2
            //                            select n.Original;
            //foreach (var i in temp)
            //    Console.Write(i.VowelLess + " | ");
            //Console.WriteLine(Environment.NewLine);
            //foreach (var i in query)
            //    Console.Write(i + " | ");
            //Console.WriteLine();

            ////TYPY ANONIMOWE
            ////możemy usunac klase TempProjectionItem dzięki typom anonimowym
            //string[] names = { "Tomasz", "Dariusz", "Hubert", "Maria", "Jacek" };
            //var temp = from n in names
            //            select new 
            //            {
            //                Original = n,
            //                VowelLess = Regex.Replace(n, "[aeiouy]", "")
            //            };
            //IEnumerable < string > query = from n in temp
            //                            where n.VowelLess.Length > 2
            //                            select n.Original;
            //foreach (var i in query)
            //    Console.Write(i + " | ");
            //Console.WriteLine();

            //SŁOWO  KLUCZOWE LET
            //wprowadza  nową zmienną obok zmiennej zakresowej
            //string[] names = { "Tomasz", "Dariusz", "Hubert", "Maria", "Jacek" };
            //IEnumerable<string> query = from n in names
            //                            let vowelless = Regex.Replace(n, "[aeiouy]", "")
            //                            where vowelless.Length > 2
            //                            orderby vowelless
            //                            select n;
        }
        public void ZapytaniaInterpretowane()
        {
            //dla zdalnych źródeł danych
            //interfejs IQueryable<T> jest rozszerzniem IEnumerable<t>, zawiera doddatkowe metody do tworzenia drzew wyrażeń
            //Linq to SQL

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Demo.mdf;Integrated Security=True";
            DataContext dataContext = new DataContext(connectionString);
            Table<Customer> customers = dataContext.GetTable<Customer>();
            IQueryable<string> query = from n in customers
                                       where n.Name.Contains("a")
                                       orderby n.Name.Length
                                       select n.Name.ToUpper();
            foreach (string i in query)
                Console.WriteLine(i);
            Console.WriteLine(Environment.NewLine);

            IEnumerable<string> q = customers
                .Select(c => c.Name.ToUpper())
                .OrderBy(n => n)
                .Pair()//lokalny  od tego miejsca
                .Select((n, i) => "Para " + i.ToString() + " = " + n);
            foreach (string i in q)
                Console.WriteLine(i);
            Console.WriteLine(Environment.NewLine);

            //Operator AsEnumerable - mzona w jednym zapytaniu obsłuzyc najpierw baze na serrwerze a po nim już na kliencie
            //strona 367
        }
        public void LinqiEF()
        {
            //zapytania interpretowane pobierają drzewa wyrażeń
            //DRZEWA WYRAŻEŃ
            //drzewo wyrażeń
            Expression<Func<int, bool>> expr = num => num > 4;
            // Kompilacja drzewa
            Func<int, bool> result = expr.Compile();
            // Wywołanie delegaty i wypisanie na ekranie
            Console.WriteLine(result(5));
            //Zwraca wartość true
            //Sposób uproszczony, kompilacja razem z wywołaniem
            Console.WriteLine(expr.Compile()(5));

        }
    }
    public static class rozszerzenie
    {
        public static IEnumerable<string> Pair(this IEnumerable<string> source)//metoda rozszerrzeniowa
        {
            string firsthalf = null;
            foreach (string element in source)
            {
                if (firsthalf == null)
                    firsthalf = element;
                else
                {
                    yield return firsthalf + " | " + element;
                    firsthalf = null;
                }
            }
        }
    }
    [Table] public class Customer
    {
        [Column(IsPrimaryKey = true)]
        public int ID;
        [Column]
        public string Name;
    }

    class TempProjctionItem
    {
        public string Original;
        public string VowelLess;
    }
}
