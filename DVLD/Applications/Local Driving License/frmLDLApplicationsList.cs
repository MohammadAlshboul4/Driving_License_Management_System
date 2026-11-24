using DVLD.Licenses.Local_Licenses;
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
    public partial class frmLDLApplicationsList : Form
    {
        enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 }
        enTestType _TestType = enTestType.VisionTest;

        DataTable _dtLDLApplicationsView;
        clsLDLApplications _CurrentLDLApplication;

        clsTestAppointments _CurrentclsTestAppointments;

        clsTests _CurrentclsTests;
        public frmLDLApplicationsList()
        {
            InitializeComponent();
        }

        void _ApplyFilter()
        {
            string FilterColumn = cbFilterBy.SelectedItem.ToString(); ;
            string SearchValue = txtFilterBy.Text.Trim();

            if (string.IsNullOrEmpty(SearchValue) || FilterColumn == "None")
            {
                _dtLDLApplicationsView.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvLDLApplications.Rows.Count.ToString();
                return;
            }

            string FilterExpression = "";

            switch (FilterColumn)
            {
                case "L.D.LAppID":
                    FilterExpression = $"LocalDrivingLicenseApplicationID = {SearchValue}";
                    break;
                case "Driving Class":
                    FilterExpression = $"ClassName LIKE '%{SearchValue}%'";
                    break;
                case "National No":
                    FilterExpression = $"NationalNo LIKE '%{SearchValue}%'";
                    break;
                case "Full Name":
                    FilterExpression = $"FullName LIKE '%{SearchValue}%'";
                    break;
                case "Application Date":
                    FilterExpression = $"Convert(ApplicationDate, 'System.String') LIKE '%{SearchValue}%'";
                    break;
                case "Passed Tests":
                    FilterExpression = $"PassedTestCount = {SearchValue}";
                    break;
                case "Status":
                    FilterExpression = $"Status LIKE '%{SearchValue}%'";
                    break;

                default:
                    FilterExpression = "";
                    break;
            }


            _dtLDLApplicationsView.DefaultView.RowFilter = FilterExpression;

            lblRecordsCount.Text = dgvLDLApplications.Rows.Count.ToString();

        }
        void _RefreshLDLApplicationList()   
        {
            _dtLDLApplicationsView = clsLDLApplications.GetLDLApplicationsViewList();

            dgvLDLApplications.DataSource = _dtLDLApplicationsView;

           

            if (dgvLDLApplications.Rows.Count > 0)
            {

                dgvLDLApplications.Columns[0].HeaderText = "L.D.LAppID";
                dgvLDLApplications.Columns[0].Width = 120;

                dgvLDLApplications.Columns[1].HeaderText = "Driving Class";
                dgvLDLApplications.Columns[1].Width = 300;

                dgvLDLApplications.Columns[2].HeaderText = "National No";
                dgvLDLApplications.Columns[2].Width = 150;

                dgvLDLApplications.Columns[3].HeaderText = "Full Name";
                dgvLDLApplications.Columns[3].Width = 350;

                dgvLDLApplications.Columns[4].HeaderText = "Application Date";
                dgvLDLApplications.Columns[4].Width = 170;

                dgvLDLApplications.Columns[5].HeaderText = "Passed Tests";
                dgvLDLApplications.Columns[5].Width = 150;
            }

            // 1. Set the default font for all data cells/rows
            dgvLDLApplications.DefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);

            // 2. Set the font for the column headers
            dgvLDLApplications.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            // 3. Set the font for the row headers (the blank gray boxes on the far left)
            dgvLDLApplications.RowHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);

            lblRecordsCount.Text = dgvLDLApplications.Rows.Count.ToString();
        }
        
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            if (cbFilterBy.SelectedIndex == null)
            {
                return;
            }

            string SelectedColumn = cbFilterBy.SelectedItem.ToString();
            txtFilterBy.Visible = (SelectedColumn != "None");

            if (txtFilterBy.Visible)
            {
                txtFilterBy.Text = "";
                txtFilterBy.Focus();
            }

            if (_dtLDLApplicationsView != null) _dtLDLApplicationsView.DefaultView.RowFilter = "";
            lblRecordsCount.Text = dgvLDLApplications.Rows.Count.ToString();

        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            _ApplyFilter();
            
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.SelectedItem.ToString() == "L.D.LAppID" ||
                cbFilterBy.SelectedItem.ToString() == "Passed Tests")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }


        private void frmLDLApplicationsList_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            _RefreshLDLApplicationList();
        }

        

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmAddUpdateLDLApplication frm = new frmAddUpdateLDLApplication();
            frm.ShowDialog(this);
            _RefreshLDLApplicationList();

        }
       
        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ldlApplicationID = Convert.ToInt32(dgvLDLApplications.SelectedRows[0].Cells["LocalDrivingLicenseApplicationID"].Value);
            _CurrentLDLApplication = clsLDLApplications.FindLDLApplicationDataByLDLAppID(ldlApplicationID);

            if (_CurrentLDLApplication == null)
            {
                MessageBox.Show("Application data could not be retrieved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to cancel this Application?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                if (_CurrentLDLApplication.Cancel()) 
                {
                    MessageBox.Show("Application cancelled successfully.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshLDLApplicationList(); 
                }
                else
                {
                    MessageBox.Show("Failed to cancel the Application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            _RefreshLDLApplicationList();

        }

        private void showApplicationDetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ldlApplicationID = Convert.ToInt32(dgvLDLApplications.SelectedRows[0].Cells["LocalDrivingLicenseApplicationID"].Value);

            frmLocalDrivingLicenseApplicationInfo frm = new frmLocalDrivingLicenseApplicationInfo(ldlApplicationID);
            frm.ShowDialog(this);
            _RefreshLDLApplicationList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Delete this Application?", "Confirm",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
                
            int ldlApplicationID = Convert.ToInt32(dgvLDLApplications.SelectedRows[0].Cells["LocalDrivingLicenseApplicationID"].Value);

            _CurrentLDLApplication = clsLDLApplications.FindLDLApplicationDataByLDLAppID(ldlApplicationID);

            if (_CurrentLDLApplication == null)
            {
                MessageBox.Show("Application data could not be retrieved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_CurrentLDLApplication.Delete())
            {
                MessageBox.Show("LDLApplication Deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _RefreshLDLApplicationList();
            }
            else
            {
                MessageBox.Show("Failed to Deleted the LDLApplication due to data connected to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            _RefreshLDLApplicationList();


        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ldlApplicationID = Convert.ToInt32(dgvLDLApplications.SelectedRows[0].Cells["LocalDrivingLicenseApplicationID"].Value);

            frmListTestAppointments frm = new frmListTestAppointments(ldlApplicationID,(int)enTestType.VisionTest);
            frm.ShowDialog();
            _RefreshLDLApplicationList();

        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ldlApplicationID = Convert.ToInt32(dgvLDLApplications.SelectedRows[0].Cells["LocalDrivingLicenseApplicationID"].Value);

            frmListTestAppointments frm = new frmListTestAppointments(ldlApplicationID,(int)enTestType.WrittenTest);
            frm.ShowDialog();
            _RefreshLDLApplicationList();

        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ldlApplicationID = Convert.ToInt32(dgvLDLApplications.SelectedRows[0].Cells["LocalDrivingLicenseApplicationID"].Value);

            frmListTestAppointments frm = new frmListTestAppointments(ldlApplicationID,(int)enTestType.StreetTest);
            frm.ShowDialog();
            _RefreshLDLApplicationList();

        }



        bool CheckTestsResutls_ToEnableAndDisableDropDownTSM()
        {
            bool FinalTestPassed = false;

            int ldlApplicationID = Convert.ToInt32(dgvLDLApplications.SelectedRows[0].Cells["LocalDrivingLicenseApplicationID"].Value);

            _CurrentclsTestAppointments = clsTestAppointments.FindByLDLAppID(ldlApplicationID);

            if (_CurrentclsTestAppointments == null)
            {
                scheduleWrittenTestToolStripMenuItem.Enabled = false;
                scheduleStreetTestToolStripMenuItem.Enabled = false;
            }

            if (_CurrentclsTestAppointments != null)
            {
                _CurrentclsTests = clsTests.FindLastTestByAppointmentID(_CurrentclsTestAppointments.TestAppointmentID);

                if (_CurrentclsTestAppointments.TestTypeID == (int)enTestType.VisionTest)
                {
                    if (_CurrentclsTests != null && _CurrentclsTests.TestResult == true)
                    {
                        scheduleVisionTestToolStripMenuItem.Enabled = false;
                        scheduleWrittenTestToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        scheduleWrittenTestToolStripMenuItem.Enabled = false;
                        scheduleStreetTestToolStripMenuItem.Enabled = false;
                    }
                }

                if (_CurrentclsTestAppointments.TestTypeID == (int)enTestType.WrittenTest)
                {
                    if (_CurrentclsTests != null && _CurrentclsTests.TestResult == true)
                    {
                        scheduleWrittenTestToolStripMenuItem.Enabled = false;
                        scheduleVisionTestToolStripMenuItem.Enabled = false;

                    }
                    else
                    {
                        scheduleVisionTestToolStripMenuItem.Enabled = false;
                        scheduleStreetTestToolStripMenuItem.Enabled = false;
                    }
                }

                if (_CurrentclsTestAppointments.TestTypeID == (int)enTestType.StreetTest)
                {
                    if (_CurrentclsTests != null && _CurrentclsTests.TestResult == true)
                    {
                        scheduleStreetTestToolStripMenuItem.Enabled = false;
                        scheduleVisionTestToolStripMenuItem.Enabled = false;
                        scheduleWrittenTestToolStripMenuItem.Enabled = false;
                        FinalTestPassed = true;
                    }
                    else
                    {
                        scheduleVisionTestToolStripMenuItem.Enabled = false;
                        scheduleWrittenTestToolStripMenuItem.Enabled = false;

                    }
                }

            }
            return FinalTestPassed;
        }

        private void ScheduleToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            CheckTestsResutls_ToEnableAndDisableDropDownTSM();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (CheckTestsResutls_ToEnableAndDisableDropDownTSM() == true)
            {
                tsmScheduleTest.Enabled = false;
                tsmIssueLicnese.Enabled = true;
            }
            else
            {
                tsmScheduleTest.Enabled = true;
                tsmIssueLicnese.Enabled = false;

            }

            int ldlApplicationID = Convert.ToInt32(dgvLDLApplications.SelectedRows[0].Cells["LocalDrivingLicenseApplicationID"].Value);

            _CurrentLDLApplication = clsLDLApplications.FindLDLApplicationDataByLDLAppID(ldlApplicationID);

            bool DoesLicensesExist = clsLicenses.IsLicenseExistByApplicationID(_CurrentLDLApplication.ApplicationID);

            if( DoesLicensesExist == true )
            {
                tsmEditApplication.Enabled = false;
                tsmDeleteApplication.Enabled = false;
                tsmCancelApplication.Enabled = false;
                tsmScheduleTest.Enabled = false;
                tsmIssueLicnese.Enabled = false;

            }
        }

        private void issueDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ldlApplicationID = Convert.ToInt32(dgvLDLApplications.SelectedRows[0].Cells["LocalDrivingLicenseApplicationID"].Value);
            frmIssueDriverLicenseFirstTime frm = new frmIssueDriverLicenseFirstTime(ldlApplicationID);
            frm.ShowDialog();

            _RefreshLDLApplicationList();
        }


    }
}
