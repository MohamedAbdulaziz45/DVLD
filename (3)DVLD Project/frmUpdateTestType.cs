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
    public partial class frmUpdateTestType : Form
    {
        int _TestTypeID;
        clsTestTypes _TestType;
        public frmUpdateTestType(int TestTypeID)
        {
            InitializeComponent();

            _TestTypeID = TestTypeID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmUpdateTestType_Load(object sender, EventArgs e)
        {
            _TestType = clsTestTypes.Find(_TestTypeID);
            if( _TestType == null ) 
            {

                MessageBox.Show("Error this Test not found in DataBase Contact the admin!");
                this.Close();
            }

            lblID.Text = _TestType.TestTypeID.ToString();

            txtTestTypeFees.Text = _TestType.TestTypeFees.ToString();
            txtTestTypeDescription.Text = _TestType.TestTypeDescription;
            txtTestTypeTitle.Text = _TestType.TestTypeTitle;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _TestType.TestTypeTitle = txtTestTypeTitle.Text;
            _TestType.TestTypeFees = decimal.Parse(txtTestTypeFees.Text);
            _TestType.TestTypeDescription = txtTestTypeDescription.Text;

            if (_TestType.Save())
            {
                MessageBox.Show("Data saves successfully!");

            }
            else
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.");
            }
        }
    }
}
