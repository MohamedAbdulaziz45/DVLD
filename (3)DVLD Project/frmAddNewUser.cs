using _3_DVLD_Project.Controls;
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
using static System.Net.Mime.MediaTypeNames;

namespace _3_DVLD_Project
{
    public partial class frmAddNewUser : Form
    {
        public enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode;


        clsUser _User;
        int _PersonID=-1;
        int _UserID = -1;
        public frmAddNewUser(int UserID)
        {
            InitializeComponent();

            _UserID=UserID;
            if(_UserID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;
        }

        private void _LoadData()
        {
            if(_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New User";
                lblUserID.Text = "???";
                _User = new clsUser();
                return;
            }

            _User = clsUser.Find(_UserID);

            if(_User == null )
            {
                _User = new clsUser();
     
                return;
            }

            lblTitle.Text = "Update User";
            lblUserID.Text = _User.UserID.ToString();

            txtUserName.Text=_User.UserName;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = _User.Password;
            if(_User.IsActive == true) 
            {
                chkIsActive.Checked =true;
            }
            else
            {
                chkIsActive.Checked = false;
            }
           
           ctrlPersonCardWithFilter1.LoadPersonInfo(_User.PersonID);
            ctrlPersonCardWithFilter1.DisableFilter();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
           
            if(!_ValidateAll())
            {
                MessageBox.Show("Correct your application.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _User.UserName = txtUserName.Text;
                _User.Password = txtPassword.Text;
                _User.PersonID = ctrlPersonCardWithFilter1.ctrlPersonID;
                if(chkIsActive.Checked==true)
                {
                    _User.IsActive = true;
                }
                else
                {
                    _User.IsActive=false;
                }

                if (_User.Save())
                    MessageBox.Show("Data Saved Successfully.");
                else
                {
                    MessageBox.Show("Error: Data Is not Saved Successfully.");
                    return ;
                }
                _Mode = enMode.Update;
                lblUserID.Text = _User.UserID.ToString();
                lblTitle.Text = "Update User";
                ctrlPersonCardWithFilter1.DisableFilter();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      
        private void frmAddNewUser_Load(object sender, EventArgs e)
        {
          _LoadData();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(clsUser.isUserExistByPersonID(ctrlPersonCardWithFilter1.ctrlPersonID) && _Mode==enMode.AddNew)
            {
                MessageBox.Show("selected Person Already has a user Choose another one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(ctrlPersonCardWithFilter1.ctrlPersonID != -1)
            {
                tabControl1.SelectedTab = tbpLoginInfo;
                _PersonID=ctrlPersonCardWithFilter1.ctrlPersonID;
            }
            else
            {
                MessageBox.Show("you must Complete Personal Info.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void ctrlPersonCardWithFilter1_Load(object sender, EventArgs e)
        {
              _PersonID = -1;
        }

        private bool _TxtConfirmPasswordValidate()
        {
            bool validated = true;
            if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                validated = false;
                errorProvider1.SetError(txtConfirmPassword, "empty Field!");
            }
            else
            {
                validated = true;
                errorProvider1.SetError(txtConfirmPassword, "");
            }

            return validated;
        }
        private bool _TxtConfirmPasswordValidateRight()
        {
            bool validated = true;
            if (txtConfirmPassword.Text != txtPassword.Text)
            {
                validated = false;
                errorProvider1.SetError(txtConfirmPassword, "Wrong password Try Again!");
            }
            else
            {
                validated = true;
                errorProvider1.SetError(txtConfirmPassword, "");
            }

            return validated;
        }
        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (_TxtConfirmPasswordValidate())
            {
                _TxtConfirmPasswordValidateRight();
            }
        }

        private bool _TxtUserNameValidate()
        {
            bool validated = true;
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                validated = false;
                errorProvider1.SetError(txtUserName, "empty field!");
            }
            else
            {
                validated = true;
                errorProvider1.SetError(txtUserName, "");
            }

            return validated;
        }
        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            _TxtUserNameValidate();
        }
        private bool _TxtPasswordValidate()
        {
            bool validated = true;
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                validated = false;
                errorProvider1.SetError(txtPassword, "empty field!");
            }
            else
            {
                validated = true;
                errorProvider1.SetError(txtPassword, "");
            }

            return validated;
        }
        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
          _TxtPasswordValidate();   
        }

        private bool _IsPersonIsUsed()
        {
            bool validated = true;
          
            if (clsUser.isUserExistByPersonID(ctrlPersonCardWithFilter1.ctrlPersonID)&& _Mode == enMode.AddNew)
            {
                validated = false;
                MessageBox.Show("selected Person Already has a user. Choose another one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                validated = true;
              
            }
            return validated;
        }

        private bool _ValidateAll()
        {
            bool validated = true;


            if (!_TxtUserNameValidate() || !_TxtPasswordValidate() || !_TxtConfirmPasswordValidateRight()
                || !_IsPersonIsUsed()|| !_TxtConfirmPasswordValidate() )
            {
                validated = false;
            }
            else
            {
                validated = true;
            }

            return validated;
        }
    }
}
