using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsLicenseClass
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int LicenseClassID { set; get; }
        public decimal ClassFees { set; get; }
        public string ClassName { set; get; }
        public byte MinimumAllowedAge { set; get; }
        public byte DefaultValidityLength { set; get; }
        public string ClassDescription { set; get; }


        public clsLicenseClass()

        {
            this.LicenseClassID = -1;
            this.ClassFees = -1;
            this.MinimumAllowedAge = 0;
            this.DefaultValidityLength = 0;
            this.ClassName = "";
            this.ClassDescription = "";

            Mode = enMode.AddNew;

        }

        private clsLicenseClass(int LicenseClassID, string ClassName, string ClassDescription,byte MinimumAllowedAge,byte DefaultValidityLength, decimal ClassFees)
        {
            this.LicenseClassID = LicenseClassID;
            this.ClassFees = ClassFees;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.DefaultValidityLength = DefaultValidityLength;
            this.MinimumAllowedAge=MinimumAllowedAge;

            Mode = enMode.Update;

        }

        private bool _UpdateLicenseClass()
        {
            return clsLicenseClassData.UpdateLicenseClasses(this.LicenseClassID, this.ClassName, this.ClassDescription,this.MinimumAllowedAge,this.DefaultValidityLength, this.ClassFees);
        }

        public static clsLicenseClass Find(int LicenseClassID)
        {
            string ClassDescription = "";
            string ClassName = "";
            byte MinimumAllowedAge = 0;
            byte DefaultValidityLength = 0;
            decimal ClassFees = -1;

            if (clsLicenseClassData.GetLicenseClassByID(LicenseClassID, ref ClassName, ref ClassDescription,ref MinimumAllowedAge,ref DefaultValidityLength, ref ClassFees))
            {
                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription,MinimumAllowedAge,DefaultValidityLength, ClassFees);
            }
            else
                return null;
        }
        public static clsLicenseClass Find(string ClassName)
        {
            string ClassDescription = "";
            int LicenseClassID = -1;
            byte MinimumAllowedAge = 0;
            byte DefaultValidityLength = 0;
            decimal ClassFees = -1;

            if (clsLicenseClassData.GetLicenseClassByClassName( ClassName,ref LicenseClassID, ref ClassDescription, ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))
            {
                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees);
            }
            else
                return null;
        }

        public static DataTable GetAllLicenseClasses()
        {
            return clsLicenseClassData.GetAllLicenseClasses();

        }

        public bool Save()
        {
            return _UpdateLicenseClass();

        }
    }
}
