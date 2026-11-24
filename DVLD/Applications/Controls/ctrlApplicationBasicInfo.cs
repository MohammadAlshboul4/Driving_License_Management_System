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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {
        clsApplications _CurrentApplication;
        int _ApplicationID = -1;
        public int ApplicationID
        {
            get { return _ApplicationID; }
        }

        
        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
            
        }

        public void LoadApplicationInfo(int ApplicationID)
        {
            _CurrentApplication = clsApplications.FindBaseApplication(ApplicationID);
            if (_CurrentApplication == null)
            {
                ResetApplicationInfo();
                MessageBox.Show("No Application with ApplicationID = " + ApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                _FillApplicationInfo();
        }

        private void _FillApplicationInfo()
        {
            _ApplicationID = _CurrentApplication.ApplicationID;
            lblApplicationID.Text = _CurrentApplication.ApplicationID.ToString();
            lblStatus.Text = _CurrentApplication.StatusText;
            lblType.Text = _CurrentApplication.ApplicationTypeInfo.ApplicationTypeTitle;
            lblFees.Text = _CurrentApplication.PaidFees.ToString();
            lblApplicantName.Text = _CurrentApplication.ApplicantFullName;
            lblDate.Text = clsFormat.DateToShort(_CurrentApplication.ApplicationDate);
            lblStatusDate.Text = clsFormat.DateToShort(_CurrentApplication.LastStatusDate);
            lblCreatedByUser.Text = _CurrentApplication.CreatedByUserInfo.UserName;
        }

        public void ResetApplicationInfo()
        {
            _ApplicationID = -1;

            lblApplicationID.Text = "[????]";
            lblStatus.Text = "[????]";
            lblType.Text = "[????]";
            lblFees.Text = "[????]";
            lblApplicantName.Text = "[????]";
            lblDate.Text = "[????]";
            lblStatusDate.Text = "[????]";
            lblCreatedByUser.Text = "[????]";

        }

        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo(_CurrentApplication.ApplicantPersonID);
            frm.ShowDialog();

            //Refresh
            LoadApplicationInfo(_ApplicationID);

        }
    }
}
