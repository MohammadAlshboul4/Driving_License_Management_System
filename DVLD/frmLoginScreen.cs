using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Classes;
using DVLDBusinessLayer;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;



namespace DVLD.Users
{
    public partial class frmLoginScreen : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;


        ////////////////////////////////////////////////////////////
         
         
         
        clsUsers CurrentUser;
        public frmLoginScreen()
        {
            InitializeComponent();
        }

        private void frmLoginScreen_Load(object sender, EventArgs e)
        {
            string savedUsername = "";
            string savedPassword = "";

            // Try to read saved credentials
            if (clsGlobal.GetStoredCredential(ref savedUsername, ref savedPassword))
            {
                txtUserName.Text = savedUsername;
                txtPassword.Text = savedPassword;
                chkRemmberMe.Checked = true; // mark it as checked since we found saved data
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string UserName = txtUserName.Text.Trim();
            string Password = txtPassword.Text.Trim();

            CurrentUser = clsUsers.Login(UserName, Password);

            if (CurrentUser != null)
            {
                ///save the current logged in user
                clsGlobal._CurrentUser = CurrentUser;


                if (chkRemmberMe.Checked)
                    clsGlobal.RememberUsernameAndPassword(UserName, Password);
                else
                    clsGlobal.RememberUsernameAndPassword("", "");

                if (CurrentUser.IsActive)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                    MessageBox.Show("Account is inactive.");
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Incorrect Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void frmLoginScreen_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
