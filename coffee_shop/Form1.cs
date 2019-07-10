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
    public partial class Main : Form
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

        string uRole;
        int uId;
        string uName;
        string com_id = "";
        public Main(string role, int id, string name)
        {
            uRole = role;
            uId = id;
            uName = name;
            InitializeComponent();
            m_aeroEnabled = false;
            this.FormBorderStyle = FormBorderStyle.None;
        }
        string cid = "";
        string bId = "";

        private void GetUserCompany()
        {
            string sql = "SELECT * FROM companies WHERE user_id = " + uId + ";";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            int i = 0;
            while (sqlr.Read())
            {
                if(i == 0)
                    com_id += sqlr["id"].ToString();
                else
                    com_id += ", " + sqlr["id"].ToString();
                i++;
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void CheckStock()
        {
            if(com_id != "")
            {
                string sql = "SELECT COUNT(*) FROM stocks WHERE alerted = 1 AND company_id IN(" + com_id + ");";
                SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
                int count_alert = Convert.ToInt16(sqld.ExecuteScalar());
                sqld.Dispose();
                if (count_alert > 0)
                    lbAlert.Text = "Please update your stock!";
                else
                    lbAlert.Text = "";
            }
            else
            {
                MessageBox.Show("You don't have company yet!");
            }
        }

        private int CheckBranch()
        {
            DataConn.Connection.Open();
            string sql = "SELECT COUNT(*) FROM branches WHERE company_id IN (" + com_id + ");";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            int count = Convert.ToInt16(sqld.ExecuteScalar());
            sqld.Dispose();
            DataConn.Connection.Close();
            return count;
        }

        private int CheckCompany()
        {
            DataConn.Connection.Open();
            string sql = "SELECT COUNT(*) FROM companies WHERE user_id = " + uId + ";";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            int count = Convert.ToInt16(sqld.ExecuteScalar());
            sqld.Dispose();
            DataConn.Connection.Close();
            return count;
        }

        private int CheckUser()
        {
            DataConn.Connection.Open();
            string sql = @"SELECT COUNT(*) FROM users 
                        INNER JOIN roles ON users.role_id = roles.id
                        WHERE roles.name IN ('editor', 'user');";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            int count = Convert.ToInt16(sqld.ExecuteScalar());
            sqld.Dispose();
            DataConn.Connection.Close();
            return count;
        }

        private void CheckMember()
        {
            string sql = "SELECT * FROM members WHERE LOWER(name) = '" + uName.ToLower() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                cid = sqlr["company_id"].ToString();
                bId = sqlr["branch_id"].ToString();
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "admin")
            {
                DataConn.Connection.Open();
                GetUserCompany();
                DataConn.Connection.Close();
                if (com_id != "")
                {
                    DataConn.Connection.Open();
                    CheckStock();
                    DataConn.Connection.Close();
                }
            }
        }

        private void main_closing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
            new Login().Show();
        }

        private void myUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "superadmin" || uRole.ToLower() == "admin")
            {
                new users_form(uRole).ShowDialog();
            }
            else
                MessageBox.Show("You don't have permission!");
        }

        private void myCompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "superadmin" || uRole.ToLower() == "admin")
                new company_form(uId, uRole, uName).ShowDialog();
            else
                MessageBox.Show("You don't have permission!");
        }

        private void allProductsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "admin")
            {
                if(com_id != "")
                {
                    if (CheckCompany() > 0 && CheckBranch() > 0)
                        new products_form(com_id, uRole).ShowDialog();
                    else
                        MessageBox.Show("Please create Company or Branch first!");
                }
            }
            else
            {
                DataConn.Connection.Open();
                CheckMember();
                DataConn.Connection.Close();
                if (cid != "")
                    new products_form(cid, uRole).ShowDialog();
                else
                    MessageBox.Show("You're not a member of a company!");
            }
        }

        private void myStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(uRole.ToLower() == "admin")
            {
                if (CheckCompany() > 0 && CheckBranch() > 0)
                    new stock_form(com_id, uRole).ShowDialog();
                else
                    MessageBox.Show("Please create Company or Branch first!");
            }
            else
            {
                CheckMember();
                if (cid != "")
                    new stock_form(cid, uRole).ShowDialog();
                else
                    MessageBox.Show("You're not a member of a company!");
            }
        }

        private void stockCateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "admin")
            {
                if (CheckCompany() > 0 && CheckBranch() > 0)
                    new stock_categories_form(uRole).ShowDialog();
                else
                    MessageBox.Show("Please create Company or Branch first!");
            }
            else
            {
                CheckMember();
                if (cid != "")
                    new stock_categories_form(uRole).ShowDialog();
                else
                    MessageBox.Show("You're not a member of a company!");
            }
        }

        private void branchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "admin")
            {
                if (CheckCompany() > 0)
                    new branch_form(uId, uRole).ShowDialog();
                else
                    MessageBox.Show("Please create company first!");
            }
            else
                MessageBox.Show("You don't have permission!");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnDrinks_Click(object sender, EventArgs e)
        {
            if(uRole.ToLower() == "admin")
            {
                if (CheckCompany() > 0 && CheckBranch() > 0)
                    new product_selling("drinks", com_id, bId, uRole.ToLower()).ShowDialog();
                else
                    MessageBox.Show("Please create Company or Branch first!");
            }
            else
            {
                CheckMember();
                if (cid != "")
                    new product_selling("drinks", cid, bId, uRole.ToLower()).ShowDialog();
                else
                    MessageBox.Show("You're not a member of a company!");
            }
        }

        private void btnFood_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "admin")
            {
                if (CheckCompany() > 0 && CheckBranch() > 0)
                    new product_selling("foods", com_id, bId, com_id).ShowDialog();
                else
                    MessageBox.Show("Please create Company or Branch first!");
            }
            else
            {
                DataConn.Connection.Open();
                CheckMember();
                DataConn.Connection.Close();
                if (cid != "")
                    new product_selling("foods", com_id, bId, cid).ShowDialog();
                else
                    MessageBox.Show("You're not a member of a company!");
            }
        }

        private void newProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "admin")
            {
                if (CheckCompany() > 0 && CheckBranch() > 0)
                    new product_category_form(uRole).ShowDialog();
                else
                    MessageBox.Show("Please create Company or Branch first!");
            }
            else
            {
                DataConn.Connection.Open();
                CheckMember();
                DataConn.Connection.Close();
                if (cid != "")
                    new product_category_form(uRole).ShowDialog();
                else
                    MessageBox.Show("Please create Company or Branch first!");
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutCoffeeShop().ShowDialog();
        }

        private void membersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "admin" && CheckUser() > 0)
                new member_form(uId, uName).ShowDialog();
            else if (CheckUser() <= 0)
                MessageBox.Show("Please create editor or user first!");
            else
                MessageBox.Show("You don't have permission!");
        }

        private void employeesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "admin")
            {
                if (CheckCompany() > 0 && CheckBranch() > 0)
                    new employees_form(com_id).ShowDialog();
                else
                    MessageBox.Show("Please create Company or Branch first!");
            }
            else
                MessageBox.Show("You don't have permission!");
        }

        private void suppliersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "admin")
            {
                if (CheckCompany() > 0 && CheckBranch() > 0)
                    new supplier_form(com_id).ShowDialog();
                else
                    MessageBox.Show("Please create Company or Branch first!");
            }
            else
                MessageBox.Show("You don't have permission!");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DataConn.Connection.Open();
            CheckStock();
            DataConn.Connection.Close();
        }
    }
}
