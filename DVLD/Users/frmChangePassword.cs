using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmChangePassword : Form
    {
        clsUsers _CurrentUser;

        private int _UserID = -1;
        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;

        }

        private void _ResetDefaultValues()
        {
            txtCurrentPassword.Text = string.Empty.Trim();
            txtNewPassword.Text = string.Empty.Trim();
            txtConfirmPassword.Text = string.Empty.Trim();
            txtCurrentPassword.Focus();
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _CurrentUser = clsUsers.FindUser(_UserID);
            if (_CurrentUser == null)
            {
                MessageBox.Show("Didnt found the User", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ctrlUserCard1.LoadUserInfo(_UserID);


        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            bool IsFormVaild = _ValidateCurrentPassword() & _ValidateNewPassword() & _ValidateConfirmPassword();

            if (!IsFormVaild)
            {
                MessageBox.Show("Some fields are not valid! Please correct the errors.",
             "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            _CurrentUser.Password = txtNewPassword.Text;

            if (_CurrentUser.Save())
            {
                MessageBox.Show("Password Changed Successfully", "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ResetDefaultValues();

            }
            else
            {
                MessageBox.Show("Password Faild to Change", "Error.", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }


        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Validate CurrentPassword

        private bool _ValidateCurrentPassword()
        {
            if (txtCurrentPassword.Text != _CurrentUser.Password)
            {
                errorProvider1.SetError(txtCurrentPassword, "Current Password is wrong!");
                return false;
            }
            errorProvider1.SetError(txtCurrentPassword, "");
            return true;
        }
        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            _ValidateCurrentPassword();
        }

        //Validate NewPassword
        private bool _ValidateNewPassword()
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text))
            {
                errorProvider1.SetError(txtNewPassword, "Password Cant be blank");
                return false;
            }
            errorProvider1.SetError(txtNewPassword, "");
            return true;

        }
        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            _ValidateNewPassword();
        }


        //Validate ConfirmPassword
        private bool _ValidateConfirmPassword()
        {
            if (txtConfirmPassword.Text != txtNewPassword.Text)
            {
                errorProvider1.SetError(txtConfirmPassword, "Password doesn't match with the New Password");
                return false;
            }
            errorProvider1.SetError(txtConfirmPassword, "");
            return true;

        }
        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            _ValidateConfirmPassword();
        }

        
    }
}
