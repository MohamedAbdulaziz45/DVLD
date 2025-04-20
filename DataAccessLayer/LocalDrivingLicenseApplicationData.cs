using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class LocalDrivingLicenseApplicationData
    {

        public static bool GetLDLAppByID(int LDLAppID, ref int ApplicationID, ref int LicenseClassID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Select * from LocalDrivingLicenseApplications where LocalDrivingLicenseApplicationID =@LDLAppID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLAppID", LDLAppID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //The record was found
                    isFound = true;

                    ApplicationID = (int)reader["ApplicationID"];
                    LicenseClassID = (int)reader["LicenseClassID"];
      



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

        public static bool GetLDLAppByApplicationID(ref int LDLAppID, int ApplicationID, ref int LicenseClassID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Select * from LocalDrivingLicenseApplications where ApplicationID =@ApplicationID";

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

                    LDLAppID = (int)reader["LocalDrivingLicenseApplicationID"];
                    LicenseClassID = (int)reader["LicenseClassID"];




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
        public static bool GetLDLAppByIDView(int LDLAppID, ref string ClassName,ref string NationalNo
            ,ref string FullName,ref DateTime ApplicationDate,ref int PassedTestCount,ref string Status)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Select * from LocalDrivingLicenseApplications_View where LocalDrivingLicenseApplicationID =@LDLAppID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLAppID", LDLAppID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //The record was found
                    isFound = true;

                    ClassName = (string)reader["ClassName"];
                    NationalNo = (string)reader["NationalNo"];
                    FullName = (string)reader["FullName"];
                    ApplicationDate = (DateTime)reader["ApplicationDate"];
                    PassedTestCount = (int)reader["PassedTestCount"];
                    Status = (string)reader["Status"];




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
        public static int AddNewLDLApp(int ApplicationID, int LicenseClassID)
        {
            //this function will return the new contact id if succeeded and -1 if not.
            int LDLAppID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO LocalDrivingLicenseApplications
                             (ApplicationID,LicenseClassID)
                             VALUES (@ApplicationID,@LicenseClassID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);


            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LDLAppID = insertedID;
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
            return LDLAppID;
        }

        public static bool UpdateLDLApp(int LDLAppID, int ApplicationID, int LicenseClassID)
        {
            int rowAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"update LocalDrivingLicenseApplications 
                           set ApplicationID =@ApplicationID,
                           LicenseClassID = @LicenseClassID

                           where LocalDrivingLicenseApplicationID = @LDLAppID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);


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

        public static bool DeleteLDLApp(int LDLAppID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            //will retrieve single value
            string query = @"Delete from LocalDrivingLicenseApplications
                                       where LocalDrivingLicenseApplicationID = @LDLAppID";

            SqlCommand Command = new SqlCommand(query, connection);

            Command.Parameters.AddWithValue("@LDLAppID", LDLAppID);


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

        public static DataTable GetAllLDLApp(string FilterBy = "", string SearchText = "")
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);




            string query = @"select * from LocalDrivingLicenseApplications_View;";

            if(FilterBy == "L.D.L.AppID")
            {
                query = @"Select * from LocalDrivingLicenseApplications_View 
                         where LocalDrivingLicenseApplicationID Like @LDLAppID +'%'";
            }
            
            if(FilterBy == "National No.")
            {
                query = @"Select * from LocalDrivingLicenseApplications_View 
                         where NationalNo Like @NationalNo +'%'";
            }

            if (FilterBy == "Full Name")
            {
                query = @"Select * from LocalDrivingLicenseApplications_View 
                         where FullName Like @FullName +'%'";
            }
            
            if (FilterBy == "Status")
            {
                query = @"Select * from LocalDrivingLicenseApplications_View 
                         where Status Like @Status +'%'";
            }

            SqlCommand command = new SqlCommand(query, connection);
           
            command.Parameters.AddWithValue("@LDLAppID", SearchText);
            command.Parameters.AddWithValue("@NationalNo", SearchText);
            command.Parameters.AddWithValue("@FullName", SearchText);
            command.Parameters.AddWithValue("@Status", SearchText);
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


        public static bool isLDLAppExistByID(int LDLAppID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query
                = "select Found=1 from LocalDrivingLicenseApplications where LocalDrivingLicenseApplicationID = @LDLAppID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLAppID", LDLAppID);

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

        public static bool isLDLAppExistByPersonIDandLicenseClassID(int PersonID, int LicenseClassID, bool Cancelled = false,bool completed = false,bool New= false)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query =
  @"select * from LocalDrivingLicenseApplications_View where NationalNo = (select NationalNo from People where PersonID =@PersonID)
         and ClassName = (Select ClassName from LicenseClasses where LicenseClassID =@LicenseClassID)";

            if (Cancelled == true)
            {
                query =
 @"select * from LocalDrivingLicenseApplications_View where NationalNo = (select NationalNo from People where PersonID =@PersonID)
         and ClassName = (Select ClassName from LicenseClasses where LicenseClassID =@LicenseClassID) and Status LIKE 'Cancelled'";
            }

            if(completed == true)
            {
                query =
              @"select * from LocalDrivingLicenseApplications_View where NationalNo = (select NationalNo from People where PersonID =@PersonID)
         and ClassName = (Select ClassName from LicenseClasses where LicenseClassID =@LicenseClassID) and Status LIKE 'Completed'";
            }

            if(New == true)
            {
                query =
               @"select * from LocalDrivingLicenseApplications_View where NationalNo = (select NationalNo from People where PersonID =@PersonID)
         and ClassName = (Select ClassName from LicenseClasses where LicenseClassID =@LicenseClassID) and Status LIKE 'New'";
            }
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

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

        public static int GetLDLAppIDByPersonIDandClassName(int PersonID, int LicenseClassID, bool Cancelled = false)
        {
          
            int LDLAppID = -1;
           SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query =
  @"select * from LocalDrivingLicenseApplications_View where NationalNo = (select NationalNo from People where PersonID =@PersonID)
  and ClassName = (Select ClassName from LicenseClasses where LicenseClassID =@LicenseClassID);
SELECT SCOPE_IDENTITY();";

            if (Cancelled == true)
            {
                query =
 @"select * from LocalDrivingLicenseApplications_View where NationalNo = (select NationalNo from People where PersonID =@PersonID)
  and ClassName = (Select ClassName from LicenseClasses where LicenseClassID =@LicenseClassID) and Status LIKE 'Cancelled';
SELECT SCOPE_IDENTITY();";
            }


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LDLAppID = insertedID;
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
            return (LDLAppID);

        }


    }
}
