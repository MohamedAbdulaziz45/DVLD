using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsApplicationTypes
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ApplicationTypeID { set; get; }
        public decimal ApplicationFees { set; get; }
        public string ApplicationTypeTitle { set; get; }




        public clsApplicationTypes()

        {
            this.ApplicationTypeID = -1;
            this.ApplicationFees = -1;
            this.ApplicationTypeTitle = "";

            Mode = enMode.AddNew;

        }

        private clsApplicationTypes(int ApplicationTypeID, decimal ApplicationFees, string ApplicationTypeTitle)
        {
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationFees = ApplicationFees;
            this.ApplicationTypeTitle = ApplicationTypeTitle;


            Mode = enMode.Update;

        }

        private bool _UpdateApplicationType()
        {
            return clsApplicationTypesData.UpdateApplicationType(this.ApplicationTypeID,this.ApplicationTypeTitle,this.ApplicationFees);
        }

        public static clsApplicationTypes Find(int ApplicationTypeID)
        {

            string ApplicationTypeTitle = "";
            decimal ApplicationFees = -1;

            if (clsApplicationTypesData.GetApplicationTypeByID(ApplicationTypeID, ref ApplicationTypeTitle, ref ApplicationFees))
            {
                return new clsApplicationTypes(ApplicationTypeID, ApplicationFees, ApplicationTypeTitle);
            }
            else
                return null;
        }

        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypesData.GetAllApplicationTypes();

        }

        public bool Save()
        {
          return _UpdateApplicationType();

        }

    }
}
