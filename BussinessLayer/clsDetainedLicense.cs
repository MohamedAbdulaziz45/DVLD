using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsDetainedLicense
    {
        private enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;


        public int DetainID { set; get; }
        public int LicenseID { set; get; }
        public DateTime DetainDate { set; get; }
        public decimal Finefees { set; get; }
        public int CreatedByUserID { set; get; }
        public bool IsReleased { set; get; }
        public DateTime ReleaseDate { set; get; }
        public int ReleasedByUserID { set; get; }
        public int ReleasedApplicationID { set; get; }


        public clsDetainedLicense()
        {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.DetainDate = DateTime.MinValue;
            this.Finefees = -1;
            this.CreatedByUserID = -1;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.MinValue;
            this.ReleasedByUserID = -1;
            this.ReleasedApplicationID = -1;

            Mode = enMode.AddNew;
        }

        private clsDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate
    , Decimal Finefees, int CreatedByUserID, bool IsReleased, DateTime ReleaseDate,
    int ReleasedByUserID, int ReleaseApplicationID)
        {
            this.DetainID = DetainID;
            this.LicenseID = LicenseID;
            this.DetainDate = DetainDate;
            this.Finefees = Finefees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsReleased = IsReleased;
            this.ReleaseDate = ReleaseDate;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleasedApplicationID = ReleaseApplicationID;

            Mode = enMode.Update;

        }

        private bool _AddNewDetainedLicense()
        {
            //call DataBase Layer
            this.DetainID =
                clsDetainedLicensesData.AddNewDetainedLicense(this.LicenseID, this.DetainDate
    , this.Finefees, this.CreatedByUserID, this.IsReleased, this.ReleaseDate,
     this.ReleasedByUserID, this.ReleasedApplicationID);
            return (this.DetainID != -1);
        }

        private bool _UpdateDetainedLicense()
        {
            return clsDetainedLicensesData.UpdateDetainedLicense(this.DetainID,this.LicenseID, this.DetainDate
    , this.Finefees, this.CreatedByUserID, this.IsReleased, this.ReleaseDate,
     this.ReleasedByUserID, this.ReleasedApplicationID);
        }

        public static bool DeleteDetainedLicense(int DetainID)
        {
            return clsDetainedLicensesData.DeleteDetainedLicense(DetainID);
        }
        public static clsDetainedLicense Find(int DetainID)
        {

            int LicenseID = -1;
            DateTime DetainDate = DateTime.MinValue;
            decimal Finefees = -1;
            int CreatedByUserID = -1;
            bool IsReleased = false;
            DateTime ReleaseDate = DateTime.MinValue;
            int ReleasedByUserID = -1;
            int ReleaseApplicationID = -1;

            if (clsDetainedLicensesData.GetDetainedLicenseByDetainID(DetainID, ref LicenseID, ref DetainDate
                , ref Finefees, ref CreatedByUserID, ref IsReleased, ref ReleaseDate, ref ReleasedByUserID,ref ReleaseApplicationID))
            {
                return new clsDetainedLicense(DetainID, LicenseID, DetainDate, Finefees, CreatedByUserID
          , IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID);
            }
            else
                return null;
        }

        public static clsDetainedLicense FindByLicenseID(int LicenseID)
        {

            int DetainID = -1;
            DateTime DetainDate = DateTime.MinValue;
            decimal Finefees = -1;
            int CreatedByUserID = -1;
            bool IsReleased = false;
            DateTime ReleaseDate = DateTime.MinValue;
            int ReleasedByUserID = -1;
            int ReleaseApplicationID = -1;

            if (clsDetainedLicensesData.GetDetainedLicenseByLicenseID(ref DetainID, LicenseID, ref DetainDate
                , ref Finefees, ref CreatedByUserID, ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseApplicationID))
            {
                return new clsDetainedLicense(DetainID, LicenseID, DetainDate, Finefees, CreatedByUserID
          , IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID);
            }
            else
                return null;
        }


        public static DataTable GetAllDetainedLicenses(string FilterBy = "", string searchText = "")
        {
            return clsDetainedLicensesData.GetAllDetainedLicenses(FilterBy, searchText);

        }

    
        public static bool IsDetainedLicenseExistByDetainID(int DetainID)
        {
            return clsDetainedLicensesData.IsDetainedLicenseExistByDetainID(DetainID);
        }

        public static bool IsDetainedLicenseExistByLicenseID(int LicenseID)
        {
            return clsDetainedLicensesData.IsDetainedLicenseExistByLicenseID(LicenseID);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDetainedLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDetainedLicense();

            }
            return false;
        }
    }
}
