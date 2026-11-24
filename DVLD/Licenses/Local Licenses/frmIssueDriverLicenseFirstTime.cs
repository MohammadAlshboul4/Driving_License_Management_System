using DVLD.Classes;
using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses.Local_Licenses
{
    public partial class frmIssueDriverLicenseFirstTime : Form
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;
        int _LDLApplicationID = -1;
        clsDrivers _NewDriver;
        clsLicenses _NewLicenses;
        clsLicenseClasses _CurrentLicenseClasses;
        clsLDLApplications _CurrentLDLApplication;

        clsApplicationsType _CurrentApplicationsType;

        public frmIssueDriverLicenseFirstTime(int LDLApplicationID)
        {
            InitializeComponent();
            _LDLApplicationID = LDLApplicationID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        void LoadData()
        {
            _CurrentLDLApplication = clsLDLApplications.FindLDLApplicationDataByLDLAppID(_LDLApplicationID);

            if (_CurrentLDLApplication == null)
            {
                MessageBox.Show("Didnt Find LDLApplication!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            /////

            _CurrentLicenseClasses = clsLicenseClasses.FindByID(_CurrentLDLApplication.LicenseClassID);

            if (_CurrentLicenseClasses == null)
            {
                MessageBox.Show("Didnt Find LicenseClasses!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            /////

            _CurrentApplicationsType = clsApplicationsType.FindApplicationTypeInfo(_CurrentLDLApplication.ApplicationTypeID);

            if (_CurrentApplicationsType == null)
            {
                MessageBox.Show("Didnt Find ApplicationsType!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            /////

            _NewLicenses = new clsLicenses();

        }

        private void frmIssueDriverLicenseFirstTime_Load(object sender, EventArgs e)
        {
            LoadData();
            ctrlLocalDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LDLApplicationID);

           

        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {

            _NewDriver = clsDrivers.FindByPersonID(_CurrentLDLApplication.ApplicantPersonID);

            if (_NewDriver == null)
            {
                _NewDriver = new clsDrivers();
                _NewDriver.PersonID = _CurrentLDLApplication.ApplicantPersonID;
                _NewDriver.CreatedByUserID = clsGlobal._CurrentUser.UserID;
                _NewDriver.CreatedDate = DateTime.Now;

                if (!_NewDriver.Save())
                {
                    MessageBox.Show("Failed To Save Driver", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            _NewLicenses.ApplicationID = _CurrentLDLApplication.ApplicationID;
            _NewLicenses.DriverID = _NewDriver.DriverID;
            _NewLicenses.LicenseClass = _CurrentLDLApplication.LicenseClassID;

            DateTime issueDate = DateTime.Now;
            _NewLicenses.IssueDate = issueDate;
            _NewLicenses.ExpirationDate = issueDate.AddYears(_CurrentLicenseClasses.DefaultValidityLength);

            _NewLicenses.Notes = txtNotes.Text;
            _NewLicenses.PaidFees = _CurrentLicenseClasses.ClassFees; 
            _NewLicenses.IsActive = true;
            _NewLicenses.IssueReason = (byte)clsLicenses.enIssueReason.FirstTime;
            _NewLicenses.CreatedByUserID = clsGlobal._CurrentUser.UserID;

           
            if (_NewLicenses.Save())
            {
                int LicenseID = _NewLicenses.LicenseID;
                _CurrentLDLApplication.SetComplete();

                MessageBox.Show($"License issued Successfully with LicenseID = {LicenseID}", "Succeeded",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close(); 
            }
            else
            {
                MessageBox.Show("Failed To Save License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
