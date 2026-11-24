using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_DataAccessLayer.clsDriversData;

namespace DVLDBusinessLayer
{
    public class clsDrivers
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public clsDrivers()
        {
            this.DriverID = -1;
            this.PersonID = -1;
            this.CreatedByUserID = -1;
            this.CreatedDate = DateTime.Now;

            Mode = enMode.AddNew;
        }

        private clsDrivers(int driverID, int personID, int createdByUserID, DateTime createdDate)
        {
            this.DriverID = driverID;
            this.PersonID = personID;
            this.CreatedByUserID = createdByUserID;
            this.CreatedDate = createdDate;

            Mode = enMode.Update;
        }

        public static clsDrivers FindByDriverID(int DriverID)
        {
            int personID = -1;
            int createdByUserID = -1;
            DateTime createdDate = DateTime.Now;

            bool isFound = DriversData.GetDriverInfoByDriverID(DriverID, ref personID, ref createdByUserID, ref createdDate);

            if (isFound)
            {
                return new clsDrivers(DriverID, personID, createdByUserID, createdDate);
            }
            else
            {
                return null;
            }
        }

        public static clsDrivers FindByPersonID(int PersonID)
        {
            int driverID = -1;
            int createdByUserID = -1;
            DateTime createdDate = DateTime.Now;

            bool isFound = DriversData.GetDriverInfoByPersonID(PersonID, ref driverID, ref createdByUserID, ref createdDate);

            if (isFound)
            {
                return new clsDrivers(driverID, PersonID, createdByUserID, createdDate);
            }
            else
            {
                return null;
            }
        }

        private bool _AddNewDriver()
        {
            this.DriverID = DriversData.AddNewDriver(this.PersonID, this.CreatedByUserID);

            return (this.DriverID != -1);
        }

        private bool _UpdateDriver()
        {
            return DriversData.UpdateDriver(this.DriverID, this.PersonID, this.CreatedByUserID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateDriver();
            }

            return false;
        }

        public static DataTable GetAllDrivers()
        {
            return DriversData.GetAllDrivers();
        }

        public static bool IsDriverExistByPersonID(int PersonID)
        {
            return DriversData.IsDriverExistByPersonID(PersonID);
        }
    }

}

