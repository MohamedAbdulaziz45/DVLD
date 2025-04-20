using _3_DVLD_Project.Properties;
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

namespace _3_DVLD_Project.Controls
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }

        int _LDLAppID;
        clsApplication _Application;
        clsLicense _License;
        clsPerson _Person;
        LocalDrivingLicenseApplication _LDLApp;


        public void LoadDriverLicenseInfoByLDLAppID(int  LDLAppID)
        {
            _LDLAppID = LDLAppID;

            _LDLApp = LocalDrivingLicenseApplication.Find(_LDLAppID);
           
            if (_LDLApp == null)
            {
                MessageBox.Show("Error Can't Find Data");
                _ResetDriverLicenseInfo();
                return;
            }
            _Application = clsApplication.Find(_LDLApp.ApplicationID);
            _License = clsLicense.FindLicenseByApplicationID(_Application.ApplicationID);
            _Person = clsPerson.Find(_Application.ApplicantPersonID);
            _LoadData();
        }
        public void LoadDriverLicenseInfoByLicense(clsLicense License)
        {
            _License = License;

            if(License == null)
            {
                MessageBox.Show("Error Can't Find Data");
                _ResetDriverLicenseInfo();
                return;
            }

            _DefineVariables();
            _LoadData();
        }
        private void _DefineVariables() 
        {

            _Application = clsApplication.Find(_License.ApplicationID);
            _License = clsLicense.FindLicenseByApplicationID(_Application.ApplicationID);
            _Person = clsPerson.Find(_Application.ApplicantPersonID);

          
        }

        private string _IssueReasonCases()
        {
          if  (_License.IssueReason == 1)
            {
                return "First Time";
            }
          if( _License.IssueReason == 2)
            {
                return "Renew";
            }
          if( _License.IssueReason == 3)
            {
                return "Replacement fro Damaged";
            }
          if (_License.IssueReason == 4)
            {
                return "Replacement for lost.";
            }

          else
            {
                return "";
            }
        }

        private void _ResetDriverLicenseInfo()
        {
            string Empty = "[????]";
            lblClass.Text=Empty;
             lblName.Text=Empty;
            lblLicenseID.Text=Empty;
            lblNationalNo.Text=Empty;
            lblGendor.Text =Empty;
            lblIssueDate.Text=Empty;
            lblIssueReason.Text=Empty;
            lblNotes.Text=Empty;
            lblIsActive.Text=Empty;
            lblDateOfBirth.Text=Empty;
            lblDriverID.Text=Empty;
            lblExpirationDate.Text=Empty;
            lblIsDetained.Text=Empty;

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image= Properties.Resources.person_man;

        }
        private void _IsDetained()
        {
            clsDetainedLicense DetainedLicense;
            if (clsDetainedLicense.IsDetainedLicenseExistByLicenseID(_License.LicenseID))
            {
                DetainedLicense = clsDetainedLicense.FindByLicenseID(_License.LicenseID);
               
                if (DetainedLicense.IsReleased == false)
                   lblIsDetained.Text = "Yes";
                else
                   lblIsDetained.Text = "No";
            }
            else
            {
                lblIsDetained.Text = "No";
            }
        }
        private void _LoadData()
        {
          

            if (_License == null)
            {
                MessageBox.Show("Error Can't Find Data");
                _ResetDriverLicenseInfo();
                return;
            }
            lblClass.Text = clsLicenseClass.Find(_License.LicenseClass).ClassName;
            lblName.Text  = _Person.FirstName +" "+_Person.SecondName+" "+_Person.ThirdName+" "+_Person.LastName;
            lblLicenseID.Text = _License.LicenseID.ToString();
            lblNationalNo.Text = _Person.NationalNo;

            if (_Person.Gendor == 0)
                lblGendor.Text = "Male";
            else
                lblGendor.Text = "Female";

            lblIssueDate.Text = _License.IssueDate.ToString("dd/MM/yyyy");
            lblIssueReason.Text = _IssueReasonCases();

            if (_License.Notes == "")
                lblNotes.Text = "No Notes";
            else
                lblNotes.Text = _License.Notes;

            if (_License.IsActive == true)
                lblIsActive.Text = "Yes";
            else
                lblIsActive.Text = "No";

            lblDateOfBirth.Text = _Person.DateOfBirth.ToString("dd/MM/yyyy");
            lblDriverID.Text = _License.DriverID.ToString();
            lblExpirationDate.Text = _License.ExpirationDate.ToString("dd/MM/yyyy");

            _IsDetained();
            if (_Person.ImagePath != "")
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Load(_Person.ImagePath);
            }

        }


    }
}
