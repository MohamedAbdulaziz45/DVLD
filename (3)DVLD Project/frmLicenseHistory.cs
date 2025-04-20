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
using static BussinessLayer.LocalDrivingLicenseApplication;
using static System.Net.Mime.MediaTypeNames;

namespace _3_DVLD_Project
{
    public partial class frmLicenseHistory : Form
    {
        int _LDLAppID;
        LocalDrivingLicenseApplication _LDLApp;
        clsApplication _Application;
        clsPerson _Person;
        clsLicense _License;
        public frmLicenseHistory(int LDLAppID)
        {
            InitializeComponent();
            _LDLAppID = LDLAppID;
            _LoadData();
        }
        public frmLicenseHistory(clsLicense License)
        {
            InitializeComponent();
            _License = License;
            _LoadDataByLicense();
        }

        private void _LoadDataByLicense()
        {
            _Application = clsApplication.Find(_License.ApplicationID);
            _Person = clsPerson.Find(_Application.ApplicantPersonID);
            ctrlPersonCardWithFilter1.LoadPersonInfo(_Person.ID);
            _RefreshLocalLicensesHistory();
            _RefreshInternationalLicensesHistory();
        }
        private void _DefineVariables()
        {
            _LDLApp = LocalDrivingLicenseApplication.Find(_LDLAppID);
            _Application = clsApplication.Find(_LDLApp.ApplicationID);
            _Person = clsPerson.Find(_Application.ApplicantPersonID);


            if (_LDLApp == null)
            {
                MessageBox.Show("Error Can't Find Data");
                return;
            }
        }

        private void _RefreshRecordsCount()
        {
            int numberOfDataRows = dgvLocalLicenseHistory.Rows.Count;

            lblLocalCount.Text = numberOfDataRows.ToString();
        }
        private void _RefreshRecordsCount2()
        {
            int numberOfDataRows = dgvInternationals.Rows.Count;

            lblInternationalCount.Text = numberOfDataRows.ToString();
        }


        private void _RefreshLocalLicensesHistory()
        {
            clsDriver Driver= clsDriver.FindByPersonID(_Person.ID);
            int DriverID = Driver.DriverID;
            dgvLocalLicenseHistory.DataSource = clsLicense.GetAllLicensesByTheSameDriver(DriverID);
            _RefreshRecordsCount();
        }

        private void _RefreshInternationalLicensesHistory()
        {
            clsDriver Driver = clsDriver.FindByPersonID(_Person.ID);
            int DriverID = Driver.DriverID;
            dgvInternationals.DataSource = clsInternationalLicenses.GetAllInternationalLicensesByTheSameDriver(DriverID);
            _RefreshRecordsCount2();
        }

        private void _LoadData()
        {
            _DefineVariables();

            ctrlPersonCardWithFilter1.LoadPersonInfo(_Person.ID);
            _RefreshLocalLicensesHistory();
            _RefreshInternationalLicensesHistory();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
