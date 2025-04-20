using BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3_DVLD_Project
{
    public partial class frmAddEditPersonInfo : Form
    {
        public delegate void DataBackEventHandler(object sender, int PersonID);

        //Declare an event using the delegate
        public event DataBackEventHandler DataBack;


        public enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode;

        int _PersonID;
        clsPerson _Person;


      
        public frmAddEditPersonInfo(int PersonID)
        {
            InitializeComponent();
         
            _PersonID= PersonID;    
            if (_PersonID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;

        }
        private void _LoadData()
        {


            if( _Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Person";
                lblPersonID.Text = "N/A";
                return;
            }

            _Person = clsPerson.Find(_PersonID);

          if( _Person == null )
            {
                MessageBox.Show("This form will be closed because No Person with ID = " + _PersonID);

                this.Close();

                return;
            }

            lblTitle.Text = "Update Person";
            lblPersonID.Text = _PersonID.ToString();

            //   if (_Mode == enMode.AddNew)
            // {

            //     _Person = new clsPerson();
            //return;
            // }

            //_Person = clsPerson.Find(_PersonID);

            //  if (_Person == null)
            //{
            //  MessageBox.Show("This form will be closed because No Contact with ID = " + _PersonID);
            // _Person = new clsPerson();

            //return;
            //}



            // lblMode.Text = "Edit Contact ID = " + _ContactID;
            //lblContactID.Text = _ContactID.ToString();


            //  if (_Person.ImagePath != "")
            //   {
            //     pictureBox1.Load(_Person.ImagePath);
            //  }


            //llRemoveImage.Visible = (_Person.ImagePath != "");

            //this will select the country in the combobox.
            //  cbCountry.SelectedIndex = cbCountry.FindString(clsCountry.Find(_Person.NationalityCountryID).CountryName);

        }
        private void fromAddEditPersonInfo_Load(object sender, EventArgs e)
        {
            ctrlPerson1.LoadPersonInfo(_PersonID);

           _LoadData();
        }

        private void _CheckOnPersonID(int PersonID)
        {
            if(PersonID==-1)
            {
               
                return;
            }
            else
            {
                lblTitle.Text = "Update Person";
                lblPersonID.Text = PersonID.ToString();
                DataBack?.Invoke(this,PersonID);
            }
        }

        private void ctrlPerson1_OnSaveComplete(int obj)
        {
            int PersonID = obj;
            _CheckOnPersonID(PersonID);

           
        }
        private void ctrlPerson1_onCloseClick(object sender)
        {
            this.Close();
        }

    
    }
}
