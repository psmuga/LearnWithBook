using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chapter17
{
    //silnik kontraktu danych, niejawnie
    //[DataContract]
    //[DataContract(Name = "Candidate")]//zmiana nazwy kontraktu danych, mozna tak też zmienic nazwy skaldowych danych
    [DataContract,KnownType(typeof(Student))]
    public class Person
    {
        [DataMember(IsRequired = true, Order = 0)] public string Name; //pole wymagane podczas serializacji
        //[DataMember] public int Age;
        [DataMember(Order = 1)] public int Age;//ordery nie są wymagane
        //[DataMember(Order = 1,EmitDefaultValue = false)] public int Age; //zaoszczędzenie miejsca, jesli wartosc domyslna to nei jest zapisywana

        //jesli nie jest potrzebne zapewniecnei współdziałenia z jakimkolwiek innym komponentem to nie dawać Order w ogóle
    }
}
