using _3_DVLD_Project.Controls;
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
using static System.Net.Mime.MediaTypeNames;

namespace _3_DVLD_Project
{
    public partial class frmRenewLicenseApplication : Form
    {
        clsLicense _OldLicense;
        int _LicenseID;
        clsApplication _Application;
        int _NewLicenseID;
        public frmRenewLicenseApplication()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _ValidateAll()
        {

            if (_OldLicense == null)
            {
                MessageBox.Show("there no Lincese with LicenseID = " + _LicenseID
                    , "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _DisabledAllControls();
                return;
            }

            if(_OldLicense.IsActive==false)
            {
                MessageBox.Show("this License with licenseID=" + _LicenseID +
                  " Not Allowed because this License is not Active"
                 , "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                lklShowLicenseHistory.Enabled = true;
                lklShowInternationalLicenseInfo.Enabled = false;
                btnRenew.Enabled = false;
                return;
            }
            if (_OldLicense.ExpirationDate> DateTime.Now)
            {

                MessageBox.Show("Selected License Is not Expired ,It will be expired on:"
                    + _OldLicense.ExpirationDate.ToString("dd/mm/yyyy")
                   , "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lklShowLicenseHistory.Enabled = true;
                lklShowInternationalLicenseInfo.Enabled = false;
                btnRenew.Enabled = false;
                return;
            }
            lklShowLicenseHistory.Enabled = true;
            lklShowInternationalLicenseInfo.Enabled = true;
            btnRenew.Enabled = true;

        }
        private bool _SaveRenewLicenseApp()
        {
            clsDriver _Driver;
            _Driver = clsDriver.Find(_OldLicense.DriverID);

            _Application = new clsApplication();

            _Application.ApplicantPersonID = _Driver.PersonID;
            _Application.ApplicationDate = DateTime.Now;
            _Application.ApplicationTypeID = 2;
            _Application.ApplicationStatus = 3;
            _Application.LastStatusDate = DateTime.Now;
            _Application.PaidFees = clsApplicationTypes.Find(2).ApplicationFees;
            _Application.CreatedByUserID = GlobalClass.CurrentUser.UserID;

            if (_Application.Save())
            {
                return true;
               
            }
            else
            {
                return false;
            }
        }
        private void _SaveRenewLocalLicenses()
        {
            if (!_SaveRenewLicenseApp())
            {
                MessageBox.Show("Error: Data Not Saved");
                return;

            }

            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = _Application.ApplicationID;
            NewLicense.DriverID = _OldLicense.DriverID;
            NewLicense.LicenseClass = _OldLicense.LicenseClass; 
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate 
                = DateTime.Now.AddYears(clsLicenseClass.Find(_OldLicense.LicenseClass).DefaultValidityLength);
            NewLicense.Notes = txtNotes.Text;
            NewLicense.IssueReason = 2;
            NewLicense.IsActive = true;
            NewLicense.CreatedByUserID = GlobalClass.CurrentUser.UserID;
            NewLicense.PaidFees = clsApplicationTypes.Find(2).ApplicationFees;


            if (NewLicense.Save())
            {
                MessageBox.Show("Data Saves Successfully.");
                _NewLicenseID = NewLicense.LicenseID;
                _OldLicense.IsActive = false;   
                _OldLicense.Save();
                lklShowLicenseHistory.Enabled = true;
                lklShowInternationalLicenseInfo.Enabled = true;
                btnRenew.Enabled = false;

                lblRLApp.Text = _Application.ApplicationID.ToString();
                lblRenewedLincesID.Text = NewLicense.LicenseID.ToString();
            }
            else
            {
                MessageBox.Show("Error: Data Not Saved");
                clsApplication.DeleteApplication(_Application.ApplicationID);
            }
        }
        private void btnRenew_Click(object sender, EventArgs e)
        {
            _SaveRenewLocalLicenses();
        }

        private void ctrlDriverLicenseWithFilter1_OnSearchClick(int obj)
        {
            int LicenseID = obj;
            _LicenseID = LicenseID;
            _OldLicense = clsLicense.Find(LicenseID);
            _ValidateAll();
          
            if (_OldLicense != null)
            {
                _FillRemainingApplicationInfo();
            }
            else
            {
                lblLicenseFees.Text = "[???]";
                lblOldLicenseID.Text = "[???]";
                lblExpirationDate.Text = "[???]";
                lblTotalFees.Text = "[???]";
            }
        }
        private void _DisabledAllControls()
        {
            lklShowLicenseHistory.Enabled = false;
            lklShowInternationalLicenseInfo.Enabled = false;
            btnRenew.Enabled = false;
        }
        private void _FillDataIntoApplicationInfo()
        {
            lblAppDate.Text = DateTime.Now.ToString("dd/mm/yyyy");
            lblIssueDate.Text = DateTime.Now.ToString("dd/mm/yyyy");
            lblAppFees.Text = ((int)clsApplicationTypes.Find(2).ApplicationFees).ToString();
            lblCreatedBy.Text = GlobalClass.CurrentUser.UserName;

        }

        private void _FillRemainingApplicationInfo()
        {
            lblLicenseFees.Text = ((int)clsLicenseClass.Find(_OldLicense.LicenseClass).ClassFees).ToString();
            lblOldLicenseID.Text = _OldLicense.LicenseID.ToString();
            lblExpirationDate.Text = _OldLicense.ExpirationDate.ToString("dd/mm//yyyy");
            lblTotalFees.Text =
      ((int)clsLicenseClass.Find(_OldLicense.LicenseClass).ClassFees + clsApplicationTypes.Find(2).ApplicationFees).ToString();
        }
       
        private void frmRenewLicenseApplication_Load(object sender, EventArgs e)
        {
            _DisabledAllControls();
            _FillDataIntoApplicationInfo();
        }

        private void lklShowInternationalLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clsLicense NewLicense = clsLicense.Find(_NewLicenseID);
            frmLicenseInfo frm = new frmLicenseInfo(NewLicense);
            frm.ShowDialog();
        }

        private void lklShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory frm = new frmLicenseHistory(_OldLicense);
            frm.ShowDialog();
        }
    }
}
