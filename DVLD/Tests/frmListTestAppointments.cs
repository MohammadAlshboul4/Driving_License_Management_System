using DVLD.Properties;
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
    public partial class frmListTestAppointments : Form
    {
        enum enTestType { VisionTest = 1, WrittenTest = 2 , StreetTest = 3}
        enTestType _TestType = enTestType.VisionTest;
        private DataTable _FilterList;
        private int _LDLAppID = -1;
        string lblTitleTest = "";


        clsTestAppointments _CurrentTestAppointments;
        clsTests _CurrentTests;

       

        public frmListTestAppointments(int LDLAppID,int TestType)
        {
            InitializeComponent();
            _LDLAppID = LDLAppID;
            _TestType = (enTestType)TestType;
        }

        void _RefreshListTestAppointments()
        {
            DataTable _dtListTestAppointments = clsTestAppointments.GetApplicationTestAppointmentsByLDLAppIDAndTestType(_LDLAppID,(int)_TestType);

            _FilterList = _dtListTestAppointments.DefaultView.ToTable(false, "TestAppointmentID", "AppointmentDate", "PaidFees", "IsLocked");
            

            dgvListTestAppointment.DataSource = _FilterList;

            if (dgvListTestAppointment.Rows.Count >= 0)
            {

                dgvListTestAppointment.Columns[0].HeaderText = "Appointment ID";
                dgvListTestAppointment.Columns[0].Width = 160;

                dgvListTestAppointment.Columns[1].HeaderText = "Appointment Date";
                dgvListTestAppointment.Columns[1].Width = 170;

                dgvListTestAppointment.Columns[2].HeaderText = "Paid Fees";
                dgvListTestAppointment.Columns[2].Width = 150;

                dgvListTestAppointment.Columns[3].HeaderText = "Is Locked";
                dgvListTestAppointment.Columns[3].Width = 100;

                
            }

            // 1. Set the default font for all data cells/rows
            dgvListTestAppointment.DefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);

            // 2. Set the font for the column headers
            dgvListTestAppointment.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            // 3. Set the font for the row headers (the blank gray boxes on the far left)
            dgvListTestAppointment.RowHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);

            lblRecordsCount.Text = dgvListTestAppointment.Rows.Count.ToString();

            switch (_TestType)
            {
                case enTestType.VisionTest:
                    pbTestTypeImage.Image = Resources.Vision_512;
                    lblTitle.Text = "Vision Test Appointments";
                    lblTitleTest = lblTitle.Text;
                    this.Text = "Vision Test Appointments";
                    break;

                case enTestType.WrittenTest:
                    pbTestTypeImage.Image = Resources.Written_Test_512;
                    lblTitle.Text = "Written TestAppointments";
                    lblTitleTest = lblTitle.Text;
                    this.Text = "Written TestAppointments";
                    break;

                case enTestType.StreetTest:
                    pbTestTypeImage.Image = Resources.driving_test_512;
                    lblTitle.Text = "Street Test Appointments";
                    lblTitleTest = lblTitle.Text;
                    this.Text = "Street Test Appointments";
                    break;

                default:
                    break;
            }

        }
        private void frmVisionTestsAppointments_Load(object sender, EventArgs e)
        {

            ctrlLocalDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LDLAppID);
            _RefreshListTestAppointments();

        }


        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {
            bool RetakeTest = false;

            _CurrentTestAppointments = clsTestAppointments.FindByLDLAppIDAndTestTypeID(_LDLAppID, (int)_TestType);


            if (_CurrentTestAppointments != null)
            {
                if (_CurrentTestAppointments.IsLocked == false)
                {
                    MessageBox.Show("Person Already have an active appointment for this test, You cannot add new appointment",
                        "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _CurrentTests = clsTests.FindByAppointmentID(_CurrentTestAppointments.TestAppointmentID);

                if (_CurrentTests != null && _CurrentTests.TestResult == true)
                {
                    MessageBox.Show("This Person Already passed this test before, you can only retake failed test",
                       "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }



                if (_CurrentTests != null && _CurrentTests.TestResult == false)
                {
                    RetakeTest = true;
                }

            }

            
            int TestType = (int)_TestType;


            frmScheduleTest frm = new frmScheduleTest(_LDLAppID, TestType, RetakeTest);
            frm.ShowDialog();
            _RefreshListTestAppointments();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestType = (int)_TestType;

            if (dgvListTestAppointment.SelectedRows.Count > 0)
            {
                int TestAppointmentID = Convert.ToInt32(dgvListTestAppointment.SelectedRows[0].Cells["TestAppointmentID"].Value);
                frmScheduleTest frm = new frmScheduleTest(_LDLAppID, TestType, TestAppointmentID);
                frm.ShowDialog();
                
            }
            _RefreshListTestAppointments();
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestType = (int)_TestType;


            if (dgvListTestAppointment.SelectedRows.Count > 0)
            {
                int TestAppointmentID = Convert.ToInt32(dgvListTestAppointment.SelectedRows[0].Cells["TestAppointmentID"].Value);
                frmTakeTest frm = new frmTakeTest(_LDLAppID, TestAppointmentID, TestType);
                frm.ShowDialog();
                
            }
            _RefreshListTestAppointments();

        }
    }
}
