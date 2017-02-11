using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace Chapter7
{
    class Program
    {
        static void Main(string[] args)
        {
            //string[] names = { "Piotr", "Ewa", "Natalia" };
            //string match = Array.Find(names, n => n.Contains("a"));
            //Console.WriteLine(match);

            //int[] numbers = { 1, 2, 3, 4, 5, 6, 7 };
            //Array.Sort(numbers, (x, y) => x % 2 == y % 2 ? 0 : x % 2 == 1 ? -1 : 1);//segegacja że pierwsze nie parzyste
            //foreach(var i in numbers)
            //{
            //    Console.Write(i + " ");
            //}
            //Array.Reverse(numbers);

            //LinkedList<int> abc = new LinkedList<int>();
            //abc.AddFirst(1);

            //var abc = new Queue<int>(5);
            //abc.Enqueue(1);
            //abc.Enqueue(2);
            //int a = abc.Peek();//tylko podglada ale nie usuwa z kolejki
            //abc.Dequeue();
            //Console.WriteLine(abc.Count);
            ////Podobnie Stack

            ////wartosci typu bool dynamiczna zmiana rozmiaru, efektowne
            //var bits = new BitArray(2);
            //bits[0] = true;
            //bits.Or(bits);

            ////nie dodaje duplikatow
            //var letters = new HashSet<char>("gdyby kózka nie skakała to by nóżki nie złamała");
            //Console.WriteLine(letters.Contains('g'));//true
            //foreach (char c in letters) Console.Write(c);
            //letters.UnionWith("ala ma kota a kot ma ale");///dodaje te chary którch jeszcze nie mam w letters
            //letters.IntersectWith("aeiouyó");//usuwa te elementy których nie ma w obu 
            //letters.ExceptWith("aeiouy");//usuwa te znaki z letters
            //letters.SymmetricExceptWith("toby nogi nie złamała ala");//usuwa wszystki elementy oprócz tych które wysępują tylko w jednym lub drugim zbiorze
            ////SortedSet prawie to samo co hashset ale jest posortowana


            //Słowniki niesortowane

            ////wielkosc liter ma znaczenie, nie utrzynuje nawet kolejnosci dodania
            //var d = new Dictionary<string, int>();
            //d.Add("Jeden", 1);
            //d["Dwa"] = 2;//dodaje
            //d["Dwa"] = 22;//modyfikuje
            //Console.WriteLine(d.ContainsKey("Jeden"));//prawda szybka operacja
            //Console.WriteLine(d.ContainsValue(22));//prawda wolna operacja
            //foreach (string s in d.Keys) Console.Write(s + " ");
            //Console.WriteLine(Environment.NewLine);
            //foreach (KeyValuePair<string, int> kv in d) Console.WriteLine(kv.Key + " - " + kv.Value);
            //var sl = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);//ignoruje wielkosc liter

            //Hashtable h = new Hashtable();//nie generyczna wersja dictionary
            //h.Add(1, "Jeden");

            //OrderedDictionary //przechowuje w kolejnosci dodania, kombinacha HashTable i ArrayList
            //ListDictionary //dobra do 10 elementow pozniej strasznei wolna
            //HybridDictionary // to listDictionary który konerstuje się na hashtable po osiagnieciu pewnego rozmiaru, długi czas konwersji na hastable


            //Słowniki Sortowane

            //SortedDictionary<,> bazuje na drzewie czerwono czarnym // znacznie szybsza od sortedlist
            //SortedList<,> // umożliwia odnoszenies ie po inedksie

            //uzycie refleksji czyli info system o samym sobie
            var sorted = new SortedDictionary<string, MethodInfo>();
            foreach (var m in typeof(object).GetMethods())
            {
                sorted[m.Name] = m;
            }
            foreach (string name in sorted.Keys)
                Console.WriteLine(name);
            foreach (var m in sorted.Values)
                Console.WriteLine(m.Name + " zwraca obiekt typu " + m.ReturnType);
            
            
            
            
            
                
            Console.ReadKey();
        }
    }
}
