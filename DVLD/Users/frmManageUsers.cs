using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DVLD.Users
{
    public partial class frmManageUsers : Form
    {
        public frmManageUsers()
        {
            InitializeComponent();
           
        }

        // This is the single source of truth data table
        private DataTable _dtAllUsers;
        private void _RefreshUsersList()
        {

            // Pull the data exactly as it comes from the DB
            _dtAllUsers = clsUsers.GetAllUsers();

            // Bind the DefaultView straight to the grid. 
            dgvUsers.DataSource = _dtAllUsers.DefaultView;

            // Hide the columns you don't want the user to see directly in the Grid layout
            if (dgvUsers.Columns.Count > 0)
            {
                // Example: If your GetAllUsers returns extra backend columns you don't want:
                // dgvUsers.Columns["SomeHiddenColumn"].Visible = false;
            }

            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();


        }

        private void _ApplyFilter()
        {
            string FilterColumn = cbFilterBy.SelectedItem.ToString();
           
            string SearchValue = txtFilterBy.Text.Trim();


            string FilterIsActive =
                (cbIsActive.SelectedItem != null) ? cbIsActive.SelectedItem.ToString() : "All";

            if (string.IsNullOrEmpty(SearchValue) && FilterColumn != "Is Active")
            {
                _RefreshUsersList();
                return;
            }
            
            string FilterExpression = "";

            switch (FilterColumn)
            {
                case "User ID":
                    FilterExpression = $"UserID = {SearchValue}"; 
                    break;

                case "UserName":
                    FilterExpression = $"UserName like '%{SearchValue}%'";
                    break;

                case "Person ID":
                    FilterExpression = $"_PersonID = {SearchValue}";
                    break;

                case "Full Name":
                    FilterExpression = $"FullName like '%{SearchValue}%'";
                    break;

                case "Is Active":

                    switch (FilterIsActive)
                    {
                        case "Yes":
                            FilterExpression = "IsActive = true";
                            break;
                        case "No":
                            FilterExpression = "IsActive = false";
                            break;
                        default:
                            FilterExpression = "";
                            break;
                    }
                    break;

                default:
                    FilterExpression = "";
                    break;
            }

        
            _dtAllUsers.DefaultView.RowFilter = FilterExpression;

            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();


        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.SelectedIndex == null)
            {
                return;
            }

            string SelectedColumn = cbFilterBy.SelectedItem.ToString();

            cbIsActive.Visible = (SelectedColumn == "Is Active");

            txtFilterBy.Visible = (SelectedColumn != "None" && SelectedColumn != "Is Active");

            if (txtFilterBy.Visible)
            {
                txtFilterBy.Text = "";
                txtFilterBy.Focus();
            }

            if (cbIsActive.Visible)
            {
                cbIsActive.SelectedIndex = 0;
            }

            // Clear filter when changing selection types
            if (_dtAllUsers != null) _dtAllUsers.DefaultView.RowFilter = "";
            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ApplyFilter();
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            _ApplyFilter();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.SelectedItem.ToString() == "User ID" ||
                cbFilterBy.SelectedItem.ToString() == "Person ID")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }




        private void frmManageUsers_Load(object sender, EventArgs e)
        {
          
            cbFilterBy.SelectedIndex = 0;
            _RefreshUsersList();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddNewUser frm = new frmAddNewUser();
            frm.ShowDialog();
            _RefreshUsersList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                int UserID = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserID"].Value);
                frmAddNewUser frm = new frmAddNewUser(UserID);
                frm.ShowDialog();
                _RefreshUsersList();
            }

        }

        private void deToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                int userid = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserID"].Value);
                if (clsUsers.DeleteUser(userid))
                {
                    MessageBox.Show("Delete User succeed", "Delete User", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Delete failed due to data connected to it", "Failed Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            _RefreshUsersList();
        }


        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (dgvUsers.SelectedRows.Count > 0)
            {
                int Userid = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserID"].Value);
                frmUserInfo frm = new frmUserInfo(Userid);
                frm.ShowDialog();

            }

            _RefreshUsersList();
        }


        private void tsmChangePassword_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                int Userid = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserID"].Value);
                frmChangePassword frm = new frmChangePassword(Userid);
                frm.ShowDialog();

            }

            _RefreshUsersList();
        }

       
    }
}
