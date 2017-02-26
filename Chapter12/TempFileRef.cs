using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter12
{

    //nie rozumiem tego

    //zapisuje informacje o błedzie i dodaje go to kolejki bezpiecznej
    //daje obiektowi kolejne odwołanie i gwarantuje że pozostanie aktywny aż do ostatecznego usunięcia go z kolejki
    public class TempFileRef
    {
        public static  ConcurrentQueue<TempFileRef>_failedDeletions = new ConcurrentQueue<TempFileRef>();
        public readonly string FilePath;
        public  Exception DeletionError { get; private set; }

        public TempFileRef(string filePath)
        {
            FilePath = filePath;
           File .WriteAllText(FilePath,"ala ma kota");
           
        }

        ~TempFileRef()
        {
            try
            {
                File.Delete(FilePath);
                throw new Exception("nie wiem");
            }
            catch (Exception e)
            {
                DeletionError = e;
               
                _failedDeletions.Enqueue(this);
              
            }
        }

    }
}
