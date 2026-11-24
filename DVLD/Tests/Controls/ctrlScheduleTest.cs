using DVLD.Classes;
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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD
{
    public partial class ctrlScheduleTest : UserControl
    {

        public event Action OnSaveComplete;
        public enum enMode { Addnew = 1, Update = 2 }
        private enMode _Mode = enMode.Addnew;
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 }
         enTestType _TestType = enTestType.VisionTest;
        public int TestType = 0;

        int _LDLAppID = -1;
        decimal _TotalFees = 0;
        bool _RetakeTest = false;
        int AppType_RetakeTest = 7;

        clsLDLApplications _CurrentLDLApplicant;
        clsTestAppointments _CurrentTestAppointments;
        clsApplicationsType clsApplicationsType;
        public ctrlScheduleTest()
        {
            InitializeComponent();
            

        }

        void _SetTestTypePictureBox()
        {
            _TestType = (enTestType)TestType;

            switch (_TestType)
            {
                case enTestType.VisionTest:
                    pbTestTypeImage.Image = Resources.Vision_512;
                    break;

                case enTestType.WrittenTest:
                    pbTestTypeImage.Image = Resources.Written_Test_512;
                    break;

                case enTestType.StreetTest:
                    pbTestTypeImage.Image = Resources.driving_test_512;
                    break;

                default:
                    break;
            }

        }
        
        

        public void Load_LDL_Info(int LDLAppID,bool RetakeTest)
        {
            lblUserMessage.Visible = false;
            _RetakeTest = RetakeTest;
            _LDLAppID = LDLAppID;


            _CurrentLDLApplicant = clsLDLApplications.FindLDLApplicationDataByLDLAppID(LDLAppID);

            if (_CurrentLDLApplicant == null)
            {
                MessageBox.Show("Didnt find the LDLApplicant", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal TestTypeFees = clsTestType.FindTestTypeInfoByID((clsTestType.enTestType)TestType).TestTypeFees;
            decimal RetakeTestFees = clsApplicationsType.FindApplicationTypeInfo(AppType_RetakeTest).ApplicationFees;

            _CurrentTestAppointments = new clsTestAppointments();

            lblLocalDrivingLicenseAppID.Text = LDLAppID.ToString();
            lblLicenseClass.Text = _CurrentLDLApplicant.LicenseClassesInfo.ClassName;
            lblFullName.Text = _CurrentLDLApplicant.ApplicantFullName;
            lblTrial.Text = _CurrentLDLApplicant.GetTrailsOfLDLApplication((clsLDLApplications.enTestType)TestType).ToString();
            dtpTestDate.MinDate = DateTime.Today;
            dtpTestDate.Value = DateTime.Today;
            lblFees.Text = Convert.ToInt32(TestTypeFees).ToString();
            
            if (_RetakeTest == true)
            {
                gbRetakeTestInfo.Enabled = true;
                lblRetakeAppFees.Text = Convert.ToInt32(RetakeTestFees).ToString();
                _TotalFees = (TestTypeFees + RetakeTestFees);
                lblTotalFees.Text = Convert.ToInt32(TestTypeFees + RetakeTestFees).ToString();

            }
            else
            {
                _TotalFees = TestTypeFees;
            }

        }

        public void _LoadListTestAppointmentsData(int TestAppointmentID)
        {
            lblUserMessage.Visible = false;

            _CurrentTestAppointments = clsTestAppointments.FindbyTestAppointmentID(TestAppointmentID);

            decimal TestTypeFees = clsTestType.FindTestTypeInfoByID((clsTestType.enTestType)TestType).TestTypeFees;
            decimal RetakeTestFees = clsApplicationsType.FindApplicationTypeInfo(AppType_RetakeTest).ApplicationFees;

            if (_CurrentTestAppointments == null)
            {
                MessageBox.Show("Didnt find the TestAppointments", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _CurrentLDLApplicant = clsLDLApplications.FindLDLApplicationDataByLDLAppID
                (_CurrentTestAppointments.LocalDrivingLicenseApplicationID);

            _LDLAppID = _CurrentTestAppointments.LocalDrivingLicenseApplicationID;

            lblLocalDrivingLicenseAppID.Text = _CurrentTestAppointments.LocalDrivingLicenseApplicationID.ToString();
            lblLicenseClass.Text = _CurrentLDLApplicant.LicenseClassesInfo.ClassName;
            lblFullName.Text = _CurrentLDLApplicant.ApplicantFullName;
            lblTrial.Text = _CurrentLDLApplicant.GetTrailsOfLDLApplication((clsLDLApplications.enTestType)TestType).ToString();
            // 1. Reset MinDate temporarily so past dates can be loaded without throwing an exception
            dtpTestDate.MinDate = DateTimePicker.MinDateTime;
            // 2. Set the existing date
            dtpTestDate.Value = _CurrentTestAppointments.AppointmentDate;

            // 3. Optional: If the appointment is locked/past, disable the picker entirely
            if (_CurrentTestAppointments.IsLocked)
            {
                dtpTestDate.Enabled = false;
                btnSave.Enabled = false;
            }

            lblFees.Text = Convert.ToInt32(TestTypeFees).ToString();

            if (_RetakeTest == true)
            {
                gbRetakeTestInfo.Enabled = true;
                lblRetakeAppFees.Text = Convert.ToInt32(RetakeTestFees).ToString();
                _TotalFees = (TestTypeFees + RetakeTestFees);
                lblTotalFees.Text = Convert.ToInt32(TestTypeFees + RetakeTestFees).ToString();

                if (_CurrentTestAppointments.RetakeTestApplicationID != null)
                {
                    lblRetakeTestAppID.Text = _CurrentTestAppointments.RetakeTestApplicationID.ToString();
                }

            }
            else
            {
                _TotalFees = TestTypeFees;
            }
        }
        private void ctrlScheduleTest_Load(object sender, EventArgs e)
        {

            _SetTestTypePictureBox();
            gbRetakeTestInfo.Enabled = false;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            _CurrentTestAppointments.TestTypeID = TestType;
            _CurrentTestAppointments.LocalDrivingLicenseApplicationID = _LDLAppID;
            _CurrentTestAppointments.AppointmentDate = dtpTestDate.Value;
            _CurrentTestAppointments.PaidFees = _TotalFees;
            _CurrentTestAppointments.CreatedByUserID = clsGlobal._CurrentUser.UserID;
            _CurrentTestAppointments.IsLocked = false;

            if(_RetakeTest == true)
            {
                _CurrentLDLApplicant = clsLDLApplications.FindLDLApplicationDataByLDLAppID(_LDLAppID);

                if (_CurrentLDLApplicant == null)
                {
                    MessageBox.Show("Didnt find the LDLApplicant", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsApplications NewApplications = new clsApplications();

                NewApplications.ApplicantPersonID = _CurrentLDLApplicant.ApplicantPersonID;
                NewApplications.ApplicationDate = DateTime.Now;
                NewApplications.ApplicationTypeID = AppType_RetakeTest;
                NewApplications.ApplicationStatus = clsApplications.enApplicationStatus.New;
                NewApplications.LastStatusDate = DateTime.Now;

                NewApplications.PaidFees = clsApplicationsType.FindApplicationTypeInfo(AppType_RetakeTest).ApplicationFees;
                NewApplications.CreatedByUserID = clsGlobal._CurrentUser.UserID;

                if(NewApplications.Save())
                {
                    _CurrentTestAppointments.RetakeTestApplicationID = NewApplications.ApplicationID;
                }
                else
                {
                    MessageBox.Show("Failed Saving Retake Test Application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
            else
            {
                _CurrentTestAppointments.RetakeTestApplicationID = null;
            }

            if(_CurrentTestAppointments.Save())
            {

                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OnSaveComplete?.Invoke();

            }
            else
            {
                MessageBox.Show("Failed Saving New TestAppointments", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
            }

        }




    }
}
