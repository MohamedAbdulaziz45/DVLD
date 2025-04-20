using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public class clsUser
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int UserID { set; get; }
        public int PersonID { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        
        public bool IsActive { set; get; }



        public clsUser()

        {
            this.UserID = -1;
            this.PersonID = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = false;
           

            Mode = enMode.AddNew;

        }

        private clsUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive= IsActive;

            Mode = enMode.Update;

        }

        private bool _AddNewUser()
        {
            //call DataBase Layer
            this.UserID = clsUserData.AddNewUser(this.PersonID, this.UserName,
             this.Password, this.IsActive);
            return (this.UserID != -1);
        }

        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(this.UserID, this.PersonID, this.UserName,
             this.Password, this.IsActive);
        }

        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }
        public static clsUser Find(int UserID)
        {

            string UserName = "", Password = "";
            bool IsActive = false;
            int PersonID = -1;

            if (clsUserData.GetUserInfoByID(UserID, ref PersonID, ref UserName,
            ref Password, ref IsActive))
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }
            else
                return null;
        }
        
        public static clsUser Find(string UserName)
        {
            string Password = "";
            bool IsActive = false;
            int PersonID = -1;
            int UserID = -1;

            if (clsUserData.GetUserInfoByUsername(UserName, ref PersonID, ref UserID,
           ref Password, ref IsActive))
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }
            else
                return null;
        }

        public static DataTable GetAllUsers(string FilterBy = "", string searchText = "")
        {
            return clsUserData.GetAllUsers(FilterBy, searchText);

        }

        //public static DataTable GetAllContactsFilteredByContactID(string FilterBy = "", string SearchText = "")
        //{
        //   return clsContactDataAccess.GetContactsFilteredByContactID(FilterBy, SearchText);
        //}

        public static bool isUserExistByUserID(int UserID)
        {
            return clsUserData.isUserExistByUserID(UserID);
        }

        public static bool isUserExistByPersonID(int PersonID)
        {
            return clsUserData.isUserExistByPersonID(PersonID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateUser();

            }
            return false;
        }


    }
}

