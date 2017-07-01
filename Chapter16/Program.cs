using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter16
{
    class Program
    {
        static void Main(string[] args)
        {
            //AdresyIPorty();
            //AdresyUri();
            //FunctionWebClient();
            //FunctionWebRequestAndWebResponse().Wait();
            //FunctionHttpClient().Wait();
            //FunctionProxy();
            //Uwierzytelnianie();
            //ObslugaWyjatkow();
            //Naglowki();
            //CiagTekstowyZapytania();
            //PrzekazanieDanychFormularza();
            //MechanizmCookies();
            //UwierzytelnianieNaPodstawieFormularzy();
            //UtworzenieSerweraHttp();
            //UzycieFtp();
            //UzycieDns();
            //SentEmailWithSmtp();
            //UzycieTcp();
            PocztaZUzyciemPop3();

            //WebClient nie obsługuje mechanizmu cookies, może przedstawic informacje o posteie operacji
            //Httpclient dla restfull
            //credentials jeśli wymagane jest uwierzytelnianie
            Console.ReadKey();
        }

        static void AdresyIPorty()
        {
            IPAddress a1 = new IPAddress(new byte[]{101,102,103,104});
            IPAddress a2 = IPAddress.Parse("101.102.103.104");
            Console.WriteLine(Equals(a1, a2)); //true
            Console.WriteLine(a1.AddressFamily);

            var a3 = IPAddress.Parse("[3EA0:FFFF:198A:E4A3:4FF2:54FA:41BC:8D31]");
            Console.WriteLine(a3.AddressFamily);

            //łączy Ip z portem
            IPEndPoint ep = new IPEndPoint(a1,222);//port 222
            Console.WriteLine(ep.ToString());
        }

        static void AdresyUri()
        {
            Uri info = new Uri("http://www.domain.com:80/info/");
            Uri page = new Uri("http://www.domain.com/info/page.html");
            Console.WriteLine(info.Host);
            Console.WriteLine(info.Port);
            Console.WriteLine(page.Port);

            Console.WriteLine(info.IsBaseOf(page));
            Uri relative = info.MakeRelativeUri(page);
            Console.WriteLine(relative.IsAbsoluteUri);
            Console.WriteLine(relative.ToString());
            //na końcu warto dać ukośnik bo oznacza że szuka domyślego pliku w tym katalogu
            //jeśli nie ma ukośnika to szuka pliku bez rozszerzenia z tą nazwa

        }

        static async void FunctionWebClient()
        {
            //WebClient wc = new WebClient {Proxy = null};
            //wc.DownloadFile("http://www.albahari.com/nutshell/code.aspx","code.htm");
            //System.Diagnostics.Process.Start("code.htm");

            //WebClient wc = new WebClient { Proxy = null };
            //await wc.DownloadFileTaskAsync("http://www.albahari.com/nutshell/code.aspx", "code.htm");
            //System.Diagnostics.Process.Start("code.htm");

            var wc = new WebClient();
            wc.DownloadProgressChanged += (sender, args) =>
                Console.WriteLine(args.ProgressPercentage + "% complete");
            Task.Delay(5000).ContinueWith(ant => wc.CancelAsync());
            await wc.DownloadFileTaskAsync("http://oreilly.com", "webpage.htm");
        }

        static async Task FunctionWebRequestAndWebResponse()
        {
            //WebRequest req = WebRequest.Create
            //    ("http://www.albahari.com/nutshell/code.html");
            //req.Proxy = null;
            //using (WebResponse res = req.GetResponse())
            //using (Stream rs = res.GetResponseStream())
            //using (FileStream fs = File.Create("code.html"))
            //    rs?.CopyTo(fs);

            WebRequest req = WebRequest.Create
                ("http://www.albahari.com/nutshell/code.html");
            req.Proxy = null;
            using (WebResponse res = await req.GetResponseAsync())
            using (Stream rs = res.GetResponseStream())
            using (FileStream fs = File.Create("code.html"))
                await rs?.CopyToAsync(fs);
            System.Diagnostics.Process.Start("code.html");
        }

        static async Task FunctionHttpClient()
        {
            //string html = await new HttpClient().GetStringAsync("http://linqpad.net");
            //Console.WriteLine(html);

            //var client = new HttpClient();
            //var task1 = client.GetStringAsync("http://www.linqpad.net");
            //var task2 = client.GetStringAsync("http://www.albahari.net");
            //Console.WriteLine(await task1);
            //Console.WriteLine(await task2);

            //var handler = new HttpClientHandler {UseProxy = false}; // w tej klasie zdefiniowano kilka, większosc pomocnych funkcji do obsługi
            //var client2 = new HttpClient(handler);

            //var client = new HttpClient();
            ////metoda Getasync() akceptuje również CancellationToken
            //HttpResponseMessage response = await client.GetAsync("http://www.linqpad.net");
            //response.EnsureSuccessStatusCode();//w przeciwieństwie do WebClient w przypadku braku danych yu rzuci wyjątek tylko jesli wywołamy EnsureSucess...
            //string html = await response.Content.ReadAsStringAsync();
            ////Console.WriteLine(html);
            //using (var fileStream = File.Create("linqpad.html"))
            //{
            //    await response.Content.CopyToAsync(fileStream);
            //}

            //var client = new HttpClient();
            //var request = new HttpRequestMessage(HttpMethod.Get, "http://...");
            //HttpResponseMessage response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();

            //var client = new HttpClient(new HttpClientHandler { UseProxy = false });
            //var request = new HttpRequestMessage(
            //    HttpMethod.Post, "http://www.albahari.com/EchoPost.aspx");
            //request.Content = new StringContent("This is a test");
            //HttpResponseMessage response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            //Console.WriteLine(await response.Content.ReadAsStringAsync());

            ////atrapy i testowanie
            //var mocker = new MockHandler(request =>
            //    new HttpResponseMessage(HttpStatusCode.OK)
            //    {
            //        Content = new StringContent("You asked for " + request.RequestUri)
            //    });

            //var client = new HttpClient(mocker);
            //var response = await client.GetAsync("http://www.linqpad.net");
            //string result = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(result);
            ////Assert.AreEqual("You asked for http://www.linqpad.net/", result);//to mozna dostac w frameworku testów jednsotkowych np NUnit

        }

        static void FunctionProxy()
        {
            // Create a WebProxy with the proxy's IP address and port. You can
            // optionally set Credentials if the proxy needs a username/password.

            //WebProxy p = new WebProxy("192.178.10.49", 808);
            //p.Credentials = new NetworkCredential("username", "password");
            //// or:
            //p.Credentials = new NetworkCredential("username", "password", "domain");
            //WebClient wc = new WebClient();
            //wc.Proxy = p;
            //// Same procedure with a WebRequest object:
            //WebRequest req = WebRequest.Create("...");
            //req.Proxy = p;
        }

        static void Uwierzytelnianie()
        {
            WebClient wc = new WebClient();
            wc.Proxy = null;
            wc.BaseAddress = "ftp://ftp.albahari.com/";

            // Authenticate, then upload and download a file to the FTP server.
            // The same approach also works for HTTP and HTTPS.

            string username = "nutshell";
            string password = "oreilly";
            wc.Credentials = new NetworkCredential(username, password);

            wc.DownloadFile("guestbook.txt", "guestbook.txt");

            string data = "Hello from " + Environment.UserName + "!\r\n";
            File.AppendAllText("guestbook.txt", data);

            wc.UploadFile("guestbook.txt", "guestbook.txt");

            //var handler = new HttpClientHandler();
            //handler.Credentials = new NetworkCredential(username, password);
            //var client = new HttpClient(handler);

            //CredentialCache cache = new CredentialCache();
            //Uri prefix = new Uri("http://exchange.mydomain.com");
            //cache.Add(prefix, "Digest", new NetworkCredential("joe", "passwd"));
            //cache.Add(prefix, "Negotiate", new NetworkCredential("joe", "passwd"));

            //WebClient wc = new WebClient();
            //wc.Credentials = cache;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes("nazwa_uzytkownika:haslo")));

        }

        static async void ObslugaWyjatkow()
        {
            //var client = new HttpClient();
            //var response = await client.GetAsync("http://linqpad.net/foo");
            //HttpStatusCode responseStatus = response.StatusCode;

            WebClient wc = new WebClient();
            try
            {
                wc.Proxy = null;
                string s = wc.DownloadString("http://www.albahari.com/notthere");
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.NameResolutionFailure)
                    Console.WriteLine("Bad domain name");
                else if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse response = (HttpWebResponse)ex.Response;

                    ////Important
                    //HttpStatusCode foo = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), response.StatusCode.ToString());
                    //Console.WriteLine((int)foo);

                    Console.WriteLine(response.StatusDescription +" " + (int)response.StatusCode);      // "Not Found"
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        Console.WriteLine("Not there!");                  // "Not there!"
                }
                else throw;
            }
        }

        static void Naglowki()
        {
            //WebClient wc = new WebClient();
            //wc.Proxy = null;
            //wc.Headers.Add("CustomHeader", "JustPlaying/1.0");
            //wc.DownloadString("http://www.oreilly.com");
            //foreach (string name in wc.ResponseHeaders.Keys)
            //{
            //    Console.WriteLine(name + "=" + wc.ResponseHeaders[name]);
            //}

            //default request headers stotsowany do nagłówków któe są stosowane we wszystkich żądanicach
            var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("VisualStudio","2017"));
            client.DefaultRequestHeaders.Add("CustomerHeader","VisualStudio/2017");
        }

        static void CiagTekstowyZapytania()
        {
            WebClient wc = new WebClient {Proxy = null};
            wc.QueryString.Add("q", "WebClient");
            wc.QueryString.Add("hl","pl");
            wc.DownloadFile("http://www.google.pl/search","result.html");
            System.Diagnostics.Process.Start("result.html");

            //jeśli chcemy to samo za pomocą httpclient lub webrequest 
            string resultURi = "http://www.google.pl/search?q=WebClient&hl=pl";

            //jeśłi istnieje prawdopodobieństwo wystąpienia symboli lub spacji można wykorzystac metodę EscapeDataString()
            string search = Uri.EscapeDataString("(WebClient OR HttpClient)");
            string language = Uri.EscapeDataString("pl");
            string requestURI = "http://www.google.com/search?q=" + search +
                                "&hl=" + language;
            //check AntiXSS
        }

        static async  void PrzekazanieDanychFormularza()
        {
            ////WebClient
            //WebClient wc = new WebClient();
            //wc.Proxy = null;

            //var data = new System.Collections.Specialized.NameValueCollection();
            //data.Add("Name", "Joe Albahari");
            //data.Add("Company", "O'Reilly");
            //byte[] result = wc.UploadValues("http://www.albahari.com/EchoPost.aspx",
            //    "POST", data);
            //Console.WriteLine(Encoding.UTF8.GetString(result));

            ////WebRequest
            //var req = WebRequest.Create("http://www.albahari.com/EchoPost.aspx");
            //req.Proxy = null;
            //req.Method = "POST";
            //req.ContentType = "application/x-www-form-urlencoded";
            //string reqString = "Name=Joe+Albahari&Company=O'Reilly";
            //byte[] reqData = Encoding.UTF8.GetBytes(reqString);
            //req.ContentLength = reqData.Length;
            //using (Stream reqStream = req.GetRequestStream())
            //    reqStream.Write(reqData, 0, reqData.Length);

            //using (WebResponse res = req.GetResponse())
            //using (Stream resSteam = res.GetResponseStream())
            //using (StreamReader sr = new StreamReader(resSteam))
            //    Console.WriteLine(sr.ReadToEnd());

            //HttpClient
            string uri = "http://www.albahari.com/EchoPost.aspx";
            var client = new HttpClient();
            var dict = new Dictionary<string, string>
            {
                { "Name", "Joe Albahari" },
                { "Company", "O'Reilly" }
            };
            var values = new FormUrlEncodedContent(dict);
            var response = await client.PostAsync(uri, values);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        static void MechanizmCookies()
        {
            ////WebRequest
            //CookieContainer cc = new CookieContainer();
            //var request = (HttpWebRequest)WebRequest.Create("http://www.google.com");
            //request.Proxy = null;
            //request.CookieContainer = cc;
            //using (var response = (HttpWebResponse)request.GetResponse())
            //{
            //    foreach (Cookie c in response.Cookies)
            //    {
            //        Console.WriteLine(" Name:   " + c.Name);
            //        Console.WriteLine(" Value:  " + c.Value);
            //        Console.WriteLine(" Path:   " + c.Path);
            //        Console.WriteLine(" Domain: " + c.Domain);
            //    }
            //    // Read response stream...
            //}

            //HttpClient
            var cc = new CookieContainer();
            var handler = new HttpClientHandler();
            handler.CookieContainer = cc;
            var client = new HttpClient(handler);

            //WebClient nie obsługuje cookies
        }

        static async void UwierzytelnianieNaPodstawieFormularzy()
        {
            //string loginUri = "http://www.somesite.com/login";
            //string username = "username";   // (Your username)
            //string password = "password";   // (Your password)
            //string reqString = "username=" + username + "&password=" + password;
            //byte[] requestData = Encoding.UTF8.GetBytes(reqString);
            //CookieContainer cc = new CookieContainer();
            //var request = (HttpWebRequest)WebRequest.Create(loginUri);
            //request.Proxy = null;
            //request.CookieContainer = cc;
            //request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentLength = requestData.Length;
            //using (Stream s = request.GetRequestStream())
            //    s.Write(requestData, 0, requestData.Length);
            //using (var response = (HttpWebResponse)request.GetResponse())
            //    foreach (Cookie c in response.Cookies)
            //        Console.WriteLine(c.Name + " = " + c.Value);
            //// We're now logged in. As long as we assign cc to subsequent WebRequest
            //// objects, we’ll be treated as an authenticated user.

            //HttpClient
            string loginUri = "http://www.somesite.com/login";
            string username = "username";
            string password = "password";

            CookieContainer cc = new CookieContainer();
            var handler = new HttpClientHandler { CookieContainer = cc };

            var request = new HttpRequestMessage(HttpMethod.Post, loginUri);
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "username", username },
                { "password", password }
            });

            var client = new HttpClient(handler);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        static void UtworzenieSerweraHttp()
        {
            //ListenAsync();                           // Start server
            //WebClient wc = new WebClient();          // Make a client request.
            //Console.WriteLine(wc.DownloadString
            //    ("http://localhost:51111/MyApp/Request.txt"));

            // Listen on port 51111, serving files in d:\webroot:
            var server = new WebServer("http://localhost:51111/", @"d:\test\");
            try
            {
                server.Start();
                Console.WriteLine("Server running... press Enter to stop");
                Console.ReadLine();
            }
            finally { server.Stop(); }
        }
        static async void ListenAsync()
        {
            //HttpListener dzięki niemu wiele aplikacji może nasłuchiwać na tym samym Ip i portcie , ale kazfa musi zarejestrować inny prefix adresu
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:51111/MyApp/");  // Listen on
            listener.Start();                                         // port 51111.

            // Await a client request:
            HttpListenerContext context = await listener.GetContextAsync();

            // Respond to the request:
            string msg = "You asked for: " + context.Request.RawUrl;
            context.Response.ContentLength64 = Encoding.UTF8.GetByteCount(msg);
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            using (Stream s = context.Response.OutputStream)
            using (StreamWriter writer = new StreamWriter(s))
                await writer.WriteAsync(msg);

            listener.Stop();
        }

        static void UzycieFtp()
        {
            //lokalny server ftp apache

            //WebClient wc = new WebClient();
            //wc.Proxy = null;
            //wc.Credentials = new NetworkCredential("Piotr", "piotr");
            //wc.BaseAddress = "ftp://localhost/bin/Debug/";
            ////wc.UploadString("tempfile.txt", "hello!");
            //Console.WriteLine(wc.DownloadString("tempfile.txt"));   //odczyt z pliku z ftp


            ////wyswietlenie zawartosci katalogu
            //var req = (FtpWebRequest)WebRequest.Create("ftp://localhost/bin/Debug/");
            //req.Proxy = null;
            //req.Credentials = new NetworkCredential("Piotr", "piotr");
            //req.Method = WebRequestMethods.Ftp.ListDirectory;

            //using (WebResponse resp = req.GetResponse())
            //using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
            //    Console.WriteLine(reader.ReadToEnd());

            ////GetFileSize
            ////var req = (FtpWebRequest)WebRequest.Create("ftp://localhost/bin/Debug/tempfile.txt");
            //var req = WebRequest.Create("ftp://localhost/bin/Debug/tempfile.txt") as FtpWebRequest;
            //req.Proxy = null;
            //req.Credentials = new NetworkCredential("Piotr", "piotr");
            //req.Method = WebRequestMethods.Ftp.GetFileSize;
            //using (WebResponse resp = req.GetResponse())
            //    Console.WriteLine(resp.ContentLength);

            ////LastModified
            //var req = WebRequest.Create("ftp://localhost/bin/Debug/tempfile.txt") as FtpWebRequest;
            //req.Proxy = null;
            //req.Credentials = new NetworkCredential("Piotr", "piotr");
            //req.Method = WebRequestMethods.Ftp.GetDateTimestamp;
            //using (var resp = (FtpWebResponse)req.GetResponse())
            //    Console.WriteLine(resp.LastModified);


            ////Rename
            //var req = (FtpWebRequest)WebRequest.Create("ftp://localhost/bin/Debug/tempfile.txt");
            //req.Proxy = null;
            //req.Credentials = new NetworkCredential("Piotr", "piotr");
            //req.Method = WebRequestMethods.Ftp.Rename;
            //req.RenameTo = "deleteme.txt";
            //req.GetResponse().Close();        // Perform the rename

            //Delete file
            var req = (FtpWebRequest)WebRequest.Create("ftp://localhost/bin/Debug/tempfile.txt");
            req.Proxy = null;
            req.Credentials = new NetworkCredential("Piotr", "piotr");
            req.Method = WebRequestMethods.Ftp.DeleteFile;
            req.GetResponse().Close();        // Perform the deletion
        }

        static async void UzycieDns()
        {
            foreach (IPAddress address in Dns.GetHostAddresses("albahari.com"))
            {
                Console.WriteLine(address.ToString());
            }

            IPHostEntry enntry = Dns.GetHostEntry("205.210.42.167");
            Console.WriteLine(enntry.HostName);

            IPAddress addr = new IPAddress(new byte[] {205, 210, 42, 167});
            IPHostEntry entry = Dns.GetHostEntry(addr);
            Console.WriteLine(entry.HostName);

            //asynchroniczna wersja
            foreach (IPAddress a in await Dns.GetHostAddressesAsync("albahari.com"))
            {
                Console.WriteLine(a.ToString());
            }
        }

        static void SentEmailWithSmtp()
        {
            //SmtpClient client = new SmtpClient();
            //client.Host = "mail.myisp.net";
            //client.Send("from@adomain.com", "to@adomain.com", "subject", "body");

            ////with attachement
            //SmtpClient client = new SmtpClient();
            //client.Host = "mail.myisp.net";
            //MailMessage mm = new MailMessage();
            //mm.Sender = new MailAddress("kay@domain.com", "Kay");
            //mm.From = new MailAddress("kay@domain.com", "Kay");
            //mm.To.Add(new MailAddress("bob@domain.com", "Bob"));
            //mm.CC.Add(new MailAddress("dan@domain.com", "Dan"));
            //mm.Subject = "Hello!";
            //mm.Body = "Hi there. Here's the photo!";
            //mm.IsBodyHtml = false;
            //mm.Priority = MailPriority.High;
            //Attachment a = new Attachment("photo.jpg",System.Net.Mime.MediaTypeNames.Image.Jpeg);
            //mm.Attachments.Add(a);
            //client.Send(mm);

            //zapisywanie wiadomości w plikach .elm w katalogu
            SmtpClient c = new SmtpClient();
            c.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            c.PickupDirectoryLocation = @"D:\mail";
            c.Send("from@adomain.com", "to@adomain.com", "subject", "body");
        }

        static void UzycieTcp()
        {
            //new Thread(Server).Start();       // Run server method concurrently.
            //Thread.Sleep(500);                // Give server time to start.
            //Client();

            RunServerAsync();
            
        }
        static void Client()
        {
            using (TcpClient client = new TcpClient("localhost", 51111))
            using (NetworkStream n = client.GetStream())
            {
                BinaryWriter w = new BinaryWriter(n);
                w.Write("Hello 2");
                w.Flush();
                Console.WriteLine(new BinaryReader(n).ReadString());
            }
        }

        static void Server()     // Handles a single client request, then exits.
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 51111);
            listener.Start();
            using (TcpClient c = listener.AcceptTcpClient())
            using (NetworkStream n = c.GetStream())
            {
                string msg = new BinaryReader(n).ReadString();
                BinaryWriter w = new BinaryWriter(n);
                w.Write(msg + " right back!");
                w.Flush();                      // Must call Flush because we're not
            }                                 // disposing the writer.
            listener.Stop();
        }
        static async void RunServerAsync()
        {
            var listener = new TcpListener(IPAddress.Any, 51111);
            listener.Start();
            try
            {
                while (true)
                    Accept(await listener.AcceptTcpClientAsync());
            }
            finally { listener.Stop(); }
        }

        static async Task Accept(TcpClient client)
        {
            await Task.Yield();
            try
            {
                using (client)
                using (NetworkStream n = client.GetStream())
                {
                    byte[] data = new byte[5000];

                    int bytesRead = 0; int chunkSize = 1;
                    while (bytesRead < data.Length && chunkSize > 0)
                        bytesRead += chunkSize = await n.ReadAsync(data, bytesRead, data.Length - bytesRead);

                    Array.Reverse(data);   // Reverse the byte sequence
                    await n.WriteAsync(data, 0, data.Length);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        static void PocztaZUzyciemPop3()
        {
            using (TcpClient client = new TcpClient("mail.isp.com ", 110))
            using (NetworkStream n = client.GetStream())
            {
                ReadLine(n);                             // Read the welcome message.
                SendCommand(n, "USER username");
                SendCommand(n, "PASS password");
                SendCommand(n, "LIST");                  // Retrieve message IDs
                List<int> messageIDs = new List<int>();
                while (true)
                {
                    string line = ReadLine(n);             // e.g.  "1 1876"
                    if (line == ".") break;
                    messageIDs.Add(int.Parse(line.Split(' ')[0]));   // Message ID
                }

                foreach (int id in messageIDs)         // Retrieve each message.
                {
                    SendCommand(n, "RETR " + id);
                    string randomFile = Guid.NewGuid().ToString() + ".eml";
                    using (StreamWriter writer = File.CreateText(randomFile))
                        while (true)
                        {
                            string line = ReadLine(n);      // Read next line of message.
                            if (line == ".") break;          // Single dot = end of message.
                            if (line == "..") line = ".";    // "Escape out" double dot.
                            writer.WriteLine(line);         // Write to output file.
                        }
                    SendCommand(n, "DELE " + id);       // Delete message off server.
                }
                SendCommand(n, "QUIT");
            }
        }
        static string ReadLine(Stream s)
        {
            List<byte> lineBuffer = new List<byte>();
            while (true)
            {
                int b = s.ReadByte();
                if (b == 10 || b < 0) break;
                if (b != 13) lineBuffer.Add((byte)b);
            }
            return Encoding.UTF8.GetString(lineBuffer.ToArray());
        }

        static void SendCommand(Stream stream, string line)
        {
            byte[] data = Encoding.UTF8.GetBytes(line + "\r\n");
            stream.Write(data, 0, data.Length);
            string response = ReadLine(stream);
            if (!response.StartsWith("+OK"))
                throw new Exception("POP Error: " + response);
        }

        //dwie poniższe to TCP w srodowisku WinRT
        //async void Server()
        //{
        //    var listener = new StreamSocketListener();
        //    listener.ConnectionReceived += async (sender, args) =>
        //    {
        //        using (StreamSocket socket = args.Socket)
        //        {
        //            var reader = new DataReader(socket.InputStream);
        //            await reader.LoadAsync(4);
        //            uint length = reader.ReadUInt32();
        //            await reader.LoadAsync(length);
        //            Debug.WriteLine(reader.ReadString(length));
        //        }
        //        listener.Dispose();   // Close listener after one message.
        //    };
        //    await listener.BindServiceNameAsync("51111");
        //}
        //async void Client()
        //{
        //    using (var socket = new StreamSocket())
        //    {
        //        await socket.ConnectAsync(new HostName("localhost"), "51111",
        //            SocketProtectionLevel.PlainSocket);
        //        var writer = new DataWriter(socket.OutputStream);
        //        string message = "Hello!";
        //        uint length = (uint)Encoding.UTF8.GetByteCount(message);
        //        writer.WriteUInt32(length);
        //        writer.WriteString(message);
        //        await writer.StoreAsync();
        //    }
        //}
    }
}
