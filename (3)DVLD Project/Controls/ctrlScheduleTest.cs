using BussinessLayer;
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

namespace _3_DVLD_Project.Controls
{
    public partial class ctrlScheduleTest : UserControl
    {
        public ctrlScheduleTest()
        {
            InitializeComponent();
        }

        private enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode = enMode.AddNew;

        bool _IsRetake;
        clsTestAppointment _TestAppointment;

        bool _IsLocked;

        int _RetakeTestAppID;
       public void LoadTestAppointmentInfo(clsTestAppointment testAppointment,bool isRetake)
        {
           
          _TestAppointment = testAppointment;
          _IsRetake = isRetake;
          _IsLocked = testAppointment.IsLocked;
            if (_TestAppointment.TestAppointmentID ==-1)
            {
                _Mode = enMode.AddNew;
            }
            else
            {
                
                _Mode = enMode.Update;
            }
            _LoadData();
        }


        private void _LoadAppointmentDate()
        {
            if (_IsLocked == false)
            {
                lblDate.Visible = false;
                dtpDate.Visible = true;
                dtpDate.MinDate = DateTime.Now.AddDays(-1);

                if (_Mode == enMode.Update)
                {
                    dtpDate.Value = _TestAppointment.AppointmentDate;
                }
            }
            if(_IsLocked == true) 
            {
                lblDate.Visible = true;
                dtpDate.Visible = false;
                lblDate.Text = _TestAppointment.AppointmentDate.ToString("yyyy-MM-dd");

           
            }


        }

        private void _FillLDLAppData()
        {
            LocalDrivingLicenseApplication.LDLAppView LDLAppView;
            LDLAppView = LocalDrivingLicenseApplication.LDLAppView.FindView(_TestAppointment.LDLAppID);

            lblDLAppID.Text = LDLAppView.VLDLAppID.ToString();
            lblLicenseClass.Text = LDLAppView.VClassName;
            lblName.Text = LDLAppView.VFullName;
        }

        private void _ChangeTitleAndPicture()
        {
            clsTestTypes TestType;
            TestType = clsTestTypes.Find(_TestAppointment.TestTypeID);
            if (TestType.TestTypeID == 1)
            {
                pictureBox1.Image = Properties.Resources.Eye;
                gbScheduleTest.Text = "Vision Test";
            }
            if (TestType.TestTypeID == 2)
            {
                pictureBox1.Image = Properties.Resources.WrittenTest;
                gbScheduleTest.Text = "Written Test";
            }
            if (TestType.TestTypeID == 3)
            {
                pictureBox1.Image = Properties.Resources.DrivingTest;
                gbScheduleTest.Text = "Driving Test";
            }
            if (_IsLocked == true)
            {
                lblTitleRetake.Text = "Person already sat for the test.Appointment Locked.";
            }
            if (_IsRetake == true)
            {
               
                lblTitle.Text = "Schedule Retake Test";
            }
        }
        private void _FillTestTypeData()
        {
            clsTestTypes TestType;
            TestType = clsTestTypes.Find(_TestAppointment.TestTypeID);
            lblFees.Text = Convert.ToInt32(TestType.TestTypeFees).ToString();
           _ChangeTitleAndPicture();

        }

