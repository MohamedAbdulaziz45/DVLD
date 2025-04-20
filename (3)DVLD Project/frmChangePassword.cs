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
    public partial class frmChangePassword : Form
    {
        clsUser _User;
        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _User = clsUser.Find(UserID);
            ctrlUserCard1.LoadUserInfo(UserID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool _TxtCurrentPassValidateRight()
        {
            bool validated = true;
            if (txtCurrentPass.Text != _User.Password)
            {
                validated = false;
                errorProvider1.SetError(txtCurrentPass, "Wrong User Password. Try Again!");
            }
            else
            {
                validated = true;
                errorProvider1.SetError(txtCurrentPass, "");
            }
            return validated;
        }
        private bool _txtCurrentPassValidate()
        {
            bool validated = true;
            if(string.IsNullOrWhiteSpace(txtCurrentPass.Text))
            {
                validated = false;
                errorProvider1.SetError(txtCurrentPass, "empty field!");
            }
            else
            {
                validated = true;
                errorProvider1.SetError(txtCurrentPass, "");
            }
            return validated;
        }
        private void txtCurrentPass_Validating(object sender, CancelEventArgs e)
        {
           if( _txtCurrentPassValidate())
            {
                _TxtCurrentPassValidateRight();
            }
        }

        private bool _txtNewPasswordValidate()
        {
            bool validated = true;
            if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                validated = false;
                errorProvider1.SetError(txtNewPassword, "empty field!");
            }
            else
            {
                validated = true;
                errorProvider1.SetError(txtNewPassword, "");
            }
            return validated;
        }
        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            _txtNewPasswordValidate();
        }

        private bool _txtConfirmPasswordValidate()
        {
            bool validated = true;
            if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                validated = false;
                errorProvider1.SetError(txtConfirmPassword, "empty field!");
            }
            else
            {
                validated = true;
                errorProvider1.SetError(txtNewPassword, "");
            }
            return validated;
        }
        private bool _txtConfirmPasswordValidateRight()
        {
            bool validated = true;
            if (txtConfirmPassword.Text != txtNewPassword.Text)
            {
                validated = false;
                errorProvider1.SetError(txtConfirmPassword, "Wrong Password try again!");
            }
            else
            {
                validated = true;
                errorProvider1.SetError(txtNewPassword, "");
            }
            return validated;
        }
        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if(_txtConfirmPasswordValidate())
            {
                _txtConfirmPasswordValidateRight();
            }
        }

        private bool _ValidateAll()
        {
            bool validated = true;


            if (!_txtCurrentPassValidate() || !_TxtCurrentPassValidateRight() || !_txtNewPasswordValidate()
                || !_txtConfirmPasswordValidate()|| !_txtConfirmPasswordValidateRight())
            {
                validated = false;
            }
            else
            {
                validated = true;
            }

            return validated;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!_ValidateAll())
            {
                MessageBox.Show("Correct your application.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _User.Password = txtNewPassword.Text;

                if (_User.Save())
                    MessageBox.Show("Data Saved Successfully.");
                else
                    MessageBox.Show("Error: Data Is not Saved Successfully.");
            }
        }
    }
}
