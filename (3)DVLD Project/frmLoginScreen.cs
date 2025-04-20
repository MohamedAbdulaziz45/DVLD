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
using System.IO;

namespace _3_DVLD_Project
{
    public partial class frmLoginScreen : Form
    {
        clsUser _User;
        string _UserFile = @"E:\programming advices\course 19\(3)DVLD Project\UserFile.txt";
        public frmLoginScreen()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool isUserExist()
        {
            bool validated = false;
            string UserName = txtUserName.Text;

            _User = clsUser.Find(UserName);
            if(_User != null )
            {
                validated =  true;
            }
            else
            {
                validated = false;
            }
           return validated;
        }

        private bool isPasswordRight()
        {
            bool validated = false;

            if( isUserExist() )
            {
                if( txtPassword.Text == _User.Password)
                {
                    validated = true;
                }
                else
                {
                    validated = false;
                }
            }
           return validated ;
        }

        private bool _isUserActive()
        {
            bool validated = false;
       
                    if(_User.IsActive == true)
                    {
                        validated = true;
                    }
  
            return validated;
        }
        private bool _isUserALlowedToLogin()
        {
           return isPasswordRight();
        }

        private void _isUserSavedInFile()
        {
            if(chkRememberMe.Checked==true)
            {
                string txt = _User.UserID.ToString();
                File.WriteAllText(_UserFile, txt);
            }
            else
            {
                string txt = "";
                File.WriteAllText(_UserFile, txt);
            }
        }
        private void _SaveUserInGlobalClass()
        {
            if(_User != null)
            {
                GlobalClass.CurrentUser = _User;
            }
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(_isUserALlowedToLogin())
            {
                if (_isUserActive())
                {
                    _SaveUserInGlobalClass();
                    _isUserSavedInFile();
                    Form frm = new frmMainMenue();
                    frm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("the User you entered is not ACTIVE!", "Wrong credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid Username/Password!", "Wrong credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmLoginScreen_Load(object sender, EventArgs e)
        {
          string RetreivedText = File.ReadAllText(_UserFile);

          if( RetreivedText.Length > 0 )
          {
              int UserID = Convert.ToInt32(RetreivedText);
              _User = clsUser.Find(UserID);
              txtUserName.Text = _User.UserName;
              txtPassword.Text = _User.Password;
              chkRememberMe.Checked = true;
                return;
          }
            txtUserName.Text = "";
            txtPassword.Text = "";
            chkRememberMe.Checked = false;
        }
    }
}
