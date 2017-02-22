using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
namespace Chapter10
{
    class Program
    {
        static void Main(string[] args)
        {
            ////.Load tworzy drzewo X_DOM z pliku, URL, obiektów typu Stream, TetReader i XmlReader
            ////.Parse tworzy z łańcucha
            ////XElement config = XElement.Parse(@"<configuration><client enabled = 'true'> <timeout>30</timeout></client></configuration>");
            //XElement config = XElement.Load("config.xml"); 

            //XElement client = config.Element("client");
            //bool enabled = (bool)client.Attribute("enabled");//prawda
            //client.Attribute("enabled").SetValue(!enabled);
            ////client.Add(new XElement("retries", 3));
            //Console.WriteLine(config);
            ////Console.WriteLine(config.ToString(SaveOptions.DisableFormatting));
            ////config.Save("config.xml");//zapis do pliku



            //XElement lastName = new XElement("lastname", "Kowalski");
            //lastName.Add(new XComment("fajne nazwisko"));
            //XElement customer = new XElement("customer");
            //customer.Add(new XAttribute("id", 123));
            //customer.Add(new XElement("firstname", "Jan"));
            //customer.Add(lastName);
            //Console.WriteLine(customer.ToString());

            ////konstrukcja funkcyjna
            //XElement customer = new XElement("customer",
            //                        new XAttribute("id", 123),
            //                        new XElement("firstname", "jan"),
            //                        new XElement("lastname", "kowalski",
            //                            new XComment("fajne nazwisko")
            //                        )
            //);
            //Console.WriteLine(customer.ToString());

            //var bench = new XElement("bench", 
            //    new XElement("toolbox", new XElement("handtool", "Młotek"), new XElement("handtool", "tarnik")),
            //    new XElement("toolbox", new XElement("handtool", "Piłą"), new XElement("powertool", "pistolet na goździe")),
            //    new XComment("Ostrożnie z pistoletem na gwożdzie")
            //    );
            //foreach (XNode node in bench.Nodes())
            //    Console.WriteLine(node.ToString(SaveOptions.DisableFormatting) + ".");
            //foreach (XElement e in bench.Elements())//zwraca tylko elementy potomne
            //    Console.WriteLine(e.Name + " = " + e.Value);
            //IEnumerable<string> query = from n in bench.Elements()
            //                            where n.Elements().Any(a => a.Value == "tarnik")
            //                            select n.Value;
            //foreach (string i in query)
            //    Console.WriteLine(i);
            //IEnumerable<string> query2 = from n in bench.Elements()
            //                             from a in n.Elements()
            //                             where a.Name == "handtool"
            //                             select a.Value;
            //foreach (string i in query2)
            //    Console.WriteLine(i);
            //IEnumerable<String> query3 = from n in bench.Elements("toolbox").Elements("handtool")
            //                             select n.Value.ToUpper();
            //foreach (string i in query3)
            //    Console.WriteLine(i);
            //Console.WriteLine(bench.Descendants("handtool").Count() + Environment.NewLine);

            //IEnumerable<string> query4 = from c in bench.DescendantNodes().OfType<XComment>()
            //                             where c.Value.Contains("Ostrożnie")
            //                             orderby c.Value
            //                             select c.Value;
            //foreach (string i in query4)
            //    Console.WriteLine(i);

            XElement project = XElement.Load("../../Chapter10.csproj");
            XNamespace ns = project.Name.Namespace;
            var query = new XElement("ProjectReport",
                from compileItem in project.Elements(ns + "ItemGroup").Elements(ns + "Compile")
                let include = compileItem.Attribute("Include")
                where include != null
                select new XElement("File", include.Value)
                );
            Console.WriteLine(query);

            Console.WriteLine(Environment.NewLine);
            IEnumerable<string> paths = from compileItem in project.Elements(ns + "ItemGroup").Elements(ns + "Compile")
                                        let include = compileItem.Attribute("Include")
                                        where include != null
                                        select include.Value;
            var query2 = new XElement("Project", ExpandPaths(paths));
            Console.WriteLine(query2);


            //XmlReaderSettings settings = new XmlReaderSettings();
            //settings.IgnoreWhitespace = true;
            //using (XmlReader r = XmlReader.Create("logfile.xml", settings))
            //{

            //}



                Console.ReadKey();
        }
        static IEnumerable<XElement> ExpandPaths(IEnumerable<string> paths)
        {
            var brokenUp = from path in paths
                           let split = path.Split(new char[] { '\\' }, 2)
                           orderby split[0]
                           select new
                           {
                               name = split[0],
                               remainder = split.ElementAtOrDefault(1)
                           };
            IEnumerable<XElement> files = from b in brokenUp
                                          where b.remainder == null
                                          select new XElement("file", b.name);
            IEnumerable<XElement> folders = from b in brokenUp
                                            where b.remainder != null
                                            group b.remainder by b.name into grp
                                            select new XElement("folder",
                                                new XAttribute("name", grp.Key),
                                                ExpandPaths(grp)
                                            );
            return files.Concat(folders);
        }
    }
}
