using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Chapter17
{
    class Program
    {
        static void Main(string[] args)
        {
            //UzycieSerializacji();

            //SerializacjaPodklas();

            //SerializacjaBinarna();

            UsingXmlSerializer();

            Console.ReadKey();
        }

        private static void SerializacjaPodklas()
        {
            Person p = new Person() {Age = 12, Name = "Witek"};
            Student s = new Student() {Age = 12, Name = "Witek"};
            Person p2 = DeepClone(p);
            //Student s2 = DeepClone(s) as Student; rzuca wyjątkiem bo niepoprawny typ, bezpiecześńtwo
            //rozwiazeniem moze byc deepcolone2
            Student s2 = DeepClone2(s) as Student;
            //lub w person zmiana datacontract , knowntype
            Student s3 = DeepClone(s) as Student;
            
        }

        static Person DeepClone(Person p)
        {
            DataContractSerializer ds = new DataContractSerializer(typeof(Person));
            MemoryStream stream = new MemoryStream();
            ds.WriteObject(stream, p);
            stream.Position = 0;
            return (Person)ds.ReadObject(stream);
        }
        static Person DeepClone2(Person p)
        {
            DataContractSerializer ds = new DataContractSerializer(typeof(Person),new Type[]{typeof(Student)});
            MemoryStream stream = new MemoryStream();
            ds.WriteObject(stream, p);
            stream.Position = 0;
            return (Person)ds.ReadObject(stream);
        }

        private static void UzycieSerializacji()
        {
            //netdatacontractserializer zawsze sprawdza odwołania, w przeciwieństwie do datacontactserializer, w którym można wymusić sprawdzanie odwołan np:
            //var ds = new DataContractSerializer(typeof(Person),null,1000,false,true,null);//maks 1000 odwołań może wystąpić jak wiecej to błąd


            //jawna serializacja
            Person p = new Person {Age = 22, Name = "Kamil"};
            var ds = new DataContractSerializer(typeof(Person));
            //var ns = new NetDataContractSerializer();  //uzycie takie same
            using (Stream s = File.Create("person.xml"))
            {
                ds.WriteObject(s, p);
            }
            Person p2;
            using (Stream s = File.OpenRead("person.xml"))
            {
                p2 = ds.ReadObject(s) as Person;
            }
            Console.WriteLine(p2.Name + " " + p2.Age);


            XmlWriterSettings
                settings = new XmlWriterSettings() {Indent = true}; //żadanie wcięcia danych wyjsciowych co poprawia czytelnosc
            using (XmlWriter w = XmlWriter.Create("person2.xml", settings))
            {
                ds.WriteObject(w, p);
            }
            System.Diagnostics.Process.Start("person2.xml");
        }

        //Serializacja binarna
        static void SerializacjaBinarna()
        {
            PersonB p = new PersonB() { Name = "George", Age = 25 };
            IFormatter formatter = new BinaryFormatter();
            using (FileStream s = File.Create("serialized.bin"))
                formatter.Serialize(s, p);
            using (FileStream s = File.OpenRead("serialized.bin"))
            {
                PersonB p2 = (PersonB)formatter.Deserialize(s);
                Console.WriteLine(p2.Name + " " + p.Age);     // George 25
            }
        }

        static void UsingXmlSerializer()
        {
            Person p = new Person();
            p.Name = "Stacey"; p.Age = 30;

            XmlSerializer xs = new XmlSerializer(typeof(Person));

            using (Stream s = File.Create("person.xml"))
                xs.Serialize(s, p);

            Person p2;
            using (Stream s = File.OpenRead("person.xml"))
                p2 = (Person)xs.Deserialize(s);

            Console.WriteLine(p2.Name + " " + p2.Age);   // Stacey 30

            //ma atrybut XmlIgnore
            //Kolejnosc taka jak kolejnosc zapisu w klasie
            //nie ma opcji OnDeserializing, natomiast bierze pod uwage konstruktory i przypisania wartosci
            //[XmlInclude(typeof(Student))] gdy chcemy podtyp uzyc do serializacji
            //nie dba o zachowanie odwołań do obiektów
            //kolekcje są serializowane bez ingerencji użytkownika, łatwe
            //prywatnych nie da sie serializować tym silnikiem

        }
    }

    [DataContract] public class Student : Person
    {
        
    }

    //tolerancja na wersje, dane które nie są w tej wersji przechowywane są w czarnym pudełku
    [DataContract]
    public class Person2 : IExtensibleDataObject
    {
        [DataMember] public string Name;
        [DataMember] public int Age;

        ExtensionDataObject IExtensibleDataObject.ExtensionData { get; set; }
    }


    [DataContract]
    public class Person3
    {
        [DataMember(Name = "Addresses")]
        List<Address> _addresses;
        public IList<Address> Addresses { get { return _addresses; } }
    }
    [DataContract]
    public class Address
    {
        [DataMember] public string Street, Postcode;
    }


    [CollectionDataContract(ItemName = "Residence")]
    public class AddressList : Collection<Address> { }

    [DataContract]
    public class Person4
    {
        
    [DataMember] public AddressList Addresses;
    }


    [CollectionDataContract(ItemName = "Entry",
        KeyName = "Kind",
        ValueName = "Number")]
    public class PhoneNumberList : Dictionary<string, string> { }
    [DataContract]
    public class Person5
    {
    [DataMember] public PhoneNumberList PhoneNumbers;
    }

    class Test
    {
        public DateTime DateOfBirth;

        [DataMember] public bool Confidential;

        [DataMember(Name = "DateOfBirth", EmitDefaultValue = false)]
        DateTime? _tempDateOfBirth;

        [OnSerializing]
        void PrepareForSerialization(StreamingContext sc)
        {
            if (Confidential)
                _tempDateOfBirth = DateOfBirth;
            else
                _tempDateOfBirth = null;
        }
    }

    [DataContract]
    class Test2
    {
        private bool _editable = true;
        public Test2()
        {
            _editable = true;
        }

        [OnDeserializing]
        void Init(StreamingContext sc)
        {
            _editable = true; // gdyby tego nie było to dmyslnie bool jest false a serializacja pomija stanadardowe metody inicjalizacji
        }
    }


    [Serializable]
    public sealed class PersonB
    {
        public string Name;
        public int Age;
        [NonSerialized] public int NonS = 5;

        public PersonB()
        {
            NonS = 5;
        }

        [OnDeserializing]
        void OnDeserializing(StreamingContext context)
        {
            NonS = 5; // po deserializacji tylko tutaj Nons bedzie się równać 5, bo deserializacja pomija wszystkie konstruktory czy przypisania
        }
    }
    [Serializable]
    public sealed class Team
    {
        public string Name;
        //[OptionalField(VersionAdded = 2)]public string Name;//2,3,4 ... inkrementowane z kazdą kolejna zmiana
        Person[] _playersToSerialize;

        [NonSerialized] public List<Person> Players = new List<Person>();

        [OnSerializing]
        void OnSerializing(StreamingContext context)
        {
            _playersToSerialize = Players.ToArray();
        }

        [OnSerialized]
        void OnSerialized(StreamingContext context)
        {
            _playersToSerialize = null;   // Allow it to be freed from memory
        }

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            Players = new List<Person>(_playersToSerialize);
        }


    }


    [Serializable]
    public class Team2: ISerializable
    {
        public string Name;
        public List<Person> Players;

        public virtual void GetObjectData(SerializationInfo si,
            StreamingContext sc)
        {
            si.AddValue("Name", Name);
            si.AddValue("PlayerData", Players.ToArray());
        }

        public Team2() { }

        protected Team2(SerializationInfo si, StreamingContext sc)
        {
            Name = si.GetString("Name");

            // Deserialize Players to an array to match our serialization:
            Person[] a = (Person[])si.GetValue("PlayerData", typeof(Person[]));

            // Construct a new List using this array:
            Players = new List<Person>(a);
        }
    }
}
