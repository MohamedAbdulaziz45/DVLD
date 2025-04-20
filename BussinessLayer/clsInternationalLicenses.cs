using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsInternationalLicenses
    {
        private enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;


        public int InternationalLicenseID { set; get; }
        public int ApplicationID { set; get; }
        public int DriverID { set; get; }
        public int IssuedUsingLocalLicenseID { set; get; }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public bool IsActive { set; get; }
        public int CreatedByUserID { set; get; }



        public clsInternationalLicenses()
        {
            this.InternationalLicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.MinValue;
            this.ExpirationDate = DateTime.MinValue;
            this.IsActive = false;
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;
        }

        private clsInternationalLicenses(int InternationalLicenseID, int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate
          , DateTime ExpirationDate, bool IsActive , int CreatedByUserID)
        {
            this.InternationalLicenseID = InternationalLicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;
            this.CreatedByUserID = CreatedByUserID;

            Mode = enMode.Update;

        }

        private bool _AddNewInternationalLicense()
        {
            //call DataBase Layer
            this.InternationalLicenseID =
                clsInternationalLicenseData.AddNewInternationalLicense(this.ApplicationID, this.DriverID, this.IssuedUsingLocalLicenseID, this.IssueDate
          , this.ExpirationDate, this.IsActive, this.CreatedByUserID);
            return (this.InternationalLicenseID != -1);
        }

        private bool _UpdateInternationalLicense()
        {
            return clsInternationalLicenseData.UpdateInternationalLicense(this.InternationalLicenseID, this.ApplicationID, this.DriverID, this.IssuedUsingLocalLicenseID, this.IssueDate
          , this.ExpirationDate, this.IsActive, this.CreatedByUserID);
        }

        public static bool DeleteInternationalLicense(int InternationalLicenseID)
        {
            return clsInternationalLicenseData.DeleteInternationalLicense(InternationalLicenseID);
        }
        public static clsInternationalLicenses Find(int InternationalLicense)
        {


            int ApplicationID = -1;
            int DriverID = -1;
            int IssuedUsingLocalLicenseID = -1;
            DateTime IssueDate = DateTime.MinValue;
            DateTime ExpirationDate = DateTime.MinValue;
            bool IsActive = false;
            int CreatedByUserID = -1;

            if (clsInternationalLicenseData.GetInternationalLicenseByILicenseID(InternationalLicense, ref ApplicationID, ref DriverID
                , ref IssuedUsingLocalLicenseID, ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID))
            {
                return new clsInternationalLicenses(InternationalLicense, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate
          , ExpirationDate,IsActive,CreatedByUserID);
            }
            else
                return null;
        }

        public static clsInternationalLicenses FindInternationalLicenseByLicenseID( int IssuedUsingLocalLicenseID)
        {


            int ApplicationID = -1;
            int DriverID = -1;
            int InternationalLicense = -1;
            DateTime IssueDate = DateTime.MinValue;
            DateTime ExpirationDate = DateTime.MinValue;
            bool IsActive = false;
            int CreatedByUserID = -1;

            if (clsInternationalLicenseData.GetInternationalLicenseByLicenseID(ref InternationalLicense, ref ApplicationID, ref DriverID
              ,  IssuedUsingLocalLicenseID, ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID))
            {
                return new clsInternationalLicenses(InternationalLicense, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate
          , ExpirationDate, IsActive, CreatedByUserID);
            }
            else
                return null;
        }
        public static DataTable GetAllInternationalLicenses(string FilterBy = "", string searchText = "")
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses(FilterBy, searchText);

        }

        public static DataTable GetAllInternationalLicensesByTheSameDriver(int DriverID)
        {
            return clsInternationalLicenseData.GetAllInternationalLicensesByTheSameDriver(DriverID);
        }

        public static bool IsInternationalLicenseExistByID(int ID)
        {
            return clsInternationalLicenseData.IsInternationalLicenseExistByID(ID);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewInternationalLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateInternationalLicense();

            }
            return false;
        }
    }
}
