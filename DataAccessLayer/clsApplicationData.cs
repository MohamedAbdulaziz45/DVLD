using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsApplicationData
    {

        public static bool GetApplicationByID(int ApplicationID, ref int ApplicantPersonID,ref DateTime ApplicationDate,
           ref int ApplicationTypeID,ref byte ApplicationStatus,ref DateTime LastStatusDate,ref decimal PaidFees,
           ref int CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Select * from Applications where ApplicationID =@ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //The record was found
                    isFound = true;

                    ApplicantPersonID = (int)reader["ApplicantPersonID"];
                    ApplicationDate = (DateTime)reader["ApplicationDate"];
                    ApplicationTypeID = (int)reader["ApplicationTypeID"];
                    ApplicationStatus = (byte)reader["ApplicationStatus"];
                    LastStatusDate = (DateTime)reader["LastStatusDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];

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


        public static int AddNewApplication( int ApplicantPersonID,  DateTime ApplicationDate,
            int ApplicationTypeID,  byte ApplicationStatus,  DateTime LastStatusDate,  decimal PaidFees,
            int CreatedByUserID)
        {
            //this function will return the new contact id if succeeded and -1 if not.
            int UserID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO Applications (ApplicantPersonID,ApplicationDate,ApplicationTypeID
                              ,ApplicationStatus,LastStatusDate,PaidFees,CreatedByUserID)
                             VALUES (@ApplicantPersonID,@ApplicationDate,@ApplicationTypeID,@ApplicationStatus
                              ,@LastStatusDate,@PaidFees,@CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
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

        public static bool UpdaateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate,
            int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate, decimal PaidFees,
            int CreatedByUserID)
        {
            int rowAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"update Applications 
                           set ApplicantPersonID =@ApplicantPersonID,
                           ApplicationDate = @ApplicationDate,
                           ApplicationTypeID = @ApplicationTypeID,
                           ApplicationStatus = @ApplicationStatus,
                           LastStatusDate = @LastStatusDate,
                           PaidFees = @PaidFees,
                           CreatedByUserID = @CreatedByUserID
                           where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

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

        public static bool DeleteApplication(int ApplicationID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            //will retrieve single value
            string query = @"Delete from Applications
                                       where ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(query, connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);


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

        //public static DataTable GetAllUsers(string FilterBy = "", string SearchText = "")
        //{
        //    DataTable dt = new DataTable();
        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);




        //    string query = @"
        //                      SELECT 
        //                          Users.UserID, 
        //                          Users.PersonID,
        //                          ISNULL(People.FirstName, '') + ' ' + 
        //                          ISNULL(People.SecondName, '') + ' ' + 
        //                          ISNULL(People.ThirdName, '') + ' ' + 
        //                          ISNULL(People.LastName, '') AS FullName, 
        //                          Users.UserName, 
        //                          Users.IsActive
        //                      FROM 
        //                          People
        //                      INNER JOIN 
        //                          Users ON People.PersonID = Users.PersonID;";

        //    if (FilterBy == "UserID")
        //    {
        //        query = @"
        //                      SELECT 
        //                          Users.UserID, 
        //                          Users.PersonID,
        //                          ISNULL(People.FirstName, '') + ' ' + 
        //                          ISNULL(People.SecondName, '') + ' ' + 
        //                          ISNULL(People.ThirdName, '') + ' ' + 
        //                          ISNULL(People.LastName, '') AS FullName, 
        //                          Users.UserName, 
        //                          Users.IsActive
        //                      FROM 
        //                          People
        //                      INNER JOIN 
        //                          Users ON People.PersonID = Users.PersonID where UserID Like @UserID +'%';";
        //    }
        //    if (FilterBy == "UserName")
        //    {
        //        query = @"
        //                      SELECT 
        //                          Users.UserID, 
        //                          Users.PersonID,
        //                          ISNULL(People.FirstName, '') + ' ' + 
        //                          ISNULL(People.SecondName, '') + ' ' + 
        //                          ISNULL(People.ThirdName, '') + ' ' + 
        //                          ISNULL(People.LastName, '') AS FullName, 
        //                          Users.UserName, 
        //                          Users.IsActive
        //                      FROM 
        //                          People
        //                      INNER JOIN 
        //                          Users ON People.PersonID = Users.PersonID where UserName Like @UserName +'%';";
        //    }

        //    if (FilterBy == "PersonID")
        //    {
        //        query = @"
        //                      SELECT 
        //                          Users.UserID, 
        //                          Users.PersonID,
        //                          ISNULL(People.FirstName, '') + ' ' + 
        //                          ISNULL(People.SecondName, '') + ' ' + 
        //                          ISNULL(People.ThirdName, '') + ' ' + 
        //                          ISNULL(People.LastName, '') AS FullName, 
        //                          Users.UserName, 
        //                          Users.IsActive
        //                      FROM 
        //                          People
        //                      INNER JOIN 
        //                          Users ON People.PersonID = Users.PersonID where PersonID Like @PersonID +'%';";
        //    }

        //    if (FilterBy == "FullName")
        //    {
        //        query = @"
        //                      SELECT 
        //                          Users.UserID, 
        //                          Users.PersonID,
        //                          ISNULL(People.FirstName, '') + ' ' + 
        //                          ISNULL(People.SecondName, '') + ' ' + 
        //                          ISNULL(People.ThirdName, '') + ' ' + 
        //                          ISNULL(People.LastName, '') AS FullName, 
        //                          Users.UserName, 
        //                          Users.IsActive
        //                      FROM 
        //                          People
        //                      INNER JOIN 
        //                          Users ON People.PersonID = Users.PersonID
        //                          WHERE 
        //                          ISNULL(People.FirstName, '') + ' ' + 
        //                          ISNULL(People.SecondName, '') + ' ' + 
        //                          ISNULL(People.ThirdName, '') + ' ' + 
        //                          ISNULL(People.LastName, '')  LIKE @FullName +'%';";
        //    }
        //    if (FilterBy == "IsActive")
        //    {
        //        query = @"
        //                      SELECT 
        //                          Users.UserID, 
        //                          Users.PersonID,
        //                          ISNULL(People.FirstName, '') + ' ' + 
        //                          ISNULL(People.SecondName, '') + ' ' + 
        //                          ISNULL(People.ThirdName, '') + ' ' + 
        //                          ISNULL(People.LastName, '') AS FullName, 
        //                          Users.UserName, 
        //                          Users.IsActive
        //                      FROM 
        //                          People
        //                      INNER JOIN 
        //                          Users ON People.PersonID = Users.PersonID where IsActive = @IsActive  ;";
        //    }



        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@UserID", SearchText);
        //    command.Parameters.AddWithValue("@PersonID", SearchText);
        //    command.Parameters.AddWithValue("@UserName", SearchText);
        //    command.Parameters.AddWithValue("@FullName", SearchText);
        //    if (SearchText == "Yes")
        //    {
        //        command.Parameters.AddWithValue("@IsActive", 1);
        //    }
        //    if (SearchText == "No")
        //    {
        //        command.Parameters.AddWithValue("@IsActive", 0);
        //    }

        //    try
        //    {
        //        connection.Open();

        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.HasRows)
        //        {
        //            dt.Load(reader);
        //        }

        //        reader.Close();

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }

        //    return dt;
        //}


        public static bool isApplicationExistByAppID(int ApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "select Found=1 from Applications where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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

        //public static bool isUserExistByPersonID(int PersonID)
        //{
        //    bool isFound = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        //    string query = "select Found=1 from Users where PersonID = @PersonID";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@PersonID", PersonID);

        //    try
        //    {
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        isFound = reader.HasRows;
        //        reader.Close();


        //    }
        //    catch (Exception ex)
        //    {
        //        //Console.WriteLine("Error: " + ex.Message);
        //        isFound = false;
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //    return (isFound);

        //}



    }
}
