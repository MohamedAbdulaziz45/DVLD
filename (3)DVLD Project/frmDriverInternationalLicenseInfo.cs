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
    public partial class frmDriverInternationalLicenseInfo : Form
    {
        public frmDriverInternationalLicenseInfo(int InternationalLicenseID)
        {
            InitializeComponent();

            ctrlDriverInternationalLicenseInfo1.LoadInternationalLicenseInfo(InternationalLicenseID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
