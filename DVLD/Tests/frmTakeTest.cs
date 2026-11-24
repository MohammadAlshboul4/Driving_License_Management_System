using DVLD.Classes;
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
    public partial class frmTakeTest : Form
    {
        int _TestAppointmentID = -1;

        int _LDLAppID = -1;
        enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 }
        enTestType _enTestType = enTestType.VisionTest;

        public enum enMode { Addnew = 1, Update = 2 }
        private enMode _Mode = enMode.Addnew;

        clsTests _currentTest;


        public frmTakeTest(int LDLAppID, int TestAppointmentID,int TestType)
        {
            InitializeComponent();
            _LDLAppID = LDLAppID;
            _TestAppointmentID = TestAppointmentID;
            _enTestType = (enTestType)TestType;
            ctrlScheduledTest1.TestType = (int)_enTestType;

        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            _currentTest = clsTests.FindByAppointmentID(_TestAppointmentID);

            if (_currentTest != null) 
            {
                ctrlScheduledTest1.Load_LDL_Info(_LDLAppID, _TestAppointmentID);
                lblUserMessage.Visible = true;
                rbPass.Enabled = false;
                rbFail.Enabled = false;

                if (_currentTest.TestResult == true) 
                {
                    rbPass.Checked = true;
                }
                else
                {
                    rbFail.Checked = true;
                }
                txtNotes.Text = _currentTest.Notes;

            }
            else
            {
                _currentTest = new clsTests();
                ctrlScheduledTest1.Load_LDL_Info(_LDLAppID, _TestAppointmentID);
                lblUserMessage.Visible = false;
            }


            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult result = 
            MessageBox.Show(
                            "Are you sure you want to save? After that you can't change the Pass/Fail result after you save.",
                            "Confirm",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question
                           );

            if (result == DialogResult.No)
            {
                return;
            }


            bool TestResult = false;

            if (rbPass.Checked)
            {
                TestResult = true;
            }
            else if (rbFail.Checked) 
            {
                TestResult = false;
            }


            _currentTest.TestAppointmentID = _TestAppointmentID;
            _currentTest.TestResult = TestResult;
            _currentTest.Notes = txtNotes.Text;
            _currentTest.CreatedByUserID = clsGlobal._CurrentUser.UserID;

            if (_currentTest.Save()) 
            {
                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed Saving Test Result", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
