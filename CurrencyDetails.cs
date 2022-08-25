using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CurrenciesDBService
{
    public class CurrencyDetails
    {
        private DateTime Date;
        private string Currency;
        private double Buying;
        private double Selling;

        public string Currency_
        {
            get { return Currency; }
            set { Currency = value; }
        }
        public double Buying_
        {
            get { return Buying; }
            set { Buying = value; }
        }
        public double Selling_
        {
            get { return Selling; }
            set { Selling = value; }
        }
        public DateTime Date_
        {
            get { return Date; }
            set { Date = value; }
        }
    }
}