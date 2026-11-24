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
    public partial class frmAddUpdateLDLApplication : Form
    {

        clsLDLApplications _CurrentLDLApplications;

        enum enMode { AddNew = 0, Update = 1 }

        enMode _Mode = enMode.AddNew;

        int _LDLApplicationID = -1;
        int _PersonID = -1;
        

        public frmAddUpdateLDLApplication()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdateLDLApplication(int lDLApplicationID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _LDLApplicationID = lDLApplicationID;
        }



        private void btnNext_Click(object sender, EventArgs e)
        {
            if (ctrlPersonCardWithFilter1.SelectedPersonInfo == null)
            {
                tpApplicationInfo.Enabled = false;

                { MessageBox.Show("Please select a person.", "Error", MessageBoxButtons.OK); return; }

            }
            else
            {
                btnSave.Enabled = (tcApplicationInfo.SelectedIndex != 0);
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tpApplicationInfo;
            }
        }

        private void tcApplicationInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = (tcApplicationInfo.SelectedIndex != 0);

        }

        private void _FillLicenseClassesInComboBox()
        {
            DataTable dt = new DataTable();

            dt = clsLicenseClasses.GetAllLicenseClassList();

            foreach (DataRow Row in dt.Rows)
            {
                cbLicenseClass.Items.Add(Row["ClassName"]);
            }

           
        }
        private void _ResetDefaultValues()
        {
            if (_Mode == enMode.AddNew)
            {
                _CurrentLDLApplications = new clsLDLApplications();
                lblTitle.Text = "New Local Driving License Application";
                lblApplicationDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                lblFees.Text = clsApplicationsType.FindApplicationTypeInfo
                    ((int)clsApplications.enApplicationType.NewDrivingLicense).ApplicationFees.ToString();

                lblCreatedByUser.Text = clsGlobal._CurrentUser.UserName;
                _FillLicenseClassesInComboBox();
                if (cbLicenseClass.Items.Count > 0)
                {
                    cbLicenseClass.SelectedIndex = 0;
                }
            }


        }
        private void _LoadData()
        {
            _CurrentLDLApplications = clsLDLApplications.FindLDLApplicationDataByLDLAppID(_LDLApplicationID);
            if (_CurrentLDLApplications == null)
            {
                _ResetDefaultValues();
                MessageBox.Show("Applicant Person wasnt found", "Error", MessageBoxButtons.OK);
                return;
            }

            lblApplicationDate.Text = clsFormat.DateToShort(_CurrentLDLApplications.ApplicationDate);
            cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(_CurrentLDLApplications.LicenseClassesInfo.ClassName);
            lblFees.Text = _CurrentLDLApplications.ApplicationTypeInfo.ApplicationFees.ToString();
            lblCreatedByUser.Text = clsGlobal._CurrentUser.UserName;
        }
        private void frmAddUpdateApplication_Load(object sender, EventArgs e)
        {
           if (_Mode == enMode.AddNew)
           {
                _ResetDefaultValues();
                btnSave.Enabled = false;
                tpApplicationInfo.Enabled = false;
            }
           else
           {
                lblTitle.Text = "Update Local Driving License Application";
                ctrlPersonCardWithFilter1.Enabled = false;
                tpApplicationInfo.Enabled = true;
                btnNext.Enabled = true;
                _LoadData();
           }
            
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int licenseClassID = clsLicenseClasses.FindByClassName(cbLicenseClass.Text).LicenseClassID;

            int ActiveApplicationID = clsApplications.GetActiveApplicationID_ForLicenseClass(_PersonID,
                    clsApplications.enApplicationType.NewDrivingLicense, licenseClassID);

            if (ActiveApplicationID != -1) 
            {

                MessageBox.Show("Choose another License Class, the selected Person Already have " +
                                $"an Active application for the selected class with ID: {ActiveApplicationID}",
                                "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;



            }

            int CompletedApplicationID = clsApplications.GetCompletedApplicationID_ForLicenseClass(_PersonID,
                    clsApplications.enApplicationType.NewDrivingLicense, licenseClassID);

            if (CompletedApplicationID != -1) 
            {

                MessageBox.Show("Choose another License Class, the selected Person Already have" +
                 $" an Completed application for the selected class with ID : {CompletedApplicationID}",
                     "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            _CurrentLDLApplications.ApplicantPersonID = _PersonID;
            _CurrentLDLApplications.ApplicationDate = DateTime.Now;
            _CurrentLDLApplications.ApplicationTypeID = 1;
            _CurrentLDLApplications.ApplicationStatus = clsApplications.enApplicationStatus.New;
            _CurrentLDLApplications.LastStatusDate = DateTime.Now;
            _CurrentLDLApplications.PaidFees = Convert.ToDecimal(lblFees.Text);
            _CurrentLDLApplications.CreatedByUserID = clsGlobal._CurrentUser.UserID;
            _CurrentLDLApplications.LicenseClassID = licenseClassID;

            if(_CurrentLDLApplications.Save())
            {
                lblLocalDrivingLicebseApplicationID.Text = _CurrentLDLApplications.LDLApplicationID.ToString(); 
                lblTitle.Text = "Update Local Driving License Application";
                _Mode = enMode.Update;
             
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Data didnt Save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _PersonID = obj;
        }

        
       
    }
}
