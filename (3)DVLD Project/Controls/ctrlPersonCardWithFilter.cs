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

namespace _3_DVLD_Project.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }


        int _PersonID=-1;
        
        public int ctrlPersonID
        {
            get { return _PersonID; }
        }

        public void LoadPersonInfo(int PersonID)
        {
            _PersonID = PersonID;

            gbFilter.Enabled = false;
            _RefreshPersonID();
        }
        public void DisableFilter()
        {
            gbFilter.Enabled = false;
        }
        private void _RefreshPersonID()
        {
            if (_PersonID != -1)
            {
                cmbFindBy.SelectedItem = "Person ID";
                txtFindBy.Text = _PersonID.ToString();
                ctrlPersonCard1.LoadPersonInfo(_PersonID);
                _PersonID = ctrlPersonCard1.ctrlPersonID;
            }
        }


        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cmbFindBy.SelectedIndex = 0;
            _RefreshPersonID();

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFindBy.SelectedItem == "Person ID" )
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

        private void btnAdd_Click(object sender, EventArgs e)
        {

            frmAddEditPersonInfo frm = new frmAddEditPersonInfo(-1);
            frm.DataBack += Form2_DataBack;
            frm.ShowDialog();
            
        }
        private void Form2_DataBack(object sender, int PersonID)
        {
            _PersonID = PersonID;
            _RefreshPersonID();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(cmbFindBy.SelectedItem =="Person ID")
            {
                int PersonID = Convert.ToInt32(txtFindBy.Text);
                
                if (clsPerson.isPersonExist(PersonID))
                {
                    ctrlPersonCard1.LoadPersonInfo(PersonID);

                }
                else
                {
                    ctrlPersonCard1.LoadPersonInfo(PersonID);
                    MessageBox.Show("No Person with Person ID="+PersonID.ToString(),
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

                }
           
            }

           if(cmbFindBy.SelectedItem == "National No.")
           {
               string NationalNo = txtFindBy.Text;
             
               if(clsPerson.isPersonExist(NationalNo))
               {
                   ctrlPersonCard1.LoadPersonInfo(NationalNo);

               }
               else
               {
                   ctrlPersonCard1.LoadPersonInfo(NationalNo);
                   MessageBox.Show("No Person with National No=" + NationalNo,
               "Error",
               MessageBoxButtons.OK,
               MessageBoxIcon.Error);
      
               }
           }

            _PersonID = ctrlPersonCard1.ctrlPersonID;
        }


    }
}
