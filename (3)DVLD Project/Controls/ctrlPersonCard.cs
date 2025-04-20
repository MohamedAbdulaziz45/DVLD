using BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3_DVLD_Project.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        //Declare a delegate 
        public delegate void DataBackEventHandler(object sender, int PersonID);

        //Declare an event using the delegate
        public event DataBackEventHandler DataBack;


  


        clsPerson _Person;

        int _PersonID =-1 ;

        public int ctrlPersonID
        {
           get { return _PersonID; }
        }

        public ctrlPersonCard ()
        {
            InitializeComponent();
        }

        public void LoadPersonInfo(int PersonID)
        {
           
            _Person = clsPerson.Find(PersonID);
            if( _Person != null )
            {
                _PersonID = _Person.ID;
            }
            _LoadData();
        }

        public void LoadPersonInfo(string NationalNo)
        {

            _Person = clsPerson.Find(NationalNo);
            if( _Person != null )
            {
                _PersonID = _Person.ID;
            }
           
            _LoadData();
        }

        private void _NullPersonReload()
        {
            lblFullName.Text = "[????]";
            lblPersonID.Text = "[????]";
            lblNationalNo.Text = "[????]";

            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblAddress.Text = "[????]";

            lblDateOfBirth.Text = "[????]";
            lblCountry.Text = "[????]";
            lblGendor.Text = "[????]";
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = Properties.Resources.person_man;
            lklEditPersonInfo.Enabled = false;
            _PersonID = -1;
        }
        private void _LoadData()
        {

            

            if (_Person == null)
           {
               _NullPersonReload();
                return;
            }


            lklEditPersonInfo.Enabled = true;

            lblFullName.Text =_Person.FirstName+" " + _Person.SecondName+" "+ _Person.ThirdName+" "+ _Person.LastName;
            lblPersonID.Text=_Person.ID.ToString();
            lblNationalNo.Text = _Person.NationalNo;
            
            lblEmail.Text = _Person.Email;
            lblPhone.Text = _Person.Phone;
            lblAddress.Text = _Person.Address;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToString();

            lblCountry.Text=clsCountry.Find(_Person.NationalityCountryID).CountryName.ToString();

            if (_Person.Gendor == 0)
                lblGendor.Text = "Male";
            else
                lblGendor.Text = "Female";

          if (_Person.ImagePath != "")
          {
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Load(_Person.ImagePath);
          }
          else
            {
                 pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                if (_Person.Gendor == 0)
                {
                    pictureBox1.Image = Properties.Resources.person_man;
                }
                else
                {
                    pictureBox1.Image = Properties.Resources.person_woman;
                }
            }

        }

        private void ctrlPerson_Load(object sender, EventArgs e)
        {
           if( _Person == null)
                {

                lklEditPersonInfo.Enabled = false;
            }
        }

        private void lklEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddEditPersonInfo frm = new frmAddEditPersonInfo(_Person.ID);
            frm.ShowDialog();
            _LoadData();
        }


    }
}
