using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsTestAppointments
    {
        public enum enMode { AddNew = 1, Update = 2 }
        private enMode _Mode = enMode.AddNew;

        public int TestAppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int? RetakeTestApplicationID { get; set; } // Nullable integer

        private clsLDLApplications _LocalDrivingLicenseApplicationInfo = null;

        public clsLDLApplications LocalDrivingLicenseApplicationInfo
        {
            get
            {
                if (_LocalDrivingLicenseApplicationInfo == null && this.LocalDrivingLicenseApplicationID != -1)
                {
                    _LocalDrivingLicenseApplicationInfo =
                        clsLDLApplications.FindLDLApplicationDataByLDLAppID(this.LocalDrivingLicenseApplicationID);

                }
                return _LocalDrivingLicenseApplicationInfo;
            }
        }

        // Default Constructor (AddNew Mode)
        public clsTestAppointments()
        {
            this.TestAppointmentID = -1;
            this.TestTypeID = -1;
            this.LocalDrivingLicenseApplicationID = -1;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            this.IsLocked = false;
            this.RetakeTestApplicationID = null;
            this._LocalDrivingLicenseApplicationInfo = null;

            _Mode = enMode.AddNew;
        }

        // Parameterized Constructor (Update Mode)
        private clsTestAppointments(int testAppointmentID, int testTypeID, int localDrivingLicenseApplicationID,
            DateTime appointmentDate, decimal paidFees, int createdByUserID, bool isLocked, int? retakeTestApplicationID)
        {
            this.TestAppointmentID = testAppointmentID;
            this.TestTypeID = testTypeID;
            this.LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            this.AppointmentDate = appointmentDate;
            this.PaidFees = paidFees;
            this.CreatedByUserID = createdByUserID;
            this.IsLocked = isLocked;
            this.RetakeTestApplicationID = retakeTestApplicationID;
            this._LocalDrivingLicenseApplicationInfo = null;

            _Mode = enMode.Update;
        }


        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentsData.GetAllListTestAppointments();
        }

        public static DataTable GetApplicationTestAppointmentsByLDLAppIDAndTestType(int LocalDrivingLicenseApplicationID,int TestType)
        {
            return clsTestAppointmentsData.GetAllListTestAppointmentsByLDLApplicationIDAndTestTypeID(LocalDrivingLicenseApplicationID, TestType);
        }
        private bool _AddNewTestAppointment()
        {
            // Call DataAccess Layer to insert record
            this.TestAppointmentID = clsTestAppointmentsData.AddNewTestAppointment(
                this.TestTypeID,
                this.LocalDrivingLicenseApplicationID,
                this.AppointmentDate,
                this.PaidFees,
                this.CreatedByUserID,
                this.IsLocked,
                this.RetakeTestApplicationID
            );

            return (this.TestAppointmentID != -1);
        }

        private bool _UpdateTestAppointment()
        {
            // Call DataAccess Layer to update record
            return clsTestAppointmentsData.UpdateTestAppointment(
                this.TestAppointmentID,
                this.TestTypeID,
                this.LocalDrivingLicenseApplicationID,
                this.AppointmentDate,
                this.PaidFees,
                this.CreatedByUserID,
                this.IsLocked,
                this.RetakeTestApplicationID
            );
        }

        public static bool Delete(int TestAppointmentID)
        {
            return clsTestAppointmentsData.DeleteTestAppointment(TestAppointmentID);
        }

        public static clsTestAppointments FindbyTestAppointmentID(int TestAppointmentID)
        {
            int TestTypeID = -1;
            int LocalDrivingLicenseApplicationID = -1;
            DateTime AppointmentDate = DateTime.MinValue;
            decimal PaidFees = 0;
            int CreatedByUserID = -1;
            bool IsLocked = false;
            int? RetakeTestApplicationID = null;

            // Call the data layer method that handles all DBNull protections safely
            if (clsTestAppointmentsData.GetTestAppointmentInfoByTestAppointmentID(
                TestAppointmentID, ref TestTypeID, ref LocalDrivingLicenseApplicationID,
                ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))
            {
                return new clsTestAppointments(
                    TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID,
                    AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID
                );
            }
            else
            {
                return null;
            }
        }

        public static clsTestAppointments FindByLDLAppID(int localDrivingLicenseApplicationID)
        {
            int testAppointmentID = -1;
            int testTypeID = -1;
            DateTime appointmentDate = DateTime.MinValue;
            decimal paidFees = 0;
            int createdByUserID = -1;
            bool isLocked = false;
            int? retakeTestApplicationID = null;

            if (clsTestAppointmentsData.GetTestAppointmentInfoByLDLAppID(localDrivingLicenseApplicationID,
                ref testAppointmentID,ref testTypeID, ref appointmentDate, ref paidFees,
                 ref createdByUserID, ref isLocked, ref retakeTestApplicationID))
            {
                return new clsTestAppointments(testAppointmentID, testTypeID, localDrivingLicenseApplicationID,
                    appointmentDate, paidFees, createdByUserID, isLocked, retakeTestApplicationID);
            }
            else
            {
                return null;
            }
        }

        public static clsTestAppointments FindByLDLAppIDAndTestTypeID(int localDrivingLicenseApplicationID, int TestTypeID)
        {
            int testAppointmentID = -1;
            
            DateTime appointmentDate = DateTime.MinValue;
            decimal paidFees = 0;
            int createdByUserID = -1;
            bool isLocked = false;
            int? retakeTestApplicationID = null;

            if (clsTestAppointmentsData.GetTestAppointmentInfoByLDLAppIDAndTestTypeID(localDrivingLicenseApplicationID,
                ref testAppointmentID, TestTypeID, ref appointmentDate, ref paidFees,
                 ref createdByUserID, ref isLocked, ref retakeTestApplicationID))
            {
                return new clsTestAppointments(testAppointmentID, TestTypeID, localDrivingLicenseApplicationID,
                    appointmentDate, paidFees, createdByUserID, isLocked, retakeTestApplicationID);
            }
            else
            {
                return null;
            }
        }


      

        // IsExist by TestAppointmentID
        public static bool IsTestAppointmentExist(int testAppointmentID)
        {
            return clsTestAppointmentsData.IsTestAppointmentExist(testAppointmentID);
        }

        // IsExist by LocalDrivingLicenseApplicationID
        public static bool IsTestAppointmentExistByLDLAppID(int localDrivingLicenseApplicationID)
        {
            return clsTestAppointmentsData.IsTestAppointmentExistByLDLAppID(localDrivingLicenseApplicationID);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestAppointment())
                    {
                        _Mode = enMode.Update; 
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateTestAppointment();

                default:
                    return false;
            }
        }



    }
}
