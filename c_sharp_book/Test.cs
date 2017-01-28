

using System;
using System.Dynamic;
using c_sharp_book;

class Test : IInterface1
{
    public decimal CurrentPrice{get;set;}= 900;    //wlasnosc

    void Metoda(int x) => Console.WriteLine(x.ToString()); //metoda

    private decimal _x;

    private int Zmienna => 1; //własnosć skrot od get{return 1}

    public decimal X
    {
        get { return _x; }
        private set { _x = Math.Round(value, 2); }
    }
    [Flags]
    public enum BorderSide
    {
        None = 0,
        Left =1,
        Right =2,
        Top = 4,
        Botoom = 8
    }

    public void Foo()
    {
        BorderSide a = BorderSide.Right;
        Console.WriteLine(a.ToString()); // Right
        //gdy nie ma określonego flags to można sprawdzić czy jest okreslone .IsDefined()
    }
}

