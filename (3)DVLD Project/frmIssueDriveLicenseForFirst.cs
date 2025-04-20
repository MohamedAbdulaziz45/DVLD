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
    public partial class frmIssueDriveLicenseForFirst : Form
    {
        LocalDrivingLicenseApplication _LDLApp;
        LocalDrivingLicenseApplication.LDLAppView _LDLAppView;
        int _LDLAppID;
        public frmIssueDriveLicenseForFirst(int LDLAppID)
        {
            InitializeComponent();
            _LDLAppID = LDLAppID;
            _LDLApp = LocalDrivingLicenseApplication.Find(LDLAppID);
            _LDLAppView = LocalDrivingLicenseApplication.LDLAppView.FindView(LDLAppID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmIssueDriveLicenseForFirst_Load(object sender, EventArgs e)
        {
            if (_LDLApp == null)
            {
                MessageBox.Show("Error we can't find this Application");
                this.Close();
                return;
            }
            ctrlDLAppInfo1.LoadDLAppInfo(_LDLAppID);

            
        }
        private bool _LoadDataIntoDriver(ref clsDriver  Driver)
        {

            Driver.PersonID = clsPerson.Find( _LDLAppView.VNationalNo).ID;
            Driver.CreatedByUserID = GlobalClass.CurrentUser.UserID;
            Driver.CreatedDate = DateTime.Now;

            if(Driver.Save())
            {
                return true;
            }
            else
            {
                return false;   
            }
        }

        private clsLicense _LoadDataIntoLicense(clsDriver Driver)
        {
            clsApplication application = clsApplication.Find(_LDLApp.ApplicationID);
            clsLicense license = new clsLicense();
            clsLicenseClass licenseClass = clsLicenseClass.Find(_LDLAppView.VClassName);
            license.ApplicationID =_LDLApp.ApplicationID;
            license.DriverID= Driver.DriverID;
            license.LicenseClass = licenseClass.LicenseClassID;
            license.IssueDate= DateTime.Now;
            license.ExpirationDate = DateTime.Now.AddYears(licenseClass.DefaultValidityLength);
            license.Notes = txtNotes.Text;
            license.PaidFees = licenseClass.ClassFees;
            license.IsActive = true;
            license.IssueReason =(byte) application.ApplicationTypeID;
            license.CreatedByUserID = GlobalClass.CurrentUser.UserID;   

            return license;
        }

        private void _UpdateLDLAppStatus()
        {
            clsApplication application;
            application = clsApplication.Find(_LDLApp.ApplicationID);
            application.ApplicationStatus = 3;
            application.Save();
        }

        private bool _IsThereAlreadyADriver(ref clsDriver Driver)
        {
            int PersonID = clsPerson.Find(_LDLAppView.VNationalNo).ID;

            Driver=clsDriver.FindByPersonID(PersonID);

            if (Driver==null)
            {
                return _LoadDataIntoDriver(ref Driver);
            }
            else
            {
                return true;    
            }
        }
        private void btnIssue_Click(object sender, EventArgs e)
        {
            clsDriver Driver = new clsDriver();



            if (_IsThereAlreadyADriver(ref Driver))
            {
                if(_LoadDataIntoLicense(Driver).Save())
                {
                    int LicenseID = clsLicense.FindLicenseByApplicationID(_LDLApp.ApplicationID).LicenseID;
                    MessageBox.Show("License issued successfully with license ID = " +
                       LicenseID, "Succeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _UpdateLDLAppStatus();
                    btnIssue.Enabled = false;

                }
                else
                {
                    MessageBox.Show("Error: Can't Save Data.");
                    clsDriver.DeleteDriverByPersonID(clsPerson.Find(_LDLAppView.VNationalNo).ID);
                }
            }
            else
            {
                MessageBox.Show("Error: Can't Save Data.");
            }
        }

        private void ctrlDLAppInfo1_Load(object sender, EventArgs e)
        {

        }
    }
}
