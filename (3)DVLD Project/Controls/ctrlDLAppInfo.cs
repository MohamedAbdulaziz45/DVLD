using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BussinessLayer;

namespace _3_DVLD_Project.Controls
{
    public partial class ctrlDLAppInfo : UserControl
    {
        public ctrlDLAppInfo()
        {
            InitializeComponent();
        }

        LocalDrivingLicenseApplication.LDLAppView _DLAppView;
        LocalDrivingLicenseApplication _DLApp;
        clsApplication _Application;
        int _DLAppID;
        clsLicense _License;
        public void LoadDLAppInfo(int DLAppID)
        {
            _DLAppID = DLAppID;
            _DLAppView = LocalDrivingLicenseApplication.LDLAppView.FindView(DLAppID);
            _DLApp = LocalDrivingLicenseApplication.Find(DLAppID);

            int ApplicationID = _DLApp.ApplicationID;
            _Application = clsApplication.Find(ApplicationID);

            _LoadData();
        }

        private void _EnablelklViewLicenseInfo()
        {
          _License =  clsLicense.FindLicenseByApplicationID(_Application.ApplicationID);
            if(_License == null)
            {
                lklViewLicenseInfo.Enabled = false;
            }
            else
            {
                lklViewLicenseInfo.Enabled= true;
            }

        }

        private void _FillDataInDLAppInfo()
        {
            lblDLAppID.Text = _DLAppView.VLDLAppID.ToString();
            lblLicenseClass.Text = _DLAppView.VClassName;
            lblPassedTests.Text = _DLAppView.VPassedTestCount.ToString()+"/3";
            _EnablelklViewLicenseInfo();


        }

        private void _FillDataInApplicationInfo()
        {
            lblApplicationID.Text = _Application.ApplicationID.ToString();
            lblStatus.Text = _DLAppView.VStatus;
            lblFees.Text = _Application.PaidFees.ToString();
            lblApplicationType.Text = (clsApplicationTypes.Find(_Application.ApplicationTypeID).ApplicationTypeTitle);
            lblApplicantName.Text = _DLAppView.VFullName;
            lblApplicationDate.Text =_Application.ApplicationDate.ToString("yyyy-MM-dd");
            lblLastStatusDate.Text = _Application.LastStatusDate.ToString("yyyy-MM-dd");
            lblCreatedByUser.Text = clsUser.Find(_Application.CreatedByUserID).UserName;


        }
        private void _FillDataInControl()
        {
          _FillDataInDLAppInfo();
          _FillDataInApplicationInfo();

        }
        private void _LoadData()
        {
            if (_DLAppView == null)
            {
                MessageBox.Show("Wrong ID entered");
                return;
            }

            _FillDataInControl();
        }

        private void lklViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails(_Application.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void lklViewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_License.LicenseID);
            frm.ShowDialog();
        }
    }
}
