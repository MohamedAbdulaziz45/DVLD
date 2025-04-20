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
    public partial class frmReleaseDetainedLicense : Form
    {
        clsLicense _License;
        int _LicenseID;
        clsApplication _Application;
        clsDetainedLicense _DetainedLicense;
        public frmReleaseDetainedLicense()
        {
            InitializeComponent();
        }

        public frmReleaseDetainedLicense(int LicenseID)
        {

            InitializeComponent();
            _LicenseID = LicenseID;
            _License = clsLicense.Find(LicenseID);
            _ValidateAll();
            ctrlDriverLicenseWithFilter1.DisableFilter(true);
            ctrlDriverLicenseWithFilter1.LoadDriverLicenseInfoByLicense(LicenseID);
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _ResetDetainInfo()
        {

            lblDetainID.Text = "[???]";
            lblDetainDate.Text = "[??/??/????]";
            lblApplicationFees.Text = "[$$$]";
            lblTotalFees.Text = "[$$$]";

            if(_License == null)
            {
                lblLicenseID.Text = "[???]";
            }
            else
            {
                lblLicenseID.Text = _License.LicenseID.ToString();
            }
            lblCreatedBy.Text = "[???]";
            lblFineFees.Text = "[$$$]";
            lblApplicationID.Text = "[???]";

        }
        private void _DisabledAllControls()
        {
            lklShowLicenseHistory.Enabled = false;
            lklShowNewLicenseInfo.Enabled = false;
            btnRelease.Enabled = false;
        }
        private void _ValidateAll()
        {

            if (_License == null)
            {
                MessageBox.Show("there no Lincese with LicenseID = " + _LicenseID
                    , "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _DisabledAllControls();
                _ResetDetainInfo();
                return;
            }

            if (!clsDetainedLicense.IsDetainedLicenseExistByLicenseID(_LicenseID))
            {
                MessageBox.Show("this License is not detained"
                   , "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lklShowLicenseHistory.Enabled = true;
                lklShowNewLicenseInfo.Enabled = false;
                btnRelease.Enabled = false;
                _ResetDetainInfo();
                return;
            }

            _DetainedLicense = clsDetainedLicense.FindByLicenseID( _LicenseID );
          
            if(_DetainedLicense.IsReleased ==true)
            {
                MessageBox.Show("this License is not detained"
                   , "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lklShowLicenseHistory.Enabled = true;
                lklShowNewLicenseInfo.Enabled = false;
                btnRelease.Enabled = false;
                _ResetDetainInfo();
                return;
            }
            _FillRemainingApplicationInfo();
            lklShowLicenseHistory.Enabled = true;
            lklShowNewLicenseInfo.Enabled = false;
            btnRelease.Enabled = true;

        }
        private void _FillRemainingApplicationInfo()
        {
            lblLicenseID.Text = _License.LicenseID.ToString();
            if(_License!=null)
            {
                int ApplicationFees = (int)clsApplicationTypes.Find(5).ApplicationFees;
                int FineFees = (int)_DetainedLicense.Finefees;
                lblDetainID.Text = _DetainedLicense.DetainID.ToString();
                lblDetainDate.Text =_DetainedLicense.DetainDate.ToString("dd/mm/yyyy");
                lblApplicationFees.Text = ApplicationFees.ToString();    
                lblTotalFees.Text = (ApplicationFees+FineFees).ToString();
                lblCreatedBy.Text = GlobalClass.CurrentUser.UserName;
                lblFineFees.Text = FineFees.ToString();
            }
        }
        private void ctrlDriverLicenseWithFilter1_OnSearchClick(int obj)
        {

            int LicenseID = obj;
            _LicenseID = LicenseID;
            _License = clsLicense.Find(LicenseID);
            _ValidateAll();

        }
        private bool _SaveReleaseDetainedLicenseApp()
        {
            clsDriver _Driver;
            _Driver = clsDriver.Find(_License.DriverID);
            _Application = new clsApplication();
            _Application.ApplicantPersonID = _Driver.PersonID;
            _Application.ApplicationDate = DateTime.Now;
            _Application.ApplicationTypeID = 5;
            _Application.PaidFees = clsApplicationTypes.Find(5).ApplicationFees;
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
        private void _ReleaseDetainedLicense()
        {
            if (!_SaveReleaseDetainedLicenseApp())
            {
                MessageBox.Show("Error: Application Data Not Saved");
                return;

            }


            _DetainedLicense.IsReleased = true;
            _DetainedLicense.ReleasedByUserID = GlobalClass.CurrentUser.UserID;
            _DetainedLicense.ReleasedApplicationID = _Application.ApplicationID;
            _DetainedLicense.ReleaseDate = DateTime.Now;

            if (MessageBox.Show("Are you sure you want to release this detained license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_DetainedLicense.Save())
                {
                    MessageBox.Show("Data Saves Successfully.");
                    lblApplicationID.Text = _Application.ApplicationID.ToString();

                    lklShowLicenseHistory.Enabled = true;
                    lklShowNewLicenseInfo.Enabled = true;
                    btnRelease.Enabled = false;
                    ctrlDriverLicenseWithFilter1.DisableFilter(true);

                }
                else
                {
                    MessageBox.Show("Error: Detained License Data Not Saved");
                    clsApplication.DeleteApplication(_Application.ApplicationID);
                }
            }
         }
        private void btnRelease_Click(object sender, EventArgs e)
        {
            _ReleaseDetainedLicense();
        }

        private void lklShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory frm = new frmLicenseHistory(_License);
            frm.ShowDialog();
        }

        private void lklShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_License);
            frm.ShowDialog();
        }

        private void frmReleaseDetainedLicense_Load(object sender, EventArgs e)
        {
            if (_License == null)
            {
                _DisabledAllControls();
            }
        }

    }
}
