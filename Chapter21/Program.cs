using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Chapter21
{
    class Program
    {
        static void Main(string[] args)
        {
            //póki co tylko o kryptografii
            //File.WriteAllText("myfile.txt","");
            //File.Encrypt("myfile.txt");
            //File.AppendAllText("myfile.txt", "poufne dane");

            //byte[] original = { 1, 2, 3, 4, 5 };
            //DataProtectionScope scope = DataProtectionScope.CurrentUser;
            //byte[] encrypted = ProtectedData.Protect(original, null, scope);
            //byte[] decrypted = ProtectedData.Unprotect(encrypted, null, scope);

            //hash np do zapisu haseł
            //byte[] hash;
            //using (Stream fs = File.OpenRead("checkme.doc"))
            //    hash = MD5.Create().ComputeHash(fs);           // hash is 16 bytes long
            //lub
            //byte[] data = System.Text.Encoding.UTF8.GetBytes("stRhong%pword");
            //byte[] hash2 = SHA512.Create().ComputeHash(data);
            //foreach (var b in hash2)
            //{
            //    Console.Write(b);
            //}

            //szyfrowanie symetryczne
            //byte[] key = { 145, 12, 32, 245, 98, 132, 98, 214, 6, 77, 131, 44, 221, 3, 9, 50 };
            //byte[] iv = { 15, 122, 132, 5, 93, 198, 44, 31, 9, 39, 241, 49, 250, 188, 80, 7 };//wektor inicjalizujacy nie jest tajna

            //byte[] data = { 1, 2, 3, 4, 5 };   // This is what we're encrypting.

            //using (SymmetricAlgorithm algorithm = Aes.Create())
            //using (ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv))
            //using (Stream f = File.Create("encrypted.bin"))
            //using (Stream c = new CryptoStream(f, encryptor, CryptoStreamMode.Write))
            //    c.Write(data, 0, data.Length);

            ////deszyfrowanie symetryczne
            //byte[] decrypted = new byte[5];

            //using (SymmetricAlgorithm algorithm = Aes.Create())
            //using (ICryptoTransform decryptor = algorithm.CreateDecryptor(key, iv))
            //using (Stream f = File.OpenRead("encrypted.bin"))
            //using (Stream c = new CryptoStream(f, decryptor, CryptoStreamMode.Read))
            //    for (int b; (b = c.ReadByte()) > -1;)
            //        Console.Write(b + " ");                            // 1 2 3 4 5

            //gdyby przy deszyfrowaniu użyto innego klucza niż key to zostąłby zgłoszony wyjątek cryptographicException, przechwycenie go jest jedynym spospobem na sprawdzenie poprawności klucza

            //byte[] key = new byte[16];
            //byte[] iv = new byte[16];
            //RandomNumberGenerator rand = RandomNumberGenerator.Create();
            //rand.GetBytes(key);
            //rand.GetBytes(iv);
            ////bezpieczne od random
            ////jesli programista nie zdefinuje key ani iv to zostanie wutomatycznie wygenerowany silny, odwołąc się mozna przez AES.Key i AES.IV



            ////szyfrowanie w pamięci
            //byte[] kiv = new byte[16];
            //RandomNumberGenerator.Create().GetBytes(kiv);

            //string encypted = Encrypt("Yeah!", kiv, kiv);
            //Console.WriteLine(encypted);
            //string decrypted = Decrypt(encypted, kiv, kiv);
            //Console.WriteLine(decrypted);
            ////jak się używa klucza jako iv to osłabia się siłe klucza 


            //// Use default key/iv for demo.
            //using (Aes algorithm = Aes.Create())
            //{
            //    using (ICryptoTransform encryptor = algorithm.CreateEncryptor())
            //    using (Stream f = File.Create("serious.bin"))
            //    using (Stream c = new CryptoStream(f, encryptor, CryptoStreamMode.Write))
            //    using (Stream d = new DeflateStream(c, CompressionMode.Compress))
            //    using (StreamWriter w = new StreamWriter(d))
            //        w.WriteLine("Small and secure!");
            //    //w.WriteLineAsync()

            //    using (ICryptoTransform decryptor = algorithm.CreateDecryptor())
            //    using (Stream f = File.OpenRead("serious.bin"))
            //    using (Stream c = new CryptoStream(f, decryptor, CryptoStreamMode.Read))
            //    using (Stream d = new DeflateStream(c, CompressionMode.Decompress))
            //    using (StreamReader r = new StreamReader(d))
            //        Console.WriteLine(r.ReadLine()); // Small and secure!
            //    //r.ReadLineAsync()

            //    //algorithm.Clear();//jedyny sposob aby zlikwidować obkiekt aes poza blokiem using
            //}
            //nie zaleca się trzymania kluczy szyfrowania w kodzie, lepiej losowy klcuz zapisujemy przy użyciu Windows Data Protection, lub z uzyciem klucza publicznego jesli szyfrowany jest strumienń wiadomości




            ////klucze asynchroniczne nie zaleca się do dużych wiadomosci, ale nadaje sie do przesyłu haseł lub kluczy do pózniejszej symetrycznej szyfrowania
            byte[] data2 = { 1, 2, 3, 4, 5 };   // This is what we're encrypting.

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                byte[] encrypted2 = rsa.Encrypt(data2, true);
                byte[] decrypted2 = rsa.Decrypt(encrypted2, true);
            }
            //var rsa = new RSACryptoServiceProvider(2048);


            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                File.WriteAllText("PublicKeyOnly.xml", rsa.ToXmlString(false));
                File.WriteAllText("PublicPrivate.xml", rsa.ToXmlString(true));
            }




            byte[] data = Encoding.UTF8.GetBytes("Message to encrypt");
            string publicKeyOnly = File.ReadAllText("PublicKeyOnly.xml");
            string publicPrivate = File.ReadAllText("PublicPrivate.xml");
            byte[] encrypted, decrypted;
            using (var rsaPublicOnly = new RSACryptoServiceProvider())
            {
                rsaPublicOnly.FromXmlString(publicKeyOnly);
                encrypted = rsaPublicOnly.Encrypt(data, true);

                // The next line would throw an exception because you need the private
                // key in order to decrypt:
                // decrypted = rsaPublicOnly.Decrypt (encrypted, true);
            }
            using (var rsaPublicPrivate = new RSACryptoServiceProvider())
            {
                // With the private key we can successfully decrypt:
                rsaPublicPrivate.FromXmlString(publicPrivate);
                decrypted = rsaPublicPrivate.Decrypt(encrypted, true);
            }
           



            Console.ReadKey();
        }
        //szyfrowanie w pamięci
        public static byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (Aes algorithm = Aes.Create())
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv))
                return Crypt(data, key, iv, encryptor);
        }

        public static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (Aes algorithm = Aes.Create())
            using (ICryptoTransform decryptor = algorithm.CreateDecryptor(key, iv))
                return Crypt(data, key, iv, decryptor);
        }

        static byte[] Crypt(byte[] data, byte[] key, byte[] iv,
            ICryptoTransform cryptor)
        {
            MemoryStream m = new MemoryStream();
            using (Stream c = new CryptoStream(m, cryptor, CryptoStreamMode.Write))
                c.Write(data, 0, data.Length);
            return m.ToArray();
        }


        public static string Encrypt(string data, byte[] key, byte[] iv)
        {
            return Convert.ToBase64String(
                Encrypt(Encoding.UTF8.GetBytes(data), key, iv));
        }

        public static string Decrypt(string data, byte[] key, byte[] iv)
        {
            return Encoding.UTF8.GetString(
                Decrypt(Convert.FromBase64String(data), key, iv));
        }
    }
}
