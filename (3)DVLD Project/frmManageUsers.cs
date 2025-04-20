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
    public partial class frmManageUsers : Form
    {
        public frmManageUsers()
        {
            InitializeComponent();
        }
        private void _RefreshUsersList()
        {
            dvgAllUser.DataSource = clsUser.GetAllUsers();
            _RefreshRecordsCount();

        }
        private void _disableTxtFilterBy()
        {
            cmbIsActive.Visible = false;
            if (cmbFilterBy.SelectedItem == "None")
            {
                txtFilterBy.Visible = false;
            }
            else
            {
                txtFilterBy.Visible = true;
            }
            if (cmbFilterBy.SelectedItem == "IsActive")
            {
                txtFilterBy.Visible = false;
                cmbIsActive.Visible = true;
                
            }
        }

        private void _RefreshRecordsCount()
        {
            int numberOfDataRows = dvgAllUser.Rows.Count;

            lblCount.Text = numberOfDataRows.ToString();
        }
        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            _RefreshUsersList();
             cmbFilterBy.SelectedIndex = 0;
            _disableTxtFilterBy();
            _RefreshRecordsCount();
        }

        private void cmbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterBy.Text = "";
            _disableTxtFilterBy();
            _RefreshRecordsCount();
        }

        private void cmbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbIsActive.SelectedItem == "Yes")
            {
                string OrderBy = cmbFilterBy.SelectedItem.ToString();
                dvgAllUser.DataSource = clsUser.GetAllUsers(OrderBy, "Yes");
            }
            if(cmbIsActive.SelectedItem == "No")
            {
                string OrderBy = cmbFilterBy.SelectedItem.ToString();
                dvgAllUser.DataSource = clsUser.GetAllUsers(OrderBy, "No");
            }
            if(cmbFilterBy.SelectedItem == "All")
            {
                dvgAllUser.DataSource = clsUser.GetAllUsers();
            }
            _RefreshRecordsCount();
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            if (cmbFilterBy.SelectedItem != "None")
            {
                string OrderBy = cmbFilterBy.SelectedItem.ToString();
                dvgAllUser.DataSource =clsUser.GetAllUsers(OrderBy, txtFilterBy.Text);
            }
            else
            {
                dvgAllUser.DataSource = clsUser.GetAllUsers();

            }
            _RefreshRecordsCount();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilterBy.SelectedItem == "PersonID" || cmbFilterBy.SelectedItem == "UserID")
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddNewUser frm = new frmAddNewUser(-1);
            frm.ShowDialog();
            _RefreshUsersList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete User [" + dvgAllUser.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)

            {

                if (clsUser.DeleteUser((int)dvgAllUser.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("User Deleted Successfully.");
                    _RefreshUsersList();
                }

            }

            else
                MessageBox.Show("User is not deleted Because it has Relations.");

            
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddNewUser frm = new frmAddNewUser((int)dvgAllUser.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshUsersList();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo frm = new frmUserInfo((int)dvgAllUser.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshUsersList();
        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddNewUser frm = new frmAddNewUser(-1);
            frm.ShowDialog();
           _RefreshUsersList();
        }
        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword((int)dvgAllUser.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshUsersList();
        }
        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Send Email function in Progress...");
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Phone Call function in Progress...");
        }


    }
}
