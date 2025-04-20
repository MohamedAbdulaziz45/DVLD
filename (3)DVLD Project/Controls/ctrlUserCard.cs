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
    public partial class ctrlUserCard : UserControl
    {
        public ctrlUserCard()
        {
            InitializeComponent();
        }
        clsUser _User;
        public void LoadUserInfo(int UserID)
        {
            _User = clsUser.Find(UserID);
            ctrlPersonCard1.LoadPersonInfo(_User.PersonID);
            lblUserID.Text =_User.UserID.ToString();
            lblUsername.Text = _User.UserName.ToString();
            if(_User.IsActive == true) 
                lblIsActive.Text = "yes";
            else
                lblIsActive.Text = "no";
        }
        private void ctrlUserCard_Load(object sender, EventArgs e)
        {

        }
    }
}
