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

namespace _3_DVLD_Project
{
    public partial class frmLocalDrivingLicenseApp : Form
    {
        public frmLocalDrivingLicenseApp()
        {
            InitializeComponent();
        }
        private void _RefreshRecordsCount()
        {
            int numberOfDataRows = dgvAllLDLApps.Rows.Count;

            lblCount.Text = numberOfDataRows.ToString();
        }

        private void _RefreshLocalDrivingAppsList()
        {
            txtFilterBy.Text = "";
            dgvAllLDLApps.DataSource = LocalDrivingLicenseApplication.GetAllLDLApp();
            dgvAllLDLApps.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAllLDLApps.Columns[0].HeaderText = "L.D.L.AppID";
            dgvAllLDLApps.Columns[1].HeaderText = "Driving Class";
            dgvAllLDLApps.Columns[5].HeaderText = "Passed Tests";
            dgvAllLDLApps.Columns[0].Width = 50;
            dgvAllLDLApps.Columns[1].Width = 170;
            dgvAllLDLApps.Columns[2].Width = 70;
            dgvAllLDLApps.Columns[3].Width = 250;
            _RefreshRecordsCount();

        }



        private void _disableTxtFilterBy()
        {

            if (cmbFilterBy.SelectedItem == "None")
            {
                txtFilterBy.Visible = false;
            }
            else
            {
                txtFilterBy.Visible = true;
            }
        }
        private void frmLocalDrivingLicenseApp_Load(object sender, EventArgs e)
        {
            cmbFilterBy.SelectedIndex = 0;
            _disableTxtFilterBy();
            _RefreshLocalDrivingAppsList();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TxtFilterByChanged()
        {
            if (cmbFilterBy.SelectedItem != "None")
            {
                string OrderBy = cmbFilterBy.SelectedItem.ToString();
                dgvAllLDLApps.DataSource = LocalDrivingLicenseApplication.GetAllLDLApp(OrderBy, txtFilterBy.Text);
            }
            else
            {
                dgvAllLDLApps.DataSource = LocalDrivingLicenseApplication.GetAllLDLApp();

            }
        }
        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            TxtFilterByChanged();
            _RefreshRecordsCount();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilterBy.SelectedItem == "L.D.L.AppID")
            {


                if (char.IsControl(e.KeyChar))
                {
                    return; // Allow Backspace and Delete
                }

                // Allow only digits
                if (!char.IsDigit(e.KeyChar))
                {
                    e.Handled = true; // If it's not a digit, block the key press
                }
            }
        }

        private void cmbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterBy.Text = "";
            _disableTxtFilterBy();
            _RefreshRecordsCount();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddLocalDrivingLicenseApp(-1);
            frm.ShowDialog();
            _RefreshLocalDrivingAppsList();

        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LocalDrivingLicenseApplication _LDLApp;
            _LDLApp = LocalDrivingLicenseApplication.Find((int)dgvAllLDLApps.CurrentRow.Cells[0].Value);

            int ApplicationID = _LDLApp.ApplicationID;
            clsApplication _Application;
            _Application = clsApplication.Find(ApplicationID);
            //2 = cancelled
            _Application.ApplicationStatus = 2;
            _Application.LastStatusDate = DateTime.Now;
            if (_Application.Save())
            {
                MessageBox.Show("Changed succefully");
            }
            else
            {
                MessageBox.Show("there is a problem...");
            }

            _RefreshLocalDrivingAppsList();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {


            if (!LocalDrivingLicenseApplication.DeleteLDLApp((int)dgvAllLDLApps.CurrentRow.Cells[0].Value))
            {
                MessageBox.Show("there is a problem...");

            }

            _RefreshLocalDrivingAppsList();
        }

