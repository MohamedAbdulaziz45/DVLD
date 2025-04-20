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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;

namespace _3_DVLD_Project
{
    public partial class frmManagePeople : Form
    {
        public frmManagePeople()
        {
            InitializeComponent();
        }

        private void _disableTxtFilterBy()
        {
            if (cmbFilterBy.SelectedItem =="None") 
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
            int numberOfDataRows = dvgAllPeople.Rows.Count;

            lblCount.Text = numberOfDataRows.ToString();
        }
        private void _RefreshPeopleList()
        {
            dvgAllPeople.DataSource = clsPerson.GetAllPeople();

        }



        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            cmbFilterBy.SelectedIndex = 0;
            _disableTxtFilterBy();
            _RefreshPeopleList();
            _RefreshRecordsCount();
        }

        private void cmbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterBy.Text = "";
            _disableTxtFilterBy();
            _RefreshRecordsCount();
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            if (cmbFilterBy.SelectedItem != "None")
            {
                string OrderBy = cmbFilterBy.SelectedItem.ToString();
                dvgAllPeople.DataSource = clsPerson.GetAllPeople(OrderBy, txtFilterBy.Text);
            }
            else
            {
                dvgAllPeople.DataSource = clsPerson.GetAllPeople();

            }
            _RefreshRecordsCount();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilterBy.SelectedItem == "PersonID")
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
            frmAddEditPersonInfo frm = new frmAddEditPersonInfo(-1);
            frm.ShowDialog();
            _RefreshPeopleList();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails((int)dvgAllPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshPeopleList();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditPersonInfo frm = new frmAddEditPersonInfo(-1);
            frm.ShowDialog();
            _RefreshPeopleList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditPersonInfo frm = new frmAddEditPersonInfo((int)dvgAllPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshPeopleList();
        }

        private void _DeleteOldPicture(string OldPath)
        {
            if (File.Exists(OldPath))
            {
                File.Delete(OldPath);
            }

        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete Person [" + dvgAllPeople.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)

            {
                clsPerson _Person = clsPerson.Find((int)dvgAllPeople.CurrentRow.Cells[0].Value);
                string OldPath = _Person.ImagePath;
                //Perform Delele and refresh
                if (clsPerson.DeletePerson((int)dvgAllPeople.CurrentRow.Cells[0].Value))
                {
                    _DeleteOldPicture(_Person.ImagePath);
                    MessageBox.Show("Person Deleted Successfully.");
                    _RefreshPeopleList();
                }
                else
                    MessageBox.Show("Person is not deleted cause it has Relations .");

            }
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Send Email function in Progress...");
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Phone Call Function in Progress...");
        }


    }
}
