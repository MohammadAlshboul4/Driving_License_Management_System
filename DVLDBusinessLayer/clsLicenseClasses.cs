using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsLicenseClasses
    {

        public int LicenseClassID  { get; set; }

        public string ClassName { get; set; }

        public string ClassDescription { get; set; }

        public int MinimumAllowedAge { get; set; }

        public int DefaultValidityLength { get; set; }

        public decimal ClassFees { get; set; }


        public clsLicenseClasses()
        {

            this.LicenseClassID = -1;
            this.ClassName = "";
            this.ClassDescription = "";
            this.MinimumAllowedAge = 0;
            this.DefaultValidityLength = 0;
            this.ClassFees = 0;

        }

        public clsLicenseClasses(int licenseClassID, string className, string classDescription,
              int minimumAllowedAge, int defaultValidityLength, decimal classFees)
        {
            this.LicenseClassID = licenseClassID;
            this.ClassName = className;
            this.ClassDescription = classDescription;
            this.MinimumAllowedAge = minimumAllowedAge;
            this.DefaultValidityLength = defaultValidityLength;
            this.ClassFees = classFees;
        }


        public static DataTable GetAllLicenseClassList()
        {
            return clsLicenseClassesData.GetAllLicenseClassesList();
        }
        public static clsLicenseClasses FindByID(int LicenseClasses)
        {

            string ClassName = "";
            string ClassDescription = "";
            int MinimumAllowedAge = 0;
            int DefaultValidityLength = 0;
            decimal ClassFees = 0;

            if (clsLicenseClassesData.GetLicenseClassesInfoByLicenseClassID(LicenseClasses, ref ClassName, ref ClassDescription,
                   ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))
            {

                return new clsLicenseClasses(LicenseClasses, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees);

            }
            else
            {

                return null;

            }

        }

        public static clsLicenseClasses FindByClassName(string ClassName)
        {

            int LicenseClasses = -1;
            string ClassDescription = "";
            int MinimumAllowedAge = 0;
            int DefaultValidityLength = 0;
            decimal ClassFees = 0;

            if (clsLicenseClassesData.GetLicenseClassesInfoByClassName(ClassName, ref LicenseClasses, ref ClassDescription,
                   ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))
            {

                return new clsLicenseClasses(LicenseClasses, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees);

            }
            else
            {

                return null;

            }

        }

    }
}
