using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsApplicationsType
    {

        public enum enApplicationType
        {
            NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 7
        };

        public int ApplicationTypeID { get; set; }

        public string ApplicationTypeTitle { get; set; }

        public decimal ApplicationFees { get; set; }

        public clsApplicationsType()
        {
            this.ApplicationTypeID = -1;
            this.ApplicationTypeTitle = "";
            this.ApplicationFees = 0;
        }

       
        public clsApplicationsType(int applicationTypeID, string applicationTypeTitle, decimal applicationFees)
        {
            this.ApplicationTypeID = applicationTypeID;
            this.ApplicationTypeTitle = applicationTypeTitle;
            this.ApplicationFees = applicationFees;
        }

        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationsTypeData.GettAllApplicationTypes();
        }



        public static bool UpdateApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, decimal ApplicationFees)
        {

            return clsApplicationsTypeData.UpdateApplicationType(ApplicationTypeID, ApplicationTypeTitle, ApplicationFees);

        }


        public static clsApplicationsType FindApplicationTypeInfo(int applicationTypeID)
        {
            string applicationTypeTitle = "";
            decimal applicationFees = 0;

             if(clsApplicationsTypeData.GetApplicationTypesInfoByTypeID(applicationTypeID, ref applicationTypeTitle, ref applicationFees))
            {

                return new clsApplicationsType(applicationTypeID, applicationTypeTitle, applicationFees);
            }
            else
            {
                return null;
            }


        }


        
     }


 }


    

