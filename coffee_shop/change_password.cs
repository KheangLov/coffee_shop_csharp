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
    public partial class change_password : Form
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

        string oldPass;
        int uId;
        string newPass;
        public change_password(string frt_item, int sec_item)
        {
            oldPass = frt_item;
            uId = sec_item;
            InitializeComponent();
            m_aeroEnabled = false;
            this.FormBorderStyle = FormBorderStyle.None;
        }
        HashCode hc = new HashCode();

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

        private void txtOldPass_leave(object sender, EventArgs e)
        {
            if(hc.PassHash(txtOldPass.Text.Trim()) != oldPass)
            {
                lblMessage.Text = "Wrong old password!";
                ClearTextBoxes(groupBox1);
                txtOldPass.Focus();
            }
            else
            {
                lblMessage.Text = "";
                txtNewPass.Enabled = true;
                txtConPass.Enabled = true;
            }
        }

        private void change_password_Load(object sender, EventArgs e)
        {
            txtNewPass.Enabled = false;
            txtConPass.Enabled = false;
        }

        private void txtConPass_leave(object sender, EventArgs e)
        {
            if(txtNewPass.Text.Trim() == txtConPass.Text.Trim())
            {
                newPass = hc.PassHash(txtNewPass.Text.Trim());
            }
            else
            {
                MessageBox.Show("Wrong confirmation password!");
                newPass = "";
                txtNewPass.Text = "";
                txtConPass.Text = "";
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if(newPass != "")
            {
                try
                {
                    DataConn.Connection.Open();
                    string upp_que = "UPDATE users SET password = '" + newPass + "' WHERE id = " + uId + ";";
                    SqlCommand upp_cmd = new SqlCommand(upp_que, DataConn.Connection);
                    upp_cmd.ExecuteNonQuery();
                    upp_cmd.Dispose();
                    DataConn.Connection.Close();
                    MessageBox.Show("User's password has been updated!");
                    this.Dispose();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please insert you new password!");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }
    }
}
