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
    public partial class product_category_form : Form
    {
        public product_category_form()
        {
            InitializeComponent();
        }
        ProductCategory procate = new ProductCategory();
        MyInter inter;
        StringCapitalize sc = new StringCapitalize();
        int proCateId;

        private void QueryProCate()
        {
            DataConn.Connection.Open();
            string sql = @"SELECT * FROM product_categories;";
            SqlCommand com = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = com.ExecuteReader();
            while (sqlr.Read())
            {
                string[] pro_cate_info = { sc.ToCapitalize(sqlr["name"].ToString()), sqlr["descriptions"].ToString() };
                ListViewItem item = new ListViewItem(pro_cate_info);
                lvProCate.Items.Add(item);
            }
            com.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

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

        private void add_product_category_form_Load(object sender, EventArgs e)
        {
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            MyInter procate_inter = procate;
            inter = procate_inter;
            QueryProCate();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataConn.Connection.Open();
                if (txtName.Text != "")
                {
                    string query_name = "SELECT COUNT(*) FROM product_categories WHERE LOWER(name) = '" + txtName.Text.ToLower() + "';";
                    SqlCommand check_name = new SqlCommand(query_name, DataConn.Connection);
                    int nName = Convert.ToInt16(check_name.ExecuteScalar());
                    if (nName != 0)
                    {
                        MessageBox.Show("Product Category already exist!");
                        txtName.Text = "";
                        txtName.Focus();
                    }
                    else
                    {
                        procate.Name = txtName.Text.Trim();
                        procate.Descriptions = txtDescriptions.Text.Trim();
                        inter.insert();
                        MessageBox.Show("Insert succcessfully");
                        ClearTextBoxes(gpAddproductCategory);
                        txtName.Focus();
                    }
                }
                DataConn.Connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void product_category_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void lvProCate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvProCate.SelectedItems.Count != 0)
            {
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnDelete.Enabled = false;
                btnEdit.Enabled = false;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvProCate.SelectedItems.Count != 0)
            {
                btnEdit.Enabled = false;
                if (MessageBox.Show("Are you sure, you want to delete this product category?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ListViewItem del_item = lvProCate.SelectedItems[0];
                    int val = 0;
                    string name = del_item.SubItems[0].Text;
                    string del_sql = "DELETE FROM product_categories WHERE LOWER(name) = '" + name.ToLower() + "';";
                    SqlCommand del_cmd = new SqlCommand(del_sql, DataConn.Connection);
                    val = del_cmd.ExecuteNonQuery();
                    del_cmd.Dispose();
                    MessageBox.Show("Product category has been deleted!");
                    lvProCate.Items.Clear();
                    QueryProCate();
                    btnDelete.Enabled = false;
                }
                else
                {
                    MessageBox.Show("No product category was deleted!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnDelete.Enabled = false;
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lvProCate.SelectedItems.Count != 0)
            {
                btnDelete.Enabled = false;
                if (btnEdit.Text.ToLower() == "edit")
                {
                    DataConn.Connection.Open();
                    ListViewItem list_item = lvProCate.SelectedItems[0];
                    string name = list_item.SubItems[0].Text;
                    string sql = @"SELECT * FROM product_categories;";
                    SqlCommand upd_cmd = new SqlCommand(sql, DataConn.Connection);
                    SqlDataReader upd_rd = upd_cmd.ExecuteReader();
                    while (upd_rd.Read())
                    {
                        proCateId = int.Parse(upd_rd["id"].ToString());
                        txtName.Text = upd_rd["name"].ToString();
                        txtDescriptions.Text = upd_rd["descriptions"].ToString();
                    }
                    upd_cmd.Dispose();
                    upd_rd.Close();
                    DataConn.Connection.Close();
                    btnEdit.Text = "Update";
                }
                else if (btnEdit.Text.ToLower() == "update")
                {
                    procate.Name = txtName.Text.Trim();
                    procate.Descriptions = txtDescriptions.Text.Trim();
                    DataConn.Connection.Open();
                    inter.update(proCateId);
                    DataConn.Connection.Close();
                    MessageBox.Show("Product Categories has been updated!");
                    ClearTextBoxes(gpAddproductCategory);
                    lvProCate.Items.Clear();
                    QueryProCate();
                    btnEdit.Text = "Edit";
                    btnEdit.Enabled = false;
                }
            }
        }
    }
}
