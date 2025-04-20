using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsPersonData
    {

        public static bool GetPersonInfoByID(int PersonID,ref string NationalNo, ref string FirstName,
            ref string SecondName,ref string ThirdName,ref string LastName, ref DateTime DateOfBirth,
            ref byte Gendor, ref string Address, ref string Phone, ref string Email, 
            ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Select * from People where PersonID =@PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //The record was found
                    isFound = true;

                    NationalNo = (string)reader["NationalNo"];
                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];
                   
                    LastName = (string)reader["LastName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                   Gendor = (byte)reader["Gendor"];
                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];
                    
                    NationalityCountryID = (int)reader["NationalityCountryID"];

                    if(reader["ThirdName"] != DBNull.Value)
                    {
                        ThirdName = (string)reader["ThirdName"];
                    }
                    else
                    {
                        ThirdName = "";
                    }

                    if (reader["Email"] != DBNull.Value)
                    {
                        Email = (string)reader["Email"];
                    }
                    else
                    {
                        Email = "";
                    }

                    //ImagePath : The only one that allows Null in database se we should handle Null
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


        public static bool GetPersonInfoByNationalNo( string NationalNo , ref int PersonID, ref string FirstName,
            ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth,
            ref byte Gendor, ref string Address, ref string Phone, ref string Email,
            ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Select * from People where NationalNo =@NationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //The record was found
                    isFound = true;

                    PersonID = (int)reader["PersonID"];
                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];

                    LastName = (string)reader["LastName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Gendor = (byte)reader["Gendor"];
                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];

                    NationalityCountryID = (int)reader["NationalityCountryID"];

                    if (reader["ThirdName"] != DBNull.Value)
                    {
                        ThirdName = (string)reader["ThirdName"];
                    }
                    else
                    {
                        ThirdName = "";
                    }

                    if (reader["Email"] != DBNull.Value)
                    {
                        Email = (string)reader["Email"];
                    }
                    else
                    {
                        Email = "";
                    }

                    //ImagePath : The only one that allows Null in database se we should handle Null
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
        public static int AddNewPerson(string NationalNo, string FirstName,
             string SecondName,  string ThirdName,  string LastName,  DateTime DateOfBirth,
             byte Gendor, string Address,  string Phone,  string Email,
             int NationalityCountryID, string ImagePath)
        {
            //this function will return the new contact id if succeeded and -1 if not.
            int PersonID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO People (NationalNo,FirstName,SecondName,ThirdName, LastName,DateOfBirth,
                             Gendor,Address,Phone,Email,NationalityCountryID,ImagePath)
                             VALUES (@NationalNo,@FirstName,@SecondName,@ThirdName, @LastName,@DateOfBirth,
                             @Gendor,@Address,@Phone,@Email,@NationalityCountryID,@ImagePath);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);

            if (ThirdName != "")
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                command.Parameters.AddWithValue("@ThirdName",System.DBNull.Value);

            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
           
            if(Email !="")
                command.Parameters.AddWithValue("@Email", Email);
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (ImagePath != "")
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
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
                //Console.WriteLine("Error: " + ex.Message);

            }
            finally
            {
                connection.Close();
            }
            return PersonID;
        }



        public static bool UpdatePerson(int PersonID, string NationalNo, string FirstName,
             string SecondName, string ThirdName, string LastName, DateTime DateOfBirth,
             byte Gendor, string Address, string Phone, string Email,
             int NationalityCountryID, string ImagePath)
        {
            int rowAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"update People 
                           set NationalNo =@NationalNo,
                           FirstName = @FirstName,
                           SecondName = @SecondName,
                           ThirdName = @ThirdName,
                           LastName = @LastName,
                           DateOfBirth = @DateOfBirth,
                           Gendor = @Gendor,
                           Address = @Address,
                           Phone = @Phone,
                           Email = @Email,
                           NationalityCountryID = @NationalityCountryID,
                           ImagePath =@ImagePath
                           where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);

            if (ThirdName != "")
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);

            if (Email != "")
                command.Parameters.AddWithValue("@Email", Email);
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (ImagePath != "")
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

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

        public static bool DeletePerson(int PersonID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            //will retrieve single value
            string query = @"Delete from People
                                       where PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(query, connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);


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

        public static DataTable GetAllPeople( string FilterBy = "", string SearchText = "")
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);



        
              string  query = "SELECT People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName, People.LastName, " +
        "People.DateOfBirth, Gendor.GendorName, People.Phone, Countries.CountryName, People.Email " +
        "FROM People " +
        "INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID " +
        "INNER JOIN Gendor ON People.Gendor = Gendor.GendorID";
        

            if( FilterBy=="PersonID")
            {

                query = "SELECT People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName, People.LastName, " +
        "People.DateOfBirth, Gendor.GendorName, People.Phone, Countries.CountryName, People.Email " +
        "FROM People " +
        "INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID " +
        "INNER JOIN Gendor ON People.Gendor = Gendor.GendorID where PersonID like @PersonID + '%'";
            }
            if ( FilterBy == "NationalNo")
            {
                query = "SELECT People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName, People.LastName, " +
      "People.DateOfBirth, Gendor.GendorName, People.Phone, Countries.CountryName, People.Email " +
      "FROM People " +
      "INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID " +
      "INNER JOIN Gendor ON People.Gendor = Gendor.GendorID where NationalNo like @NationalNo + '%'";
            }
            if( FilterBy == "FirstName")
            {
                query = "SELECT People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName, People.LastName, " +
     "People.DateOfBirth, Gendor.GendorName, People.Phone, Countries.CountryName, People.Email " +
     "FROM People " +
     "INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID " +
     "INNER JOIN Gendor ON People.Gendor = Gendor.GendorID where FirstName like @FirstName + '%'";
            }
            if (FilterBy == "SecondName")
            {
                query = "SELECT People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName, People.LastName, " +
    "People.DateOfBirth, Gendor.GendorName, People.Phone, Countries.CountryName, People.Email " +
    "FROM People " +
    "INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID " +
    "INNER JOIN Gendor ON People.Gendor = Gendor.GendorID where SecondName like @SecondName + '%'";
            }
            if ( FilterBy == "ThirdName")
            {
                query = "SELECT People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName, People.LastName, " +
 "People.DateOfBirth, Gendor.GendorName, People.Phone, Countries.CountryName, People.Email " +
 "FROM People " +
 "INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID " +
 "INNER JOIN Gendor ON People.Gendor = Gendor.GendorID where ThirdName like @ThirdName + '%'";
            }
            if ( FilterBy == "LastName")
            {
                query = "SELECT People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName, People.LastName, " +
 "People.DateOfBirth, Gendor.GendorName, People.Phone, Countries.CountryName, People.Email " +
 "FROM People " +
 "INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID " +
 "INNER JOIN Gendor ON People.Gendor = Gendor.GendorID where LastName like @LastName + '%'";
            }

            if ( FilterBy == "GendorName")
            {
                query = "SELECT People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName, People.LastName, " +
"People.DateOfBirth, Gendor.GendorName, People.Phone, Countries.CountryName, People.Email " +
"FROM People " +
"INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID " +
"INNER JOIN Gendor ON People.Gendor = Gendor.GendorID where GendorName like @GendorName + '%'";
            }
            if ( FilterBy == "CountryName")
            {
                query = "SELECT People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName, People.LastName, " +
"People.DateOfBirth, Gendor.GendorName, People.Phone, Countries.CountryName, People.Email " +
"FROM People " +
"INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID " +
"INNER JOIN Gendor ON People.Gendor = Gendor.GendorID where CountryName like @CountryName + '%'";
            }

            if ( FilterBy == "Phone")
            {
                query = "SELECT People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName, People.LastName, " +
"People.DateOfBirth, Gendor.GendorName, People.Phone, Countries.CountryName, People.Email " +
"FROM People " +
"INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID " +
"INNER JOIN Gendor ON People.Gendor = Gendor.GendorID where Phone like @Phone + '%'";
            }

            if (FilterBy == "Email")
            {
                query = "SELECT People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName, People.LastName, " +
"People.DateOfBirth, Gendor.GendorName, People.Phone, Countries.CountryName, People.Email " +
"FROM People " +
"INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID " +
"INNER JOIN Gendor ON People.Gendor = Gendor.GendorID where Email like @Email + '%'";
            }

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", SearchText);
            command.Parameters.AddWithValue("@NationalNo", SearchText);
            command.Parameters.AddWithValue("@FirstName", SearchText);
            command.Parameters.AddWithValue("@SecondName", SearchText);
            command.Parameters.AddWithValue("@ThirdName", SearchText);
            command.Parameters.AddWithValue("@LastName", SearchText);
            command.Parameters.AddWithValue("@GendorName", SearchText);
            command.Parameters.AddWithValue("@CountryName", SearchText);
            command.Parameters.AddWithValue("@Phone", SearchText);
            command.Parameters.AddWithValue("@Email", SearchText);
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

        //public static DataTable GetContactsFilteredByContactID(string FilterBy = "", string SearchText = "")
        //{
        //    DataTable dt = new DataTable();
        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "Select * from Contacts";

        //    if (FilterBy == "ContactID")
        //    {


        //        query = "Select * from Contacts Where ContactID like @ContactID + '%'";
        //    }
        //    if (FilterBy == "FirstName")
        //    {
        //        query = "Select * from Contacts Where FirstName like @FirstName + '%'";
        //    }


        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@ContactID", SearchText);
        //    command.Parameters.AddWithValue("FirstName", SearchText);

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

        public static bool isPersonExist(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "select Found=1 from People where PersonID = @PersonID";

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

        public static bool isPersonExist(string NationalNo)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "select Found=1 from People where NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);

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
