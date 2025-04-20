using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsUserData
    {

        public static bool GetUserInfoByID(int UserID, ref int PersonID, ref string UserName,
            ref string Password, ref bool IsActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Select * from Users where UserID =@UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //The record was found
                    isFound = true;

                    PersonID = (int)reader["PersonID"];
                    UserName = (string)reader["UserName"];
                    Password = (string)reader["Password"];

                    IsActive = (bool)reader["IsActive"];
                   

               
                }
                else
                {
                    // the record was not found
                    isFound = false;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;

        }

        public static bool GetUserInfoByUsername(string UserName , ref int PersonID, ref int UserID,
           ref string Password, ref bool IsActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Select * from Users where UserName =@UserName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //The record was found
                    isFound = true;

                    PersonID = (int)reader["PersonID"];
                    UserID   = (int)reader["UserID"];
                    Password = (string)reader["Password"];

                    IsActive = (bool)reader["IsActive"];



                }
                else
                {
                    // the record was not found
                    isFound = false;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;

        }

        public static int AddNewUser(int PersonID, string UserName, string Password, bool IsActive)
        {
            //this function will return the new contact id if succeeded and -1 if not.
            int UserID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO Users (PersonID,UserName,Password,IsActive)
                             VALUES (@PersonID,@UserName,@Password,@IsActive);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

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
                //Console.WriteLine("Error: " + ex.Message);

            }
            finally
            {
                connection.Close();
            }
            return UserID;
        }

        public static bool UpdateUser(int UserID, int PersonID, string UserName,
          string Password, bool IsActive)
        {
            int rowAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"update Users 
                           set PersonID =@PersonID,
                           UserName = @UserName,
                           Password = @Password,
                           IsActive = @IsActive

                           where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);

            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
            return (rowAffected > 0);

        }

        public static bool DeleteUser(int UserID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            //will retrieve single value
            string query = @"Delete from Users
                                       where UserID = @UserID";

            SqlCommand Command = new SqlCommand(query, connection);

            Command.Parameters.AddWithValue("@UserID", UserID);


            try
            {
                connection.Open();
                rowsAffected = Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);


        }

        public static DataTable GetAllUsers(string FilterBy = "", string SearchText = "")
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);




            string query = @"
                              SELECT 
                                  Users.UserID, 
                                  Users.PersonID,
                                  ISNULL(People.FirstName, '') + ' ' + 
                                  ISNULL(People.SecondName, '') + ' ' + 
                                  ISNULL(People.ThirdName, '') + ' ' + 
                                  ISNULL(People.LastName, '') AS FullName, 
                                  Users.UserName, 
                                  Users.IsActive
                              FROM 
                                  People
                              INNER JOIN 
                                  Users ON People.PersonID = Users.PersonID;";

            if (FilterBy == "UserID")
            {
                query = @"
                              SELECT 
                                  Users.UserID, 
                                  Users.PersonID,
                                  ISNULL(People.FirstName, '') + ' ' + 
                                  ISNULL(People.SecondName, '') + ' ' + 
                                  ISNULL(People.ThirdName, '') + ' ' + 
                                  ISNULL(People.LastName, '') AS FullName, 
                                  Users.UserName, 
                                  Users.IsActive
                              FROM 
                                  People
                              INNER JOIN 
                                  Users ON People.PersonID = Users.PersonID where UserID Like @UserID +'%';";
            }
            if (FilterBy == "UserName")
            {
                query = @"
                              SELECT 
                                  Users.UserID, 
                                  Users.PersonID,
                                  ISNULL(People.FirstName, '') + ' ' + 
                                  ISNULL(People.SecondName, '') + ' ' + 
                                  ISNULL(People.ThirdName, '') + ' ' + 
                                  ISNULL(People.LastName, '') AS FullName, 
                                  Users.UserName, 
                                  Users.IsActive
                              FROM 
                                  People
                              INNER JOIN 
                                  Users ON People.PersonID = Users.PersonID where UserName Like @UserName +'%';";
            }

            if (FilterBy == "PersonID")
            {
                query = @"
                              SELECT 
                                  Users.UserID, 
                                  Users.PersonID,
                                  ISNULL(People.FirstName, '') + ' ' + 
                                  ISNULL(People.SecondName, '') + ' ' + 
                                  ISNULL(People.ThirdName, '') + ' ' + 
                                  ISNULL(People.LastName, '') AS FullName, 
                                  Users.UserName, 
                                  Users.IsActive
                              FROM 
                                  People
                              INNER JOIN 
                                  Users ON People.PersonID = Users.PersonID where PersonID Like @PersonID +'%';";
            }

            if (FilterBy == "FullName")
            {
                query = @"
                              SELECT 
                                  Users.UserID, 
                                  Users.PersonID,
                                  ISNULL(People.FirstName, '') + ' ' + 
                                  ISNULL(People.SecondName, '') + ' ' + 
                                  ISNULL(People.ThirdName, '') + ' ' + 
                                  ISNULL(People.LastName, '') AS FullName, 
                                  Users.UserName, 
                                  Users.IsActive
                              FROM 
                                  People
                              INNER JOIN 
                                  Users ON People.PersonID = Users.PersonID
                                  WHERE 
                                  ISNULL(People.FirstName, '') + ' ' + 
                                  ISNULL(People.SecondName, '') + ' ' + 
                                  ISNULL(People.ThirdName, '') + ' ' + 
                                  ISNULL(People.LastName, '')  LIKE @FullName +'%';";
            }
            if(FilterBy == "IsActive")
            {
                query = @"
                              SELECT 
                                  Users.UserID, 
                                  Users.PersonID,
                                  ISNULL(People.FirstName, '') + ' ' + 
                                  ISNULL(People.SecondName, '') + ' ' + 
                                  ISNULL(People.ThirdName, '') + ' ' + 
                                  ISNULL(People.LastName, '') AS FullName, 
                                  Users.UserName, 
                                  Users.IsActive
                              FROM 
                                  People
                              INNER JOIN 
                                  Users ON People.PersonID = Users.PersonID where IsActive = @IsActive  ;";
            }



            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", SearchText);
            command.Parameters.AddWithValue("@PersonID", SearchText);
            command.Parameters.AddWithValue("@UserName", SearchText);
            command.Parameters.AddWithValue("@FullName", SearchText);
            if(SearchText == "Yes")
            {
                command.Parameters.AddWithValue("@IsActive", 1);
            }
            if(SearchText == "No")
            {
                command.Parameters.AddWithValue("@IsActive", 0);
            }

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

            }
            finally
            {
                connection.Close();
            }

            return dt;
        }


        public static bool isUserExistByUserID(int UserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "select Found=1 from Users where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;
                reader.Close();


            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return (isFound);

        }

        public static bool isUserExistByPersonID(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "select Found=1 from Users where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

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
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return (isFound);

        }

      

    }
}
