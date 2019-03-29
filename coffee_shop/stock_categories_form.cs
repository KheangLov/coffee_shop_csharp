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
    public partial class stock_categories_form : Form
    {
        public stock_categories_form()
        {
            InitializeComponent();
        }
        StockCategory my_stockcate = new StockCategory();
        MyInter inter;
        StringCapitalize sc = new StringCapitalize();

        void ClearTextBoxes(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                TextBox textBox = child as TextBox;
                if (textBox == null)
                    ClearTextBoxes(child);
                else
                    textBox.Text = string.Empty;
            }
        }

        private void loadComboBranch()
        {
            string sql = "SELECT id, name FROM branches;";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while(sqlr.Read())
            {
                cbBranch.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void addBranch()
        {
            string sql = "SELECT id, name FROM branches WHERE LOWER(name) = '" + cbBranch.Text.Trim() +"';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                my_stockcate.BranchId = int.Parse(sqlr["id"].ToString());
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void stock_categories_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }

        private void stock_categories_form_Load(object sender, EventArgs e)
        {
            DataConn.Connection.Open();
            MyInter stockcat_inter = my_stockcate;
            inter = stockcat_inter;
            loadComboBranch();
            cbBranch.SelectedIndex = 0;
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            my_stockcate.Name = txtName.Text.Trim();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            my_stockcate.Name = txtName.Text.Trim();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                inter.insert();
                MessageBox.Show("Insert successful!");
                ClearTextBoxes(groupBox1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtDesc_Leave(object sender, EventArgs e)
        {
            my_stockcate.Descriptions = txtDesc.Text.Trim();
        }

        private void txtDesc_TextChanged(object sender, EventArgs e)
        {
            my_stockcate.Descriptions = txtDesc.Text.Trim();
        }

        private void cbBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            addBranch();
        }
    }
}
