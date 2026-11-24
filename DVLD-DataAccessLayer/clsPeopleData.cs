using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace DVLD_DataAccessLayer
{

    public class clsPeopleData
    {

        public static DataTable GetAllPeople()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query =
              @"SELECT People.PersonID, People.NationalNo,
              People.FirstName, People.SecondName, People.ThirdName, People.LastName,
			  People.DateOfBirth, People.Gendor,  
				  CASE
                  WHEN People.Gendor = 0 THEN 'Male'

                  ELSE 'Female'

                  END as GendorCaption ,
			  People.Address, People.Phone, People.Email, 
              People.NationalityCountryID, Countries.CountryName, People.ImagePath
              FROM            People INNER JOIN
                         Countries ON People.NationalityCountryID = Countries.CountryID
                ORDER BY People.FirstName";




            SqlCommand command = new SqlCommand(query, connection);
            

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;

        }


        public static int AddNewPerson(string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth,
        short Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            int PersonID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"
                               INSERT INTO [dbo].[People]
                                          ([NationalNo]
                                          ,[FirstName]
                                          ,[SecondName]
                                          ,[ThirdName]
                                          ,[LastName]
                                          ,[DateOfBirth]
                                          ,[Gendor]
                                          ,[Address]
                                          ,[Phone]
                                          ,[Email]
                                          ,[NationalityCountryID]
                                          ,[ImagePath])
                                    VALUES
                                          (@NationalNo
                                          ,@FirstName
                                          ,@SecondName
                                          ,@ThirdName
                                          ,@LastName
                                          ,@DateOfBirth
                                          ,@Gendor
                                          ,@Address
                                          ,@Phone
                                          ,@Email
                                          ,@NationalityCountryID
                                          ,@ImagePath);
                               
                               SELECT SCOPE_IDENTITY();";



            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);
            command.Parameters.AddWithValue("@ThirdName", ThirdName);
            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.Add("@DateOfBirth", SqlDbType.Date).Value = DateOfBirth;
            command.Parameters.Add("@Gendor", SqlDbType.TinyInt).Value = Gendor;
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (!string.IsNullOrWhiteSpace(ImagePath))
            {
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            }
            else
            {
                command.Parameters.AddWithValue("@ImagePath", DBNull.Value);
            }

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    PersonID = insertedID;
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return PersonID;

        }


        public static bool GetPersonInfoByID(int ID, ref string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName,
         ref string LastName, ref DateTime DateOfBirth, ref short Gendor, ref string Address, ref string Phone, ref string Email,
          ref int NationalityCountryID, ref string ImagePath)

        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM People WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", ID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    IsFound = true;

                    NationalNo = reader["NationalNo"] != DBNull.Value ? (string)reader["NationalNo"] : "";
                    FirstName = reader["FirstName"] != DBNull.Value ? (string)reader["FirstName"] : "";
                    SecondName = reader["SecondName"] != DBNull.Value ? (string)reader["SecondName"] : "";
                    ThirdName = reader["ThirdName"] != DBNull.Value ? (string)reader["ThirdName"] : "";
                    LastName = reader["LastName"] != DBNull.Value ? (string)reader["LastName"] : "";
                    DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime)reader["DateOfBirth"] : DateTime.MinValue;
                    Gendor = reader["Gendor"] != DBNull.Value ? Convert.ToByte(reader["Gendor"]) : (byte)0;
                    Address = reader["Address"] != DBNull.Value ? (string)reader["Address"] : "";
                    Phone = reader["Phone"] != DBNull.Value ? (string)reader["Phone"] : "";
                    Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : "";
                    NationalityCountryID = reader["NationalityCountryID"] != DBNull.Value ? (int)reader["NationalityCountryID"] : -1;

                    if (reader["ImagePath"] != DBNull.Value)
                        ImagePath = (string)reader["ImagePath"];
                    else
                        ImagePath = "";
                }
                else
                {
                    IsFound = false;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // optionally log ex.Message
                IsFound = false;
            }
            finally
            {
                connection.Close();
            }

            return IsFound;
        }

        public static bool GetPersonInfoByNationalNo(string nationalNo,
            ref int PersonID, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName,
            ref DateTime DateOfBirth, ref short Gendor, ref string Address, ref string Phone, ref string Email,
            ref int NationalityCountryID, ref string ImagePath)
        {
            bool IsFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM People WHERE NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", nationalNo);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    IsFound = true;

                    PersonID = reader["PersonID"] != DBNull.Value ? (int)reader["PersonID"] : -1;
                    FirstName = reader["FirstName"] != DBNull.Value ? (string)reader["FirstName"] : "";
                    SecondName = reader["SecondName"] != DBNull.Value ? (string)reader["SecondName"] : "";
                    ThirdName = reader["ThirdName"] != DBNull.Value ? (string)reader["ThirdName"] : "";
                    LastName = reader["LastName"] != DBNull.Value ? (string)reader["LastName"] : "";
                    DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime)reader["DateOfBirth"] : DateTime.MinValue;
                    Gendor = reader["Gendor"] != DBNull.Value ? Convert.ToByte(reader["Gendor"]) : (byte)0;
                    Address = reader["Address"] != DBNull.Value ? (string)reader["Address"] : "";
                    Phone = reader["Phone"] != DBNull.Value ? (string)reader["Phone"] : "";
                    Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : "";
                    NationalityCountryID = reader["NationalityCountryID"] != DBNull.Value ? (int)reader["NationalityCountryID"] : -1;

                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = (string)reader["ImagePath"];
                    }
                    else
                    {
                        ImagePath = "";
                    }
  

                }
                else
                {
                    IsFound = false;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // optionally log ex.Message
                IsFound = false;
            }
            finally
            {
                connection.Close();
            }

            return IsFound;
        }



        public static bool UpdatePerson(int PersonID, string NationalNo,string FirstName, string SecondName, string ThirdName, string LastName,
        DateTime DateOfBirth, short Gendor, string Address, string Phone, string Email,
        int NationalityCountryID, string ImagePath)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"
            UPDATE [dbo].[People]
            SET [NationalNo] =@NationalNo,
                [FirstName] = @FirstName,
                [SecondName] = @SecondName,
                [ThirdName] = @ThirdName,
                [LastName] = @LastName,
                [DateOfBirth] = @DateOfBirth,
                [Gendor] = @Gendor,
                [Address] = @Address,
                [Phone] = @Phone,
                [Email] = @Email,
                [NationalityCountryID] = @NationalityCountryID,
                [ImagePath] = @ImagePath
            WHERE [PersonID] = @PersonID";

              

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@SecondName", SecondName);
                    command.Parameters.AddWithValue("@ThirdName", ThirdName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    command.Parameters.AddWithValue("@Gendor", Gendor);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Phone", Phone);
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

                    if (!string.IsNullOrEmpty(ImagePath))
                    {
                        command.Parameters.AddWithValue("@ImagePath", ImagePath);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ImagePath", DBNull.Value);
                    }
                    

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }

            return (rowsAffected > 0);
        }


        public static bool DeletePerson(int PersonID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "DELETE FROM  [dbo].[People] WHERE PersonID = @PersonID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }

            return (rowsAffected > 0);
        }

        public static bool IsPersonExistByPersonID(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT found=1 FROM People WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(@query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;



        }

        public static bool IsPersonExistByNationalNO(string NationalNo)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT found=1 FROM People WHERE NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(@query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;



        }





    }
}
