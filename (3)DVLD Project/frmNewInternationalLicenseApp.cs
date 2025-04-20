using BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3_DVLD_Project
{
    public partial class frmNewInternationalLicenseApp : Form
    {
        clsLicense _LocalLicense;
        clsInternationalLicenses _InternationalLicenses;
        clsApplication _Application;
        clsDriver _Driver;

        int _InternationalLicenseID;
        public frmNewInternationalLicenseApp()
        {
            InitializeComponent();
        }
        int _LicenseID;
        private void _DisabledAllControls()
        {
            lklShowLicenseHistory.Enabled = false;
            lklShowInternationalLicenseInfo.Enabled = false;
            btnIssue.Enabled = false;
        }

        private void _FillDataIntoApplicationInfo()
        {
            lblAppDate.Text = DateTime.Now.ToString("dd/mm/yyyy");
            lblIssueDate.Text = DateTime.Now.ToString("dd/mm/yyyy");
            lblFees.Text = ((int)clsApplicationTypes.Find(6).ApplicationFees).ToString();
            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToString("dd/mm/yyyy");
            lblCreatedBy.Text = GlobalClass.CurrentUser.UserName;

        }
        private void frmNewInternationalLicenseApp_Load(object sender, EventArgs e)
        {
            _DisabledAllControls();
            _FillDataIntoApplicationInfo();
        }

        private void _ValidateAll()
        {

            if (_LocalLicense == null)
            {
                MessageBox.Show("there no Lincese with LicenseID = " + _LicenseID
                    , "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _DisabledAllControls();
                return;
            }

            clsInternationalLicenses internationalLicense
                = clsInternationalLicenses.FindInternationalLicenseByLicenseID(_LocalLicense.LicenseID);

            if (internationalLicense != null)
            {
                MessageBox.Show("there is already International License with International LicenseID = "
                    + internationalLicense.InternationalLicenseID, "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                _InternationalLicenseID = internationalLicense.InternationalLicenseID;
                lklShowLicenseHistory.Enabled = true;
                lklShowInternationalLicenseInfo.Enabled = true;
                btnIssue.Enabled = false;
                return;
            }

            if (_LocalLicense.LicenseClass != 3)
            {

                MessageBox.Show("this License with licenseID=" + _LicenseID +
                    " Not Allowed because this License Calss Not Third Class"
                   , "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lklShowLicenseHistory.Enabled = true;
                lklShowInternationalLicenseInfo.Enabled = false;
                btnIssue.Enabled = false;
                return;
            }

            if (_LocalLicense.IsActive == false)
            {
                MessageBox.Show("this License with licenseID=" + _LicenseID +
                   " Not Allowed because this License is not Active"
                  , "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
             
                lklShowLicenseHistory.Enabled = true;
                lklShowInternationalLicenseInfo.Enabled = false;
                btnIssue.Enabled = false;
                return;
            }

            if (_LocalLicense.ExpirationDate < DateTime.Now)
            {
                MessageBox.Show("this License with licenseID=" + _LicenseID +
                   " Not Allowed because this License is Expired"
                  , "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                lklShowLicenseHistory.Enabled = true;
                lklShowInternationalLicenseInfo.Enabled = false;
                btnIssue.Enabled = false;
                return;
            }

            lklShowLicenseHistory.Enabled = true;
            lklShowInternationalLicenseInfo.Enabled = false;
            btnIssue.Enabled = true;

        }

        private void ctrlDriverLicenseWithFilter1_OnSearchClick(int obj)
        {
            int LicenseID=obj;
            _LicenseID= LicenseID;
            _LocalLicense = clsLicense.Find(LicenseID);
            _ValidateAll();
            if(_LocalLicense != null)
            {
                lblLocalLicensesID.Text = _LocalLicense.LicenseID.ToString();
            }
            else
            {
                lblLocalLicensesID.Text= "[???]";
            }
           

        }
        private bool _SaveNewInternationalLicenseApp()
        {
            _Driver = clsDriver.Find(_LocalLicense.DriverID);
            _Application= new clsApplication();
            
            _Application.ApplicantPersonID = _Driver.PersonID;
            _Application.ApplicationDate = DateTime.Now;
            _Application.ApplicationTypeID = 6;
            _Application.ApplicationStatus = 3;
            _Application.LastStatusDate = DateTime.Now;
            _Application.PaidFees = clsApplicationTypes.Find(6).ApplicationFees;
            _Application.CreatedByUserID = GlobalClass.CurrentUser.UserID;

            if(_Application.Save())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // application is not _License.ApplicationID.
        private void _SaveInternationalLicenses()
        {
            if(!_SaveNewInternationalLicenseApp())
            {
                MessageBox.Show("Error: Data Not Saved");
                return;

            }

            _InternationalLicenses = new clsInternationalLicenses();

            _InternationalLicenses.ApplicationID =_Application.ApplicationID;
            _InternationalLicenses.DriverID = _LocalLicense.DriverID;
            _InternationalLicenses.IssuedUsingLocalLicenseID = _LocalLicense.LicenseID;
            _InternationalLicenses.IssueDate = DateTime.Now;
            _InternationalLicenses.ExpirationDate = DateTime.Now.AddYears(1);
            _InternationalLicenses.IsActive = true;
            _InternationalLicenses.CreatedByUserID = GlobalClass.CurrentUser.UserID;


            if (_InternationalLicenses.Save())
            {
                MessageBox.Show("Data Saves Successfully.");
                _InternationalLicenseID = _InternationalLicenses.InternationalLicenseID;
                lklShowInternationalLicenseInfo.Enabled = true;
            }
            else
            {
                MessageBox.Show("Error: Data Not Saved");
                clsApplication.DeleteApplication(_Application.ApplicationID);
            }
        }
        private void btnIssue_Click(object sender, EventArgs e)
        {
            _SaveInternationalLicenses();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lklShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LocalDrivingLicenseApplication LDLApp = LocalDrivingLicenseApplication.FindByApplicationID(_LocalLicense.ApplicationID);
            int LDLAppID = LDLApp.LDLAppID;
            frmLicenseHistory frm = new frmLicenseHistory(LDLAppID);
            frm.ShowDialog();
        }

        private void lklShowInternationalLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDriverInternationalLicenseInfo frm = new frmDriverInternationalLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();
        }
    }
}
