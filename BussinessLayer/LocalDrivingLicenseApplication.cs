using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BussinessLayer
{
    public class LocalDrivingLicenseApplication
    {

        public class LDLAppView
        {
            public enum enMode { AddNew = 0, Update = 1 };
            public enMode Mode = enMode.AddNew;
            public int VLDLAppID { set; get; }
            public string VClassName { set; get; }
            public string VNationalNo { set; get; }
            public string VFullName { set; get; }
            public DateTime VApplicationDate { set; get; }
            public int VPassedTestCount { set; get; }
            public string VStatus { set; get; }

            public LDLAppView()
            {
                this.VClassName = "";
                this.VNationalNo = "";
                this.VFullName = "";
                this.VApplicationDate = DateTime.MinValue;
                this.VPassedTestCount = -1;
                this.VStatus = "";

                Mode = enMode.AddNew;
            }

            private LDLAppView(int LDLAppID, string ClassName, string NationalNo
          , string FullName, DateTime ApplicationDate, int PassedTestCount, string Status)
            {
                this.VLDLAppID = LDLAppID;
                this.VClassName = ClassName;
                this.VNationalNo = NationalNo;
                this.VFullName = FullName;
                this.VApplicationDate = ApplicationDate;
                this.VPassedTestCount = PassedTestCount;
                this.VStatus = Status;
                Mode = enMode.Update;
            }

            public static LDLAppView FindView(int LDLAppID)
            {

                string ClassName = "";
                string NationalNo = "";
                string FullName = "";
                DateTime ApplicationDate = DateTime.MinValue;
                int PassedTestCount = -1;
                string Status = "";

                if (LocalDrivingLicenseApplicationData.GetLDLAppByIDView(LDLAppID, ref ClassName, ref NationalNo
                , ref FullName, ref ApplicationDate, ref PassedTestCount, ref Status))
                {
                    return new LDLAppView(LDLAppID, ClassName, NationalNo, FullName, ApplicationDate
                       , PassedTestCount, Status);
                }
                else
                    return null;
            }

        }

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
       
      
        public int LDLAppID { set; get; }
        public int ApplicationID { set; get; }
        public int LicenseClassID { set; get; }

     
        public LocalDrivingLicenseApplication()

        {
            this.LDLAppID = -1;
            this.ApplicationID = -1;
            this.LicenseClassID = -1;


            Mode = enMode.AddNew;

        }

        private LocalDrivingLicenseApplication(int LDLAppID, int ApplicationID, int LicenseClassID)
        {
            this.LDLAppID = LDLAppID;
            this.ApplicationID = ApplicationID;
            this.LicenseClassID = LicenseClassID;

            Mode = enMode.Update;

        }


        private bool _AddNewLDLApp()
        {
            //call DataBase Layer
            this.LDLAppID = LocalDrivingLicenseApplicationData.AddNewLDLApp(this.ApplicationID, this.LicenseClassID);
            return (this.LDLAppID != -1);
        }

        private bool _UpdateLDLApp()
        {
            return LocalDrivingLicenseApplicationData.UpdateLDLApp(this.LDLAppID, this.ApplicationID, this.LicenseClassID);
        }

        public static bool DeleteLDLApp(int LDLAppID)
        {
            return LocalDrivingLicenseApplicationData.DeleteLDLApp(LDLAppID);
        }
        public static LocalDrivingLicenseApplication Find(int LDLAppID)
        {

            int LicenseClassID = -1;
            int ApplicationID = -1;

            if (LocalDrivingLicenseApplicationData.GetLDLAppByID(LDLAppID, ref ApplicationID, ref LicenseClassID))
            {
                return new LocalDrivingLicenseApplication(LDLAppID, ApplicationID, LicenseClassID);
            }
            else
                return null;
        }

        public static LocalDrivingLicenseApplication FindByApplicationID(int ApplicationID )
        {
            int LicenseClassID = -1;
            int LDLAppID = -1;

            if (LocalDrivingLicenseApplicationData.GetLDLAppByApplicationID(ref LDLAppID, ApplicationID, ref LicenseClassID))
            {
                return new LocalDrivingLicenseApplication(LDLAppID, ApplicationID, LicenseClassID);
            }
            else
                return null;
        }


        public static DataTable GetAllLDLApp(string FilterBy = "", string searchText = "")
        {
            return LocalDrivingLicenseApplicationData.GetAllLDLApp(FilterBy, searchText);

        }


        public static bool isLDLAppExistByID(int LDLAppID)
        {
            return LocalDrivingLicenseApplicationData.isLDLAppExistByID(LDLAppID);
        }

        public static bool isLDLAppExistByPersonIDandLicenseClassID(int PersonID,int LicenseClassID,bool Cancelled =false,bool Completed=false,bool New =false)
        {
            return LocalDrivingLicenseApplicationData.isLDLAppExistByPersonIDandLicenseClassID(PersonID,LicenseClassID,Cancelled,Completed,New);
        }

        public static int GetLDLAppIDByPersonIDandClassName(int PersonID, int LicenseClassID, bool Cancelled = false)
        {
            return LocalDrivingLicenseApplicationData.GetLDLAppIDByPersonIDandClassName(PersonID, LicenseClassID, Cancelled);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLDLApp())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateLDLApp();

            }
            return false;
        }

    }
}
