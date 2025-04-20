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
    public partial class frmUpdateApplicationType : Form
    {
        int _ApplicationTypeID;
        clsApplicationTypes _ApplicationType;
        public frmUpdateApplicationType(int ApplicationTypeID)
        {
            InitializeComponent();

            _ApplicationTypeID = ApplicationTypeID;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _ApplicationType.ApplicationTypeTitle = txtApplicationTypeTitle.Text;
            _ApplicationType.ApplicationFees = decimal.Parse(txtApplicationTypeFees.Text);

            if(_ApplicationType.Save())
            {
                MessageBox.Show("Data saves successfully!");

            }
            else
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {
           _ApplicationType= clsApplicationTypes.Find(_ApplicationTypeID);
            if( _ApplicationType == null )
            {
                MessageBox.Show("Error this Application not found in DataBase Contact the admin!");
                this.Close();
            }
            lblID.Text = _ApplicationType.ApplicationTypeID.ToString();

            txtApplicationTypeFees.Text =  _ApplicationType.ApplicationFees.ToString();

            txtApplicationTypeTitle.Text = _ApplicationType.ApplicationTypeTitle;
        }
    }
}
