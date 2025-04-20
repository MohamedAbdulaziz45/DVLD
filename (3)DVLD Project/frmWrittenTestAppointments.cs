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
    public partial class frmWrittenTestAppointments : Form
    {

        int _LDLAppID;

        int _TestType = 2;
        public frmWrittenTestAppointments(int LDLAppID)
        {
            InitializeComponent();
            _LDLAppID = LDLAppID;
        }
        private void _RefreshRecordsCount()
        {
            int numberOfDataRows = dgvAppointments.Rows.Count;

            lblCount.Text = numberOfDataRows.ToString();
        }
        private void _RefreshAppointmentsList()
        {
            dgvAppointments.DataSource = clsTestAppointment.GetAllTestAppointmentsByDLAppAndTestType(_LDLAppID, _TestType);

            int numberOfDataRows = dgvAppointments.Rows.Count;
            if (numberOfDataRows > 0)
            {
                dgvAppointments.Columns.RemoveAt(1);
                dgvAppointments.Columns.RemoveAt(1);
                dgvAppointments.Columns.RemoveAt(3);
                dgvAppointments.Columns.RemoveAt(4);
            }
            _RefreshRecordsCount();

        }

        private void frmWrittenTestAppointments_Load(object sender, EventArgs e)
        {

            ctrlDLAppInfo1.LoadDLAppInfo(_LDLAppID);
            _RefreshAppointmentsList();
        }
        private bool _CheckLastTestAppointment()
        {
            clsTestAppointment testAppointment = clsTestAppointment.FindLastByLDLAppIDAndTestTypeID(_LDLAppID, _TestType);
            if (testAppointment == null)
            {
                return true;
            }

            if (testAppointment.IsLocked == false)
            {
                MessageBox.Show("Person Already have an active appointment for this test, You " +
                    "connot add new appointment", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            clsTest Test = clsTest.FindByTestAppointmentID(testAppointment.TestAppointmentID);
            if (Test.TestResult == true)
            {
                MessageBox.Show("This person already passed this test before, you can only retake faild Test "
                               , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        private bool _IsThereRetakeApp()
        {
            clsTestAppointment testAppointment = clsTestAppointment.FindLastByLDLAppIDAndTestTypeID(_LDLAppID, _TestType);
            if (testAppointment == null)
            {
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_CheckLastTestAppointment())
            {
                clsTestAppointment testAppointment = new clsTestAppointment();
                testAppointment.TestTypeID = _TestType;
                testAppointment.LDLAppID = _LDLAppID;

                bool IsRetake = _IsThereRetakeApp();
                Form frm = new frmScheduleTest(testAppointment, IsRetake);
                frm.ShowDialog();
                _RefreshAppointmentsList();
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsTestAppointment TestAppointment1 = clsTestAppointment.Find((int)dgvAppointments.CurrentRow.Cells[0].Value);
            bool IsRetakeTest = (TestAppointment1.RetakeTestID != -1);
            Form frm = new frmScheduleTest(TestAppointment1, IsRetakeTest);
            frm.ShowDialog();
            _RefreshAppointmentsList();
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsTestAppointment TestAppointment1 = clsTestAppointment.Find((int)dgvAppointments.CurrentRow.Cells[0].Value);
            Form frm = new frmTakeTest(TestAppointment1);
            frm.ShowDialog();
            _RefreshAppointmentsList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
