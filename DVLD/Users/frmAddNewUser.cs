using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Controls;
using DVLDBusinessLayer;

namespace DVLD.Users
{
    public partial class frmAddNewUser : Form
    {

        public delegate void DataBackEvenHandler(object sender, int UserID);

        public DataBackEvenHandler DataBack;


        clsUsers _CurrentUser;

        clsPerson _CurrentPersonInfo;

        private int _UserID = -1;

        enum enMode { AddNew = 0, Update = 1 }

        enMode _Mode = enMode.AddNew;

        int PersonID
        {
            get { return ctrlPersonCardWithFilter1.PersonID; }
        }


        public frmAddNewUser()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;

        }

        public frmAddNewUser(int userID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _UserID = userID;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool IsFormVaild = ValidateUserName() & ValidatePassword() & ValidateConfirmPassword();

            if (!IsFormVaild)
            {
                MessageBox.Show("Some fields are not valid! Please correct the errors.",
             "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _CurrentUser.PersonID = PersonID;
            _CurrentUser.UserName = txtUserName.Text;

            if (txtPassword.Text == txtConfirmPassword.Text)
            {
                _CurrentUser.Password = txtPassword.Text;
            }

            _CurrentUser.IsActive = chkIsActive.Checked;

            if (_CurrentUser.Save())
            {
                lblUserID.Text = _CurrentUser.UserID.ToString();
                _Mode = enMode.Update;
                lblAddUpdateUser.Text = "Update User";
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Data didnt Save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.AddNew)
            {
                if (ctrlPersonCardWithFilter1.SelectedPersonInfo == null)
                {
                    tpLoginInfo.Enabled = false;

                    { MessageBox.Show("Please select a person.", "Error", MessageBoxButtons.OK); return; }

                }

                _CurrentPersonInfo = ctrlPersonCardWithFilter1.SelectedPersonInfo;
                int personid = _CurrentPersonInfo.PersonID;

                if (!clsUsers.IsUserExistByPersonID(personid))
                {
                    tabControl1.SelectedTab = tpLoginInfo;
                    tpLoginInfo.Enabled = true;
                }
                else
                {
                    tpLoginInfo.Enabled = false;
                    MessageBox.Show("Selected person already has a user, choose another one.", "Select another Person",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                btnNext.Enabled = true;
                tabControl1.SelectedTab = tpLoginInfo;
            }


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = (tabControl1.SelectedIndex != 0);
        }

        private void _ResetDefaultValues()
        {
            if (_Mode == enMode.AddNew)
            {
                txtUserName.Text = "";
                txtPassword.Text = "";
                txtConfirmPassword.Text = "";
                chkIsActive.Checked = false;
                lblAddUpdateUser.Text = "Add New User";
                _CurrentUser = new clsUsers();
            }


        }
        private void _LoadData()
        {
            _CurrentUser = clsUsers.FindUser(_UserID);
            if (_CurrentUser == null)
            {
                _ResetDefaultValues();
                MessageBox.Show("User wasnt found", "Error", MessageBoxButtons.OK);
                return;
            }

            ctrlPersonCardWithFilter1.LoadCtrlPersonCard(_CurrentUser.PersonID);
            txtUserName.Text = _CurrentUser.UserName;
            txtPassword.Text = _CurrentUser.Password;
            txtConfirmPassword.Text = _CurrentUser.Password;
            chkIsActive.Checked = _CurrentUser.IsActive;
            lblUserID.Text = _CurrentUser.UserID.ToString();

        }
        private void frmAddNewUser_Load(object sender, EventArgs e)
        {
            if (_Mode == enMode.AddNew)
            {
                _ResetDefaultValues();
                btnSave.Enabled = false;
                tpLoginInfo.Enabled = false;
                ctrlPersonCardWithFilter1.OnPersonSelected += _ctrlPersonCardWithFilter1_OnPersonSelected;
            }
            else
            {

                lblAddUpdateUser.Text = "Update User";
                ctrlPersonCardWithFilter1.Enabled = false;
                tpLoginInfo.Enabled = true;
                btnNext.Enabled = true;
                _LoadData();
            }

        }

        private void _ctrlPersonCardWithFilter1_OnPersonSelected(int personid)
        {
            tpLoginInfo.Enabled = false;

            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            chkIsActive.Checked = false;
        }

        //ConfirmPassword Valdation
        private bool ValidateConfirmPassword()
        {
            // 1. Check if blank first
            if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                errorProvider1.SetError(txtConfirmPassword, "Password cannot be blank!");
                return false;
            }

            // 2. Check if it matches the password textbox
            if (txtConfirmPassword.Text != txtPassword.Text)
            {
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match the Password!");
                return false;
            }

            // 3. Clear error if it passes both checks
            errorProvider1.SetError(txtConfirmPassword, "");
            return true;
        }
        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            ValidateConfirmPassword();
        }

        //Password Valdation
        private bool ValidatePassword()
        {
            if (txtPassword.Text == "")
            {
                errorProvider1.SetError(txtPassword, "Password cannot be blank");
                return false;
            }

            errorProvider1.SetError(txtPassword, "");
            return true;
        }
        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            ValidatePassword();
        }


        //UserName Valdation
        private bool ValidateUserName()
        {

            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                errorProvider1.SetError(txtUserName, "UserName cannot be blank");
                return false;
            }
            if (_Mode == enMode.AddNew)
            {
                if (clsUsers.IsUserExistByUserName(txtUserName.Text.Trim()))
                {
                    errorProvider1.SetError(txtUserName, "UserName is already used");
                    return false;
                }
            }

            errorProvider1.SetError(txtUserName, "");
            return true;
        }
        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {

            ValidateUserName();

        }

       
    }
}
