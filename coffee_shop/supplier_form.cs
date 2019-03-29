using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace coffee_shop
{
    public partial class supplier_form : Form
    {
        public supplier_form()
        {
            InitializeComponent();
        }
        Supplier my_supplier = new Supplier();
        MyInter inter;

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void QuerySupplier()
        {
            string sql = @"SELECT suppliers.*, companies.name AS company_name,branches.name AS branch_name,stocks.name AS stock_name
                FROM suppliers
                INNER JOIN companies ON suppliers.company_id = companies.id
                INNER JOIN branches ON suppliers.branch_id = branches.id
                INNER JOIN stocks ON suppliers.stock_id = stocks.id; ";
            SqlCommand com = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = com.ExecuteReader();
            while (sqlr.Read())
            {
                string[] supplier_info = { sqlr["name"].ToString(), sqlr["address"].ToString(), sqlr["phone"].ToString(), sqlr["email"].ToString(), sqlr["company_name"].ToString(), sqlr["branch_name"].ToString(), sqlr["stock_name"].ToString(), };
                ListViewItem item = new ListViewItem(supplier_info);
                lvSupplier.Items.Add(item);
            }
            com.Dispose();
            sqlr.Close();
        }

        private void loadCombos(string tblName)
        {
            string sql = "SELECT * FROM "+ tblName.ToLower() +";";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                if (tblName.ToLower() == "companies")
                    cbCompany.Items.Add(sqlr["name"].ToString());
                else if (tblName.ToLower() == "branches")
                    cbBranch.Items.Add(sqlr["name"].ToString());
                else if (tblName.ToLower() == "stocks")
                    cbStock.Items.Add(sqlr["name"].ToString());
                else
                    MessageBox.Show("Not found!");
            }
            sqlr.Close();
            sqld.Dispose();
        }

        private void supplier_form_Load(object sender, EventArgs e)
        {
            try
            {
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                DataConn.Connection.Open();
                MyInter supplier_inter = my_supplier;
                inter = supplier_inter;
                QuerySupplier();
                loadCombos("companies");
                loadCombos("branches");
                loadCombos("stocks");
                cbCompany.SelectedIndex = 0;
                cbBranch.SelectedIndex = 0;
                cbStock.SelectedIndex = 0;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtName_Leave(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }
    }
}
