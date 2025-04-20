using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsDetainedLicensesData
    {
        public static bool GetDetainedLicenseByDetainID(int DetainID, ref int LicenseID, ref DateTime DetainDate
    , ref Decimal Finefees, ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate,
    ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Select * from DetainedLicenses where DetainID = @DetainID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainID", DetainID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //The record was found
                    isFound = true;


                    LicenseID = (int)reader["LicenseID"];
                    DetainDate = (DateTime)reader["DetainDate"];
                    Finefees = (decimal)reader["Finefees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsReleased = (bool)reader["IsReleased"];
            

                    if (reader["ReleaseDate"] != DBNull.Value)
                    {
                        ReleaseDate = (DateTime)reader["ReleaseDate"];
                    }
                    else
                    {
                        ReleaseDate = DateTime.MinValue;
                    }
                
                    if (reader["ReleasedByUserID"] != DBNull.Value)
                    {
                        ReleasedByUserID = (int)reader["ReleasedByUserID"];
                    }
                    else
                    {
                        ReleasedByUserID = -1;
                    }


                    if (reader["ReleaseApplicationID"] != DBNull.Value)
                    {
                        ReleaseApplicationID = (int)reader["ReleaseApplicationID"];
                    }
                    else
                    {
                        ReleaseApplicationID = -1;
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


        public static bool GetDetainedLicenseByLicenseID(ref int DetainID, int LicenseID, ref DateTime DetainDate
   , ref Decimal Finefees, ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate,
   ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT TOP 1 * 
                                           FROM DetainedLicenses 
                                           WHERE LicenseID = @LicenseID
                                           ORDER BY DetainID desc;";
                                           
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //The record was found
                    isFound = true;


                    DetainID = (int)reader["DetainID"];
                    DetainDate = (DateTime)reader["DetainDate"];
                    Finefees = (decimal)reader["Finefees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsReleased = (bool)reader["IsReleased"];


                    if (reader["ReleaseDate"] != DBNull.Value)
                    {
                        ReleaseDate = (DateTime)reader["ReleaseDate"];
                    }
                    else
                    {
                        ReleaseDate = DateTime.MinValue;
                    }

                    if (reader["ReleasedByUserID"] != DBNull.Value)
                    {
                        ReleasedByUserID = (int)reader["ReleasedByUserID"];
                    }
                    else
                    {
                        ReleasedByUserID = -1;
                    }


                    if (reader["ReleaseApplicationID"] != DBNull.Value)
                    {
                        ReleaseApplicationID = (int)reader["ReleaseApplicationID"];
                    }
                    else
                    {
                        ReleaseApplicationID = -1;
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
        public static int AddNewDetainedLicense(int LicenseID, DateTime DetainDate
    , Decimal Finefees, int CreatedByUserID, bool IsReleased, DateTime ReleaseDate,
    int ReleasedByUserID, int ReleaseApplicationID)
        {
            //this function will return the new contact id if succeeded and -1 if not.
            int DetainID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO DetainedLicenses
                             (LicenseID,DetainDate,Finefees,CreatedByUserID,IsReleased,ReleaseDate,ReleasedByUserID,ReleaseApplicationID)
                             VALUES (@LicenseID,@DetainDate,@Finefees,@CreatedByUserID,@IsReleased,@ReleaseDate,@ReleasedByUserID,@ReleaseApplicationID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@DetainDate", DetainDate);
            command.Parameters.AddWithValue("@Finefees", Finefees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsReleased", IsReleased);

            if (ReleaseDate != DateTime.MinValue)
            {
                command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
            }
            else
            {
                command.Parameters.AddWithValue("@ReleaseDate", System.DBNull.Value);
            }
            if (ReleasedByUserID != -1)
            {
                command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            }
            else
            {
                command.Parameters.AddWithValue("@ReleasedByUserID", System.DBNull.Value);
            }

            if (ReleaseApplicationID != -1)
            {
                command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
            }
            else
            {
                command.Parameters.AddWithValue("@ReleaseApplicationID", System.DBNull.Value);
            }

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    DetainID = insertedID;
                }


            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            {
                connection.Close();
            }
            return DetainID;
        }

        public static bool UpdateDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate
    , Decimal Finefees, int CreatedByUserID, bool IsReleased, DateTime ReleaseDate,
    int ReleasedByUserID, int ReleaseApplicationID)
        {
            int rowAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"update DetainedLicenses
                           set LicenseID =@LicenseID,
                           DetainDate = @DetainDate,
                           Finefees = @Finefees,                              
                           CreatedByUserID = @CreatedByUserID,
                           IsReleased = @IsReleased,
                           ReleaseDate = @ReleaseDate,
                           ReleasedByUserID = @ReleasedByUserID,
                           ReleaseApplicationID = @ReleaseApplicationID
  
                           where DetainID = @DetainID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainID", DetainID);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@DetainDate", DetainDate);
            command.Parameters.AddWithValue("@Finefees", Finefees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsReleased", IsReleased);

            if (ReleaseDate != DateTime.MinValue)
            {
                command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
            }
            else
            {
                command.Parameters.AddWithValue("@ReleaseDate", System.DBNull.Value);
            }
            if (ReleasedByUserID != -1)
            {
                command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            }
            else
            {
                command.Parameters.AddWithValue("@ReleasedByUserID", System.DBNull.Value);
            }

            if (ReleaseApplicationID != -1)
            {
                command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
            }
            else
            {
                command.Parameters.AddWithValue("@ReleaseApplicationID", System.DBNull.Value);
            }

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

        public static bool DeleteDetainedLicense(int DetainID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            //will retrieve single value
            string query = @"Delete from DetainedLicenses
                                       where DetainID = @DetainID";

            SqlCommand Command = new SqlCommand(query, connection);

            Command.Parameters.AddWithValue("@DetainID", DetainID);


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

        public static DataTable GetAllDetainedLicenses(string FilterBy = "", string SearchText = "")
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT        DetainedLicenses.DetainID, DetainedLicenses.LicenseID, DetainedLicenses.DetainDate, DetainedLicenses.IsReleased, DetainedLicenses.FineFees, DetainedLicenses.ReleaseDate, Drivers_View.NationalNo,Drivers_View.FullName, 
                         DetainedLicenses.ReleaseApplicationID
FROM            DetainedLicenses INNER JOIN
                         Licenses ON DetainedLicenses.LicenseID = Licenses.LicenseID INNER JOIN
                         Drivers_View ON Licenses.DriverID = Drivers_View.DriverID;";
           

            if(FilterBy == "Detain ID")
            {
                query = @"SELECT        DetainedLicenses.DetainID, DetainedLicenses.LicenseID, DetainedLicenses.DetainDate, DetainedLicenses.IsReleased, DetainedLicenses.FineFees, DetainedLicenses.ReleaseDate, Drivers_View.NationalNo,Drivers_View.FullName, 
                         DetainedLicenses.ReleaseApplicationID
FROM            DetainedLicenses INNER JOIN
                         Licenses ON DetainedLicenses.LicenseID = Licenses.LicenseID INNER JOIN
                         Drivers_View ON Licenses.DriverID = Drivers_View.DriverID where DetainID like @DetainID + '%';";
            }

            if (FilterBy == "Is Released")
            {
                query = @"SELECT        DetainedLicenses.DetainID, DetainedLicenses.LicenseID, DetainedLicenses.DetainDate, DetainedLicenses.IsReleased, DetainedLicenses.FineFees, DetainedLicenses.ReleaseDate, Drivers_View.NationalNo,Drivers_View.FullName, 
                         DetainedLicenses.ReleaseApplicationID
FROM            DetainedLicenses INNER JOIN
                         Licenses ON DetainedLicenses.LicenseID = Licenses.LicenseID INNER JOIN
                         Drivers_View ON Licenses.DriverID = Drivers_View.DriverID where IsReleased = @IsReleased;";
            }

            if (FilterBy == "National No.")
            {
                query = @"SELECT        DetainedLicenses.DetainID, DetainedLicenses.LicenseID, DetainedLicenses.DetainDate, DetainedLicenses.IsReleased, DetainedLicenses.FineFees, DetainedLicenses.ReleaseDate, Drivers_View.NationalNo,Drivers_View.FullName, 
                         DetainedLicenses.ReleaseApplicationID
FROM            DetainedLicenses INNER JOIN
                         Licenses ON DetainedLicenses.LicenseID = Licenses.LicenseID INNER JOIN
                         Drivers_View ON Licenses.DriverID = Drivers_View.DriverID where NationalNo like @NationalNo + '%';";
            }

            if (FilterBy == "Full Name")
            {
                query = @"SELECT        DetainedLicenses.DetainID, DetainedLicenses.LicenseID, DetainedLicenses.DetainDate, DetainedLicenses.IsReleased, DetainedLicenses.FineFees, DetainedLicenses.ReleaseDate, Drivers_View.NationalNo,Drivers_View.FullName, 
                         DetainedLicenses.ReleaseApplicationID
FROM            DetainedLicenses INNER JOIN
                         Licenses ON DetainedLicenses.LicenseID = Licenses.LicenseID INNER JOIN
                         Drivers_View ON Licenses.DriverID = Drivers_View.DriverID where FullName like @FullName + '%';";
            }

            if (FilterBy == "Release Application ID")
            {
                query = @"SELECT        DetainedLicenses.DetainID, DetainedLicenses.LicenseID, DetainedLicenses.DetainDate, DetainedLicenses.IsReleased, DetainedLicenses.FineFees, DetainedLicenses.ReleaseDate, Drivers_View.NationalNo,Drivers_View.FullName, 
                         DetainedLicenses.ReleaseApplicationID
FROM            DetainedLicenses INNER JOIN
                         Licenses ON DetainedLicenses.LicenseID = Licenses.LicenseID INNER JOIN
                         Drivers_View ON Licenses.DriverID = Drivers_View.DriverID where ReleaseApplicationID like @ReleaseApplicationID + '%';";
            }


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainID", SearchText);
            
  
            command.Parameters.AddWithValue("@NationalNo", SearchText);
            command.Parameters.AddWithValue("@FullName", SearchText);
            command.Parameters.AddWithValue("@ReleaseApplicationID", SearchText);
            if(SearchText == "Yes")
            command.Parameters.AddWithValue("@IsReleased", true);
            if( SearchText =="No")
                command.Parameters.AddWithValue("@IsReleased", false);
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

      
        public static bool IsDetainedLicenseExistByDetainID(int DetainID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query
                = "select Found=1 from DetainedLicenses where DetainID = @DetainID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainID", DetainID);

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

        public static bool IsDetainedLicenseExistByLicenseID(int LicenseID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query
                = "select Found=1 from DetainedLicenses where LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

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
