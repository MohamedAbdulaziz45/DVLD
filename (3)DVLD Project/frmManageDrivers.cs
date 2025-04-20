using BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3_DVLD_Project
{
    public partial class frmManageDrivers : Form
    {
        public frmManageDrivers()
        {
            InitializeComponent();
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

        private void _RefreshRecordsCount()
        {
            int numberOfDataRows = dvgAllDrivers.Rows.Count;

            lblCount.Text = numberOfDataRows.ToString();
        }
    
        private void _RefreshDriversList()
        {
            txtFilterBy.Text = "";
            dvgAllDrivers.DataSource = clsDriver.GetAllDrivers();
            dvgAllDrivers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dvgAllDrivers.Columns[0].HeaderText = "Driver ID";
            dvgAllDrivers.Columns[1].HeaderText = "Person ID";
            dvgAllDrivers.Columns[2].HeaderText = "National No.";
            dvgAllDrivers.Columns[3].HeaderText = "Full Name";
            dvgAllDrivers.Columns[4].HeaderText = "Date";
            dvgAllDrivers.Columns[5].HeaderText = "Active Licenses";

            _RefreshRecordsCount();

        }


        private void frmManageDrivers_Load(object sender, EventArgs e)
        {
            cmbFilterBy.SelectedIndex = 0;
            _disableTxtFilterBy();
            _RefreshDriversList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            if (cmbFilterBy.SelectedItem != "None")
            {
                string OrderBy = cmbFilterBy.SelectedItem.ToString();
                dvgAllDrivers.DataSource = clsDriver.GetAllDrivers(OrderBy, txtFilterBy.Text);
            }
            else
            {
                dvgAllDrivers.DataSource = clsDriver.GetAllDrivers();
            }
            _RefreshRecordsCount();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
       {
            if (cmbFilterBy.SelectedItem == "Driver ID"||cmbFilterBy.SelectedItem == "Person ID")
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
    }
}
