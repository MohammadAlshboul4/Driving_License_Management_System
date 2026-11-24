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
    public partial class frmEditTestType : Form
    {

        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        clsTestType _CurrentTestType;
        public frmEditTestType(clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
            _TestTypeID = TestTypeID;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool IsFormValid = _txtTilteValidating() & _txtDescriptionValidating() & _txtFeesValidating();

            if (!IsFormValid)
            {
                MessageBox.Show("Some fields are not valid! Please correct the errors.",
             "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }


            if (clsTestType.UpdateTestTypes((int)_TestTypeID, txtTitle.Text, txtDescription.Text, Convert.ToDecimal(txtFees.Text)))
            {

                MessageBox.Show("Test Type Updated successfully.", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {

                MessageBox.Show("Faild to Updated Test Type!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        void _LoadData()
        {
            _CurrentTestType = clsTestType.FindTestTypeInfoByID(_TestTypeID);

            if ( _CurrentTestType == null )
            {
                MessageBox.Show("TestType wasnt found !", "Error", MessageBoxButtons.OK);
                return;

            }

            lblTestTypeID.Text = ((int)_TestTypeID).ToString();
            txtTitle.Text = _CurrentTestType.TestTypeTitle;
            txtDescription.Text = _CurrentTestType.TestTypeDescription;
            txtFees.Text = _CurrentTestType.TestTypeFees.ToString();

        }

        private void frmEditTestType_Load(object sender, EventArgs e)
        {

            _LoadData();


        }


        //--------------------------------------------------------

        // txtTitle Validating

        bool _txtTilteValidating()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {

                //txtTitle.Focus();
                errorProvider1.SetError(txtTitle, "Title cannot be blank!");
                return false;
            }

            clsTestType tempTestType = clsTestType.FindTestTypeInfoByTitle(txtTitle.Text.Trim());

            if (tempTestType != null && tempTestType.ID != _TestTypeID) 
            {
               // txtTitle.Focus();
                errorProvider1.SetError(txtTitle, "Title Already used!");
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
            _txtTilteValidating();

        }


        // txtDescription Validating
        bool _txtDescriptionValidating()
        {
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {

                //txtDescription.Focus();
                errorProvider1.SetError(txtDescription, "Description cannot be blank!");
                return false;
            }
            else
            {

                errorProvider1.SetError(txtDescription, "");
                return true;
            }
        }
        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            _txtDescriptionValidating();
        }

        //  txtFees Validating
        bool _txtFeesValidating()
        {
            if (string.IsNullOrWhiteSpace(txtFees.Text))
            {

               // txtFees.Focus();
                errorProvider1.SetError(txtFees, "Fees cannot be blank!");
                return false;
            }
            else
            {

                errorProvider1.SetError(txtFees, "");
                return true;
            }
        }
        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            _txtFeesValidating();
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

        //--------------------------------------------------------


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
