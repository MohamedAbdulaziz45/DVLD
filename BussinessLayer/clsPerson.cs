using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BussinessLayer
{
    public class clsPerson
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ID { set; get; }
        public string NationalNo { set; get; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string ThirdName { set; get; }
        public string LastName { set; get; }
        public DateTime DateOfBirth { set; get; }
        public byte Gendor {  set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public int NationalityCountryID { set; get; }
        public string ImagePath { set; get; }



        public clsPerson()

        {
            this.ID = -1;
            this.NationalNo = "";
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            this.Gendor = 100;
            this.Address = "";
            this.Email = "";
            this.Phone = "";
            this.NationalityCountryID = -1;
            this.ImagePath = "";

            Mode = enMode.AddNew;

        }

        private clsPerson(int ID,string NationalNo, string FirstName,
             string SecondName, string ThirdName, string LastName, DateTime DateOfBirth,
             byte Gendor, string Address, string Phone, string Email,
             int NationalityCountryID, string ImagePath)
        {
            this.ID = ID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.Gendor = Gendor;
            this.Email = Email;
            this.Phone = Phone;
            this.Address = Address;
            this.DateOfBirth = DateOfBirth;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;

            Mode = enMode.Update;

        }

        private bool _AddNewPerson()
        {
            //call DataBase Layer
            this.ID = clsPersonData.AddNewPerson(this.NationalNo, this.FirstName,
             this.SecondName,this.ThirdName,this.LastName, this.DateOfBirth,
              this.Gendor, this.Address, this.Phone, this.Email,
             this.NationalityCountryID,this.ImagePath);
            return (this.ID != -1);
        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(this.ID,this.NationalNo, this.FirstName,
             this.SecondName, this.ThirdName, this.LastName, this.DateOfBirth,
              this.Gendor, this.Address, this.Phone, this.Email,
             this.NationalityCountryID, this.ImagePath);
        }

        public static bool DeletePerson(int ID)
        {
            return clsPersonData.DeletePerson(ID);
        }
        public static clsPerson Find(int ID)
        {

            string NationalNo="", FirstName = "",SecondName="",ThirdName="", LastName = "", Email = ""
                  , Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            byte Gendor = 100;
            int NationalityCountryID = -1;

            if (clsPersonData.GetPersonInfoByID( ID, ref NationalNo, ref FirstName,
            ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth,
            ref Gendor, ref Address, ref Phone, ref Email,
            ref NationalityCountryID, ref ImagePath))
            {
                return new clsPerson(ID, NationalNo, FirstName,
            SecondName,ThirdName, LastName, DateOfBirth,
             Gendor,  Address,  Phone, Email,
             NationalityCountryID, ImagePath);
            }
            else
                return null;
        }
        public static clsPerson Find(string NationalNo)
        {

               string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Email = ""
                  , Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            byte Gendor = 100;
            int NationalityCountryID = -1;
            int PersonID = -1;

            if (clsPersonData.GetPersonInfoByNationalNo(NationalNo, ref PersonID, ref FirstName,
            ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth,
            ref Gendor, ref Address, ref Phone, ref Email,
            ref NationalityCountryID, ref ImagePath))
            {
                return new clsPerson(PersonID, NationalNo, FirstName,
            SecondName, ThirdName, LastName, DateOfBirth,
             Gendor, Address, Phone, Email,
             NationalityCountryID, ImagePath);
            }
            else
                return null;
        }

        public static DataTable GetAllPeople(string FilterBy ="",string searchText ="")
        {
            return clsPersonData.GetAllPeople(FilterBy,searchText);

        }

        //public static DataTable GetAllContactsFilteredByContactID(string FilterBy = "", string SearchText = "")
        //{
        //   return clsContactDataAccess.GetContactsFilteredByContactID(FilterBy, SearchText);
        //}

        public static bool isPersonExist(int ID)
        {
            return clsPersonData.isPersonExist(ID);
        }

        public static bool isPersonExist(string NationalNo)
        {
            return clsPersonData.isPersonExist(NationalNo);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdatePerson();

            }
            return false;
        }


    }
}
