using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsTestAppointment
    {

        private enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;


        public int TestAppointmentID { set; get; }
        public int TestTypeID { set; get; }
        public int LDLAppID { set; get; }
        public DateTime AppointmentDate { set; get; }
        public decimal PaidFees { set; get; }
        public int CreatedByUserID { set; get; }
        public bool IsLocked { set; get; }
        public int RetakeTestID { set; get; }


        public class clsTestAppointmentView
        {
            private enum enMode { AddNew = 0, Update = 1 };
            private enMode Mode = enMode.AddNew;
            public int VTestAppointmentID { set; get; }
            public int VLDLAppID { set; get; }
            public string VTestTypeTitle { set; get; }
            public string VClassName {  set; get; }
            public DateTime VAppointmentDate { set; get; }
            public decimal VPaidFees { set; get; }
            public string VFullName { set; get; }
            public bool VIsLocked {  set; get; }

            public clsTestAppointmentView()
            {
                this.VTestAppointmentID = -1;
                this.VLDLAppID = -1;
                this.VTestTypeTitle = "";
                this.VClassName = "";
                this.VAppointmentDate = DateTime.MinValue;
                this.VPaidFees = -1;
                this.VFullName = "";
                this.VIsLocked = false;
              
                Mode = enMode.AddNew;
            }
            private clsTestAppointmentView(int TestAppointmentID, int LDLAppID, string TestTypeTitle,
          string ClassName, DateTime AppointmentDate, decimal PaidFees,string FullName, bool IsLocked)
            {
                this.VTestAppointmentID = TestAppointmentID;
                this.VLDLAppID = LDLAppID;
                this.VTestTypeTitle = TestTypeTitle;
                this.VClassName= ClassName;
                this.VAppointmentDate = AppointmentDate;
                this.VPaidFees = PaidFees;
                this.VFullName = FullName;
                this.VIsLocked = IsLocked;

                Mode = enMode.Update;

            }
            public static clsTestAppointmentView FindView(int TestAppointmentID)
            {



               int LDLAppID = -1;
               string TestTypeTitle = "";
               string ClassName = "";
               DateTime AppointmentDate = DateTime.MinValue;
               decimal PaidFees = -1;
               string FullName = "";
               bool IsLocked = false;

                if (clsTestAppointmentsData.GetTestAppointmentByIDView( TestAppointmentID, ref LDLAppID, ref TestTypeTitle,
          ref ClassName, ref AppointmentDate, ref PaidFees, ref FullName, ref IsLocked))
                {
                    return new clsTestAppointmentView( TestAppointmentID, LDLAppID, TestTypeTitle,
           ClassName,  AppointmentDate,  PaidFees,  FullName,  IsLocked);
                }
                else
                    return null;
            }


        }

        public clsTestAppointment()
        {
            this.TestAppointmentID = -1;
            this.TestTypeID = -1;
            this.LDLAppID = -1;
            this.AppointmentDate = DateTime.MinValue;
            this.PaidFees = -1;
            this.CreatedByUserID = -1;
            this.IsLocked = false;
            this.RetakeTestID = -1;

            Mode = enMode.AddNew;
        }

        private clsTestAppointment(int TestAppointmentID, int TestTypeID, int LDLAppID
            , DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked
            , int RetakeTestID)
        {
            this.TestAppointmentID =TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this.LDLAppID = LDLAppID;
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsLocked = IsLocked;
            this.RetakeTestID = RetakeTestID;

            Mode = enMode.Update;

        }

        private bool _AddNewTestAppointment()
        {
            //call DataBase Layer
            this.TestAppointmentID = 
                clsTestAppointmentsData.AddNewTestAppointment(this.TestTypeID, this.LDLAppID
            , this. AppointmentDate, this.PaidFees,this.CreatedByUserID, this.IsLocked, this. RetakeTestID);
            return (this.TestAppointmentID != -1);
        }

        private bool _UpdateTestAppointment()
        {
            return clsTestAppointmentsData.UpdateTestAppointment(this.TestAppointmentID, this.TestTypeID, this.LDLAppID
            , this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked , this.RetakeTestID);
        }

        public static bool DeleteTestAppointment(int TestAppointmentID)
        {
            return clsTestAppointmentsData.DeleteTestAppointment(TestAppointmentID);
        }
        public static clsTestAppointment Find(int TestAppointmentID)
        {


           int TestTypeID = -1;
           int LDLAppID = -1;
           DateTime  AppointmentDate = DateTime.MinValue;
           decimal PaidFees = -1;
           int CreatedByUserID = -1;
           bool IsLocked = false;
           int RetakeTestID = -1;

            if (clsTestAppointmentsData.GetTestAppointmentByID( TestAppointmentID, ref TestTypeID, ref LDLAppID
            , ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestID))
            {
                return new clsTestAppointment(TestAppointmentID, TestTypeID, LDLAppID
            , AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestID);
            }
            else
                return null;
        }

        public static clsTestAppointment FindLastByLDLAppIDAndTestTypeID(int LDLAppID, int TestTypeID)
        {
            int TestAppointmentID = -1;

            DateTime AppointmentDate = DateTime.MinValue;
            decimal PaidFees = -1;
            int CreatedByUserID = -1;
            bool IsLocked = false;
            int RetakeTestID = -1;

            if (clsTestAppointmentsData.GetLastTestAppointmentByDlAppIDAndTestTypeID(ref TestAppointmentID,  TestTypeID, LDLAppID
, ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestID))
            {
                return new clsTestAppointment(TestAppointmentID, TestTypeID, LDLAppID
            , AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestID);
            }
            else
                return null;
        }

        public static DataTable GetAllTestAppointment(string FilterBy = "", string searchText = "")
        {
            return clsTestAppointmentsData.GetAllTestAppointments(FilterBy, searchText);

        }
        public static DataTable GetAllTestAppointmentsByDLAppAndTestType(int LDLAppID , int TestTypeID,bool CountIsLocked =false )
        {
            return clsTestAppointmentsData.GetAllTestAppointmentsByDLAppAndTestType(LDLAppID, TestTypeID,  CountIsLocked);

        }

        public static bool isTestAppointmentExistByID(int TestAppointmentID)
        {
            return clsTestAppointmentsData.isTestAppointmentExistByID(TestAppointmentID);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestAppointment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTestAppointment();

            }
            return false;
        }
    }
}
