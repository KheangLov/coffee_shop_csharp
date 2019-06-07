using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_shop
{
    public class StaticPrintReceipt
    {
        public string Number { get; set; }
        public int WaitingNumber { get; set; }
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string EmployeeName { get; set; }
        public string Date { get; set; }
        public double Discount { get; set; }
    }
}
