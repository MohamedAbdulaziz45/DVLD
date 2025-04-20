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
    public partial class frmMainMenue : Form
    {
        public frmMainMenue()
        {
            InitializeComponent();
        }

        private void frmMainMenue_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
        
          
            if (GlobalClass.CurrentUser != null)
            {
                int UserID = GlobalClass.CurrentUser.UserID;
                frmUserInfo frm = new frmUserInfo(UserID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("User Not Found please Contact the admin!","Error",MessageBoxButtons.OK);
            }

        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
           
            if (GlobalClass.CurrentUser != null)
            {
                int UserID = GlobalClass.CurrentUser.UserID;
                frmChangePassword frm = new frmChangePassword(UserID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("User Not Found please Contact the admin!", "Error", MessageBoxButtons.OK);
            }
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManagePeople frm = new frmManagePeople();
            frm.ShowDialog();
        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageUsers frm = new frmManageUsers();
            frm.ShowDialog();
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageApplicationTypes frm = new frmManageApplicationTypes();
            frm.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageTestTypes frm = new frmManageTestTypes();
            frm.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddLocalDrivingLicenseApp frm = new frmAddLocalDrivingLicenseApp(-1);
            frm.ShowDialog();
        }

        private void localDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocalDrivingLicenseApp frm = new frmLocalDrivingLicenseApp();
            frm.ShowDialog();   
        }

        private void driverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageDrivers frm = new frmManageDrivers(); 
            frm.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNewInternationalLicenseApp frm = new frmNewInternationalLicenseApp();
            frm.ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenewLicenseApplication frm = new frmRenewLicenseApplication();
            frm.ShowDialog();
        }

        private void internationalLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListInternationalLicenseApp frm = new frmListInternationalLicenseApp();
            frm.ShowDialog();
        }

        private void replacementForLostOrDamagedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReplacementForDamagedLicense frm = new frmReplacementForDamagedLicense();
            frm.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();   
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.ShowDialog();
        }

        private void manageDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListDetainedLicenses frm= new frmListDetainedLicenses();
            frm.ShowDialog();
        }
    }
}
