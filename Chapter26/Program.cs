using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chapter26
{
    class Program
    {
        static void Main(string[] args)
        {

            string tekst = "Color kolor Dowolny Ulubiony kolor 123 b2-c4";
            string wzorzec = @"kolory?";//? poprzedni symbol jest opcjonalny
            Match m = Regex.Match(tekst, wzorzec);
            Console.WriteLine(m);
            Match m2 = m.NextMatch();

            

            foreach (Match match in Regex.Matches(tekst,wzorzec))
            {
                Console.WriteLine(match);
            }


            // symbol | alternatywa 
            // () pozwalajć odzielic 


            //kompilowane wyrazenia - szybsze dopasowanie, dłuższy czas poczatkowej kompilacji
            Regex r = new Regex(wzorzec, RegexOptions.Compiled);
            Console.WriteLine(r.Match(tekst));

            Console.WriteLine(Regex.Match(tekst,@"[a-h][1-8]-[a-h][1-8]"));//zapis ruchu szachowego


            //LinkPad examples
            //książka!!!

            Console.ReadKey();
        }
    }
}
