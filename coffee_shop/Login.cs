using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace coffee_shop
{
    public partial class Login : Form
    {
        private bool Drag;
        private int MouseX;
        private int MouseY;

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        private bool m_aeroEnabled;

        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [DllImport("dwmapi.dll")]

        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();
                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW; return cp;
            }
        }
        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0; DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
                        }; DwmExtendFrameIntoClientArea(this.Handle, ref margins);
                    }
                    break;
                default: break;
            }
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT) m.Result = (IntPtr)HTCAPTION;
        }
        private void PanelMove_MouseDown(object sender, MouseEventArgs e)
        {
            Drag = true;
            MouseX = Cursor.Position.X - this.Left;
            MouseY = Cursor.Position.Y - this.Top;
        }
        private void PanelMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (Drag)
            {
                this.Top = Cursor.Position.Y - MouseY;
                this.Left = Cursor.Position.X - MouseX;
            }
        }
        private void PanelMove_MouseUp(object sender, MouseEventArgs e) { Drag = false; }

        public Login()
        {
            InitializeComponent();
            m_aeroEnabled = false;
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }
        HashCode hc = new HashCode();
        string user_role = "";
        string user_name = "";
        int user_id;
        int login_count;
        string user_status = "";

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUser.Text != "" && txtPassword.Text != "")
            {
                DataConn.Connection.Open();
                string sql = @"SELECT users.*, roles.name AS role_name FROM users
                    INNER JOIN roles ON users.role_id = roles.id
                    WHERE LOWER(username) = '" + txtUser.Text.ToLower() + "' AND password = '" + hc.PassHash(txtPassword.Text) + "';";
                SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
                SqlDataReader sqlr = sqld.ExecuteReader();
                while(sqlr.Read())
                {
                    user_id = int.Parse(sqlr["id"].ToString());
                    user_name = sqlr["username"].ToString();
                    user_role = sqlr["role_name"].ToString();
                    login_count = int.Parse(sqlr["login_count"].ToString());
                    user_status = sqlr["status"].ToString().ToLower();
                }
                sqld.Dispose();
                sqlr.Close();
                string count = "SELECT COUNT(*) FROM users WHERE LOWER(username) = '" + txtUser.Text.ToLower() + "' AND password = '" + hc.PassHash(txtPassword.Text) + "';";
                SqlCommand count_cmd = new SqlCommand(count, DataConn.Connection);
                int count_data = Convert.ToInt16(count_cmd.ExecuteScalar());
                count_cmd.Dispose();
                if (count_data != 0)
                {
                    string log_count_upd = "UPDATE users SET login_count = " + (login_count++) + " WHERE id = " + user_id + ";";
                    SqlCommand sqld_upd = new SqlCommand(log_count_upd, DataConn.Connection);
                    sqld_upd.ExecuteNonQuery();
                    sqld_upd.Dispose();

                    DataConn.Connection.Close();
                    if (user_role.ToLower() == "superadmin" && user_status == "active")
                    {
                        this.Hide();
                        new Mainsa_Form(user_role, user_id, user_name).Show();
                    }
                    else if ((user_role.ToLower() == "admin" || user_role.ToLower() == "editor" || user_role.ToLower() == "user") && user_status == "active")
                    {
                        this.Hide();
                        new Main(user_role, user_id, user_name).Show();
                    }
                    else
                    {
                        MessageBox.Show("You can't login to the system");
                        txtPassword.Clear();
                        txtUser.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Wrong username or password!");
                    txtPassword.Clear();
                    txtUser.Focus();
                }
            }
            else
            {
                MessageBox.Show("Please input user and password first!");
                txtUser.Focus();
            }
        }

        private void login_load(object sender, EventArgs e)
        {
            try
            {
                DataConn.ConnectionDB();
                DataConn.Connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
        }

        private void login_closing(object sender, FormClosingEventArgs e)
        {
            DataConn.Connection.Close();
            Application.Exit();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DataConn.Connection.Close();
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtUser_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsControl(e.KeyChar) != true && Char.IsNumber(e.KeyChar) == true) || Char.IsPunctuation(e.KeyChar) == true)
            {
                e.Handled = true;
            }
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            DataConn.Connection.Close();
            Application.Exit();
        }
    }
}
