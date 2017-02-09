using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter6_2
{
    //nie ważna jest czy prostokat ma a*b czy b*a
    class Area : IEquatable<Area>
    {
        public readonly int Measure1;
        public readonly int Measure2;
        public Area(int a, int b)
        {
            Measure1 = Math.Min(a, b);
            Measure2 = Math.Max(a, b);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Area)) return false;
            return Equals((Area)obj);//wywołanie ponizszej metody
        }
        public bool Equals(Area other) => Measure1 == other.Measure1 && Measure2 == other.Measure2;

        public override int GetHashCode() => Measure2 * 31 + Measure1;

        public static bool operator == (Area a1, Area a2) => a1.Equals(a2);
        public static bool operator != (Area a1, Area a2) => !a1.Equals(a2);
    }
}
