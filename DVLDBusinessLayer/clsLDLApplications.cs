using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DVLDBusinessLayer
{
    public class clsLDLApplications : clsApplications
    {

        private enum enMode { Addnew = 1, Update = 2 }
        private enMode _Mode = enMode.Addnew;

        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 }
        private enTestType _TestType = enTestType.VisionTest;
        public int LDLApplicationID { get; set; }
        
        public int LicenseClassID { get; set; }

        // --- Lazy Loading for License Classes Info ---
        private clsLicenseClasses _LicenseClassesInfo = null;
        public clsLicenseClasses LicenseClassesInfo
        {
            get
            {
                if (_LicenseClassesInfo == null && this.LicenseClassID != -1)
                {
                    _LicenseClassesInfo = clsLicenseClasses.FindByID(this.LicenseClassID);
                }
                return _LicenseClassesInfo;
            }
        }

        // Default Constructor (AddNew Mode)
        public clsLDLApplications()
        {
            this.LDLApplicationID = -1;
            this.ApplicationID = -1;
            this.LicenseClassID = -1;

            this._LicenseClassesInfo = null;

            _Mode = enMode.Addnew;

            base.Mode = clsApplications.enMode.AddNew;
        }

        public clsLDLApplications(int lDLApplicationID, int applicationID, int licenseClassID,
         int applicantPersonID, DateTime applicationDate, int applicationTypeID,
         clsApplications.enApplicationStatus applicationStatus, DateTime lastStatusDate, decimal paidFees, int createdByUserID)
        {
            this.LDLApplicationID = lDLApplicationID;
            this.ApplicationID = applicationID;
            this.LicenseClassID = licenseClassID;

            this.ApplicantPersonID = applicantPersonID;
            this.ApplicationDate = applicationDate;
            this.ApplicationTypeID = applicationTypeID;
            this.ApplicationStatus = applicationStatus;
            this.LastStatusDate = lastStatusDate;
            this.PaidFees = paidFees;
            this.CreatedByUserID = createdByUserID;

            this._LicenseClassesInfo = null;

            _Mode = enMode.Update;
            base.Mode = clsApplications.enMode.Update;
        }




        public static DataTable GetLDLApplicationsViewList()
        {
            return clsLocalDrivingLicenseApplicationsData.GetAllLocalDrivingLicenseApplications();

        }

        private bool _AddNewLDLApplication()
        {
            this.LDLApplicationID = clsLocalDrivingLicenseApplicationsData.AddNewLDLApplications(this.ApplicationID, this.LicenseClassID);
            return this.LDLApplicationID != -1;
        }

        private bool _UpdateLDLApplication()
        {
            // Call DataAccess to update the child table link if values change
            return clsLocalDrivingLicenseApplicationsData.UpdateLocalDrivingLicenseApplication(
                this.LDLApplicationID, this.ApplicationID, this.LicenseClassID);
        }

        public override bool Delete()
        {
            if (clsLocalDrivingLicenseApplicationsData.DeleteLDLApplication(this.LDLApplicationID))
            {
                if (base.Delete()) 
                {
                    return true;
                }
                
            }
            return false;

        }
        public static clsLDLApplications FindLDLApplicationDataByLDLAppID(int lDLApplicationID)
        {
            int applicationID = -1;
            int licenseClassID = -1;

            if (clsLocalDrivingLicenseApplicationsData.GetLocalDrivingLicenseApplicationInfoByID(lDLApplicationID, ref applicationID, ref licenseClassID))
            {
                clsApplications baseApp = clsApplications.FindBaseApplication(applicationID);

                if (baseApp != null)
                {
                    return new clsLDLApplications(
                        lDLApplicationID,
                        applicationID,
                        licenseClassID,
                        baseApp.ApplicantPersonID,
                        baseApp.ApplicationDate,
                        baseApp.ApplicationTypeID,
                        baseApp.ApplicationStatus,
                        baseApp.LastStatusDate,
                        baseApp.PaidFees,
                        baseApp.CreatedByUserID
                    );
                }
            }

            return null;
        }

        public static clsLDLApplications FindLdDLApplicationDataByApplicationID(int applicationID)
        {
            int lDLApplicationID = -1;
            int licenseClassID = -1;

            if (clsLocalDrivingLicenseApplicationsData.GetLocalDrivingLicenseApplicationInfoBy_ApplicationID(applicationID, ref lDLApplicationID, ref licenseClassID))
            {
                clsApplications baseApp = clsApplications.FindBaseApplication(applicationID);

                if (baseApp != null)
                {
                    return new clsLDLApplications(
                        lDLApplicationID,
                        applicationID,
                        licenseClassID,
                        baseApp.ApplicantPersonID,
                        baseApp.ApplicationDate,
                        baseApp.ApplicationTypeID,
                        baseApp.ApplicationStatus,
                        baseApp.LastStatusDate,
                        baseApp.PaidFees,
                        baseApp.CreatedByUserID
                    );
                }
            }

            return null;
        }

        public static bool IsLocalDrivingLicenseApplicationExist(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenseApplicationsData.IsLocalDrivingLicenseApplicationExist(LocalDrivingLicenseApplicationID);
        }


        public int GetTrailsOfLDLApplication(enTestType testType)
        {
            return clsLocalDrivingLicenseApplicationsData.GetTrailsOfLDLApplication(this.LDLApplicationID, (int)testType);
        }

        public static int GetPassedTestCount(int LDLApplicationID)
        {
            return clsLocalDrivingLicenseApplicationsData.GetPassedTestCountByLDLApplicationID(LDLApplicationID);
        }
        public bool Save()
        {
            if (!base.Save())
                return false;

            this.ApplicationID = base.ApplicationID;

            switch (_Mode)
            {
                case enMode.Addnew:
                    if (_AddNewLDLApplication())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateLDLApplication();

                default:
                    return false;
            }
        }



    }
}
