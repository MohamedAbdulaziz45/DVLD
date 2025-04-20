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
    public partial class ctrlDriverLicenseWithFilter : UserControl
    {

        // Define a custom event handler delegate with parameters
        public event Action<int> OnSearchClick;

        // Create a protected method to raise the event with parameter

        protected virtual void SearchClick(int Result)
        {
            Action<int> handler = OnSearchClick;
            if (handler != null)
            {
                handler(Result);//Raise the event with parameter
            }
        }
        public ctrlDriverLicenseWithFilter()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int LicenseID = Convert.ToInt32(txtFindBy.Text);
            clsLicense license = clsLicense.Find(LicenseID);
            ctrlDriverLicenseInfo1.LoadDriverLicenseInfoByLicense(license);

            if (OnSearchClick != null)
            {
                SearchClick(LicenseID);
            }
        }

        public void LoadDriverLicenseInfoByLicense(int LicenseID)
        {
            clsLicense license = clsLicense.Find(LicenseID);
            ctrlDriverLicenseInfo1.LoadDriverLicenseInfoByLicense(license);
            txtFindBy.Text = LicenseID.ToString();
            if (OnSearchClick != null)
            {
                SearchClick(LicenseID);
            }
        }
        private void txtFindBy_KeyPress(object sender, KeyPressEventArgs e)
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
        public void DisableFilter(bool Disable)
        {
            gbFilter.Enabled = !Disable;
        }
    }
}
