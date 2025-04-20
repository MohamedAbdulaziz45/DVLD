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
    public partial class frmPersonDetails : Form
    {

        public enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode;

        int _PersonID;
        clsPerson _Person;



        public frmPersonDetails(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;
            if (_PersonID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;
        }
        private void _LoadData()
        {

            _Person = clsPerson.Find(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show("This form will be closed because No Contact with ID = " + _PersonID);

                this.Close();

                return;
            }

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPersonDetails_Load(object sender, EventArgs e)
        {
            _LoadData();
            ctrlPersonCard1.LoadPersonInfo(_PersonID);
        }
    }
}
