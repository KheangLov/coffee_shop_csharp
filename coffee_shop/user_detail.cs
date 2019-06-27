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
    public partial class user_detail : Form
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

        string userName;
        public user_detail(string uName)
        {
            InitializeComponent();
            m_aeroEnabled = false;
            userName = uName;
        }

        StringCapitalize sc = new StringCapitalize();

        private void user_detail_Load(object sender, EventArgs e)
        {
            txtUsername.Enabled = false;
            txtEmail.Enabled = false;
            txtPhone.Enabled = false;
            txtAddress.Enabled = false;
            txtGender.Enabled = false;
            DataConn.Connection.Open();
            string sql = @"SELECT * FROM users 
                        INNER JOIN roles ON users.role_id = roles.id 
                        WHERE users.role_id = roles.id 
                        AND LOWER(users.username) = '" + userName.ToLower() + "' ORDER BY roles.id;";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if(sqlr.Read())
            {
                txtUsername.Text = sc.ToCapitalize(sqlr["username"].ToString());
                txtEmail.Text = sqlr["email"].ToString();
                txtPhone.Text = sqlr["phone"].ToString();
                txtAddress.Text = sqlr["address"].ToString();
                if(sqlr["status"].ToString().ToLower() == "ban")
                    lblStatus.BackColor = Color.FromArgb(255, 128, 0);
                else if(sqlr["status"].ToString().ToLower() == "inactive")
                    lblStatus.BackColor = Color.FromArgb(204, 0, 0);
                lblStatus.Text = sc.ToCapitalize(sqlr["status"].ToString());
                lblRole.Text = sc.ToCapitalize(sqlr["name"].ToString());
                txtGender.Text = sc.ToCapitalize(sqlr["gender"].ToString());
                pbProfile.ImageLocation = sqlr["image"].ToString();
            }
            sqld.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
