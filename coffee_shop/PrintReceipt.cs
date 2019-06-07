using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_shop
{
    public class PrintReceipt
    {
        public string ProductName { get; set; }
        public double Qty { get; set; }
        public double Price { get; set; }
        public double TotalPerPro
        {
            get
            {
                return Qty * Price;
            }
        }
        public double Discount { get; set; }
        public double DisPrice
        {
            get
            {
                return (TotalPerPro * Discount) / 100;
            }
        }
    }
}
