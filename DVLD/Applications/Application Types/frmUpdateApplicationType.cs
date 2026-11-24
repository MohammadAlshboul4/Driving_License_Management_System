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
    public partial class frmUpdateApplicationType : Form
    {
        private int _ApplicationTypeID;

        clsApplicationsType _CurrentApplicationType;



        public frmUpdateApplicationType(int applicationTypeID)
        {
            InitializeComponent();
            _ApplicationTypeID = applicationTypeID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            bool IsFormValid = _ValidatingtxtTilte() & _ValidatingtxtFees();

            if (!IsFormValid)
            {
                MessageBox.Show("Some fields are not valid! Please correct the errors.",
             "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(clsApplicationsType.UpdateApplicationType(_ApplicationTypeID, txtTitle.Text, Convert.ToDecimal(txtFees.Text)))
            {
                MessageBox.Show("ApplicationType Updated Successfully.",
            "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed Updating ApplicationType.",
           "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        }

        private void _LoadData()
        {

            _CurrentApplicationType = clsApplicationsType.FindApplicationTypeInfo(_ApplicationTypeID);
            if (_CurrentApplicationType == null)
            {
                MessageBox.Show("ApplicationType wasnt found", "Error", MessageBoxButtons.OK);
                return;
            }
            lblApplicationTypeID.Text = _ApplicationTypeID.ToString();
            txtTitle.Text = _CurrentApplicationType.ApplicationTypeTitle;
            txtFees.Text = _CurrentApplicationType.ApplicationFees.ToString();

        }
        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {
            _LoadData();
        }

        bool _ValidatingtxtTilte()
        {
           
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
               
                txtTitle.Focus(); 
                errorProvider1.SetError(txtTitle, "Title cannot be blank!"); 
                return false;
            }
            else
            {
               
                errorProvider1.SetError(txtTitle, "");
                return true;
            }

        }
        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            _ValidatingtxtTilte();
        }

        ///------------------

        bool _ValidatingtxtFees()
        {
            if (string.IsNullOrWhiteSpace(txtFees.Text))
            {
                errorProvider1.SetError(txtFees, "Fees cannot be blank!");
                return false;
            }

            // Check if it's a valid decimal/float number
            if (!float.TryParse(txtFees.Text.Trim(), out float parsedFees))
            {
                errorProvider1.SetError(txtFees, "Only valid numbers are allowed!");
                return false;
            }
            // Ensure the number isn't negative
            else if (parsedFees < 0)
            {
                errorProvider1.SetError(txtFees, "Fees cannot be negative!");
                return false;
            }
            else
            {
                errorProvider1.SetError(txtFees, "");
                return true;
            }
        }

       
        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control keys (like Backspace) and digits
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // Allow the decimal point, but ONLY if the textbox doesn't already have one
                if (e.KeyChar == '.' && !txtFees.Text.Contains("."))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true; // Reject everything else
                }
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            _ValidatingtxtFees();
        }
    }
}
