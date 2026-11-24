using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccessLayer
{
    public class clsLocalDrivingLicenseApplicationsData
    {

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT *
                              FROM LocalDrivingLicenseApplications_View
                              order by ApplicationDate Desc";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            dt.Load(reader);
                    }



                }
                catch (Exception ex )
                {

                }


            }

            return dt;
        }


        public static int AddNewLDLApplications(int ApplicationID, int LicenseClassID)
        {
            int LocalDrivingLicenseApplicationID = -1;

            string query = @"INSERT INTO [dbo].[LocalDrivingLicenseApplications]
                           ([ApplicationID], [LicenseClassID])
                     VALUES
                           (@ApplicationID, @LicenseClassID);
                     SELECT SCOPE_IDENTITY();";



            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar(); 

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        LocalDrivingLicenseApplicationID = insertedID;
                    }
                }
                catch (Exception ex)
                {
                    // Log your error here if needed
                }
            }

            return LocalDrivingLicenseApplicationID;
        }

        public static bool UpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE [dbo].[LocalDrivingLicenseApplications]
                        SET [ApplicationID] = @ApplicationID,
                            [LicenseClassID] = @LicenseClassID
                      WHERE [LocalDrivingLicenseApplicationID] = @LocalDrivingLicenseApplicationID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

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


        public static bool GetLocalDrivingLicenseApplicationInfoByID
          (int LocalDrivingLicenseApplicationID, ref int ApplicationID, ref int LicenseClassID)
        {
            bool isFound = false;
            
            
            string query = @"SELECT * FROM LocalDrivingLicenseApplications 
                     WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

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

                            ApplicationID = reader["ApplicationID"] != DBNull.Value ? Convert.ToInt32(reader["ApplicationID"]) : -1;
                            LicenseClassID = reader["LicenseClassID"] != DBNull.Value ? Convert.ToInt32(reader["LicenseClassID"]) : -1;
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

        public static bool GetLocalDrivingLicenseApplicationInfoBy_ApplicationID
         (int ApplicationID, ref int LocalDrivingLicenseApplicationID , ref int LicenseClassID)
        {
            bool isFound = false;


            string query = @"SELECT * FROM LocalDrivingLicenseApplications 
                     WHERE ApplicationID = @ApplicationID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            LocalDrivingLicenseApplicationID = reader["LocalDrivingLicenseApplicationID"] != DBNull.Value ? Convert.ToInt32(reader["LocalDrivingLicenseApplicationID"]) : -1;
                            LicenseClassID = reader["LicenseClassID"] != DBNull.Value ? Convert.ToInt32(reader["LicenseClassID"]) : -1;
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

        public static bool DeleteLDLApplication(int LocalDrivingLicenseApplicationID)
        {
            int RowsAffected = 0;

            string query = @"delete from LocalDrivingLicenseApplications
                      where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                try
                {
                    connection.Open();
                    RowsAffected = command.ExecuteNonQuery();

                }
                catch(Exception ex)
                {
                   
                }
            }

            return RowsAffected > 0;
        }


        public static int GetTrailsOfLDLApplication(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {

            int trailCount = 0;

            string query = @"SELECT TotalTrialsPerTest = COUNT(t.TestID)
                             FROM Tests t 
                             INNER JOIN TestAppointments ta ON t.TestAppointmentID = ta.TestAppointmentID
                             WHERE ta.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                               AND ta.TestTypeID = @TestTypeID;";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int parsedValue))
                    {
                        trailCount = parsedValue;
                    }

                }
                catch (Exception ex)
                {
                    trailCount = 0;
                }
            }

            return trailCount;
        }

        public static bool IsLocalDrivingLicenseApplicationExist(int LocalDrivingLicenseApplicationID)
        {
            bool isFound = false;

            string query = "SELECT Found = 1 FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
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

        public static int GetPassedTestCountByLDLApplicationID(int LDLApplicationID)
        {
            int PassedTestCount = 0;

            string query = @"SELECT PassedTestCount 
                     FROM LocalDrivingLicenseApplications_View 
                     WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LDLApplicationID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value && int.TryParse(result.ToString(), out int count))
                    {
                        PassedTestCount = count;
                    }
                }
                catch (Exception ex)
                {
                    PassedTestCount = 0;
                }
            }

            return PassedTestCount;
        }
    }
}
