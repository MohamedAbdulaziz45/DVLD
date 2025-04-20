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
    public partial class frmListDetainedLicenses : Form
    {
        public frmListDetainedLicenses()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
      

        private void _AdjustdgvDetainedLicenses()
        {
            if (dgvDetainedLicenses.RowCount > 0)
            {
                dgvDetainedLicenses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvDetainedLicenses.Columns[0].HeaderText = "D.ID";
                dgvDetainedLicenses.Columns[1].HeaderText = "L.ID";
                dgvDetainedLicenses.Columns[2].HeaderText = "D.Date";
                dgvDetainedLicenses.Columns[3].HeaderText = "Is Released";
                dgvDetainedLicenses.Columns[4].HeaderText = "Fine Fees";
                dgvDetainedLicenses.Columns[5].HeaderText = "Release Date";
                dgvDetainedLicenses.Columns[6].HeaderText = "N.No.";
                dgvDetainedLicenses.Columns[7].HeaderText = "Full Name";
                dgvDetainedLicenses.Columns[8].HeaderText = "Release App.ID";
            }

        }
        private void _RefreshDetainedLicensesList()
        {
            txtFilterBy.Text = "";
            dgvDetainedLicenses.DataSource = clsDetainedLicense.GetAllDetainedLicenses();
            _AdjustdgvDetainedLicenses();
            _RefreshRecordsCount();

        }
        private void _RefreshRecordsCount()
        {
            int numberOfDataRows = dgvDetainedLicenses.Rows.Count;

            lblCount.Text = numberOfDataRows.ToString();
        }
        private void cmbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterBy.Text = "";

            _RefreshRecordsCount();
            _DisableAndEnableComboBox();

        }
        private void _DisableAndEnableComboBox()
        {
            if (cmbFilterBy.SelectedItem == "Is Released")
            {
                cmbIsReleased.Visible = true;
                cmbIsReleased.SelectedIndex= 0;
                txtFilterBy.Visible = false;
            }
            else
            {
                cmbIsReleased.Visible = false;
                txtFilterBy.Visible = true;
            }

            if (cmbFilterBy.SelectedItem == "None")
            {
                txtFilterBy.Visible = false;
            }
        }
        private void frmListDetainedLicenses_Load(object sender, EventArgs e)
        {
            cmbFilterBy.SelectedIndex = 0;
        
            _RefreshDetainedLicensesList();
            _DisableAndEnableComboBox();

        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.ShowDialog();
            _RefreshDetainedLicensesList();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();
            _RefreshDetainedLicensesList();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilterBy.SelectedItem == "Detain ID" || cmbFilterBy.SelectedItem == "Release Application ID")
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

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            if (cmbFilterBy.SelectedItem != "None")
            {
                string OrderBy = cmbFilterBy.SelectedItem.ToString();
                dgvDetainedLicenses.DataSource = clsDetainedLicense.GetAllDetainedLicenses(OrderBy, txtFilterBy.Text);
            }
            else
            {
                dgvDetainedLicenses.DataSource = clsDetainedLicense.GetAllDetainedLicenses();
            }
           
                _AdjustdgvDetainedLicenses();
        
            _RefreshRecordsCount();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DetainID = (int)dgvDetainedLicenses.CurrentRow.Cells[0].Value;
            clsDetainedLicense detainedLicense = clsDetainedLicense.Find(DetainID);
            clsLicense license = clsLicense.Find(detainedLicense.LicenseID);
            clsDriver Driver = clsDriver.Find(license.DriverID);
            

            frmPersonDetails frm = new frmPersonDetails(Driver.PersonID);
            frm.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DetainID = (int)dgvDetainedLicenses.CurrentRow.Cells[0].Value;
            clsDetainedLicense detainedLicense = clsDetainedLicense.Find(DetainID);
            clsLicense license = clsLicense.Find(detainedLicense.LicenseID);
            frmLicenseInfo frm = new frmLicenseInfo(license);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DetainID = (int)dgvDetainedLicenses.CurrentRow.Cells[0].Value;
            clsDetainedLicense detainedLicense = clsDetainedLicense.Find(DetainID);
            clsLicense license = clsLicense.Find(detainedLicense.LicenseID);
            frmLicenseHistory frm = new frmLicenseHistory(license);
            frm.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DetainID = (int)dgvDetainedLicenses.CurrentRow.Cells[0].Value;
            clsDetainedLicense detainedLicense = clsDetainedLicense.Find(DetainID);
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense(detainedLicense.LicenseID);
            frm.ShowDialog();

            _RefreshDetainedLicensesList();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int DetainID = (int)dgvDetainedLicenses.CurrentRow.Cells[0].Value;
            clsDetainedLicense detainedLicense = clsDetainedLicense.Find(DetainID);
        
            if(detainedLicense.IsReleased == true)
            {
                releaseDetainedLicenseToolStripMenuItem.Enabled = false;
            }
            else
            {
                releaseDetainedLicenseToolStripMenuItem.Enabled = true;
            }
        }

        private void cmbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            string OrderBy = cmbFilterBy.SelectedItem.ToString();

            if (cmbIsReleased.SelectedItem == "Yes")
            {
                dgvDetainedLicenses.DataSource = clsDetainedLicense.GetAllDetainedLicenses(OrderBy, "Yes");
            }
            if (cmbIsReleased.SelectedItem =="No")
            {
                dgvDetainedLicenses.DataSource = clsDetainedLicense.GetAllDetainedLicenses(OrderBy, "No");
            }
            if (cmbIsReleased.SelectedItem == "All")
            {
                dgvDetainedLicenses.DataSource = clsDetainedLicense.GetAllDetainedLicenses();
            }

            _AdjustdgvDetainedLicenses();

            _RefreshRecordsCount();
        }
    }
}
