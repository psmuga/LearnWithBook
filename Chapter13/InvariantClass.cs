using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter13
{
    class InvariantClass
    {
        private int _x, _y;

        //metoda ta moze przechowywać wywołanie metod invariant
        //oznajmia ze w całej klasie x i y nie może być mniejsze od zera
        [ContractInvariantMethod]
        void ObjectInvariant()
        {
            Contract.Invariant(_x>=0);
            Contract.Invariant(_y>= 0);
        }

        public int X
        {
            get { return _x; }
            set { _x = value; } 
        }
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public void Test()
        {
            _x = 3;
            _y = 3;
        }
    }
}