        private void _LoadContextMenuStripTests(LocalDrivingLicenseApplication.LDLAppView LDLAppView)
        {
            if (LDLAppView.VPassedTestCount == 0)
            {
                scheduleVisionTestToolStripMenuItem.Enabled = true;
                scheduleWrittenTestToolStripMenuItem.Enabled = false;
                scheduleStreetTestToolStripMenuItem.Enabled = false;
            }
            if (LDLAppView.VPassedTestCount == 1)
            {
                scheduleVisionTestToolStripMenuItem.Enabled = false;
                scheduleWrittenTestToolStripMenuItem.Enabled = true;
                scheduleStreetTestToolStripMenuItem.Enabled = false;
            }
            if (LDLAppView.VPassedTestCount == 2)
            {
                scheduleVisionTestToolStripMenuItem.Enabled = false;
                scheduleWrittenTestToolStripMenuItem.Enabled = false;
                scheduleStreetTestToolStripMenuItem.Enabled = true;
            }
            if (LDLAppView.VPassedTestCount == 3)
            {
                sechduleTestsToolStripMenuItem.Enabled = false;
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = true;
            }
        }
        private void _CheckIfLDLAppCancelled(LocalDrivingLicenseApplication.LDLAppView LDLAppView)
        {
            if (LDLAppView.VStatus == "Cancelled" || LDLAppView.VStatus == "Completed")
            {
                sechduleTestsToolStripMenuItem.Enabled = false;
            }
            if(LDLAppView.VStatus == "Cancelled")
            {
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
            }

        }
        private void _RefreshContextMenue()
        {
            showApplicationDetailsToolStripMenuItem.Enabled = true;
            editApplicationToolStripMenuItem.Enabled = true;
            deleteApplicationToolStripMenuItem.Enabled = true;
            cancelApplicationToolStripMenuItem.Enabled = true;
            sechduleTestsToolStripMenuItem.Enabled = true;
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = false;
            showPersonLicenseHistoryToolStripMenuItem.Enabled = true;

        }
        private void _CheckIfLicenseIssued(LocalDrivingLicenseApplication LDLApp)
        {
           clsLicense license = clsLicense.FindLicenseByApplicationID(LDLApp.ApplicationID);
            if(license !=null)
            {
                editApplicationToolStripMenuItem.Enabled = false;
                deleteApplicationToolStripMenuItem.Enabled = false;
                cancelApplicationToolStripMenuItem.Enabled = false;
                sechduleTestsToolStripMenuItem.Enabled = false;
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                showLicenseToolStripMenuItem.Enabled = true;

            }
        }
        private void _LoadContextMenuStripData()
        {
            LocalDrivingLicenseApplication.LDLAppView _LDLAppView;
            _LDLAppView = LocalDrivingLicenseApplication.LDLAppView.FindView((int)dgvAllLDLApps.CurrentRow.Cells[0].Value);

            LocalDrivingLicenseApplication LDLApp;
            LDLApp = LocalDrivingLicenseApplication.Find((int)dgvAllLDLApps.CurrentRow.Cells[0].Value);



            _RefreshContextMenue();

            _LoadContextMenuStripTests(_LDLAppView);
            
            _CheckIfLDLAppCancelled(_LDLAppView);
            _CheckIfLicenseIssued(LDLApp);
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            _LoadContextMenuStripData();
        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVisionTestAppointments frm = new frmVisionTestAppointments((int)dgvAllLDLApps.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshLocalDrivingAppsList();
        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmWrittenTestAppointments frm = new frmWrittenTestAppointments((int)dgvAllLDLApps.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshLocalDrivingAppsList();
        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStreetTestAppointments frm = new frmStreetTestAppointments((int)dgvAllLDLApps.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshLocalDrivingAppsList();
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIssueDriveLicenseForFirst frm = new frmIssueDriveLicenseForFirst((int)dgvAllLDLApps.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshLocalDrivingAppsList();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo((int)dgvAllLDLApps.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshLocalDrivingAppsList();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLicenseHistory frm = new frmLicenseHistory((int)dgvAllLDLApps.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshLocalDrivingAppsList();
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
