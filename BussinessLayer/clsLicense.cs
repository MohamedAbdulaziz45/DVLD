using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsLicense
    {
        private enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;


        public int LicenseID { set; get; }
        public int ApplicationID { set; get; }
        public int DriverID { set; get; }
        public int LicenseClass { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public string Notes { set; get; }   
        public decimal PaidFees { set; get; }
        public bool IsActive { set; get; }
        public byte IssueReason { set; get; }
        public int CreatedByUserID { set; get; }


     
        public clsLicense()
        {
            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClass = -1;
            this.IssueDate = DateTime.MinValue;
            this.ExpirationDate = DateTime.MinValue;
            this.Notes = "";
            this.PaidFees = -1;
            this.IsActive = false;
            this.IssueReason = 0;
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;
        }

        private clsLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate
          , DateTime ExpirationDate, string Notes, decimal PaidFees, bool IsActive, byte IssueReason
            , int CreatedByUserID)
        {
            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.LicenseClass = LicenseClass;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.IsActive = IsActive;
            this.IssueReason = IssueReason;
            this.CreatedByUserID = CreatedByUserID;

            Mode = enMode.Update;

        }

        private bool _AddNewLicense()
        {
            //call DataBase Layer
            this.LicenseID =
                clsLicensesData.AddNewLicense (this.ApplicationID, this.DriverID, this.LicenseClass, this.IssueDate
          , this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, this.IssueReason
            , this.CreatedByUserID);
            return (this.LicenseID != -1);
        }

        private bool _UpdateLicense()
        {
            return clsLicensesData.UpdateLicense(this.LicenseID,this.ApplicationID, this.DriverID, this.LicenseClass, this.IssueDate
          , this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, this.IssueReason
            , this.CreatedByUserID);
        }

        public static bool DeleteLicense(int LicenseID)
        {
            return clsLicensesData.DeleteLicense(LicenseID);
        }
        public static clsLicense Find(int LicenseID)
        {


            int ApplicationID = -1;
            int DriverID = -1;
            int LicenseClass = -1;
            DateTime IssueDate = DateTime.MinValue;
            DateTime ExpirationDate = DateTime.MinValue;
            string Notes = "";
            decimal PaidFees = -1;
            bool IsActive = false;
            byte IssueReason = 0;
            int CreatedByUserID = -1;

            if (clsLicensesData.GetLicenseByLicenseID( LicenseID,ref ApplicationID,ref DriverID, ref LicenseClass, ref IssueDate
          , ref ExpirationDate, ref Notes,ref PaidFees,ref IsActive,ref IssueReason
            , ref CreatedByUserID))
            {
                return new clsLicense(LicenseID,ApplicationID, DriverID,LicenseClass, IssueDate
          , ExpirationDate, Notes, PaidFees, IsActive,IssueReason ,CreatedByUserID);
            }
            else
                return null;
        }

        public static clsLicense FindLicenseByApplicationID(int ApplicationID)
        {


            int LicenseID = -1;
            int DriverID = -1;
            int LicenseClass = -1;
            DateTime IssueDate = DateTime.MinValue;
            DateTime ExpirationDate = DateTime.MinValue;
            string Notes = "";
            decimal PaidFees = -1;
            bool IsActive = false;
            byte IssueReason = 0;
            int CreatedByUserID = -1;

            if (clsLicensesData.GetLicenseByApplicationID(ref LicenseID,  ApplicationID, ref DriverID, ref LicenseClass, ref IssueDate
          , ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive, ref IssueReason
            , ref CreatedByUserID))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClass, IssueDate
          , ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID);
            }
            else
                return null;
        }
        public static DataTable GetAllLicenses(string FilterBy = "", string searchText = "")
        {
            return clsLicensesData.GetAllLicenses(FilterBy, searchText);

        }
    
        public static DataTable GetAllLicensesByTheSameDriver(int DriverID)
        {
            return clsLicensesData.GetAllLicensesByTheSameDriver(DriverID);
        }

        public static bool IsLicenseExistByLicenseID(int LicenseID)
        {
            return clsLicensesData.IsLicenseExistByLicenseID(LicenseID);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateLicense();

            }
            return false;
        }
    }
}
