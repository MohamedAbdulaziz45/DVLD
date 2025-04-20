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
    public partial class ctrlDriverInternationalLicenseInfo : UserControl
    {
        public ctrlDriverInternationalLicenseInfo()
        {
            InitializeComponent();
        }


        clsInternationalLicenses _InternationalLicense;
        clsLicense _LocalLicense;
        int _InternationalLicenseID;
        LocalDrivingLicenseApplication _LDLApp;
        LocalDrivingLicenseApplication.LDLAppView _LDLAppView;
        clsPerson _Person;


        public void LoadInternationalLicenseInfo(int InternationalLicenseID)
        {
            _InternationalLicenseID = InternationalLicenseID;
            _InternationalLicense = clsInternationalLicenses.Find(_InternationalLicenseID);

            if( _InternationalLicense == null ) 
            {
                MessageBox.Show("Error: Can't Find Data");
                return;
            }

            _LoadData();
        }

        private void _DefineVariables()
        {
            _LocalLicense = clsLicense.Find(_InternationalLicense.IssuedUsingLocalLicenseID);
            _LDLApp= LocalDrivingLicenseApplication.FindByApplicationID(_LocalLicense.ApplicationID);
            _LDLAppView = LocalDrivingLicenseApplication.LDLAppView.FindView(_LDLApp.LDLAppID);
            _Person = clsPerson.Find(_LDLAppView.VNationalNo);
        }

        private void _FillDataInformation()
        {
            lblName.Text = _LDLAppView.VFullName;
            lblInternationalLicenseID.Text = _InternationalLicenseID.ToString();
            lblLicenseID.Text = _InternationalLicense.IssuedUsingLocalLicenseID.ToString();
            lblNationalNo.Text = _LDLAppView.VNationalNo;

            if (_Person.Gendor == 0)
                lblGendor.Text = "Male";
            else
                lblGendor.Text = "Female";

            lblIssueDate.Text = _InternationalLicense.IssueDate.ToString("dd/mm/yyyy");
            lblAppID.Text = _InternationalLicense.ApplicationID.ToString();

            if (_InternationalLicense.IsActive == true)
                lblIsActive.Text = "Yes";
            else
                lblIsActive.Text = "No";

            lblDateOfBirth.Text = _Person.DateOfBirth.ToString("dd/mm/yyyy");
            lblDriverID.Text = _InternationalLicense.DriverID.ToString();
            lblExpirationDate.Text = _InternationalLicense.ExpirationDate.ToString("dd/mm/yyyy");


            if (_Person.ImagePath != "")
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Load(_Person.ImagePath);
            }

        }
        private void _LoadData()
        {
            _DefineVariables();

            _FillDataInformation();
        }
    }
}
