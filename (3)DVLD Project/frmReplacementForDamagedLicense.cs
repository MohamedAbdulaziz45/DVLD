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
    public partial class frmReplacementForDamagedLicense : Form
    {
        clsLicense _OldLicense;
        int _LicenseID;
        clsApplication _Application;
        int _NewLicenseID;
        public frmReplacementForDamagedLicense()
        {
            InitializeComponent();
        }

        private void _DisabledAllControls()
        {
            lklShowLicenseHistory.Enabled = false;
            lklShowNewLicenseInfo.Enabled = false;
            btnIssueReplacement.Enabled = false;
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

            if (_OldLicense.IsActive == false)
            {
                MessageBox.Show("this License with licenseID=" + _LicenseID +
                  " Not Allowed because this License is not Active"
                 , "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                lklShowLicenseHistory.Enabled = true;
                lklShowNewLicenseInfo.Enabled = false;
                btnIssueReplacement.Enabled = false;
                return;
            }
            if (_OldLicense.ExpirationDate < DateTime.Now)
            {

                MessageBox.Show("Selected License Is Expired ,It is expired on:"
                    + _OldLicense.ExpirationDate.ToString("dd/mm/yyyy")
                   , "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lklShowLicenseHistory.Enabled = true;
                lklShowNewLicenseInfo.Enabled = false;
                btnIssueReplacement.Enabled = false;
                return;
            }
            lklShowLicenseHistory.Enabled = true;
            lklShowNewLicenseInfo.Enabled = false;
            btnIssueReplacement.Enabled = true;

        }

        private bool _SaveReplaceLicenseApp()
        {
            clsDriver _Driver;
            _Driver = clsDriver.Find(_OldLicense.DriverID);

            _Application = new clsApplication();

            _Application.ApplicantPersonID = _Driver.PersonID;
            _Application.ApplicationDate = DateTime.Now;

            if (rbDamagedLicense.Checked)
            {
                _Application.ApplicationTypeID = 4;
                _Application.PaidFees = clsApplicationTypes.Find(4).ApplicationFees;
            }
            else
            {
                _Application.ApplicationTypeID = 3;
                _Application.PaidFees = clsApplicationTypes.Find(3).ApplicationFees;
            }


            _Application.ApplicationStatus = 3;
            _Application.LastStatusDate = DateTime.Now;

           
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
        private void _SaveNewLocalLicenses()
        {
            if (!_SaveReplaceLicenseApp())
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
            NewLicense.Notes = "";
            if (rbLostLicense.Checked)
            {
                NewLicense.PaidFees = clsApplicationTypes.Find(3).ApplicationFees;
                NewLicense.IssueReason = 4;
            }

            else {
                NewLicense.PaidFees = clsApplicationTypes.Find(4).ApplicationFees;
                NewLicense.IssueReason = 3;
            }

            NewLicense.IsActive = true;
            NewLicense.CreatedByUserID = GlobalClass.CurrentUser.UserID;


            if (NewLicense.Save())
            {
                MessageBox.Show("Data Saves Successfully.");
                _NewLicenseID = NewLicense.LicenseID;
                _OldLicense.IsActive = false;
                _OldLicense.Save();
               
                lklShowLicenseHistory.Enabled = true;
                lklShowNewLicenseInfo.Enabled = true;
                btnIssueReplacement.Enabled = false;
                ctrlDriverLicenseWithFilter1.DisableFilter(true);
                gbReplacementFor.Enabled = false;

                lblRLApp.Text = _Application.ApplicationID.ToString();
                lblRenewedLincesID.Text = NewLicense.LicenseID.ToString();
            }
            else
            {
                MessageBox.Show("Error: Data Not Saved");
                clsApplication.DeleteApplication(_Application.ApplicationID);
            }
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
                lblOldLicenseID.Text = "[???]";

            }
        }
        private void _FillDataIntoApplicationInfo()
        {
            lblAppDate.Text = DateTime.Now.ToString("dd/mm/yyyy");

            _UpdateAppFees();


            lblCreatedBy.Text = GlobalClass.CurrentUser.UserName;

        }

        private void _FillRemainingApplicationInfo()
        {
            lblOldLicenseID.Text = _OldLicense.LicenseID.ToString();
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            _SaveNewLocalLicenses();
        }

        private void frmReplacementForDamagedLicense_Load(object sender, EventArgs e)
        {
            _DisabledAllControls();
            _FillDataIntoApplicationInfo();
        }

        private void lklShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory frm = new frmLicenseHistory(_OldLicense);
            frm.ShowDialog();
        }

        private void lklShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clsLicense NewLicense = clsLicense.Find(_NewLicenseID);
            frmLicenseInfo frm = new frmLicenseInfo(NewLicense);
            frm.ShowDialog();
        }

        private void _UpdateAppFees()
        {
            if (rbDamagedLicense.Checked)
                lblAppFees.Text = ((int)clsApplicationTypes.Find(4).ApplicationFees).ToString();
            else
                lblAppFees.Text = ((int)clsApplicationTypes.Find(3).ApplicationFees).ToString();
        }
        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            _UpdateAppFees();
        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            _UpdateAppFees();
        }
    }
}
