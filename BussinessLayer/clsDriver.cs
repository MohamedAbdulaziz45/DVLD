using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsDriver
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public int DriverID { set; get; }
        public int PersonID { set; get; }
        public int CreatedByUserID { set; get; }
        public DateTime CreatedDate { set; get; }   


        public clsDriver()

        {
            this.DriverID = -1;
            this.PersonID = -1;
            this.CreatedByUserID = -1;
            this.CreatedDate = DateTime.MinValue;

            Mode = enMode.AddNew;

        }

        private clsDriver(int DriverID, int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            this.DriverID = DriverID;
            this.PersonID = PersonID;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedDate = CreatedDate;

            Mode = enMode.Update;
        }

        private bool _AddNewDriver()
        {
            //call DataBase Layer
            this.DriverID = clsDriversData.AddNewDriver(this.PersonID, this.CreatedByUserID, this.CreatedDate);
            return (this.DriverID != -1);
        }

        private bool _UpdateDriver()
        {
            return clsDriversData.UpdateDriver(this.DriverID, this.PersonID, this.CreatedByUserID, this.CreatedDate);
        }

        public static bool DeleteDriverByPersonID(int PersonID)
        {
            return clsDriversData.DeleteDriverByPersonID(PersonID);
        }

        public static bool DeleteDriver(int DriverID)
        {
            return clsDriversData.DeleteDriver(DriverID);
        }
        public static clsDriver Find(int DriverID)
        {
            int PersonID = -1;
            int CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.MinValue;


            if (clsDriversData.GetDriverByDriverID(DriverID, ref PersonID, ref CreatedByUserID, ref CreatedDate))
            {
                return new clsDriver(DriverID,PersonID, CreatedByUserID, CreatedDate);
            }
            else
                return null;
        }
        public static clsDriver FindByPersonID( int PersonID)
        {

            int DriverID = -1;
            int CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.MinValue;

            if (clsDriversData.GetDriverByPersonID(ref DriverID, PersonID, ref CreatedByUserID, ref CreatedDate))
            {
                return new clsDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
            }
            else
                return null;

        }
        public static DataTable GetAllDrivers(string FilterBy = "", string searchText = "")
        {
            return clsDriversData.GetAllDrivers(FilterBy, searchText);

        }


        public static bool IsDriverExistByDriverID(int DriverID)
        {
            return clsDriversData.IsDriverExistByDriverID(DriverID);
        }

        public static bool IsDriverExistByPersonID(int PersonID)
        {
            return clsDriversData.IsDriverExistByPersonID(PersonID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDriver();

            }
            return false;
        }
    }
}
