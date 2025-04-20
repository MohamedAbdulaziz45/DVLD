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
    public partial class frmScheduleTest : Form
    {
        clsTestAppointment _TestAppointment;
        bool _IsRetake;
        public frmScheduleTest(clsTestAppointment TestAppointment,bool isRetake)
        {
            InitializeComponent();
            _TestAppointment=TestAppointment;
            _IsRetake=isRetake;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            ctrlScheduleTest1.LoadTestAppointmentInfo(_TestAppointment, _IsRetake);
        }

        private void ctrlScheduleTest1_Load(object sender, EventArgs e)
        {

        }
    }
}
