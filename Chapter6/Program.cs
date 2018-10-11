using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter6
{
    class Program
    {
        static void Main(string[] args)
        {
            //string s = "pon wt sr cz pt sob nd".Split()[(int)DateTime.Now.DayOfWeek - 1];
            //Console.WriteLine(s);


            //Console.WriteLine(char.ToUpper('a'));//A
            //Console.WriteLine(char.IsWhiteSpace('\t'));//true
            //Console.WriteLine(new String('*', 10));

            //char[] s = "hello".ToCharArray();
            //string a = new string(s);

            //string b = string.Empty;
            //if(string.IsNullOrEmpty(b))
            //{

            //}
            //foreach (char c in a)
            //{
            //    Console.Write(c + " ");
            //}

            //Console.WriteLine("ala ma kota a kot ma ale".Contains("kot"));//true
            //Console.WriteLine("ala ma kota a kot ma ale".EndsWith("ale"));//true

            //Console.WriteLine("abcd abcd".IndexOf("cd", 6, StringComparison.CurrentCultureIgnoreCase));
            //string d = "ala ma".Substring(0, 3);//ala
            //string e = "ala ma".Substring(2);//a ma
            //string f = "witajswiecie".Insert(5, ", ");
            //string g = f.Remove(5, 2);

            //Console.WriteLine("12345".PadLeft(9,'*'));//****12345
            //Console.WriteLine("12345".PadLeft(9));//     12345;

            //Console.WriteLine(" abc d\t".Trim());//trim usuwa znaki białe 
            //Console.WriteLine("ala ma kota".Replace(' ', '|'));//ala|ma|kota

            //string[] words = "ala ma kota".Split();
            ////join działa na odwrót
            //string h = string.Join(" ", words);

            //string i = $"jest {DateTime.Now.DayOfWeek} rano i strasznie doskwiera upał ;)";

            //string composite = "Name={0,-20} Credit Limit={1,15:C}";// - wyrównanie do lewej
            //Console.WriteLine(string.Format(composite, "Piotr", 100000));//to samo można uzyskac stosujac PadLeft i PadRight

            //CultureInfo polandd = CultureInfo.GetCultureInfo("pl-PL");
            //Console.WriteLine(string.Compare("łalka", "lalka", false, polandd));//1

            //StringBuilder sb = new StringBuilder();
            //for (int j=0; j<50;j++)
            //{
            //    sb.Append(i);
            //    sb.Append(',');
            //}
            //sb.AppendLine(); //dodaje znak nowej lini
            //Console.WriteLine(sb.ToString());

            foreach (EncodingInfo info in Encoding.GetEncodings())
            {
                Console.WriteLine(info.Name);
            }

            //zapisuje wszystko w pliku z kodowaniem Unicode zamiast domyslnie utf-8
            //System.IO.File.WriteAllText("dane.txt", "Testowanie...", Encoding.Unicode);

            //byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes("0123456789");
            //string original = System.Text.Encoding.UTF8.GetString(utf8Bytes);
            //Console.WriteLine(original);//0123456789

            //TimeSpan godzinIle = new TimeSpan(); //połnoc
            //TimeSpan godzinIle = new TimeSpan(100000000); //10s bo kazdt Tick = 100ns
            //domyclna wartosc TimeSPan = TimeSpan.Zero
            //TimeSpan godzinIle = new TimeSpan(20,21,35);
            //TimeSpan godzinIle = TimeSpan.FromDays(2);
            //TimeSpan godzinIle = TimeSpan.FromHours(24);
            //TimeSpan godzinIle = TimeSpan.FromHours(24) - TimeSpan.FromMinutes(30);
            //Console.WriteLine(godzinIle);

            //DateTime dt = new DateTime(1995, 6, 6);
            //Console.WriteLine(dt.DayOfWeek);
            //DateTimeOffset dto = new DateTimeOffset(dt);//domyslnie przesuniecie 0 gdy dt.datetimekind = utc, else przesuniecie domyslnie dla strefy utc
            //Console.WriteLine(DateTime.Today);//2017-02-07 00:00:00
            //Console.WriteLine(DateTime.Now.AddDays(-1));//odejmuje jeden dzien
            //Console.WriteLine(DateTime.Now.ToLongDateString());
            //Console.WriteLine(DateTime.Now.ToShortDateString());
            //Console.WriteLine(DateTime.Now.ToLongTimeString());
            //Console.WriteLine(DateTime.Now.ToShortTimeString());//tylko godzina i minuty

            //DateTime outDt;
            //Console.WriteLine(DateTime.TryParse("2017-02-06",out outDt));




            //
            // DATY I STREFY CZASOWE
            //

            //DateTime dt1 = new DateTime(2015, 1, 1);//unspecified
            //DateTime dt2 = DateTime.SpecifyKind(dt1, DateTimeKind.Utc);//utc
            //DateTime dt3 = dt2.ToUniversalTime();//do utc

            //DateTimeOffset porównuje do czasu utc

            //TimeZone zone = TimeZone.CurrentTimeZone;
            //Console.WriteLine(zone.DaylightName);
            //Console.WriteLine(zone.StandardName);

            //DaylightTime day = zone.GetDaylightChanges(2017);
            //Console.WriteLine(day.Start.ToString("M"));//tylko dzień i miesiac
            //Console.WriteLine(day.End.ToString());
            //Console.WriteLine(day.Delta);

            //TimeZoneInfo zone = TimeZoneInfo.Local;
            //Console.WriteLine(zone.StandardName);
            //foreach(TimeZoneInfo z in TimeZoneInfo.GetSystemTimeZones())
            //{
            //    Console.WriteLine(z.Id);//wyswietla wszyskie identyfikatory zstref czasowych
            //}

            //TimeZoneInfo wa = TimeZoneInfo.Local;
            //foreach(TimeZoneInfo.AdjustmentRule rule in wa.GetAdjustmentRules())
            //{
            //    Console.WriteLine("Reguła ważna od " + rule.DateStart + " do " + rule.DateEnd);//od keidy do kiedy obowiazuje reguła
            //    Console.WriteLine("Różnica: " + rule.DaylightDelta);
            //    Console.WriteLine("Początek: " + FormatTransitionTime(rule.DaylightTransitionStart, false));
            //    Console.WriteLine("Koniec: " + FormatTransitionTime(rule.DaylightTransitionEnd, true));
            //}

            //Console.WriteLine(DateTime.Now.IsDaylightSavingTime());//prawda lub fałsz // czy czas letni
            //Console.WriteLine(DateTime.UtcNow.IsDaylightSavingTime());//zawsze fałsz











            Console.ReadKey();
        }

        private static string FormatTransitionTime(TimeZoneInfo.TransitionTime tt, bool endTime)
        {
            CultureInfo plCi = new CultureInfo("pl-PL");
            Thread.CurrentThread.CurrentCulture = plCi;
            if (endTime && tt.IsFixedDateRule && tt.Day == 1 && tt.Month == 1 && tt.TimeOfDay == DateTime.MinValue)
                return "-";
            string s;
            if (tt.IsFixedDateRule)
                s = tt.Day.ToString();
            else
                s = "Pierwszy drugi trzeci czwarty ostatni".Split()[tt.Week - 1] + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(tt.DayOfWeek) + " w miesiacu ";
            return s + DateTimeFormatInfo.CurrentInfo.MonthNames[tt.Month - 1] + " o godzinie " + tt.TimeOfDay.TimeOfDay;
        }
    }
}
