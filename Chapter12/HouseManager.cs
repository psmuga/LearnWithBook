using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter12
{

    //wzorzec zwalniania zasobów na żądanie użytkownika
    internal class HouseManager :IDisposable
    {
        private readonly bool _checkMailOnDispose;

        private void CheckMail()
        {
            Console.WriteLine("Sprawdzam Mail");
        }

        private void LockDoor()
        {
            Console.WriteLine("Zamykam drzwi");
        }

        public HouseManager(bool checkMailOnDispose)
        {
            _checkMailOnDispose = checkMailOnDispose;
        }

        public void Dispose()
        {
            if(_checkMailOnDispose) CheckMail();
            LockDoor();
        }
    }
}
