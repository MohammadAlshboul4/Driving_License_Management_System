using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsTests
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }

        // Backing field for Lazy Loading
        private clsTestAppointments _TestAppointmentInfo = null;

        // Lazy Loading Property
        public clsTestAppointments TestAppointmentInfo
        {
            get
            {
                if (_TestAppointmentInfo == null && this.TestAppointmentID != -1)
                {
                    _TestAppointmentInfo = clsTestAppointments.FindbyTestAppointmentID(this.TestAppointmentID);
                }
                return _TestAppointmentInfo;
            }
        }

        // Default Constructor (AddNew Mode)
        public clsTests()
        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Notes = string.Empty;
            this.CreatedByUserID = -1;

            this._TestAppointmentInfo = null;
            Mode = enMode.AddNew;
        }

        // Parameterized Constructor (Update Mode)
        private clsTests(int testID, int testAppointmentID, bool testResult, string notes, int createdByUserID)
        {
            this.TestID = testID;
            this.TestAppointmentID = testAppointmentID;
            this.TestResult = testResult;
            this.Notes = notes;
            this.CreatedByUserID = createdByUserID;

            this._TestAppointmentInfo = null; // Ready for lazy loading
            Mode = enMode.Update;
        }

        // FindbyTestAppointmentID by TestID
        public static clsTests Find(int testID)
        {
            int testAppointmentID = -1;
            bool testResult = false;
            string notes = string.Empty;
            int createdByUserID = -1;

            if (clsTestsData.GetTestInfoByID(testID, ref testAppointmentID, ref testResult, ref notes, ref createdByUserID))
            {
                return new clsTests(testID, testAppointmentID, testResult, notes, createdByUserID);
            }
            else
            {
                return null;
            }
        }

        // FindbyTestAppointmentID by TestAppointmentID
        public static clsTests FindByAppointmentID(int testAppointmentID)
        {
            int testID = -1;
            bool testResult = false;
            string notes = string.Empty;
            int createdByUserID = -1;

            if (clsTestsData.GetTestInfoByAppointmentID(testAppointmentID, ref testID, ref testResult, ref notes, ref createdByUserID))
            {
                return new clsTests(testID, testAppointmentID, testResult, notes, createdByUserID);
            }
            else
            {
                return null;
            }
        }

        public static clsTests FindLastTestByAppointmentID(int testAppointmentID)
        {
            int testID = -1;
            bool testResult = false;
            string notes = string.Empty;
            int createdByUserID = -1;

            if (clsTestsData.GetLastTestInfoByAppointmentID(testAppointmentID, ref testID, ref testResult, ref notes, ref createdByUserID))
            {
                return new clsTests(testID, testAppointmentID, testResult, notes, createdByUserID);
            }
            else
            {
                return null;
            }
        }

        // Add New Test (Internal)
        private bool _AddNewTest()
        {
            this.TestID = clsTestsData.AddNewTest(this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);

            return (this.TestID != -1);
        }

        // Update Test (Internal)
        private bool _UpdateTest()
        {
            return clsTestsData.UpdateTest(this.TestID, this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
        }

        // Save Method
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTest())
                    {
                        // Lock the appointment in the database once the test is taken
                        if (this.TestAppointmentInfo != null)
                        {
                            this.TestAppointmentInfo.IsLocked = true;
                            this.TestAppointmentInfo.Save();
                        }

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateTest();
            }

            return false;
        }

        // Delete Test
        public static bool DeleteTest(int testID)
        {
            return clsTestsData.DeleteTest(testID);
        }

        // Get All Tests
        public static DataTable GetAllTests()
        {
            return clsTestsData.GetAllTests();
        }

        public static byte GetPassedTestCount(int localDrivingLicenseApplicationID)
        {
            return clsTestsData.GetPassedTestCount(localDrivingLicenseApplicationID);
        }

        // Check if an application has passed a specific test type
        public static bool PassedAllTests(int localDrivingLicenseApplicationID)
        {
            return GetPassedTestCount(localDrivingLicenseApplicationID) == 3;
        }

    }
}
