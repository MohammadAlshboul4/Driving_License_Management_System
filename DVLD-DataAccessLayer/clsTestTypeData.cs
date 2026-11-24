using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsTestTypeData
    {

        public static DataTable GettAllTestTypesList()
        {
            DataTable dt = new DataTable();

            string query = "select * from TestTypes";


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

        public static bool UpdateTestTypes(int TestTypeID, string TestTypeTitle, string TestTypeDescription, decimal TestTypeFees)
        {
            int RowsAffected = 0;
            string query = @"UPDATE TestTypes 
                     SET TestTypeTitle = @TestTypeTitle, 
                         TestTypeDescription = @TestTypeDescription ,
                         TestTypeFees = @TestTypeFees
                         WHERE TestTypeID = @TestTypeID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
                command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
                command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);

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

        public static bool GetTestTypesInfoByTestTypeID(int TestTypeID, ref string TestTypeTitle,ref string TestTypeDescription, ref decimal TestTypeFees)
        {
            bool isFound = false;
            string query = "SELECT * FROM TestTypes WHERE TestTypeID = @TestTypeID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            TestTypeTitle = reader["TestTypeTitle"] != DBNull.Value
                                ? (string)reader["TestTypeTitle"]
                                : "";

                            TestTypeDescription = reader["TestTypeDescription"] != DBNull.Value
                              ? (string)reader["TestTypeDescription"]
                              : "";

                            TestTypeFees = reader["TestTypeFees"] != DBNull.Value
                                ? Convert.ToDecimal(reader["TestTypeFees"])
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


        public static bool GetTestTypesInfoByTitle(string TestTypeTitle, ref int TestTypeID, ref string TestTypeDescription, ref decimal TestTypeFees)
        {
            bool isFound = false;
            string query = "SELECT * FROM TestTypes WHERE TestTypeTitle = @TestTypeTitle";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            TestTypeID = reader["TestTypeID"] != DBNull.Value
                                ? (int)reader["TestTypeID"]
                                : -1;

                            TestTypeDescription = reader["TestTypeDescription"] != DBNull.Value
                              ? (string)reader["TestTypeDescription"]
                              : "";

                            TestTypeFees = reader["TestTypeFees"] != DBNull.Value
                                ? Convert.ToDecimal(reader["TestTypeFees"])
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
