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

namespace _3_DVLD_Project.Controls
{
    public partial class ctrlTakeTest : UserControl
    {
        public ctrlTakeTest()
        {
            InitializeComponent();
        }


        clsTestAppointment _TestAppointment;

        bool _IsLocked;


        public void LoadTakeTestInfo(clsTestAppointment testAppointment)
        {

            _TestAppointment = testAppointment;
            _IsLocked = testAppointment.IsLocked;

            _LoadData();
        }


        private void _LoadAppointmentDate()
        {
                lblDate.Text = _TestAppointment.AppointmentDate.ToString("yyyy-MM-dd");
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
                gbScheduleTest.Text = "Vision Test";
            }
        }
        private void _FillTestTypeData()
        {
            clsTestTypes TestType;
            TestType = clsTestTypes.Find(_TestAppointment.TestTypeID);
            lblFees.Text = Convert.ToInt32(TestType.TestTypeFees).ToString();
            _ChangeTitleAndPicture();

        }


        private void _FillTrialCount()
        {

            DataTable dt = clsTestAppointment.GetAllTestAppointmentsByDLAppAndTestType(_TestAppointment.LDLAppID
                                                  , _TestAppointment.TestTypeID, true);
            int rowCount = dt.Rows.Count;
            lblTestTrial.Text = rowCount.ToString();
        }
        private void _FillTestID()
        {
            clsTest Test = clsTest.FindByTestAppointmentID(_TestAppointment.TestAppointmentID);
            if(Test == null)
            {
                lblTestID.Text = "Not Taken Yet";
            }
            if(Test != null)
            {
                lblTestID.Text = Test.TestID.ToString();
            }
        }
        private void _LoadPicture()
        {
            if(_TestAppointment.TestTypeID == 1) 
            {
                pictureBox1.Image = Properties.Resources.Eye;
                gbScheduleTest.Text = "Vision Test";
            }
            if(_TestAppointment.TestTypeID == 2)
            {
                pictureBox1.Image = Properties.Resources.WrittenTest;
                gbScheduleTest.Text = "Written Test";
            }
            if (_TestAppointment.TestTypeID == 3)
            {
                pictureBox1.Image = Properties.Resources.DrivingTest;
                gbScheduleTest.Text = "Driving Test";
            }
        }
        private void _FillData()
        {
            _LoadPicture();
            _LoadAppointmentDate();
            _FillLDLAppData();
            _FillTestTypeData();
            _FillTrialCount();
            _FillTestID();


        }
        private void _LoadData()
        {
            _FillData();
        }
}
}
