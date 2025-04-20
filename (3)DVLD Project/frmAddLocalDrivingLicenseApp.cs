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
    public partial class frmAddLocalDrivingLicenseApp : Form
    {
        public enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode;
        clsApplicationTypes _applicationType = clsApplicationTypes.Find(1);
        clsApplication _Application = new clsApplication();
        LocalDrivingLicenseApplication _LDLApplication;
        clsLicenseClass _LicenseClass;
        int _LDLAppID = -1;
        int _PersonID = -1;
        public frmAddLocalDrivingLicenseApp(int LDLAppID)
        {
            InitializeComponent();
            _LDLAppID = LDLAppID;
            if (_LDLAppID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;
        }
        private void _FillLicenseClassesInComoboBox()
        {
            DataTable dtLicenseClasses = clsLicenseClass.GetAllLicenseClasses();

            foreach (DataRow row in dtLicenseClasses.Rows)
            {

                cmbLicenseClass.Items.Add(row["ClassName"]);

            }

        }
        private void _LoadApplicationData()
        {

            lblApplicationDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            lblApplicationFees.Text = (Convert.ToInt32(_applicationType.ApplicationFees)).ToString();
            //lblCreatedBy.Text = clsUser.Find(1).UserName;
            lblCreatedBy.Text= GlobalClass.CurrentUser.UserName;
            _FillLicenseClassesInComoboBox();
            cmbLicenseClass.SelectedIndex = 2;

        }

        private void _FillDataIntoAppliction()
        {
            _Application.ApplicationDate = DateTime.Now;
            _Application.ApplicationStatus = 1;
            _Application.ApplicationTypeID = 1;
            _Application.ApplicantPersonID = _PersonID;
            _Application.LastStatusDate = DateTime.Now;
            _Application.PaidFees = _applicationType.ApplicationFees;
            _Application.CreatedByUserID = GlobalClass.CurrentUser.UserID;
            //_Application.CreatedByUserID = 1;
        }
        private void _FillDataIntoLDLApp()
        {

            _LDLApplication.ApplicationID=_Application.ApplicationID;
            _LDLApplication.LicenseClassID=_LicenseClass.LicenseClassID;
        }
        private void frmAddLocalDrivingLicenseApp_Load(object sender, EventArgs e)
        {
            if (_Mode == enMode.AddNew)
            {
                _LoadApplicationData();
                btnSave.Enabled = false;
                _LDLApplication = new LocalDrivingLicenseApplication();
                return;
            }

            _LDLApplication = LocalDrivingLicenseApplication.Find(_LDLAppID);

            if (_LDLApplication == null)
            {
                _LoadApplicationData();
                btnNext.Enabled = false;
                _LDLApplication = new LocalDrivingLicenseApplication();

                return;
            }


        }

        private bool _CheckApplicantBeforeSave()
        {
            _LicenseClass= clsLicenseClass.Find(cmbLicenseClass.SelectedItem.ToString());
            if (LocalDrivingLicenseApplication.isLDLAppExistByPersonIDandLicenseClassID(_PersonID,_LicenseClass.LicenseClassID ))
            {

                if(LocalDrivingLicenseApplication.isLDLAppExistByPersonIDandLicenseClassID(_PersonID, _LicenseClass.LicenseClassID, true) )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private bool _CheckApplicantBeforeSave2()
        {
            _LicenseClass = clsLicenseClass.Find(cmbLicenseClass.SelectedItem.ToString());
            if (LocalDrivingLicenseApplication.isLDLAppExistByPersonIDandLicenseClassID(_PersonID, _LicenseClass.LicenseClassID, false, false, true))
            {
                return false;
            }
            if (LocalDrivingLicenseApplication.isLDLAppExistByPersonIDandLicenseClassID(_PersonID, _LicenseClass.LicenseClassID, false, true))
            {
                return false;
            }

            return true;
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (ctrlPersonCardWithFilter1.ctrlPersonID > 0)
            {
                tabControl1.SelectedTab = tbpApplicationInfo;
                _PersonID = ctrlPersonCardWithFilter1.ctrlPersonID;
                btnSave.Enabled = true;
            }
            else
            {
                MessageBox.Show("Complete this page First.");
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ErrorMessageBoxForApplicant()
        {
            int ApplicantID = -1;
            int LDLAppID= LocalDrivingLicenseApplication.GetLDLAppIDByPersonIDandClassName(_PersonID, _LicenseClass.LicenseClassID);

            LocalDrivingLicenseApplication LDLApp =LocalDrivingLicenseApplication.Find(LDLAppID);
            ApplicantID = LDLApp.ApplicationID;
           
            if (LocalDrivingLicenseApplication.isLDLAppExistByPersonIDandLicenseClassID(_PersonID, _LicenseClass.LicenseClassID, false,true))
            {
                MessageBox.Show("Person Already has a license with the same Applied driving class" +
            "Choose different driving class", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
            }
            else
            {
                MessageBox.Show("Choose Another License Class. the selected Person already has an active " +
                  "for the selected class with Id =" + ApplicantID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!_CheckApplicantBeforeSave2())
            {
                ErrorMessageBoxForApplicant();
                return;
            }
            else
            {
                _FillDataIntoAppliction();

                if (_Application.Save())
                {
                    _FillDataIntoLDLApp();
                    if (_LDLApplication.Save())
                    {
                        MessageBox.Show("Data Saved Successfully.");
                    }
                    else
                    {
                        clsApplication.DeleteApplication(_Application.ApplicationID);
                        MessageBox.Show("Error: Data Is not Saved Successfully.");
                        return;
                    };
                }
                else
                {
                    MessageBox.Show("Error: Data Is not Saved Successfully.");
                    return;
                };
               
            }
        }
    }
}
