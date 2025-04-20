using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsTestTypes
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int TestTypeID { set; get; }
        public decimal TestTypeFees { set; get; }
        public string TestTypeTitle { set; get; }

        public string TestTypeDescription { set; get; }


    public clsTestTypes()

        {
            this.TestTypeID = -1;
            this.TestTypeFees = -1;
            this.TestTypeTitle = "";
            this.TestTypeDescription = "";

            Mode = enMode.AddNew;

        }

        private clsTestTypes(int TestTypeID, string TestTypeTitle,string TestTypeDescription, decimal TestTypeFees)
        {
            this.TestTypeID = TestTypeID;
            this.TestTypeFees = TestTypeFees;
            this.TestTypeTitle = TestTypeTitle;
            this.TestTypeDescription = TestTypeDescription;

            Mode = enMode.Update;

        }

        private bool _UpdateTestType()
        {
            return clsTestTypesData.UpdateTestType(this.TestTypeID, this.TestTypeTitle, this.TestTypeDescription, this.TestTypeFees);
        }

        public static clsTestTypes Find(int TestTypeID)
        {
            string TestTypeDescription = "";
            string TestTypeTitle = "";
            decimal TestTypeFees = -1;

            if (clsTestTypesData.GetTestTypeByID(TestTypeID, ref TestTypeTitle,ref TestTypeDescription, ref TestTypeFees))
            {
                return new clsTestTypes(TestTypeID, TestTypeTitle, TestTypeDescription, TestTypeFees);
            }
            else
                return null;
        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypesData.GetAllTestTypes();

        }

        public bool Save()
        {
            return _UpdateTestType();

        }
    }
}
