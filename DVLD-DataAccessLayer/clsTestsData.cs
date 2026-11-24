using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public class clsTestsData
    {
        public static bool GetTestInfoByID(int TestID, ref int TestAppointmentID,
            ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = @"SELECT * FROM Tests WHERE TestID = @TestID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestID", TestID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            // Full DBNull checks for ALL columns
                            TestAppointmentID = reader["TestAppointmentID"] != DBNull.Value ? (int)reader["TestAppointmentID"] : -1;
                            TestResult = reader["TestResult"] != DBNull.Value ? (bool)reader["TestResult"] : false;
                            Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : string.Empty;
                            CreatedByUserID = reader["CreatedByUserID"] != DBNull.Value ? (int)reader["CreatedByUserID"] : -1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    isFound = false;
                }
            }

            return isFound;
        }

        public static bool GetTestInfoByAppointmentID(int TestAppointmentID, ref int TestID,
            ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = @"SELECT * FROM Tests WHERE TestAppointmentID = @TestAppointmentID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            // Full DBNull checks for ALL columns
                            TestID = reader["TestID"] != DBNull.Value ? (int)reader["TestID"] : -1;
                            TestResult = reader["TestResult"] != DBNull.Value ? (bool)reader["TestResult"] : false;
                            Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : string.Empty;
                            CreatedByUserID = reader["CreatedByUserID"] != DBNull.Value ? (int)reader["CreatedByUserID"] : -1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    isFound = false;
                }
            }

            return isFound;
        }

        public static bool GetLastTestInfoByAppointmentID(int TestAppointmentID, ref int TestID,
           ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = @"SELECT TOP 1 * FROM Tests 
                             WHERE TestAppointmentID = @TestAppointmentID 
                             ORDER BY TestID DESC ";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            // Full DBNull checks for ALL columns
                            TestID = reader["TestID"] != DBNull.Value ? (int)reader["TestID"] : -1;
                            TestResult = reader["TestResult"] != DBNull.Value ? (bool)reader["TestResult"] : false;
                            Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : string.Empty;
                            CreatedByUserID = reader["CreatedByUserID"] != DBNull.Value ? (int)reader["CreatedByUserID"] : -1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    isFound = false;
                }
            }

            return isFound;
        }

        public static int AddNewTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int testID = -1;

            string query = @"INSERT INTO Tests (TestAppointmentID, TestResult, Notes, CreatedByUserID)
                             VALUES (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // DBNull check for input parameters
                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID != -1 ? (object)TestAppointmentID : DBNull.Value);
                command.Parameters.AddWithValue("@TestResult", TestResult);

                if (string.IsNullOrWhiteSpace(Notes))
                    command.Parameters.AddWithValue("@Notes", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@Notes", Notes.Trim());

                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID != -1 ? (object)CreatedByUserID : DBNull.Value);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int insertedID))
                    {
                        testID = insertedID;
                    }
                }
                catch (Exception ex)
                {
                    testID = -1;
                }
            }

            return testID;
        }

        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE Tests
                             SET TestAppointmentID = @TestAppointmentID,
                                 TestResult = @TestResult,
                                 Notes = @Notes,
                                 CreatedByUserID = @CreatedByUserID
                             WHERE TestID = @TestID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestID", TestID);
                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID != -1 ? (object)TestAppointmentID : DBNull.Value);
                command.Parameters.AddWithValue("@TestResult", TestResult);

                if (string.IsNullOrWhiteSpace(Notes))
                    command.Parameters.AddWithValue("@Notes", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@Notes", Notes.Trim());

                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID != -1 ? (object)CreatedByUserID : DBNull.Value);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return (rowsAffected > 0);
        }

        public static bool DeleteTest(int TestID)
        {
            int rowsAffected = 0;

            string query = @"DELETE FROM Tests WHERE TestID = @TestID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestID", TestID);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return (rowsAffected > 0);
        }

        public static DataTable GetAllTests()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT * FROM Tests ORDER BY TestID DESC;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dt.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle error
                }
            }

            return dt;
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            byte passedTestCount = 0;

            string query = @"SELECT PassedTestCount = COUNT(TestID)
                             FROM Tests 
                             INNER JOIN TestAppointments 
                             ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                             WHERE TestAppointments.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                               AND Tests.TestResult = 1;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value && byte.TryParse(result.ToString(), out byte count))
                    {
                        passedTestCount = count;
                    }
                }
                catch (Exception ex)
                {
                    passedTestCount = 0;
                }
            }

            return passedTestCount;
        }
    }
}