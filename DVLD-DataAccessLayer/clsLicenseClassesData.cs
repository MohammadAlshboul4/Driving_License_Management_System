using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsLicenseClassesData
    {
        public static DataTable GetAllLicenseClassesList()
        {
            DataTable dt = new DataTable();

            string query = "Select * from LicenseClasses";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection)) 
            {
                try
                {
                    connection.Open();

                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                            dt.Load(reader);
                        }

                    }

                }
                catch (Exception ex)
                {
                   // Console.WriteLine(ex.Message);
                }

            }

            return dt;
        }

        public static bool GetLicenseClassesInfoByLicenseClassID(int LicenseClassID, ref string ClassName, ref string ClassDescription,
        ref int MinimumAllowedAge, ref int DefaultValidityLength, ref decimal ClassFees)
        {
            bool IsFound = false;

            string query = @"SELECT * FROM LicenseClasses 
                     WHERE LicenseClassID = @LicenseClassID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;

                            // Mapping string fields safely
                            ClassName = reader["ClassName"] != DBNull.Value ? (string)reader["ClassName"] : "";
                            ClassDescription = reader["ClassDescription"] != DBNull.Value ? (string)reader["ClassDescription"] : "";

                            // Casting tinyint database types cleanly to int parameters using your exact requested syntax
                            MinimumAllowedAge = reader["MinimumAllowedAge"] != DBNull.Value ? Convert.ToInt32(reader["MinimumAllowedAge"]) : -1;
                            DefaultValidityLength = reader["DefaultValidityLength"] != DBNull.Value ? Convert.ToInt32(reader["DefaultValidityLength"]) : -1;

                            // Mapping money/smallmoney safely to decimal
                            ClassFees = reader["ClassFees"] != DBNull.Value ? (decimal)reader["ClassFees"] : 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    IsFound = false;
                }
            }

            return IsFound;
        }

        public static bool GetLicenseClassesInfoByClassName(string ClassName, ref int LicenseClassID, ref string ClassDescription,
       ref int MinimumAllowedAge, ref int DefaultValidityLength, ref decimal ClassFees)
        {
            bool IsFound = false;

            string query = @"SELECT * FROM LicenseClasses 
                     WHERE ClassName = @ClassName";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ClassName", ClassName);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            IsFound = true;

                            // Mapping string fields safely
                            LicenseClassID = reader["LicenseClassID"] != DBNull.Value ? (int)reader["LicenseClassID"] : -1;
                            ClassDescription = reader["ClassDescription"] != DBNull.Value ? (string)reader["ClassDescription"] : "";

                            // Casting tinyint database types cleanly to int parameters using your exact requested syntax
                            MinimumAllowedAge = reader["MinimumAllowedAge"] != DBNull.Value ? Convert.ToInt32(reader["MinimumAllowedAge"]) : -1;
                            DefaultValidityLength = reader["DefaultValidityLength"] != DBNull.Value ? Convert.ToInt32(reader["DefaultValidityLength"]) : -1;

                            // Mapping money/smallmoney safely to decimal
                            ClassFees = reader["ClassFees"] != DBNull.Value ? (decimal)reader["ClassFees"] : 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    IsFound = false;
                }
            }

            return IsFound;
        }
    }
}
