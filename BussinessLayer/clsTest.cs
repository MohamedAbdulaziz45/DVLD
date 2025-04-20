using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsTest
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public int TestID { set; get; }
        public int TestAppointmentID { set; get; }
        public bool TestResult { set; get; }
        public string  Notes { set; get; }
        public int CreatedByUserID { set; get; }

        public clsTest()

        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Notes = "";
            this.CreatedByUserID = -1;



            Mode = enMode.AddNew;

        }

        private clsTest(int TestID, int TestAppointmentID, bool TestResult,string Notes,int CreatedByUserID)
        {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.TestResult = TestResult;
            this.Notes = Notes;
            this.CreatedByUserID= CreatedByUserID;

            Mode = enMode.Update;

        }

        private bool _AddNewTest()
        {
            //call DataBase Layer
            this.TestID = clsTestsData.AddNewTest(this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
            return (this.TestID != -1);
        }

        private bool _UpdateTest()
        {
            return clsTestsData.UpdateTest(this.TestID,this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
        }

        public static bool DeleteTest(int TestID)
        {
            return clsTestsData.DeleteTest(TestID);
        }
        public static clsTest Find(int TestID)
        {
            int TestAppointmentID = -1;
            bool TestResult = false;
            string Notes = "";
            int CreatedByUserID = -1;

            if (clsTestsData.GetTestByTestID(TestID, ref TestAppointmentID, ref TestResult,ref Notes,ref CreatedByUserID))
            {
                return new clsTest(TestID,TestAppointmentID,TestResult, Notes, CreatedByUserID);
            }
            else
                return null;
        }
        public static clsTest FindByTestAppointmentID(int TestAppointmentID)
        {

            int TestID = -1;
            bool TestResult = false;
            string Notes = "";
            int CreatedByUserID = -1;

            if (clsTestsData.GetTestByTestAppointmentID(ref TestID, TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUserID))
            {
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            }
            else
                return null;

        }
        public static DataTable GetAllTest(string FilterBy = "", string searchText = "")
        {
            return clsTestsData.GetAllTests(FilterBy, searchText);

        }


        public static bool isTestExistByTestID(int TestID)
        {
            return clsTestsData.isTestExistByTestID(TestID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTest())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTest();

            }
            return false;
        }


    }
}
