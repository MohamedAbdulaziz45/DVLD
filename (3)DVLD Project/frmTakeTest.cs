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
    public partial class frmTakeTest : Form
    {
        clsTest _Test;
        clsTestAppointment _TestAppointment;
        bool _IsLocked;
        public frmTakeTest(clsTestAppointment TestAppointment)
        {
            InitializeComponent();
            _TestAppointment = TestAppointment;
            _IsLocked = TestAppointment.IsLocked;
            ctrlTakeTest1.LoadTakeTestInfo(TestAppointment);
        }

        private void _IsTakeTestLocked()
        {
            clsTest Test = new clsTest();
            Test.TestAppointmentID = _TestAppointment.TestAppointmentID;
            if (_IsLocked==true)
            {
                if(Test.TestResult==true)
                {
                    rbPass.Checked = true;
                }
                else
                {
                    rbFail.Checked = true;
                }

                txtNotes.Text = Test.Notes;
                rbPass.Enabled=false;
                rbFail.Enabled=false;
                txtNotes.Enabled=false;
                btnSave.Enabled=false;


            }
        }
        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            _IsTakeTestLocked();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _LoadDateIntoTest()
        {
             _Test = new clsTest();
            _Test.TestAppointmentID=_TestAppointment.TestAppointmentID;

            if (rbPass.Checked == true)

                _Test.TestResult = true;
            else
                _Test.TestResult = false;

            _Test.Notes= txtNotes.Text;
            _Test.CreatedByUserID = GlobalClass.CurrentUser.UserID;

          
            if (_Test.Save())
            {
                MessageBox.Show("Data saved successfully.");
            }
            else
            {
                MessageBox.Show("Error Data Not Saved.");
            }
        }
        private bool _ChangeRetakeTestAppStatus()
        {
            bool isSaved = true;
            clsApplication RetakeTestApp = clsApplication.Find(_TestAppointment.RetakeTestID);
            if (RetakeTestApp != null)
            {
                RetakeTestApp.ApplicationStatus = 3;

              if(RetakeTestApp.Save())
                {
                    isSaved = true;
                }
              else
                {
                    isSaved = false;
                }
            }
           
            return isSaved;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            _LoadDateIntoTest();
            _TestAppointment.IsLocked = true;
            
            if (_TestAppointment.Save() && _ChangeRetakeTestAppStatus())
            {

                MessageBox.Show("Data saved successfully.");
                btnSave.Enabled = false;
            }
            else
            {
                clsTest.DeleteTest(_Test.TestID);
                MessageBox.Show("Error Data Not Saved.");
            }
        }
    }
}