        private void _FillRetakeTestInCaseOfAddNewInfo()
        {
            clsApplicationTypes applicationTypes = clsApplicationTypes.Find(7);
            clsTestTypes TestType;
            TestType = clsTestTypes.Find(_TestAppointment.TestTypeID);

            if (_IsRetake == true)
            {
                lblRAppFees.Text = Convert.ToInt32(applicationTypes.ApplicationFees).ToString();
                lblTotalFees.Text = Convert.ToInt32(TestType.TestTypeFees + applicationTypes.ApplicationFees).ToString();
            }
            else
            {
                gbRetakeTestInfo.Enabled = false;
                lblTotalFees.Text = Convert.ToInt32(TestType.TestTypeFees).ToString();
            }
        }
        private void _FillRetakeTestInCaseOfUpdateInfo()
        {
            clsApplication RetakeTestApplication = clsApplication.Find(_TestAppointment.RetakeTestID);
            clsTestTypes TestType;
            TestType = clsTestTypes.Find(_TestAppointment.TestTypeID);
            if (RetakeTestApplication != null)
            {

                lblRAppFees.Text = Convert.ToInt32(RetakeTestApplication.PaidFees).ToString();
                lblTotalFees.Text = Convert.ToInt32(TestType.TestTypeFees + RetakeTestApplication.PaidFees).ToString();
                lblRTestAppID.Text = RetakeTestApplication.ApplicationID.ToString();
            }
            else
            {
                lblRAppFees.Text = 0.ToString();
                lblTotalFees.Text = Convert.ToInt32(TestType.TestTypeFees).ToString();
                lblRTestAppID.Text= "N/A";
                gbRetakeTestInfo.Enabled = false;
            }
        }
        private void _FillRetakeTestInfo()
        {
         if(_IsLocked == true||_Mode == enMode.Update)
            {
                _FillRetakeTestInCaseOfUpdateInfo();
            }
         else
            {
                _FillRetakeTestInCaseOfAddNewInfo();
            }
        }
        private void _FillTrialCount()
        {

           DataTable dt= clsTestAppointment.GetAllTestAppointmentsByDLAppAndTestType(_TestAppointment.LDLAppID
                                                 , _TestAppointment.TestTypeID,true);
            int rowCount = dt.Rows.Count;
            lblTestTrial.Text = rowCount.ToString();
        }
        private void _FillData()
        {
            if(_IsLocked == true)
            {
                btnSave.Enabled = false;
            }
            _LoadAppointmentDate();
            _FillLDLAppData();
            _FillTestTypeData();
            _FillTrialCount();
            _FillRetakeTestInfo();
        
        }
        private void _LoadData()
        {
            _FillData();
        }

        private clsApplication _LoadDataIntoRetakeTestApplication()
        {
            clsApplication RetakeTestApplication = new clsApplication();
            LocalDrivingLicenseApplication.LDLAppView LDLAppView;
            LDLAppView = LocalDrivingLicenseApplication.LDLAppView.FindView(_TestAppointment.LDLAppID);

            RetakeTestApplication.ApplicantPersonID= clsPerson.Find(LDLAppView.VNationalNo).ID;
            RetakeTestApplication.ApplicationDate= DateTime.Now;
            RetakeTestApplication.ApplicationTypeID = 7;
            RetakeTestApplication.ApplicationStatus = 1;
            RetakeTestApplication.LastStatusDate= DateTime.Now;

            clsApplicationTypes applicationTypes = clsApplicationTypes.Find(7);

            RetakeTestApplication.PaidFees = applicationTypes.ApplicationFees;
            RetakeTestApplication.CreatedByUserID = GlobalClass.CurrentUser.UserID;

            return RetakeTestApplication;
        }


        private void _LoadDataIntoTestAppointment()
        {

            clsTestTypes TestType;
            TestType = clsTestTypes.Find(_TestAppointment.TestTypeID);

            _TestAppointment.AppointmentDate = dtpDate.Value;
            _TestAppointment.PaidFees = TestType.TestTypeFees;
            _TestAppointment.CreatedByUserID = GlobalClass.CurrentUser.UserID;
            _TestAppointment.IsLocked = false;
        
            if(_IsRetake == true)
            {
                int RetakeTestAppID = _LoadDataIntoRetakeTestApplication().AddNewApplicationAndReturnID();
                _RetakeTestAppID = RetakeTestAppID; 
                if (RetakeTestAppID != -1)
                {
                    _TestAppointment.RetakeTestID = RetakeTestAppID;
                   if ( _TestAppointment.Save())
                    {
                        MessageBox.Show("Data saved successfully.");
                    }
                   else
                    {
                        MessageBox.Show("Error Data Not Saved.");
                        clsApplication.DeleteApplication(RetakeTestAppID);
                    }
                }

            }
           if( _IsRetake == false) 
            {
                if (_TestAppointment.Save())
                {
                    MessageBox.Show("Data saved successfully.");
                }
                else
                {
                    MessageBox.Show("Error Data Not Saved.");
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {


            _LoadDataIntoTestAppointment();
            if (_IsRetake == true)
            {
                if (_RetakeTestAppID != -1)
                {
                    lblRTestAppID.Text = _RetakeTestAppID.ToString();
                }
            }
            btnSave.Enabled = false;
        }
    }
}
