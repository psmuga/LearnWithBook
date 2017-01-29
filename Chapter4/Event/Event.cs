using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter4.Event
{
    //public delegate void PriceChangedHandler(decimal oldPrice, decimal newPrice);
    //class Stock
    //{
    //    private string symbol;
    //    private decimal price;

    //    public Stock(string symbol)
    //    {
    //        this.symbol = symbol;
    //    }

    //    public event PriceChangedHandler PriceChanged;

    //    public decimal Price
    //    {
    //        get { return price;}
    //        set
    //        {
    //            if (price == value)
    //            {
    //                return;
    //            }
    //            decimal oldPrice = price;
    //            price = value;
    //            if (PriceChanged!= null)
    //            {
    //                PriceChanged(oldPrice, price);
    //            }
    //            //PriceChanged?.Invoke(oldPrice, price); // to samo co ten cały if wyżej, i nawet bezpieczniejsze
    //        }
    //    }
    //}

    public class PriceChangedEventArgs : EventArgs
    {
        public readonly decimal LastPrice;
        public readonly decimal NewPrice;

        public PriceChangedEventArgs(decimal lastPrice, decimal newPrice)
        {
            LastPrice = lastPrice;
            NewPrice = newPrice;
        }
    }

    public class Stock
    {
        private string symbol;
        private decimal price;
        public Stock(string symbol)
        {
            this.symbol = symbol;
        }

        public event EventHandler<PriceChangedEventArgs> PriceChanged;

        protected virtual void OnPriceChanged(PriceChangedEventArgs e)
        {
            PriceChanged?.Invoke(this,e);
        }

        public decimal Price
        {
            get { return price;}
            set
            {
                if (price == value) return;
                decimal oldPrice = price;
                price = value;
                OnPriceChanged(new PriceChangedEventArgs(oldPrice,price));
            }
        }
    }

}
