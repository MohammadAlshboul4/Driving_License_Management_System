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

namespace DVLD
{
    public partial class ctrlScheduledTest : UserControl
    {

        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 }
        enTestType _TestType = enTestType.VisionTest;
        public int TestType = 0;

        clsTestAppointments _CurrentTestAppointments;
        clsLDLApplications _CurrentLDLApplicant;


        public ctrlScheduledTest()
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
        public void Load_LDL_Info(int LDLAppID,int TestAppointmentID)
        {
            decimal TestTypeFees = clsTestType.FindTestTypeInfoByID((clsTestType.enTestType)TestType).TestTypeFees;

            _CurrentLDLApplicant = clsLDLApplications.FindLDLApplicationDataByLDLAppID(LDLAppID);

            if (_CurrentLDLApplicant == null)
            {
                MessageBox.Show("Didnt find the LDLApplicant", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _CurrentTestAppointments = clsTestAppointments.FindbyTestAppointmentID(TestAppointmentID);

            if (_CurrentTestAppointments == null)
            {
                MessageBox.Show("Didnt find the Test Appointment", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblLocalDrivingLicenseAppID.Text = LDLAppID.ToString();
            lblLicenseClass.Text = _CurrentLDLApplicant.LicenseClassesInfo.ClassName;
            lblFullName.Text = _CurrentLDLApplicant.ApplicantFullName;
            lblTrial.Text = _CurrentLDLApplicant.GetTrailsOfLDLApplication((clsLDLApplications.enTestType)TestType).ToString();
            lblDate.Text = clsFormat.DateToShort(_CurrentTestAppointments.AppointmentDate);
            lblFees.Text = Convert.ToInt32(TestTypeFees).ToString();
        }


       
    }
}
