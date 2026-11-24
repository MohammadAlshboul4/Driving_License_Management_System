using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsApplicationsTypeData
    {

        public static DataTable GettAllApplicationTypes()
        {
            DataTable dt = new DataTable();

            string query = "select * from ApplicationTypes";


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
                catch (Exception ex)
                {
                    // log or handle error
                }
            }

            return dt;
        }

        public static bool UpdateApplicationType(int ApplicationTypeID, string applicationTypeTitle, decimal applicationFees)
        {
            int RowsAffected = 0;
            string query = @"UPDATE ApplicationTypes 
                     SET ApplicationTypeTitle = @ApplicationTypeTitle, 
                         ApplicationFees = @ApplicationFees 
                     WHERE ApplicationTypeID = @ApplicationTypeID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                command.Parameters.AddWithValue("@ApplicationTypeTitle", applicationTypeTitle);
                command.Parameters.AddWithValue("@ApplicationFees", applicationFees); // Handles fractional input gracefully

                try
                {
                    connection.Open();
                    RowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return RowsAffected > 0;
        }

        public static bool GetApplicationTypesInfoByTypeID(int ApplicationTypeID, ref string applicationTypeTitle, ref decimal applicationFees)
        {
            bool isFound = false;
            string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            applicationTypeTitle = reader["ApplicationTypeTitle"] != DBNull.Value
                                ? (string)reader["ApplicationTypeTitle"]
                                : "";
                         
                            applicationFees = reader["ApplicationFees"] != DBNull.Value
                                ? Convert.ToDecimal(reader["ApplicationFees"])
                                : 0;
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


       
    }
}
