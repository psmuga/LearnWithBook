using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter23
{
    class Program
    {
        static void Main(string[] args)
        {
            ////AsParallel wykorzystuje wszystkie rdzenie jest najlepsza z dotychczasowych rozwiązań
            //IEnumerable<int> numbers = Enumerable.Range(2, 100000 - 2);
            //var parallerQuery = from n in numbers.AsParallel()
            //    where Enumerable.Range(2, (int) Math.Sqrt(n)).All(i => n % i > 0)
            //    select n;
            ////int[] primes = parallerQuery.ToArray();
            //foreach (var i in parallerQuery)
            //{
            //    Console.Write(i + " ");
            //}
            ////Plinq działa tylko na kolekcjach lokalnych, nie jest utrzyamna kolejność wejsciowa



            //if (!File.Exists("WordLookup.txt"))
            //    new WebClient().DownloadFile("http://www.albahari.com/ispell/allwords.txt", "WordLookup.txt");
            //var workLookup = new HashSet<string>(File.ReadAllLines("WordLookup.txt"), StringComparer.InvariantCultureIgnoreCase);

            ////var random = new Random();
            //var localRandom = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));
            //string[] wordList = workLookup.ToArray();
            ////string[] wordsToTest = Enumerable.Range(0, 1000000).Select(i => wordList[random.Next(0, wordList.Length)]).ToArray();
            //string[] wordsToTest = Enumerable.Range(0, 1000000)
            //    .AsParallel()
            //    .Select(i => wordList[localRandom.Value.Next(0, wordList.Length)]).ToArray();
            //wordsToTest[12345] = "woozsh";
            //wordsToTest[23456] = "wubsie";

            //var query = wordsToTest
            //    .AsParallel()
            //    .Select((word, index) => new IndexedWord {Word = word, Index = index})
            //    .Where(iword => !workLookup.Contains(iword.Word))
            //    .OrderBy(iword => iword.Index);

            //foreach (var mistake in query)
            //{
            //    Console.WriteLine(mistake.Word+ " - indeks = "+mistake.Index);
            //}



            //ConcurrentBag nie wazna jest kolejnosc pobierania danych
            ////troche dalszy rozdzial
            //var misspelings = new ConcurrentBag<Tuple<int,string>>();
            //Parallel.ForEach(wordsToTest, (word, state, i) =>
            //{
            //    if (!workLookup.Contains(word))
            //        misspelings.Add(Tuple.Create((int) i, word));
            //});
            //foreach (var s in misspelings)
            //{
            //    Console.WriteLine(s);
            //}



            //var query2 = Enumerable.Range(0, 999).AsParallel().Select((n, i) => n * i);
            //foreach (var i in query2)
            //{
            //    Console.WriteLine(i + " ");
            //}



            //var million = Enumerable.Range(3, 1000000);
            //var cancelSource = new CancellationTokenSource();
            //var primeNumberQuery = from n in million.AsParallel().WithCancellation(cancelSource.Token)
            //    where Enumerable.Range(2, (int) Math.Sqrt(n)).All(i => n % i > 0)
            //    select n;
            //new Thread(() =>
            //{
            //    Thread.Sleep(100);
            //    cancelSource.Cancel();
            //}).Start();

            //try
            //{
            //    int[] primes = primeNumberQuery.ToArray();
            //}
            //catch (OperationCanceledException e)
            //{
            //    Console.WriteLine("Zapytanie anulowane");
            //}


            ////Optymalizacje PLinq
            //"abcdefghijklmnoprstuwxyz".AsParallel().Select(c=>char.ToUpper(c)).ForAll(Console.Write);//forall -> wykonanie jakiejs metody na wszystkich elementach

            ////sekwencyjnie
            //string tekst = "zalozmy ze to jest bardzo dlugi tekst";
            //var letterFrequencies = new int[26];
            //foreach (char c in tekst)
            //{
            //    int index = char.ToUpper(c) - 'A';
            //    if (index >= 0 && index <= 26) letterFrequencies[index]++;
            //}
            ////równolegle
            //int[] result = tekst.AsParallel().Aggregate(()=>new int[26],
            //    (localFrequencies, c) =>
            //    {
            //        int index = char.ToUpper(c) - 'A';
            //        if (index >= 0 && index <= 26) localFrequencies[index]++;
            //        return localFrequencies;
            //    },
            //    (mainFreq,localFreq)=> mainFreq.Zip(localFreq,(f1,f2)=>f1+f2).ToArray(),
            //    finalResult=>finalResult
            //    );
            //Parallel.ForEach(result, x => Console.Write(x));

            Program3.Test();

            Console.ReadKey();
        }

        class IndexedWord
        {
            public string Word;
            public int Index;
        }
    }
}
