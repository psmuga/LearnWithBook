using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.IO.IsolatedStorage;
using System.IO.MemoryMappedFiles;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Chapter15
{
    class Program
    {
        static void Main(string[] args)
        {
            //Fuction1();
            //Function1_1().Wait();
            //Function_FileStream();
            //Function1_2();
            //Function2();
            //Function_OperacjeNaPlikachiKatalogach();

            //Function_CheckInfoAboutVolumine();
            //Function_ZdarzeniaSystemuPlikow();

            // w np UWP i sklepie nie ma FileStream, Directory i File ale jest StorageFolder i Storage File str 648


            //Cast<string> jesli w liscie np jest string i int a bedziesz chciał stringa to rzuci wyjatek
            //OffType<string> --> nie rzuci wyjatku i wybierze tylko te stringi


            //Function_MapowaniePlikowWPamięci();
            Function_OdizolowanyMagazynDanych();


            Console.ReadKey();
        }

       


        static void Fuction1()
        {
            using (Stream s = new FileStream("test.txt", FileMode.Create))
            {
                Console.WriteLine(s.CanRead);
                Console.WriteLine(s.CanWrite);
                s.WriteByte(101);
                s.WriteByte(102);
                byte[] block = { 1, 2, 3, 4, 5 };
                s.Write(block, 0, block.Length);
                Console.WriteLine(s.Length);
                Console.WriteLine(s.Position);
                s.Position = 0;
                Console.WriteLine(s.ReadByte());
                Console.WriteLine(s.ReadByte());
                Console.WriteLine(s.Read(block, 0, block.Length));
                Console.WriteLine(s.Read(block, 0, block.Length));

            }


        }

        static async Task Function1_1()
        {
            using (Stream s = new FileStream("test.txt", FileMode.Create))
            {
                byte[] block = { 1, 2, 3, 4, 5 };
                await s.WriteAsync(block, 0, block.Length);
                s.Position = 0;
                Console.WriteLine(await s.ReadAsync(block, 0, block.Length));
            }
        }

        static void Function_FileStream()
        {
            //FileStream nie nadaje się do Windows Store
            //var fs1 = File.OpenRead("readme.bin");//tylko do odczytu
            //var fs2 = File.OpenWrite("readme.bin");//tylko do zapisu
            //var fs3 = File.Create("readme.bin");//do odczytu i zapisu 


            //var fs = new FileStream("plik.tmp",FileMode.Open);//otworzenie istniejącego pliku bez nadpisania

            //File.ReadAllText();//zwraca ciąge teksotwy
            //File.ReadAllLines();//zwraca tablice ciągów tekstowych
            //File.ReadAllBytes();//zwraca tablicę bajtów

            //File.ReadLines();//zwraca tworzony z opóznieniem IEnumerable<string> nie wczytuje całego pliku do pamięci od razu tylko wtedy kiedy trzeba
            int longLine = File.ReadLines("test.txt").Count(l => l.Length > 80);//ustala liczbę wierszy które długość maja większa niż 80 znakow

            string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            string logoPath = Path.Combine(baseFolder, "logo.jpg");
            Console.WriteLine(File.Exists(logoPath));

            //using (var fs = new FileStream("plik.txt",FileMode.Open))
            //{
            //    fs.Seek(0, SeekOrigin.End);//ustawia na koniec wskazni

            //    //do sth
            //    //dołaczanie do strumienia z trybem odczytu is zapisu
            //    //FIle.Append() tylko zapis 
            //}

            //argumenty opcjonalne podczas tworzenia egzemplarza filestream
            //FileSecurity;
            //FileOptions;
            //FileShare;


        }

        static void Function1_2()
        {
            //MEMORY STREAM
            //memoty stream jest przechowywawny w tablicy, czyli w całosci w pamięci, ale czasem pożyteczny np 
            //var ms = new MemoryStream();
            //sourceStream.CopyTo(ms);

            //PIPE STREAM
            //potok anonimowy(1-way the same pc, parent process to children) i nazwany(2-way any process any computer)
            //potoki nie są dozwolone dla aplikacji windows store

            //potoki nazwane
            ////server
            //using (var s = new NamedPipeServerStream("pipedream"))
            //{
            //    s.WaitForConnection();
            //    s.WriteByte(100);
            //    Console.WriteLine(s.ReadByte());
            //} 
            ////client
            //using (var s = new NamedPipeClientStream("pipedream"))
            //{
            //    s.Connect();
            //    Console.WriteLine(s.ReadByte());
            //    s.WriteByte(200);
            //}

            new Task(NamedPipeServerAsync).Start();
            new Task(NamedPipeClientAsync).Start();

            //BUFFERED STREAM
            //opakowuje inny strumien aby   mógłbyć bufferowany

            //inne strumienie opakowujące: deflatestream, gzipstream, cryptostream,authenticatedstream


            File.WriteAllBytes("myFile.bin", new byte[100000]);
            using (FileStream fs = File.OpenRead("myFile.bin"))
            {
                using (BufferedStream bs = new BufferedStream(fs, 20000))//bufor ma 20000 byte
                {
                    //wczytuje tylko jeden byte
                    bs.ReadByte();
                    Console.WriteLine(fs.Position);
                }
            }


        }

        static void NamedPipeServerAsync()
        {
            using (var s = new NamedPipeServerStream("pipedream", PipeDirection.InOut, 1, PipeTransmissionMode.Message))
            {
                s.WaitForConnection();
                byte[] msg = Encoding.UTF8.GetBytes("Tu server");
                s.Write(msg, 0, msg.Length);
                Console.WriteLine(Encoding.UTF8.GetString(ReadMessage(s)));
            }
        }

        static void NamedPipeClientAsync()
        {
            using (var s = new NamedPipeClientStream("pipedream"))
            {
                s.Connect();
                s.ReadMode = PipeTransmissionMode.Message;
                Console.WriteLine(Encoding.UTF8.GetString(ReadMessage(s)));
                byte[] msg = Encoding.UTF8.GetBytes("A tu client");
                s.Write(msg, 0, msg.Length);
            }
        }
        static byte[] ReadMessage(PipeStream s)
        {
            MemoryStream ms = new MemoryStream();
            byte[] buffer = new byte[0x1000];
            do
            {
                ms.Write(buffer, 0, s.Read(buffer, 0, buffer.Length));
            } while (!s.IsMessageComplete);
            return ms.ToArray();
        }

        static async void Function2()
        {
            //ADAPTERY TEKSTOWE
            //using (FileStream fs = File.Create("test.txt"))
            //{
            //    using (TextWriter writer = new StreamWriter(fs))
            //    //using (TextWriter writer = new StreamWriter(fs, Encoding.Unicode))
            //    {
            //        writer.WriteLine("wiersz1");
            //        writer.WriteLine("wiersz2");
            //    }
            //}
            //using (FileStream fs = File.OpenRead("test.txt"))
            //{
            //    using (TextReader reader = new StreamReader(fs))
            //    {
            //        Console.WriteLine(reader.ReadLine());
            //        Console.WriteLine(reader.ReadLine());
            //    }
            //}
            //to samo nizej bo klasa File oferuje statyczne CreateText,AppendText i OpenText w celu skrócenai


            //using (TextWriter writer = File.CreateText("text.txt"))
            //{
            //    writer.WriteLine("wiersz1");
            //    writer.WriteLine("wiersz2");
            //}
            //using (TextWriter writer = File.AppendText("text.txt"))
            //    writer.WriteLine("wiersz3");
            //using (TextReader reader = File.OpenText("text.txt"))
            //    while (reader.Peek() > -1)
            //    {
            //        Console.WriteLine(reader.ReadLine());
            //    }


            //KOMPRESJA STRUMIENIA
            //string[] words = "The quixk brown fox jumps over the lazy dog".Split();
            //Random rand = new Random();
            //using (Stream s = File.Create("compressed.bin"))
            //using (Stream ds = new DeflateStream(s, CompressionMode.Compress))
            //using (TextWriter w = new StreamWriter(ds))
            //{
            //    for (int i = 0; i < 1000; i++)
            //        await w.WriteAsync(words[rand.Next(words.Length)] + " ");
            //}
            //Console.WriteLine(new FileInfo("compressed.bin").Length);

            //using (Stream s = File.Create(("compressed.bin")))
            //using (Stream ds = new DeflateStream(s, CompressionMode.Decompress))
            //using (TextReader r = new StreamReader(ds))
            //    Console.Write(await r.ReadToEndAsync());

            //Kompresja w pamięci
            byte[] data = new byte[1000];
            MemoryStream ms = new MemoryStream();
            using (Stream ds = new DeflateStream(ms, CompressionMode.Compress, true))
                await ds.WriteAsync(data, 0, data.Length);
            Console.WriteLine(ms.Length);
            ms.Position = 0;
            using (Stream ds = new DeflateStream(ms,CompressionMode.Decompress))
            {
                for (int i = 0; i < 1000; i += await ds.ReadAsync(data, i, 1000 - i))
                {
                }
            }

            //praca z plikami w postacci archiwum zipstr. 637
            //dodać referencje do projektu ZipFile
            //ZipFile.CreateFromDirectory(@"D:\Dokumenty\Visual Studio 2015\Projects\c_sharp_book\Chapter15",
            //    @"D:\Dokumenty\Visual Studio 2015\Projects\c_sharp_book\skompresowany15.zip");
            //dekompresja
            //ZipFile.ExtractToDirectory(@"D:\Dokumenty\Visual Studio 2015\Projects\c_sharp_book\skompresowany15.zip", @"D:\Dokumenty\Visual Studio 2015\Projects\c_sharp_book\Chapter15");

            using (ZipArchive zip =
                ZipFile.Open(@"D:\Dokumenty\Visual Studio 2015\Projects\c_sharp_book\skompresowany15.zip",
                    ZipArchiveMode.Read))
            {
                foreach (var entry in zip.Entries)
                {
                    Console.WriteLine(entry.FullName+" "+entry.Length);
                }
            }

            byte[] data2 =
                File.ReadAllBytes(@"D:\Dokumenty\Visual Studio 2015\Projects\c_sharp_book\README.md");
            using (ZipArchive zip =
                ZipFile.Open(@"D:\Dokumenty\Visual Studio 2015\Projects\c_sharp_book\skompresowany15.zip",
                    ZipArchiveMode.Update))
            {
                zip.CreateEntry(@"Readme.md").Open().Write(data2,0,data2.Length);
            }

            var letters = new string[] {"A", "B", "C", "D"};
            var numbers = new int[] {1, 2, 3};
            var q = letters.Zip(numbers, (l, n) => l + n.ToString());
            foreach (var s in q)
            {
                Console.WriteLine(s);
            }
        }

        static void Function_OperacjeNaPlikachiKatalogach()
        {
             //niedostępne dla apek w sklepie
             //Klasa File
            //string filePath = @"D:\Dokumenty\Visual Studio 2015\Projects\c_sharp_book\skompresowany15.zip";
            //FileAttributes fa = File.GetAttributes(filePath);
            //if ((fa & FileAttributes.ReadOnly) != 0)
            //{
            //    fa ^=FileAttributes.ReadOnly;
            //    File.SetAttributes(filePath,fa);

            //}

            //FileSecurity sec =
            //    File.GetAccessControl(@"D:\Dokumenty\Visual Studio 2015\Projects\c_sharp_book\skompresowany15.zip");
            //AuthorizationRuleCollection rules = sec.GetAccessRules(true, true, typeof(NTAccount));
            //foreach (FileSystemAccessRule rule in rules)
            //{
            //    Console.WriteLine(rule.AccessControlType);
            //    Console.WriteLine(rule.FileSystemRights);
            //    Console.WriteLine(rule.IdentityReference.Value);
            //}
            //var sid = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
            //string userAccount = sid.Translate(typeof(NTAccount)).ToString();
            //FileSystemAccessRule newRule = new FileSystemAccessRule(userAccount,FileSystemRights.ExecuteFile, AccessControlType.Allow);
            //sec.AddAccessRule(newRule);
            //File.SetAccessControl(@"D:\Dokumenty\Visual Studio 2015\Projects\c_sharp_book\skompresowany15.zip",sec);


            //Klasa Directory
            //if (!Directory.Exists(@"d:\temp"))
            //    Directory.CreateDirectory(@"d:\temp");

            ////Klasa FileInfo i DirectoryInfo
            //FileInfo fi = new FileInfo(@"d:\temp\PlikInfo.txt");
            //Console.WriteLine(fi.Exists);
            //using (TextWriter w = fi.CreateText())
            //    w.Write("Dowolny tekst");
            //Console.WriteLine(fi.Exists);
            //fi.Refresh();
            //Console.WriteLine(fi.Exists);
            //Console.WriteLine(fi.Name);
            //Console.WriteLine(fi.FullName);
            //Console.WriteLine(fi.DirectoryName);
            //Console.WriteLine(fi.Directory.Name);
            //Console.WriteLine(fi.Extension);
            //Console.WriteLine(fi.Length);
            //fi.Encrypt();
            //fi.Attributes^=FileAttributes.Hidden;
            //fi.IsReadOnly = true;
            //Console.WriteLine(fi.Attributes);
            //Console.WriteLine(fi.CreationTime);
            ////fi.MoveTo(@"d:\temp\PlikInfoX.txt");
            //DirectoryInfo di = fi.Directory;
            //Console.WriteLine(di.Name);
            //Console.WriteLine(di.FullName);
            //Console.WriteLine(di.Parent.Name);
            //di.CreateSubdirectory("Podkatalog");

            //DirectoryInfo d = new DirectoryInfo(@"D:\");
            //foreach (var file in d.GetFiles("*.jpg"))
            //{
            //    Console.WriteLine(file.Name);
            //}
            //foreach (var subdir in d.GetDirectories())
            //{
            //    Console.WriteLine(subdir.FullName);
            //}

            //Klasa Path
            //rpzeznacozny do pracy z sciezkami dostepu oraz nazwami pliku
            //string dir = @"d:\temp";
            //string file = @"PlikInfo.txt";
            //string path = @"d:\temp\PlikInfo.txt";
            //Directory.SetCurrentDirectory(@"d:\");
            ////Path
             
            //Katalogi specjalne
            string myDocPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Console.WriteLine(myDocPath);
            string myDocPath2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            Console.WriteLine(myDocPath2);

            //we wszystkich katalogach typach nalezy utworzyc folder z nazwa aplikacji jak ponizej
            string localAppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Chapter15");
            if (!Directory.Exists(localAppDataPath))
                Directory.CreateDirectory(localAppDataPath);

            //applicationData -> może zawierać ustawienia które podróżują wraz z uzytkownikiem po sieci
            //localApplicationData -> może zawierać informacje charakterystycznych dla zalogowanego uzytkowniaka, bez roamingu
            //commonApplicationData -> jest wsóddzielony przez każdego uzytkownika komputera, preferowany wzamian za rejestr

        }

        public void AssignUsersFullControlToFolder(string path)
        {
            //rozwiazanie problemu str 645 podczas commonapplicationData
            //gwarantuje ze każdy z użytkowników bedzie miał nieograniczony sotęp , natychmiast po utworzeniu katalogu w commonApplicationData
            try
            {
                var sec = Directory.GetAccessControl(path);
                if (UsersHaveFullControl(sec)) return;
                var rule = new FileSystemAccessRule(GetUsersAccount().ToString(),
                    FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);
                sec.AddAccessRule(rule);
                Directory.SetAccessControl(path, sec);
            }
            catch (UnauthorizedAccessException)
            {
                //katalog zostął już utworzony przez innego użytkownika
            }
        }

        bool UsersHaveFullControl(FileSystemSecurity sec)
        {
            var usersAccount = GetUsersAccount();
            var rules = sec.GetAccessRules(true, true, typeof(NTAccount)).OfType<FileSystemAccessRule>();
            return rules.Any(r =>
                r.FileSystemRights == FileSystemRights.FullControl &&
                r.AccessControlType == AccessControlType.Allow &&
                r.InheritanceFlags == (InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit) &&
                r.IdentityReference == usersAccount);
        }

        NTAccount GetUsersAccount()
        {
            var sid = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid,null);
            return (NTAccount) sid.Translate(typeof(NTAccount));
        }
        private static void Function_CheckInfoAboutVolumine()
        {
            DriveInfo c = new DriveInfo("C");
            Console.WriteLine($"Total Size: {c.TotalSize} B");
            Console.WriteLine($"Total Free Space: {c.TotalFreeSpace} B");
            Console.WriteLine($"Available Free Space: {c.AvailableFreeSpace} B");

            foreach (var drive in DriveInfo.GetDrives())
            {
                Console.WriteLine(drive.Name);
                Console.WriteLine(drive.DriveType);
                Console.WriteLine(drive.RootDirectory);
                if (drive.IsReady)
                {
                    Console.WriteLine(drive.VolumeLabel);
                    Console.WriteLine(drive.DriveFormat);
                }
                Console.WriteLine();
            }
        }
        private static void Function_ZdarzeniaSystemuPlikow()
        {
            Watch(@"D:\", "*.cs", true);

        }

        private static void Watch(string path, string filter, bool includeSubDirs)
        {
            using (var watcher = new FileSystemWatcher(path,filter))
            {
                watcher.Created += FileCreatedChangedDeleted;
                watcher.Changed += FileCreatedChangedDeleted;
                watcher.Deleted += FileCreatedChangedDeleted;
                watcher.Renamed += FileRenamed;
                watcher.Error += FileError;

                watcher.IncludeSubdirectories = includeSubDirs;
                watcher.EnableRaisingEvents = true;
                Console.WriteLine("Nasłuchiwanie zdarzeń - nacisnij <enter>, aby zakończyć.");
                Console.ReadLine();
                
            }
        }

        static void FileCreatedChangedDeleted(object o, FileSystemEventArgs e) => Console.WriteLine(
            $"Na pliku {e.FullPath} wykonano akcje {e.ChangeType}");

        static void FileRenamed(object o, RenamedEventArgs e) => Console.WriteLine(
            $"Zmiana nazwy: {e.OldFullPath} -> {e.FullPath}");

        static void FileError(object o, ErrorEventArgs e) => Console.WriteLine($"Blad: {e.GetException().Message}");

        private static void Function_MapowaniePlikowWPamięci()
        {
            //niedostepne dla sklepu windows

            //losowe operacje plikowe lepiej ta metoda gdy kiilka wątków ,
            //filestream dla sekwencyjnego dostepu do danych

            //File.WriteAllBytes("long.bin",new byte[1000000]);
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile("long.bin"))
            using (MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor())
            {
                accessor.Write(500000,(byte)77);
                Console.WriteLine(accessor.ReadByte(500000));
            }


            ////pamiec wspóldzielona
            //using (MemoryMappedFile mmFile = MemoryMappedFile.CreateNew("Demo", 500))
            //using (MemoryMappedViewAccessor accessor = mmFile.CreateViewAccessor())
            //{
            //    accessor.Write(0, 12345);
            //    Console.ReadLine();//pamiec wspoldzileona pozostaje aktywna ąz do naciśniecia enter
            //}

            ////może być w innym pliku exe
            //using (MemoryMappedFile mmFile = MemoryMappedFile.OpenExisting("Demo"))
            //{
            //    using (MemoryMappedViewAccessor accessor = mmFile.CreateViewAccessor())
            //    {
            //        Console.WriteLine(accessor.ReadInt32(0));//12345
            //    }
            //}


        }

        private static void Function_OdizolowanyMagazynDanych()
        {
            //do aplikacji w oparciu np o technologię ClickOnce

            //zapis
            using (IsolatedStorageFile f = IsolatedStorageFile.GetMachineStoreForDomain())
            using (var s = new IsolatedStorageFileStream("hi.txt",FileMode.Create,f))
            {
                using (var writer = new StreamWriter(s))
                    writer.WriteLine("Hello World!");
            }

            //odczyt
            using (IsolatedStorageFile f = IsolatedStorageFile.GetMachineStoreForDomain())
            using (var s = new IsolatedStorageFileStream("hi.txt", FileMode.Open, f))
            {

                using (var reader = new StreamReader(s))
                    Console.WriteLine(reader.ReadToEnd());
            }

        }
    }
}
