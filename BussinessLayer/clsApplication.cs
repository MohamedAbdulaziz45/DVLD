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
    public class clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ApplicationID { set; get; }
        public int ApplicantPersonID { set; get; }
        public DateTime ApplicationDate { set; get; }   
        public int ApplicationTypeID { set; get; }  
        public byte ApplicationStatus { set; get; }
        public DateTime LastStatusDate { set; get; }    
        public decimal PaidFees { set; get; }
        public int CreatedByUserID { set; get; }





        public clsApplication()

        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.MinValue;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = 0;
            this.LastStatusDate = DateTime.MinValue;
            this.PaidFees =-1;
            this.CreatedByUserID= -1;

            Mode = enMode.AddNew;

        }

        private clsApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate,
            int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate, decimal PaidFees,
            int CreatedByUserID)
        {
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;

            Mode = enMode.Update;

        }

        private bool _AddNewApplication()
        {
            //call DataBase Layer
            this.ApplicationID = clsApplicationData.AddNewApplication(this.ApplicantPersonID,this.ApplicationDate
                ,this.ApplicationTypeID,this.ApplicationStatus,this.LastStatusDate,this.PaidFees,this.CreatedByUserID);
            return (this.ApplicationID != -1);
        }
        public int AddNewApplicationAndReturnID()
        {
            this.ApplicationID = clsApplicationData.AddNewApplication(this.ApplicantPersonID, this.ApplicationDate
               , this.ApplicationTypeID, this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
            return (this.ApplicationID);
        }


        private bool _UpdateApplication()
        {
            return clsApplicationData.UpdaateApplication(this.ApplicationID,this.ApplicantPersonID, this.ApplicationDate
                ,this.ApplicationTypeID, this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
        }

        public static bool DeleteApplication(int ApplicationID)
        {
            return clsApplicationData.DeleteApplication(ApplicationID);
        }
        public static clsApplication Find(int ApplicationID)
        {
            int ApplicantPersonID = -1, ApplicationTypeID=-1, CreatedByUserID=-1;
            byte ApplicationStatus = 0;
            decimal PaidFees = -1;
            DateTime ApplicationDate = DateTime.MinValue, LastStatusDate=DateTime.MinValue;


            if (clsApplicationData.GetApplicationByID( ApplicationID, ref ApplicantPersonID, ref ApplicationDate,
            ref ApplicationTypeID,ref ApplicationStatus, ref LastStatusDate, ref PaidFees,
            ref CreatedByUserID))
            {
                return new clsApplication(ApplicationID,  ApplicantPersonID, ApplicationDate, ApplicationTypeID
                    ,  ApplicationStatus,  LastStatusDate,  PaidFees,  CreatedByUserID);
            }
            else
                return null;
        }



        public static bool isApplicationExistByAppID(int ApplicationID)
        {
            return clsApplicationData.isApplicationExistByAppID(ApplicationID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplication())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateApplication();

            }
            return false;
        }


    }
}

