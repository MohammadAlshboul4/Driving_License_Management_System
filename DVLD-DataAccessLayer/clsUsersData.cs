using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsUsersData
    {


        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();

            string query = @"
    SELECT 
        Users.UserID, 
        Users.PersonID, 
        (People.FirstName + ' ' + People.SecondName + ' ' + People.ThirdName + ' ' + People.LastName) AS FullName,
        Users.UserName, 
        Users.IsActive
    FROM 
        Users
    INNER JOIN 
        People ON Users.PersonID = People.PersonID;";

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

        public static bool GetUserInfoByUsernameAndPassword(string UserName, string Password,
         ref int UserID, ref int PersonID, ref bool IsActive)
        {
            bool isFound = false;
            string query = "SELECT * FROM Users WHERE Username = @Username and Password=@Password;";


            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", UserName);
                command.Parameters.AddWithValue("@Password", Password);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // The record was found
                            isFound = true;
                            UserID = (int)reader["UserID"];
                            PersonID = (int)reader["PersonID"];
                            IsActive = (bool)reader["IsActive"];

                        }
                        else
                        {
                            // The record was not found
                            isFound = false;
                        }
                    }

                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error: " + ex.Message);

                    isFound = false;
                }


            }

            return isFound;
        }

        public static int AddNewUser(int PersonID, string UserName, string Password, bool IsActive)
        {
            int UserID = -1;

            string query = @"INSERT INTO [dbo].[Users] 
                 ([PersonID], [UserName], [Password], [IsActive]) 
                 VALUES (@PersonID, @UserName, @Password, @IsActive);
                 SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@UserName", UserName);
                command.Parameters.AddWithValue("@Password", Password);
                command.Parameters.AddWithValue("@IsActive", IsActive);


                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        UserID = insertedID;
                    }

                }
                catch (Exception ex)
                {

                }

            }


            return UserID;

        }

        public static bool UpdateUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            int RowsAffected = 0;

            string query = @"UPDATE [dbo].[Users] 
                 SET [PersonID] = @PersonID, 
                     [UserName] = @UserName, 
                     [Password] = @Password, 
                     [IsActive] = @IsActive
                 WHERE [UserID] = @UserID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserID", UserID);
                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@UserName", UserName);
                command.Parameters.AddWithValue("@Password", Password);
                command.Parameters.AddWithValue("@IsActive", IsActive);


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

        public static bool DeleteUser(int UserID)
        {
            int RowsAffected = 0;

            string query = "DELETE FROM  [dbo].[Users] WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserID", UserID);

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

        public static bool IsUserExistByUserID(int UserID)
        {
            bool IsFound = false;

            string query = "SELECT found=1 from Users where UserID = @UserID";


            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserID", UserID);

                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        IsFound = reader.HasRows;
                    }
                }
                catch (Exception ex)
                {

                }
            }


            return IsFound;
        }

        public static bool IsUserExistByPersonID(int PersonID)
        {
            bool IsFound = false;

            string query = "SELECT found=1 from Users where PersonID = @PersonID";


            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", PersonID);

                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        IsFound = reader.HasRows;
                    }
                }
                catch (Exception ex)
                {

                }
            }


            return IsFound;
        }


        public static bool IsUserExistByUserName(string UserName)
        {
            bool IsFound = false;

            string query = "SELECT found=1 from Users where UserName = @UserName";


            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserName", UserName);

                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        IsFound = reader.HasRows;
                    }
                }
                catch (Exception ex)
                {

                }
            }


            return IsFound;
        }

        public static bool GetUserInfoByUserID(int UserID, ref int PesronID, ref string UserName, ref string Password, ref bool IsActive)
        {
            bool IsFound = false;

            string query = "select * from Users where UserID = @UserID ";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue($"@UserID", UserID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            PesronID = reader["PersonID"] != DBNull.Value ? (int)reader["PersonID"] : -1;
                            UserName = reader["UserName"] != DBNull.Value ? (string)reader["UserName"] : "";
                            Password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : "";
                            IsActive = reader["IsActive"] != DBNull.Value ? (bool)reader["IsActive"] : false;

                            IsFound = true;
                        }
                    }
                }
                catch (Exception ex)
                {

                }

            }
            return IsFound;
        }


        public static bool ChangePasswordForUser(int UserID, string Password)
        {
            int RowsAffected = 0;

            string query = "UPDATE Users SET Password = @Password WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                       using (SqlCommand command = new SqlCommand(query, connection))
            {

                command.Parameters.AddWithValue("@UserID", UserID);
                command.Parameters.AddWithValue("@Password", Password);

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

    }
}
