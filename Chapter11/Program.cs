using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Chapter11
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlReaderSettings r_settings = new XmlReaderSettings();
            r_settings.IgnoreComments = true;
            r_settings.IgnoreWhitespace = true;


            XmlWriterSettings w_settings = new XmlWriterSettings();
            w_settings.Indent = true;//ustawia wcięcia

            Contacts abc = new Contacts();
            abc.Suppliers.Add(new Supplier() { Name = "Dostawca" });
            abc.Customers.Add(new Customer() { firstName = "Piotr", lastName = "Smuga", ID = 1});
            abc.Customers.Add(new Customer() { firstName = "Marcin", lastName = "Smuga", ID = 2 });
            abc.Customers.Add(new Customer() { firstName = "Dawid", lastName = "Smuga" });

            //using (XmlWriter writer = XmlWriter.Create("..\\..\\foo.xml", w_settings))
            //{
            //    writer.WriteStartElement("contacts");
            //    abc.WriteXml(writer);
            //    writer.WriteEndElement();
            //}

            //Contacts cba;
            //using (XmlReader reader = XmlReader.Create("..\\..\\foo.xml", r_settings))
            //{
            //    cba = new Contacts(reader);

            //}
            //Console.WriteLine(cba.ToString());
            //Console.WriteLine("End");


            XElement test = XElement.Load("..\\..\\foo.xml");
            Console.WriteLine(test);
            Console.WriteLine(Environment.NewLine);
            //using (XmlReader r = XmlReader.Create("..\\..\\foo.xml", r_settings))
            //{
            //    while(r.Read())
            //    {
            //        Console.Write(new string(' ', r.Depth * 2));
            //        Console.WriteLine(r.NodeType);
            //    }
            //}
                Console.WriteLine(Environment.NewLine);
            var query = from n in test.Elements("customer")
                        where n.Attributes().Any(a => a.Name == "id")
                        from ala in n.Elements("firstname")                               
                        select ala.Value;      //wyswietla tylko imina tych co maja id z drzewa                    
            foreach (var i in query)
                Console.WriteLine(i);

            //konstrukcja funkcyjna
            Customer[] ak = { new Customer(), new Customer() };
            XElement q = new XElement("customers",
                from c in ak
                select
                    new XElement("customer",c.ID  == null ? null: new XAttribute("id", c.ID),
                        new XElement("firstname", c.firstName),
                        new XElement("lastname", c.lastName,
                            new XComment("fajne nazwisko"))
                    )
                );
            q.AddAnnotation("Witaj");//lepiej umiwescic obiekt własnej klasy
            q.AddAnnotation(new CustomData { Message = "Hello World!" });
            Console.WriteLine(q.ToString());
            Console.WriteLine(q.Annotation<string>());//Witaj
            Console.WriteLine(q.Annotations<CustomData>().Last().Message);//Hello world

            //XText jak się tworzy to jako wartosc dodaja stringi zamiast nadpisywac




            Console.ReadKey();
        }
    }
    class CustomData
    {
        internal string Message;
    }

    public class Contacts
    {
        public IList<Customer> Customers = new List<Customer>();
        public IList<Supplier> Suppliers = new List<Supplier>();
        public Contacts() { }
        public Contacts(XmlReader r)
        {
            ReadXml(r);
        }

        public override string ToString()
        {
            Console.WriteLine("Klienci:");
            foreach(Customer c in Customers)
            {
                Console.WriteLine("\t" + c.ToString());
            }
            Console.WriteLine("Dostawcy:");
            foreach (Supplier s in Suppliers)
            {
                Console.WriteLine("\t" + s.ToString());
            }
            return "";
        }
        public void ReadXml(XmlReader r)
        {
            bool isEmpty = r.IsEmptyElement;
            r.ReadStartElement();
            if (isEmpty) return;
            while(r.NodeType == XmlNodeType.Element)
            {
                if (r.Name == Customer.XmlName)
                    Customers.Add(new Customer(r));
                else if (r.Name == Supplier.XmlName)
                    Suppliers.Add(new Supplier(r));
                else
                    throw new XmlException("Nieoczekiwany węzeł: " + r.Name);
            }
            r.ReadEndElement();
        }
        public void WriteXml(XmlWriter w)
        {
            foreach(Customer c in Customers)
            {
                w.WriteStartElement(Customer.XmlName);
                c.WriteXml(w);
                w.WriteEndElement();
            }
            foreach(Supplier s in Suppliers)
            {
                w.WriteStartElement(Supplier.XmlName);
                s.WriteXml(w);
                w.WriteEndElement();
            }
        }
    }
    public class Customer
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public static  string XmlName = "customer";
        public int? ID;
        public Customer() {
            firstName = "";
            lastName = "";
            //ID = 1;
        }
        public Customer(XmlReader r)
        {
            ReadXml(r);
        }
        public override string ToString()
        {
            string help = "";
            if(ID.HasValue)
            help = " Id: " + ID;
            return firstName + " " + lastName + help; ;
        }
        public void ReadXml(XmlReader r)
        {
            if (r.MoveToAttribute("id"))
                ID = r.ReadContentAsInt();
            r.ReadStartElement();
            firstName = r.ReadElementContentAsString("firstname", "");
            lastName = r.ReadElementContentAsString("lastname", "");
            r.ReadEndElement();
        }
        public void WriteXml(XmlWriter w)
        {
            if (ID.HasValue) w.WriteAttributeString("id", "", ID.ToString());
            w.WriteElementString("firstname", firstName);
            w.WriteElementString("lastname", lastName);
        }
    }
    public class Supplier
    {
        public string Name { get; set; }
        public static string XmlName = "supplier";
        public Supplier() { }
        public Supplier(XmlReader r)
        {
            ReadXml(r);
        }
        public override string ToString()
        {
            return Name;
        }
        public void ReadXml(XmlReader r)
        {
            r.ReadStartElement();
            Name = r.ReadElementContentAsString("name", "");
            r.ReadEndElement();
        }
        public void WriteXml(XmlWriter w)
        {
            w.WriteElementString("name", Name);
        }
    }
}
