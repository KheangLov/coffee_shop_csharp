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
    public partial class employees_form : Form
    {
        string cId;
        public employees_form(string com_id)
        {
            cId = com_id;
            InitializeComponent();
        }
        Employees my_employee = new Employees();
        StringCapitalize sc = new StringCapitalize();
        MyInter inter;
        int empId;

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

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool alreadyExist(string _text, ref char KeyChar)
        {
            if (_text.IndexOf('.') > -1)
            {
                KeyChar = '.';
                return true;
            }
            if (_text.IndexOf(',') > -1)
            {
                KeyChar = ',';
                return true;
            }
            return false;
        }

        private void QueryEmployees()
        {
            try
            {
                DataConn.Connection.Open();
                string sql = @"SELECT employees.*, companies.name AS company_name, branches.name AS branch_name
                        FROM employees
                        INNER JOIN companies ON employees.company_id = companies.id
                        INNER JOIN branches ON employees.branch_id = branches.id
                        WHERE companies.id IN (" + cId + ");";
                SqlCommand com = new SqlCommand(sql, DataConn.Connection);
                SqlDataReader sqlr = com.ExecuteReader();
                while (sqlr.Read())
                {
                    string[] employees_info = { sc.ToCapitalize(sqlr["fullname"].ToString()), sqlr["gender"].ToString(), sqlr["dob"].ToString(), sqlr["phone"].ToString(), sqlr["email"].ToString(), sqlr["address"].ToString(), sqlr["salary"].ToString(), sqlr["positions"].ToString(), sqlr["work_times"].ToString(), sc.ToCapitalize(sqlr["company_name"].ToString()), sc.ToCapitalize(sqlr["branch_name"].ToString()) };
                    ListViewItem item = new ListViewItem(employees_info);
                    lvEmployees.Items.Add(item);
                }
                com.Dispose();
                sqlr.Close();
                DataConn.Connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadComboCompany()
        {
            try
            {
                DataConn.Connection.Open();
                string sql = "SELECT id, name FROM companies WHERE id IN (" + cId + ");";
                SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
                SqlDataReader sqlr = sqld.ExecuteReader();
                while (sqlr.Read())
                {
                    cbCompany.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
                }
                sqld.Dispose();
                sqlr.Close();
                DataConn.Connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadComboBranch()
        {
            try
            {
                DataConn.Connection.Open();
                string sql = "SELECT id, name FROM branches WHERE company_id = " + my_employee.CompanyId + ";";
                SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
                SqlDataReader sqlr = sqld.ExecuteReader();
                while (sqlr.Read())
                {
                    cbBranch.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
                }
                sqld.Dispose();
                sqlr.Close();
                DataConn.Connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addComboCompany()
        {
            DataConn.Connection.Open();
            string sql = "SELECT * FROM companies WHERE LOWER(name) = '" + cbCompany.Text.ToLower() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                my_employee.CompanyId = int.Parse(sqlr["id"].ToString());
            }
            else
            {
                MessageBox.Show("Nothing found!");
            }
            sqlr.Close();
            sqld.Dispose();
            DataConn.Connection.Close();
        }

        private void addComboBranch()
        {
            DataConn.Connection.Open();
            string sql = "SELECT * FROM branches WHERE LOWER(name) = '" + cbBranch.Text.ToLower() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                my_employee.BranchId = int.Parse(sqlr["id"].ToString());
            }
            else
            {
                MessageBox.Show("Nothing found!");
            }
            sqlr.Close();
            sqld.Dispose();
            DataConn.Connection.Close();
        }

        private void employees_form_Load(object sender, EventArgs e)
        {
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            MyInter employee_inter = my_employee;
            inter = employee_inter;
            QueryEmployees();
            loadComboCompany();
            if (cbCompany.Items.Count > 0)
                cbCompany.SelectedIndex = 0;
            addComboCompany();
            loadComboBranch();
            if (cbBranch.Items.Count > 0)
                cbBranch.SelectedIndex = 0;
            cbGender.SelectedIndex = 0;
            cbPosition.SelectedIndex = 0;
            cbWorktime.SelectedIndex = 0;
        }

        private void employees_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void txtSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            //check if '.' , ',' pressed
            char sepratorChar = 's';
            if (e.KeyChar == '.' || e.KeyChar == ',')
            {
                // check if it's in the beginning of text not accept
                if (txtSalary.Text.Length == 0) e.Handled = true;
                // check if it's in the beginning of text not accept
                if (txtSalary.SelectionStart == 0) e.Handled = true;
                // check if there is already exist a '.' , ','
                if (alreadyExist(txtSalary.Text, ref sepratorChar)) e.Handled = true;
                //check if '.' or ',' is in middle of a number and after it is not a number greater than 99
                if (txtSalary.SelectionStart != txtSalary.Text.Length && e.Handled == false)
                {
                    // '.' or ',' is in the middle
                    string AfterDotString = txtSalary.Text.Substring(txtSalary.SelectionStart);

                    if (AfterDotString.Length > 2)
                    {
                        e.Handled = true;
                    }
                }
            }
            //check if a number pressed

            if (Char.IsDigit(e.KeyChar))
            {
                //check if a coma or dot exist
                if (alreadyExist(txtSalary.Text, ref sepratorChar))
                {
                    int sepratorPosition = txtSalary.Text.IndexOf(sepratorChar);
                    string afterSepratorString = txtSalary.Text.Substring(sepratorPosition + 1);
                    if (txtSalary.SelectionStart > sepratorPosition && afterSepratorString.Length > 1)
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void lvEmployees_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvEmployees.SelectedItems.Count != 0)
            {
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (!IsValidEmail(txtEmail.Text.Trim()))
            {
                MessageBox.Show("Invalid Email!");
                txtEmail.Text = "";
                txtEmail.Focus();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataConn.Connection.Open();
            lvEmployees.Items.Clear();
            string search_query = @"SELECT employees.*, companies.name AS company_name, branches.name AS branch_name
                        FROM employees
                        INNER JOIN companies ON employees.company_id = companies.id
                        INNER JOIN branches ON employees.branch_id = branches.id
                        WHERE companies.id IN (" + cId + ") AND LOWER(employees.fullname) LIKE '%" + txtSearch.Text.Trim().ToLower() + "%' ORDER BY roles.id;";
            SqlCommand srh_cmd = new SqlCommand(search_query, DataConn.Connection);
            SqlDataReader sqlr = srh_cmd.ExecuteReader();
            while (sqlr.Read())
            {
                string[] employees_info = { sc.ToCapitalize(sqlr["fullname"].ToString()), sqlr["gender"].ToString(), sqlr["dob"].ToString(), sqlr["phone"].ToString(), sqlr["email"].ToString(), sqlr["address"].ToString(), sqlr["salary"].ToString(), sqlr["positions"].ToString(), sqlr["work_times"].ToString(), sc.ToCapitalize(sqlr["company_name"].ToString()), sc.ToCapitalize(sqlr["branch_name"].ToString()) };
                ListViewItem item = new ListViewItem(employees_info);
                lvEmployees.Items.Add(item);
            }
            srh_cmd.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFirstname.Text != "" && txtLastname.Text != "")
                {
                    DataConn.Connection.Open();
                    string my_emp_name = txtFirstname.Text + txtLastname.Text;
                    string check_name = "SELECT COUNT(*) FROM employees WHERE LOWER(fullname) = '" + my_emp_name.ToLower() + "';";
                    SqlCommand check_com = new SqlCommand(check_name, DataConn.Connection);
                    int find_emp = Convert.ToInt16(check_com.ExecuteScalar());
                    check_com.Dispose();
                    DataConn.Connection.Close();
                    if (find_emp != 0)
                    {
                        MessageBox.Show("Employee already exist!");
                        txtFirstname.Focus();
                    }
                    else
                    {
                        my_employee.Firstname = txtFirstname.Text.Trim();
                        my_employee.Lastname = txtLastname.Text.Trim();
                        my_employee.Fullname = my_employee.Firstname + my_employee.Lastname;
                        my_employee.Dob = DateTime.Parse(dtpDob.Text).ToString("yyyy-MM-dd");
                        my_employee.Address = txtAddress.Text.Trim();
                        my_employee.Email = txtEmail.Text.Trim();
                        my_employee.Phone = txtPhone.Text.Trim();
                        my_employee.Positions = cbPosition.Text.Trim();
                        my_employee.Salary = decimal.Parse(txtSalary.Text);
                        my_employee.WorkTimes = cbWorktime.Text.Trim();
                        my_employee.Gender = cbGender.Text.Trim();
                        addComboBranch();
                        addComboCompany();
                        inter.insert();
                        MessageBox.Show("Insert successfully!");
                        ClearTextBoxes(groupBox1);
                        lvEmployees.Clear();
                        QueryEmployees();
                    }
                }
                else
                {
                    MessageBox.Show("Please insert name!");
                    txtFirstname.Focus();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvEmployees.SelectedItems.Count != 0)
            {
                if (MessageBox.Show("Are you sure, you want to delete this employee?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ListViewItem del_item = lvEmployees.SelectedItems[0];
                    int val = 0;
                    string name = del_item.SubItems[0].Text;
                    string del_sql = "DELETE FROM employees WHERE fullname = '" + name + "';";
                    SqlCommand del_cmd = new SqlCommand(del_sql, DataConn.Connection);
                    val = del_cmd.ExecuteNonQuery();
                    del_cmd.Dispose();
                    MessageBox.Show("Employee has been deleted!");
                    lvEmployees.Items.Clear();
                    QueryEmployees();
                }
                else
                {
                    MessageBox.Show("No employee was deleted!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lvEmployees.SelectedItems.Count != 0)
            {
                if (btnEdit.Text.ToLower() == "edit")
                {
                    ListViewItem list_item = lvEmployees.SelectedItems[0];
                    string name = list_item.SubItems[0].Text;
                    string sql = @"SELECT employees.*, companies.name AS company_name, branches.name AS branch_name
                        FROM employees
                        INNER JOIN companies ON employees.company_id = companies.id
                        INNER JOIN branches ON employees.branch_id = branches.id
                        WHERE LOWER(fullname) = '" + name.ToLower() + "';";
                    SqlCommand upd_cmd = new SqlCommand(sql, DataConn.Connection);
                    SqlDataReader upd_rd = upd_cmd.ExecuteReader();
                    while (upd_rd.Read())
                    {
                        empId = int.Parse(upd_rd["id"].ToString());
                        txtFirstname.Text = upd_rd["firstname"].ToString();
                        txtLastname.Text = upd_rd["lastname"].ToString();
                        txtEmail.Text = upd_rd["email"].ToString();
                        txtPhone.Text = upd_rd["phone"].ToString();
                        txtSalary.Text = upd_rd["salary"].ToString();
                        txtAddress.Text = upd_rd["address"].ToString();
                        dtpDob.Text = upd_rd["dob"].ToString();
                        cbPosition.SelectedText = upd_rd["positions"].ToString();
                        cbWorktime.SelectedText = upd_rd["work_times"].ToString();
                        cbGender.SelectedText = upd_rd["gender"].ToString();
                        cbCompany.SelectedText = upd_rd["company_name"].ToString();
                        cbBranch.SelectedText = upd_rd["branch_name"].ToString();
                    }
                    upd_cmd.Dispose();
                    upd_rd.Close();
                    btnEdit.Text = "Update";
                }
                else if (btnEdit.Text.ToLower() == "update")
                {
                    my_employee.Firstname = txtFirstname.Text.Trim();
                    my_employee.Lastname = txtLastname.Text.Trim();
                    my_employee.Fullname = my_employee.Firstname + my_employee.Lastname;
                    my_employee.Dob = DateTime.Parse(dtpDob.Text).ToString("yyyy-MM-dd");
                    my_employee.Address = txtAddress.Text.Trim();
                    my_employee.Email = txtEmail.Text.Trim();
                    my_employee.Phone = txtPhone.Text.Trim();
                    my_employee.Positions = cbPosition.Text.Trim();
                    my_employee.Salary = decimal.Parse(txtSalary.Text);
                    my_employee.WorkTimes = cbWorktime.Text.Trim();
                    my_employee.Gender = cbGender.Text.Trim();
                    addComboBranch();
                    addComboCompany();
                    inter.update(empId);
                    MessageBox.Show("Employee has been updated!");
                    ClearTextBoxes(groupBox1);
                    lvEmployees.Items.Clear();
                    QueryEmployees();
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            addComboCompany();
            cbBranch.Items.Clear();
            loadComboBranch();
            if (cbBranch.Items.Count > 0)
                cbBranch.SelectedIndex = 0;
        }
    }
}
