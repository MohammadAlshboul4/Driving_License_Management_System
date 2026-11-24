using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsApplications
    {

        
            public enum enMode { AddNew = 0, Update = 1 };
            public enum enApplicationType
            {
                NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
                ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 7
            };
            public enum enApplicationStatus { New = 1, Cancelled = 2, Completed = 3 };

            public enMode Mode = enMode.AddNew;

            public int ApplicationID { set; get; }
            public int ApplicantPersonID { set; get; }

            // Lazy Loading for Applicant Person Name (Only fetches if PersonID is valid)
            public string ApplicantFullName
            {
                get
                {
                    if (this.ApplicantPersonID != -1)
                    {
                        clsPerson person = clsPerson.Find(this.ApplicantPersonID);
                        return person != null ? person.FullName : "";
                    }
                    return "";
                }
            }

            public DateTime ApplicationDate { set; get; }
            public int ApplicationTypeID { set; get; }

            // --- Lazy Loading for Application Type Info ---
            private clsApplicationsType _ApplicationTypeInfo = null;
            public clsApplicationsType ApplicationTypeInfo
            {
                get
                {
                    if (_ApplicationTypeInfo == null && this.ApplicationTypeID != -1)
                    {
                        _ApplicationTypeInfo = clsApplicationsType.FindApplicationTypeInfo(this.ApplicationTypeID);
                    }
                    return _ApplicationTypeInfo;
                }
            }

            public enApplicationStatus ApplicationStatus { set; get; }

            public string StatusText
            {
                get
                {
                    switch (ApplicationStatus)
                    {
                        case enApplicationStatus.New:
                            return "New";
                        case enApplicationStatus.Cancelled:
                            return "Cancelled";
                        case enApplicationStatus.Completed:
                            return "Completed";
                        default:
                            return "Unknown";
                    }
                }
            }

            public DateTime LastStatusDate { set; get; }
            public decimal PaidFees { set; get; } // Adjusted to decimal
            public int CreatedByUserID { set; get; }

            // --- Lazy Loading for Created By User Info ---
            private clsUsers _CreatedByUserInfo = null;
            public clsUsers CreatedByUserInfo
            {
                get
                {
                    if (_CreatedByUserInfo == null && this.CreatedByUserID != -1)
                    {
                        _CreatedByUserInfo = clsUsers.FindUser(this.CreatedByUserID);
                    }
                    return _CreatedByUserInfo;
                }
            }




            public clsApplications()
            {
                this.ApplicationID = -1;
                this.ApplicantPersonID = -1;
                this.ApplicationDate = DateTime.Now;
                this.ApplicationTypeID = -1;
                this.ApplicationStatus = enApplicationStatus.New;
                this.LastStatusDate = DateTime.Now;
                this.PaidFees = 0; 
                this.CreatedByUserID = -1;

               
                this._ApplicationTypeInfo = null;
                this._CreatedByUserInfo = null;

                Mode = enMode.AddNew;
            }

            private clsApplications(int ApplicationID, int ApplicantPersonID,
                DateTime ApplicationDate, int ApplicationTypeID,
                enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
                decimal PaidFees, int CreatedByUserID) 
            {
                this.ApplicationID = ApplicationID;
                this.ApplicantPersonID = ApplicantPersonID;
                this.ApplicationDate = ApplicationDate;
                this.ApplicationTypeID = ApplicationTypeID;
                this.ApplicationStatus = ApplicationStatus;
                this.LastStatusDate = LastStatusDate;
                this.PaidFees = PaidFees;
                this.CreatedByUserID = CreatedByUserID;

                
                this._ApplicationTypeInfo = null;
                this._CreatedByUserInfo = null;

                Mode = enMode.Update;
            }





            private bool _AddNewApplication()
            {
                this.ApplicationID = clsApplicationsData.AddNewApplication(
                    this.ApplicantPersonID, this.ApplicationDate,
                    this.ApplicationTypeID, (int)this.ApplicationStatus, // Cast enum cleanly to int
                    this.LastStatusDate, this.PaidFees, this.CreatedByUserID);

                return (this.ApplicationID != -1);
            }

            private bool _UpdateApplication()
            {
                return clsApplicationsData.UpdateApplication(
                    this.ApplicationID, this.ApplicantPersonID, this.ApplicationDate,
                    this.ApplicationTypeID, (int)this.ApplicationStatus,
                    this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
            }

            public static clsApplications FindBaseApplication(int ApplicationID)
            {
                int ApplicantPersonID = -1;
                DateTime ApplicationDate = DateTime.Now;
                int ApplicationTypeID = -1;
                int ApplicationStatus = 1;
                DateTime LastStatusDate = DateTime.Now;
                decimal PaidFees = 0; // Adjusted to decimal
                int CreatedByUserID = -1;

                bool IsFound = clsApplicationsData.GetApplicationsInfoByApplicationID
                (
                    ApplicationID, ref ApplicantPersonID,
                    ref ApplicationDate, ref ApplicationTypeID,
                    ref ApplicationStatus, ref LastStatusDate,
                    ref PaidFees, ref CreatedByUserID
                );

                if (IsFound)
                    return new clsApplications(ApplicationID, ApplicantPersonID,
                                         ApplicationDate, ApplicationTypeID,
                                        (enApplicationStatus)ApplicationStatus, LastStatusDate,
                                         PaidFees, CreatedByUserID);
                else
                    return null;
            }


        public bool Cancel()
            {
                return clsApplicationsData.UpdateStatus(this.ApplicationID, (int)enApplicationStatus.Cancelled);
            }

            public bool SetComplete()
            {
                return clsApplicationsData.UpdateStatus(this.ApplicationID, (int)enApplicationStatus.Completed);
            }

            public bool Save()
            {
                switch (Mode)
                {
                    case enMode.AddNew:
                        if (_AddNewApplication())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case enMode.Update:
                        return _UpdateApplication();
                }

                return false;
            }

            public virtual bool Delete()
            {
                return clsApplicationsData.DeleteApplication(this.ApplicationID);
            }

            public static bool IsApplicationExist(int ApplicationID)
            {
                return clsApplicationsData.IsApplicationExist(ApplicationID);
            }

        //public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        //{
        //    return clsApplicationsData.DoesPersonHaveActiveApplication(PersonID, ApplicationTypeID);
        //}

        //public bool DoesPersonHaveActiveApplication(int ApplicationTypeID)
        //{
        //    return DoesPersonHaveActiveApplication(this.ApplicantPersonID, ApplicationTypeID);
        //}

        //public static int GetActiveApplicationID(int PersonID, clsApplication.enApplicationType ApplicationTypeID)
        //{
        //    return clsApplicationsData.GetActiveApplicationID(PersonID, (int)ApplicationTypeID);
        //}

        public static int GetActiveApplicationID_ForLicenseClass(int PersonID,
            clsApplications.enApplicationType ApplicationTypeID, int LicenseClassID)
        {
            return clsApplicationsData.GetActiveApplicationID_ForLicenseClass(PersonID, (int)ApplicationTypeID, LicenseClassID);
        }

        public static int GetCompletedApplicationID_ForLicenseClass(int PersonID,
            clsApplications.enApplicationType ApplicationTypeID, int LicenseClassID)
        {
            return clsApplicationsData.GetCompletedApplicationID_ForLicenseClass(PersonID, (int)ApplicationTypeID, LicenseClassID);
        }

        //public int GetActiveApplicationID(clsApplication.enApplicationType ApplicationTypeID)
        //{
        //    return GetActiveApplicationID(this.ApplicantPersonID, ApplicationTypeID);
        //}
    }

    
}
