using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace coffee_shop
{
    public partial class receipt_form : Form
    {
        public receipt_form()
        {
            InitializeComponent();
        }

        StringCapitalize sc = new StringCapitalize();

        public void QueryReceipt()
        {
            DataConn.Connection.Open();
            string sql = "SELECT number, COUNT(number) AS count_number FROM receipts GROUP BY number;";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while(sqlr.Read())
            {
                string[] receipt_info = { sqlr["number"].ToString(), sqlr["count_number"].ToString() };
                ListViewItem item = new ListViewItem(receipt_info);
                lvReceipt.Items.Add(item);
            }
            sqld.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void receipt_form_Load(object sender, EventArgs e)
        {
            btnPrint.Enabled = false;
            QueryReceipt();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if(lvReceipt.SelectedItems.Count > 0)
            {
                List<PrintReceipt> printRec = new List<PrintReceipt>();
                StaticPrintReceipt spr = new StaticPrintReceipt();
                ListViewItem item = lvReceipt.SelectedItems[0];
                string recNum = item.SubItems[0].Text;
                DataConn.Connection.Open();
                string sql = "SELECT * FROM receipts WHERE number = '" + recNum + "';";
                SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
                SqlDataReader sqlr = sqld.ExecuteReader();
                while(sqlr.Read())
                {
                    spr.Number = sqlr["number"].ToString();
                    spr.WaitingNumber = int.Parse(sqlr["waiting_number"].ToString());
                    spr.CompanyName = sqlr["company_name"].ToString();
                    spr.BranchName = sqlr["branch_name"].ToString();
                    spr.EmployeeName = sqlr["employee_name"].ToString();
                    spr.Date = sqlr["date"].ToString();
                    PrintReceipt pr = new PrintReceipt();
                    pr.ProductName = sqlr["product_name"].ToString().ToUpper();
                    pr.Qty = double.Parse(sqlr["qty"].ToString());
                    pr.Price = double.Parse(sqlr["price"].ToString());
                    pr.Discount = double.Parse(sqlr["discount"].ToString());
                    printRec.Add(pr);
                }
                sqld.Dispose();
                sqlr.Close();
                DataConn.Connection.Close();
                new ReceiptPrint(spr, printRec).ShowDialog();
            }
        }

        private void lvReceipt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lvReceipt.SelectedItems.Count > 0)
            {
                btnPrint.Enabled = true;
            }
        }
    }
}
