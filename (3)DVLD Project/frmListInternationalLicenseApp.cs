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
    public partial class frmListInternationalLicenseApp : Form
    {
        public frmListInternationalLicenseApp()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();   
        }
        private void _RefreshRecordsCount()
        {
            int numberOfDataRows = dgvILicenseApps.Rows.Count;

            lblCount.Text = numberOfDataRows.ToString();
        }

        private void _AdjustForILicenseList()
        {
            dgvILicenseApps.Columns.RemoveAt(7);
            dgvILicenseApps.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvILicenseApps.Columns[0].HeaderText = "Int.License ID";
            dgvILicenseApps.Columns[1].HeaderText = "Application ID";
            dgvILicenseApps.Columns[2].HeaderText = "Driver ID";
            dgvILicenseApps.Columns[3].HeaderText = "L.License ID";
        }
        private void _RefreshInternationalLicesnesList()
        {
            txtFilterBy.Text = "";
            dgvILicenseApps.DataSource = clsInternationalLicenses.GetAllInternationalLicenses();
            _AdjustForILicenseList();
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
        private void frmListInternationalLicenseApp_Load(object sender, EventArgs e)
        {

            cmbFilterBy.SelectedIndex = 0;
            _disableTxtFilterBy();
            _RefreshInternationalLicesnesList();
        }
        private void TxtFilterByChanged()
        {
            if (cmbFilterBy.SelectedItem != "None")
            {
                string OrderBy = cmbFilterBy.SelectedItem.ToString();
                dgvILicenseApps.DataSource = clsInternationalLicenses.GetAllInternationalLicenses(OrderBy, txtFilterBy.Text);
                _AdjustForILicenseList();
            }
            else
            {
                dgvILicenseApps.DataSource = clsInternationalLicenses.GetAllInternationalLicenses();
                _AdjustForILicenseList();

            }
        }
        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            TxtFilterByChanged();
            _RefreshRecordsCount();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilterBy.SelectedItem == "Int.License ID"||cmbFilterBy.SelectedItem == "Application ID"
                || cmbFilterBy.SelectedItem == "Driver ID" || cmbFilterBy.SelectedItem == "L.License ID")
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
            frmNewInternationalLicenseApp frm = new frmNewInternationalLicenseApp();
            frm.ShowDialog();
        }
    }
}
