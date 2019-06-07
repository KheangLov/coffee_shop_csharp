using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_shop
{
    public class PrintReceipt
    {
        double disPrice = 0;
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
                disPrice += (TotalPerPro * Discount) / 100;
                return disPrice;
            }
        }
    }
}
