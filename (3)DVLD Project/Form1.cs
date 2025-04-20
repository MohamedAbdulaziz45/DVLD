using _3_DVLD_Project.Controls;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            ctrlDLAppInfo1.LoadDLAppInfo(30);
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form frm = new frmVisionTestAppointments(31);
            frm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form frm = new frmManageUsers();
            frm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddNewUser(-1);
            frm.ShowDialog();
        }
    }
}
