using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsTestAppointmentsData
    {

        public static DataTable GetAllListTestAppointments()
        {
            DataTable dt = new DataTable();

            string query = "select * from TestAppointments_View";
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

                }

            }

            return dt;
        }

        public static DataTable GetAllListTestAppointmentsByLDLApplicationIDAndTestTypeID(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {
            DataTable dt = new DataTable();

            string query = @"select * from TestAppointments
                             where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                             and TestTypeID = @TestTypeID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


                try
                {

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        
                            dt.Load(reader);
                        
                    }
                }
                catch (Exception ex)
                {

                }

            }

            return dt;
        }




        public static int AddNewTestAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID,
            DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked, int? RetakeTestApplicationID)
        {
            int TestAppointmentID = -1;

            string query = @"INSERT INTO TestAppointments 
                             (TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID)
                             VALUES 
                             (@TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate, @PaidFees, @CreatedByUserID, @IsLocked, @RetakeTestApplicationID);
                             SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                command.Parameters.AddWithValue("@IsLocked", IsLocked);

                // Safe handling for the nullable type
                command.Parameters.AddWithValue("@RetakeTestApplicationID",
                    (object)RetakeTestApplicationID ?? DBNull.Value);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        TestAppointmentID = insertedID;
                    }
                }
                catch (Exception ex)
                {
                    // Log exception if needed
                }
            }

            return TestAppointmentID;
        }

        public static bool UpdateTestAppointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID,
            DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked, int? RetakeTestApplicationID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE TestAppointments 
                             SET TestTypeID = @TestTypeID,
                                 LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID,
                                 AppointmentDate = @AppointmentDate,
                                 PaidFees = @PaidFees,
                                 CreatedByUserID = @CreatedByUserID,
                                 IsLocked = @IsLocked,
                                 RetakeTestApplicationID = @RetakeTestApplicationID
                             WHERE TestAppointmentID = @TestAppointmentID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                command.Parameters.AddWithValue("@IsLocked", IsLocked);

                // Safe handling for the nullable type
                command.Parameters.AddWithValue("@RetakeTestApplicationID",
                    (object)RetakeTestApplicationID ?? DBNull.Value);

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
        
        public static bool DeleteTestAppointment(int TestAppointmentID)
        {
            int rowsAffected = 0;

            string query = "DELETE FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Log exception if needed (e.g., if a foreign key constraint blocks deletion)
                    return false;
                }
            }

            return (rowsAffected > 0);
        }


        public static bool GetTestAppointmentInfoByTestAppointmentID(int TestAppointmentID,
        ref int TestTypeID, ref int LocalDrivingLicenseApplicationID,
        ref DateTime AppointmentDate, ref decimal PaidFees,
        ref int CreatedByUserID, ref bool IsLocked, ref int? RetakeTestApplicationID)
        {
            bool isFound = false;
            string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";

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

                            // Inline conditional checks for every field to protect against DBNull values
                            TestTypeID = (reader["TestTypeID"] != DBNull.Value) ? Convert.ToInt32(reader["TestTypeID"]) : -1;

                            LocalDrivingLicenseApplicationID = (reader["LocalDrivingLicenseApplicationID"] != DBNull.Value)
                                ? Convert.ToInt32(reader["LocalDrivingLicenseApplicationID"]) : -1;

                            AppointmentDate = (reader["AppointmentDate"] != DBNull.Value)
                                ? Convert.ToDateTime(reader["AppointmentDate"]) : DateTime.MinValue;

                            PaidFees = (reader["PaidFees"] != DBNull.Value) ? Convert.ToDecimal(reader["PaidFees"]) : 0;

                            CreatedByUserID = (reader["CreatedByUserID"] != DBNull.Value) ? Convert.ToInt32(reader["CreatedByUserID"]) : -1;

                            IsLocked = (reader["IsLocked"] != DBNull.Value) ? Convert.ToBoolean(reader["IsLocked"]) : false;

                            // Nullable assignment evaluation mapping
                            RetakeTestApplicationID = (reader["RetakeTestApplicationID"] != DBNull.Value)
                                ? Convert.ToInt32(reader["RetakeTestApplicationID"]) : (int?)null;
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

        public static bool GetTestAppointmentInfoByLDLAppID(int LocalDrivingLicenseApplicationID, ref int TestAppointmentID,
             ref int TestTypeID, ref DateTime AppointmentDate, ref decimal PaidFees, ref int CreatedByUserID,
            ref bool IsLocked, ref int? RetakeTestApplicationID)
        {
            bool isFound = false;

            // Retrieves the latest appointment for this application
            string query = @"SELECT TOP 1 * FROM TestAppointments 
                     WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                     ORDER BY TestAppointmentID DESC;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            TestAppointmentID = (reader["TestAppointmentID"] != DBNull.Value) ? Convert.ToInt32(reader["TestAppointmentID"]) : -1;
                            TestTypeID = (reader["TestTypeID"] != DBNull.Value) ? Convert.ToInt32(reader["TestTypeID"]) : -1;
                            AppointmentDate = (reader["AppointmentDate"] != DBNull.Value) ? Convert.ToDateTime(reader["AppointmentDate"]) : DateTime.MinValue;
                            PaidFees = (reader["PaidFees"] != DBNull.Value) ? Convert.ToDecimal(reader["PaidFees"]) : 0;
                            CreatedByUserID = (reader["CreatedByUserID"] != DBNull.Value) ? Convert.ToInt32(reader["CreatedByUserID"]) : -1;
                            IsLocked = (reader["IsLocked"] != DBNull.Value) ? Convert.ToBoolean(reader["IsLocked"]) : false;

                            RetakeTestApplicationID = (reader["RetakeTestApplicationID"] != DBNull.Value)
                                ? Convert.ToInt32(reader["RetakeTestApplicationID"]) : (int?)null;
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

        public static bool GetTestAppointmentInfoByLDLAppIDAndTestTypeID(int LocalDrivingLicenseApplicationID, ref int TestAppointmentID,
             int TestTypeID, ref DateTime AppointmentDate, ref decimal PaidFees, ref int CreatedByUserID,
            ref bool IsLocked, ref int? RetakeTestApplicationID)
        {
            bool isFound = false;

            // Retrieves the latest appointment for this application
            string query = @"SELECT TOP 1 * FROM TestAppointments 
                     WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                     and TestTypeID = @TestTypeID
                     ORDER BY TestAppointmentID DESC;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            TestAppointmentID = (reader["TestAppointmentID"] != DBNull.Value) ? Convert.ToInt32(reader["TestAppointmentID"]) : -1;
                            AppointmentDate = (reader["AppointmentDate"] != DBNull.Value) ? Convert.ToDateTime(reader["AppointmentDate"]) : DateTime.MinValue;
                            PaidFees = (reader["PaidFees"] != DBNull.Value) ? Convert.ToDecimal(reader["PaidFees"]) : 0;
                            CreatedByUserID = (reader["CreatedByUserID"] != DBNull.Value) ? Convert.ToInt32(reader["CreatedByUserID"]) : -1;
                            IsLocked = (reader["IsLocked"] != DBNull.Value) ? Convert.ToBoolean(reader["IsLocked"]) : false;

                            RetakeTestApplicationID = (reader["RetakeTestApplicationID"] != DBNull.Value)
                                ? Convert.ToInt32(reader["RetakeTestApplicationID"]) : (int?)null;
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

        // 2. IsExist by TestAppointmentID
        public static bool IsTestAppointmentExist(int TestAppointmentID)
        {
            bool isFound = false;

            string query = @"SELECT Found = 1 FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        isFound = true;
                    }
                }
                catch (Exception ex)
                {
                    isFound = false;
                }
            }

            return isFound;
        }

        // 3. IsExist by LocalDrivingLicenseApplicationID
        public static bool IsTestAppointmentExistByLDLAppID(int LocalDrivingLicenseApplicationID)
        {
            bool isFound = false;

            string query = @"SELECT Found = 1 FROM TestAppointments WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        isFound = true;
                    }
                }
                catch (Exception ex)
                {
                    isFound = false;
                }
            }

            return isFound;
        }


    }
}
