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
    public partial class frmDetainLicense : Form
    {
        clsLicense _License;
        int _LicenseID;

        clsDetainedLicense _DetainedLicense;

        public frmDetainLicense()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

 
        private void _ValidateAll()
        {

            if (_License == null)
            {
                MessageBox.Show("there no Lincese with LicenseID = " + _LicenseID
                    , "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _DisabledAllControls();
                return;
            }

            if (_License.IsActive == false)
            {
                MessageBox.Show("this License with licenseID=" + _LicenseID +
                  " Not Allowed because this License is not Active"
                 , "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                lklShowLicenseHistory.Enabled = true;
                lklShowNewLicenseInfo.Enabled = false;
                btnDetain.Enabled = false;
                txtFineFees.Enabled = false;
                return;
            }
            if (_License.ExpirationDate < DateTime.Now)
            {

                MessageBox.Show("Selected License Is Expired ,It is expired on:"
                    + _License.ExpirationDate.ToString("dd/mm/yyyy")
                   , "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lklShowLicenseHistory.Enabled = true;
                lklShowNewLicenseInfo.Enabled = false;
                btnDetain.Enabled = false;
                txtFineFees.Enabled = false;
                return;
            }
            if(clsDetainedLicense.IsDetainedLicenseExistByLicenseID(_LicenseID))
            {
                clsDetainedLicense DetainedLicense = clsDetainedLicense.FindByLicenseID(_LicenseID);
                if (DetainedLicense.IsReleased == false)
                {
                    MessageBox.Show("this License is already is detained"
                       , "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lklShowLicenseHistory.Enabled = true;
                    lklShowNewLicenseInfo.Enabled = false;
                    btnDetain.Enabled = false;
                    txtFineFees.Enabled = false;
                    return;
                }
            }
            lklShowLicenseHistory.Enabled = true;
            lklShowNewLicenseInfo.Enabled = false;
            btnDetain.Enabled = true;
            txtFineFees.Enabled = true;
        }

        private void _FillRemainingApplicationInfo()
        {
            lblLicenseID.Text = _License.LicenseID.ToString();
        }
        private void ctrlDriverLicenseWithFilter1_OnSearchClick(int obj)
        {
            int LicenseID = obj;
            _LicenseID = LicenseID;
            _License = clsLicense.Find(LicenseID);
            _ValidateAll();

            if (_License != null)
            {
                _FillRemainingApplicationInfo();
            }
            else
            {
                lblLicenseID.Text = "[???]";

            }
        }

        private void txtFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {

                if (char.IsControl(e.KeyChar))
                {
                    return; // Allow Backspace and Delete
                }

                // Allow only digits
                if (!char.IsDigit(e.KeyChar))
                {
                    e.Handled = true; // If it's not a digit, block the key press
                }
        }

        private void _DisabledAllControls()
        {
            lklShowLicenseHistory.Enabled = false;
            lklShowNewLicenseInfo.Enabled = false;
            btnDetain.Enabled = false;
            txtFineFees.Enabled = false;
        }
        private void _FillDataIntoApplicationInfo()
        {
            lblDetainDate.Text = DateTime.Now.ToString("dd/mm/yyyy");

            lblCreatedBy.Text = GlobalClass.CurrentUser.UserName;

        }
        private void frmDetainLicense_Load(object sender, EventArgs e)
        {
            _DisabledAllControls();
            _FillDataIntoApplicationInfo();
        }

        private void _DetainLicense()
        {
             _DetainedLicense = new clsDetainedLicense();
            _DetainedLicense.LicenseID = _LicenseID;
            _DetainedLicense.DetainDate = DateTime.Now;
            _DetainedLicense.Finefees = decimal.Parse(txtFineFees.Text);
            _DetainedLicense.CreatedByUserID = GlobalClass.CurrentUser.UserID;
            _DetainedLicense.IsReleased = false;

            if (MessageBox.Show("Are you sure you want to detain this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_DetainedLicense.Save())
                {
                    MessageBox.Show("Data Saves Successfully.");
                    lblDetainID.Text = _DetainedLicense.DetainID.ToString();

                    lklShowLicenseHistory.Enabled = true;
                    lklShowNewLicenseInfo.Enabled = true;
                    btnDetain.Enabled = false;
                    ctrlDriverLicenseWithFilter1.DisableFilter(true);
                    txtFineFees.Enabled = false;

                }
                else
                {
                    MessageBox.Show("Error: Data Not Saved");
                }
            }
        }
        private void btnDetain_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtFineFees.Text))
                MessageBox.Show("Enter Fine fees");
            else
               _DetainLicense();
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
    }
}
