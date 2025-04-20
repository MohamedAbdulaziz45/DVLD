using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Eventing.Reader;

namespace DataAccessLayer
{
    public class clsTestAppointmentsData
    {
        public static bool GetTestAppointmentByID(int TestAppointmentID, ref int TestTypeID, ref int LDLAppID
            ,ref DateTime AppointmentDate,ref decimal PaidFees,ref int CreatedByUserID,ref bool IsLocked
            ,ref int RetakeTestID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Select * from TestAppointments where TestAppointmentID =@TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //The record was found
                    isFound = true;

                    TestTypeID= (int)reader["TestTypeID"];
                    LDLAppID = (int)reader["LocalDrivingLicenseApplicationID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsLocked = (bool)reader["IsLocked"];

                    if (reader["RetakeTestID"] != DBNull.Value)
                    {
                        RetakeTestID = (int)reader["RetakeTestID"];
                    }
                    else
                    {
                        RetakeTestID = -1;
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

        public static bool GetLastTestAppointmentByDlAppIDAndTestTypeID(ref int TestAppointmentID, int TestTypeID
            , int LDLAppID , ref DateTime AppointmentDate, ref decimal PaidFees
            , ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"  SELECT TOP 1 * 
                                              FROM TestAppointments
                                            WHERE 
                                                LocalDrivingLicenseApplicationID = @LDLAppID 
                                                AND TestTypeID = @TestTypeID
                                            ORDER BY TestAppointmentID DESC;
                                            ";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //The record was found
                    isFound = true;

                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsLocked = (bool)reader["IsLocked"];
                    if (reader["RetakeTestID"] != DBNull.Value)
                    {
                        RetakeTestID = (int)reader["RetakeTestID"];
                    }
                    else
                    {
                        RetakeTestID = -1;
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
        public static bool GetTestAppointmentByIDView(int TestAppointmentID, ref int LDLAppID,ref string TestTypeTitle
             ,ref string ClassName,ref DateTime AppointmentDate,ref decimal PaidFees,ref string FullName,ref bool IsLocked)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Select * from TestAppointments_View where TestAppointmentID =@TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    //The record was found
                    isFound = true;

                    LDLAppID = (int)reader["LocalDrivingLicenseApplicationID"];
                    TestTypeTitle = (string)reader["TestTypeTitle"];
                    ClassName = (string)reader["ClassName"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    FullName = (string)reader["FullName"];
                    IsLocked = (bool)reader["IsLocked"];
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
        public static int AddNewTestAppointment( int TestTypeID,  int LDLAppID
            , DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked
            , int RetakeTestID)
        {
            //this function will return the new contact id if succeeded and -1 if not.
            int TestAppointmentID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO TestAppointments
                             (TestTypeID,LocalDrivingLicenseApplicationID,AppointmentDate,PaidFees,CreatedByUserID,IsLocked,RetakeTestID)
                             VALUES (@TestTypeID,@LDLAppID,@AppointmentDate,@PaidFees,@CreatedByUserID,@IsLocked,@RetakeTestID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsLocked", IsLocked);
          
            if (RetakeTestID != -1)
            {
                command.Parameters.AddWithValue("@RetakeTestID", RetakeTestID);
            }
            else
            {
                command.Parameters.AddWithValue("@RetakeTestID", System.DBNull.Value);

            }

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestAppointmentID = insertedID;
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
            return TestAppointmentID;
        }

        public static bool UpdateTestAppointment(int TestAppointmentID, int TestTypeID, int LDLAppID
            , DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked
            , int RetakeTestID)
        {
            int rowAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"update TestAppointments
                           set TestTypeID =@TestTypeID,
                           LocalDrivingLicenseApplicationID = @LDLAppID,
                           AppointmentDate = @AppointmentDate,                              
                           PaidFees = @PaidFees,
                           CreatedByUserID = @CreatedByUserID,
                           IsLocked = @IsLocked,
                           RetakeTestID = @RetakeTestID
  
                           where TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsLocked", IsLocked);
            if (RetakeTestID != -1)
            {
                command.Parameters.AddWithValue("@RetakeTestID", RetakeTestID);
            }
            else
            {
                command.Parameters.AddWithValue("@RetakeTestID", System.DBNull.Value);

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

        public static bool DeleteTestAppointment(int TestAppointmentID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            //will retrieve single value
            string query = @"Delete from TestAppointments
                                       where TestAppointmentID = @TestAppointmentID";

            SqlCommand Command = new SqlCommand(query, connection);

            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);


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

        public static DataTable GetAllTestAppointments(string FilterBy = "", string SearchText = "")
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);




            string query = @"select * from TestAppointments;";



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

            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static DataTable GetAllTestAppointmentsByDLAppAndTestType(int LDLAppID,int TestTypeID,bool CountIsLocked)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);




            string query = @"select * from TestAppointments where LocalDrivingLicenseApplicationID = @LDLAppID
                             and TestTypeID = @TestTypeID ;";


            if(CountIsLocked == true)
            {
                query = @"select * from TestAppointments where LocalDrivingLicenseApplicationID = @LDLAppID
                             and TestTypeID = @TestTypeID and IsLocked = 1 ;";
            }
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

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

        public static bool isTestAppointmentExistByID(int TestAppointmentID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query
                = "select Found=1 from TestAppointments where TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

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
