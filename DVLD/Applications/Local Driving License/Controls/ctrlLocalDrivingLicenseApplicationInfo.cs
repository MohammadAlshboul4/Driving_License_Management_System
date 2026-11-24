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
    public partial class ctrlLocalDrivingLicenseApplicationInfo : UserControl
    {
        clsLDLApplications _CurrentLDLApplication;
        int _LDLAppID = -1;

        public int LDLApplicationID
        {

            get { return _LDLAppID; }
                    
        }

        public ctrlLocalDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
            
        }

        private void _FillLocalDrivingLicenseApplicationInfo()
        {

            lblLocalDrivingLicenseApplicationID.Text = _CurrentLDLApplication.LDLApplicationID.ToString();
            lblAppliedFor.Text = clsLicenseClasses.FindByID(_CurrentLDLApplication.LicenseClassID).ClassName;
            ctrlApplicationBasicInfo1.LoadApplicationInfo(_CurrentLDLApplication.ApplicationID);
            lblPassedTests.Text = (clsLDLApplications.GetPassedTestCount(_CurrentLDLApplication.LDLApplicationID)).ToString();

        }
        public void LoadApplicationInfoByLocalDrivingAppID(int LocalDrivingLicenseApplicationID)
        {
            _CurrentLDLApplication = clsLDLApplications.FindLDLApplicationDataByLDLAppID(LocalDrivingLicenseApplicationID);
            if (_CurrentLDLApplication == null)
            {
                _ResetLocalDrivingLicenseApplicationInfo();


                MessageBox.Show("No Application with ApplicationID = " + LocalDrivingLicenseApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FillLocalDrivingLicenseApplicationInfo();
        }

        public void LoadApplicationInfoByApplicationID(int ApplicationID)
        {
            _CurrentLDLApplication = clsLDLApplications.FindLdDLApplicationDataByApplicationID(ApplicationID);
            if (_CurrentLDLApplication == null)
            {
                _ResetLocalDrivingLicenseApplicationInfo();


                MessageBox.Show("No Application with ApplicationID = " + ApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FillLocalDrivingLicenseApplicationInfo();


        }

        

        private void _ResetLocalDrivingLicenseApplicationInfo()
        {
            ctrlApplicationBasicInfo1.ResetApplicationInfo();
            lblLocalDrivingLicenseApplicationID.Text = "[????]";
            lblAppliedFor.Text = "[????]";


        }
        
    }
}
