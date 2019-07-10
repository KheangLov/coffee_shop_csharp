using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace coffee_shop
{
    public partial class member_form : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
            (
                int nLeftRect, // x-coordinate of upper-left corner
                int nTopRect, // y-coordinate of upper-left corner
                int nRightRect, // x-coordinate of lower-right corner
                int nBottomRect, // y-coordinate of lower-right corner
                int nWidthEllipse, // height of ellipse
                int nHeightEllipse // width of ellipse
             );

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        private bool m_aeroEnabled;                     // variables for box shadow
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        public struct MARGINS                           // struct for box shadow
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        private const int WM_NCHITTEST = 0x84;          // variables for dragging the form
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();

                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW;

                return cp;
            }
        }

        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:                        // box shadow
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 1,
                            rightWidth = 1,
                            topHeight = 1
                        };
                        DwmExtendFrameIntoClientArea(this.Handle, ref margins);

                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);

            //if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)     // drag the form
            //    m.Result = (IntPtr)HTCAPTION;

        }

        int uId;
        string uName;
        public member_form(int id, string name)
        {
            uId = id;
            uName = name;
            InitializeComponent();
            m_aeroEnabled = false;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        Member my_member = new Member();
        StringCapitalize sc = new StringCapitalize();
        MyInter inter;
        int user_id;
        int company_id;
        int memId;

        private void QueryMembers()
        {
            string sql = @"SELECT members.*, users.username AS user_name, companies.name AS company_name,
                branches.name AS branch_name FROM members 
                INNER JOIN users ON members.user_id = users.id 
                INNER JOIN companies ON members.company_id = companies.id
                INNER JOIN branches ON members.branch_id = branches.id
                WHERE members.user_id = " + uId + ";";
            SqlCommand com = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = com.ExecuteReader();
            while (sqlr.Read())
            {
                string[] member_info = { sc.ToCapitalize(sqlr["name"].ToString()), sc.ToCapitalize(sqlr["user_name"].ToString()), sc.ToCapitalize(sqlr["company_name"].ToString()), sc.ToCapitalize(sqlr["branch_name"].ToString()) };
                ListViewItem item = new ListViewItem(member_info);
                lvMember.Items.Add(item);
            }
            com.Dispose();
            sqlr.Close();
        }
        
        private int CheckMember()
        {
            string sql = "SELECT COUNT(*) FROM members;";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            int count = Convert.ToInt16(sqld.ExecuteScalar());
            sqld.Dispose();
            return count;
        }

        private void loadComboName()
        {
            DataConn.Connection.Open();
            string mName = "";
            if (CheckMember() > 0)
            {
                string sql_member = "SELECT * FROM members;";
                SqlCommand com_mem = new SqlCommand(sql_member, DataConn.Connection);
                SqlDataReader read_mem = com_mem.ExecuteReader();
                int i = 0;
                while (read_mem.Read())
                {
                    if (i == 0)
                        mName += "'" + read_mem["name"].ToString().ToLower() + "'";
                    else
                        mName += ", '" + read_mem["name"].ToString().ToLower() + "'";
                    i++;
                }
                com_mem.Dispose();
                read_mem.Close();
            }
            string sql = "";
            if (mName != "")
            {
                sql = @"SELECT users.*, roles.name AS role_name FROM users 
                    INNER JOIN roles ON users.role_id = roles.id
                    WHERE LOWER(roles.name) IN('editor', 'user')
                    AND LOWER(users.username) NOT IN(" + mName + ");";
            }
            else
            {
                sql = @"SELECT users.*, roles.name AS role_name FROM users 
                    INNER JOIN roles ON users.role_id = roles.id
                    WHERE LOWER(roles.name) IN('editor', 'user');";
            }
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                cbName.Items.Add(sc.ToCapitalize(sqlr["username"].ToString()));
            }
            sqld.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void addUser()
        {
            if (txtUser.Text != "")
            {
                DataConn.Connection.Open();
                string sql = "SELECT * FROM users WHERE LOWER(username) = '" + txtUser.Text.ToLower() + "';";
                SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
                SqlDataReader sqlr = sqld.ExecuteReader();
                if (sqlr.Read())
                {
                    user_id = int.Parse(sqlr["id"].ToString());
                }
                else
                {
                    MessageBox.Show("Nothing found!");
                }
                sqlr.Close();
                sqld.Dispose();
                DataConn.Connection.Close();
            }
        }

        private void loadComboCompany()
        {
            DataConn.Connection.Open();
            string sql = @"SELECT * FROM companies WHERE user_id = " + user_id + ";";
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

        private void addCompany()
        {
            if (cbCompany.Text != "")
            {
                DataConn.Connection.Open();
                string sql = "SELECT * FROM companies WHERE LOWER(name) = '" + cbCompany.Text.ToLower() + "';";
                SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
                SqlDataReader sqlr = sqld.ExecuteReader();
                if (sqlr.Read())
                {
                    company_id = int.Parse(sqlr["id"].ToString());
                }
                else
                {
                    MessageBox.Show("Nothing found!");
                }
                sqlr.Close();
                sqld.Dispose();
                DataConn.Connection.Close();
            }
        }

        private void loadComboBranch()
        {
            DataConn.Connection.Open();
            string sql = @"SELECT * FROM branches WHERE company_id = " + company_id + ";";
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

        private void addBranch()
        {
            DataConn.Connection.Open();
            string sql = @"SELECT * FROM branches WHERE LOWER(name) = '" + cbBranch.SelectedItem.ToString().ToLower() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                my_member.BranchId = int.Parse(sqlr["id"].ToString());
            }
            sqld.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void member_form_Load(object sender, EventArgs e)
        {
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            txtUser.Enabled = false;
            MyInter member_inter = my_member;
            inter = member_inter;
            loadComboName();
            if (cbName.Items.Count > 0)
                cbName.SelectedIndex = 0;
            txtUser.Text = sc.ToCapitalize(uName);
            addUser();
            cbCompany.Items.Clear();
            loadComboCompany();
            if (cbCompany.Items.Count > 0)
                cbCompany.SelectedIndex = 0;
            addCompany();
            cbBranch.Items.Clear();
            loadComboBranch();
            if (cbBranch.Items.Count > 0)
                cbBranch.SelectedIndex = 0;
            DataConn.Connection.Open();
            QueryMembers();
            DataConn.Connection.Close();
        }

        private void cbUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            addUser();
            cbCompany.Items.Clear();
            loadComboCompany();
            if (cbCompany.Items.Count > 0)
                cbCompany.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                my_member.Name = cbName.SelectedItem.ToString().Trim();
                my_member.UserId = user_id;
                my_member.CompanyId = company_id;
                addBranch();
                DataConn.Connection.Open();
                inter.insert();
                MessageBox.Show("Insert successfully!");
                lvMember.Items.Clear();
                QueryMembers();
                DataConn.Connection.Close();
                cbName.Items.Clear();
                loadComboName();
                if (cbName.Items.Count > 0)
                    cbName.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lvMember_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvMember.SelectedItems.Count != 0)
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
            if (lvMember.SelectedItems.Count != 0)
            {
                DataConn.Connection.Open();
                btnEdit.Enabled = false;
                if (MessageBox.Show("Are you sure, you want to delete this member?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ListViewItem lvi = lvMember.SelectedItems[0];
                    int val = 0;
                    string proName = lvi.SubItems[0].Text;
                    string del_que = "DELETE FROM members WHERE LOWER(name) = '" + proName.ToLower() + "';";
                    SqlCommand del_com = new SqlCommand(del_que, DataConn.Connection);
                    val = del_com.ExecuteNonQuery();
                    del_com.Dispose();
                    MessageBox.Show("Member has been deleted!");
                    lvMember.Items.Clear();
                    QueryMembers();
                    btnDelete.Enabled = false;
                    btnEdit.Enabled = false;
                }
                else
                {
                    MessageBox.Show("No Member was deleted!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                DataConn.Connection.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lvMember.SelectedItems.Count != 0)
            {
                btnDelete.Enabled = false;
                if (btnEdit.Text.ToLower() == "edit")
                {
                    try
                    {
                        ListViewItem item = lvMember.SelectedItems[0];
                        string lv_members = item.SubItems[0].Text;
                        string sql = @"SELECT members.*, users.name AS user_name, companies.name AS company_name,
                                branches.name AS branch_name FROM members 
                                INNER JOIN users ON members.user_id = users.id 
                                INNER JOIN companies ON members.company_id = companies.id
                                INNER JOIN branches ON members.branch_id = branches.id
                                WHERE LOWER(members.name) = '" + lv_members.ToLower() + "';";
                        SqlCommand command = new SqlCommand(sql, DataConn.Connection);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            memId = int.Parse(reader["id"].ToString());
                            cbName.SelectedItem = reader["name"].ToString();
                            txtUser.Text = reader["user_name"].ToString();
                            cbCompany.SelectedItem = reader["company_name"].ToString();
                            cbBranch.SelectedItem = reader["branch_name"].ToString();
                            btnEdit.Text = "Update";
                        }
                        command.Dispose();
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else if (btnEdit.Text.ToLower() == "update")
                {
                    try
                    {
                        my_member.Name = cbName.SelectedItem.ToString().Trim();
                        my_member.UserId = user_id;
                        my_member.CompanyId = company_id;
                        addBranch();
                        inter.update(memId);
                        MessageBox.Show("Update Successfully!");
                        lvMember.Items.Clear();
                        QueryMembers();
                        btnEdit.Text = "Edit";
                        btnEdit.Enabled = false;
                        btnDelete.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            addCompany();
            cbBranch.Items.Clear();
            loadComboBranch();
            if (cbBranch.Items.Count > 0)
                cbBranch.SelectedIndex = 0;
        }

        private void cbBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            addBranch();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lvMember.Items.Clear();
            DataConn.Connection.Open();
            string search_query = @"SELECT members.*, users.username AS user_name, companies.name AS company_name,
                branches.name AS branch_name FROM members
                INNER JOIN users ON members.user_id = users.id
                INNER JOIN companies ON members.company_id = companies.id
                INNER JOIN branches ON members.branch_id = branches.id
                WHERE members.user_id = " + uId + " AND LOWER(members.name) LIKE '%" + txtSearch.Text.Trim().ToLower() + "%';";
            SqlCommand srh_cmd = new SqlCommand(search_query, DataConn.Connection);
            SqlDataReader srh_rd = srh_cmd.ExecuteReader();
            while (srh_rd.Read())
            {
                string[] member_info = { sc.ToCapitalize(srh_rd["name"].ToString()), sc.ToCapitalize(srh_rd["user_name"].ToString()), sc.ToCapitalize(srh_rd["company_name"].ToString()), sc.ToCapitalize(srh_rd["branch_name"].ToString()) };
                ListViewItem item = new ListViewItem(member_info);
                lvMember.Items.Add(item);
            }
            srh_cmd.Dispose();
            srh_rd.Close();
            DataConn.Connection.Close();
        }
    }
}
