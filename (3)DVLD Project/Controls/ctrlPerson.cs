using BussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace _3_DVLD_Project.Controls
{
    public partial class ctrlPerson : UserControl
    {
        // Define a custom event handler delegate with parameters
        public event Action<int> OnSaveComplete;

        // Create a protected method to raise the event with parameter

        protected virtual void SaveComplete(int Result)
        {
            Action<int> handler = OnSaveComplete;
            if (handler != null)
            {
                handler(Result);//Raise the event with parameter
            }
        }

 
    

        public delegate void DataBackEventHandler(object sender);

        //declare an event using the delegate 
        public event DataBackEventHandler onCloseClick;

       // public event DataBackEventHandler OnSaveClick;

        public enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode;

        int _PersonID;
        clsPerson _Person;

        string _SourcePath="";
        string _PictureName="";
        bool _isOpenFileDialog = false;

        public int ctrlPersonID
        {
            get { return _Person.ID; }
        }

        public ctrlPerson()
        {
            InitializeComponent();
        }

        public void LoadPersonInfo(int PersonID)
        {
            _PersonID = PersonID;

            if (_PersonID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;

            _LoadData();
        }

        private void _FillCountriesInComoboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();

            foreach (DataRow row in dtCountries.Rows)
            {

                cbCountry.Items.Add(row["CountryName"]);

            }

        }

        private void _ChangePersonImageWithGender()
        {
            if(Convert.ToInt32(pbPersonImage.Tag)==0)
            {
                //if (pbPersonImage.Image != null)
                //{
                //    pbPersonImage.Image.Dispose();
                //    pbPersonImage.Image = null;
                //}

                pbPersonImage.SizeMode = PictureBoxSizeMode.StretchImage;
                if (rbMale.Checked)
                {
                    pbPersonImage.Image = Properties.Resources.person_man;
                }
                else
                {
                    pbPersonImage.Image= Properties.Resources.person_woman;
                }
            }
        }
        private void _LoadData()
        {

            _FillCountriesInComoboBox();
            _ChangePersonImageWithGender();


            cbCountry.SelectedItem = "Jordan";
            if (_Mode == enMode.AddNew)
            {
                dtpDateOfBirth.MaxDate= DateTime.Now.AddYears(-18);
                lklRemoveImage.Visible= false;
                _Person = new clsPerson();
                return;
            }

            _Person = clsPerson.Find(_PersonID);

            if (_Person == null)
            {
             //   MessageBox.Show("This form will be closed because No Contact with ID = " + _PersonID);
                _Person = new clsPerson();

                return;
            }



            // lblMode.Text = "Edit Contact ID = " + _ContactID;
            //lblContactID.Text = _ContactID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtNationalNo.Text = _Person.NationalNo;

            txtEmail.Text = _Person.Email;
            txtPhone.Text = _Person.Phone;
            txtAddress.Text = _Person.Address;
            dtpDateOfBirth.Value = _Person.DateOfBirth;

            if (_Person.ImagePath != "")
             {
                pbPersonImage.Load(_Person.ImagePath);
             }

            lklRemoveImage.Visible = (_Person.ImagePath !="");

            cbCountry.SelectedIndex = cbCountry.FindString(clsCountry.Find(_Person.NationalityCountryID).CountryName);
    
        }

        private void ctrlPerson_Load(object sender, EventArgs e)
        {
            _LoadData();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {

            onCloseClick?.Invoke(this);

        }

        private void _DeleteOldPicture(string OldPath)
        {
            if (File.Exists(OldPath))
            {
                string TempFile = @"E:\programming advices\course 19\(2)Images\Personal Pictures\TempFile\" + _PictureName;
                File.Move(OldPath, TempFile);
                File.Delete(TempFile);
            }
            
        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (!_ValidateAll())
            {
                MessageBox.Show("Some Fields Are not Valid!,Put the mouse over the Red Icon(s) to see the error.","Validation Error",MessageBoxButtons.OK,MessageBoxIcon.Error);

            }
            else
            {
                int CountryID = clsCountry.Find(cbCountry.Text).ID;

                _Person.NationalNo = txtNationalNo.Text;
                _Person.FirstName = txtFirstName.Text;
                _Person.SecondName = txtSecondName.Text;
                _Person.ThirdName = txtThirdName.Text;
                _Person.LastName = txtLastName.Text;
                _Person.Email = txtEmail.Text;
                _Person.Phone = txtPhone.Text;
                _Person.Address = txtAddress.Text;
                _Person.DateOfBirth = dtpDateOfBirth.Value;
                _Person.NationalityCountryID = CountryID;

                if (rbMale.Checked)
                {
                    _Person.Gendor = 0;
                }
                else
                {
                    _Person.Gendor = 1;
                }
                if (pbPersonImage.ImageLocation != null&& _isOpenFileDialog == true)
                {
                    _DeleteOldPicture(_Person.ImagePath);
                    _Person.ImagePath = _CopyPasteAndRenamePicture(_SourcePath, _PictureName);
                
                }
                
                if(pbPersonImage.ImageLocation ==null)
                {

                    _Person.ImagePath = "";
                }

                if (_Person.Save())
                {
                    MessageBox.Show("Data Saved Successfully.");
                    if (_Person.ImagePath == "")
                    {
                        _DeleteOldPicture(_Person.ImagePath);
                    }
                }
                else
                    MessageBox.Show("Error: Data Is not Saved Successfully.");

                _Mode = enMode.Update;

           
                if(OnSaveComplete != null)
                {
                    SaveComplete(_Person.ID);
                }
            }

        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            _ChangePersonImageWithGender();
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            _ChangePersonImageWithGender();
        }

        private bool _ValidateAll()
        {
            bool validated = true;

          
            if (!_TxtEmailValidate() || !_TxtAddressValidate() || !_TxtFirstNameValidate()
                || !_TxtLastNameValidate() || !_TxtSecondNameValidate() || !_TxtNationNoUniqnessValidate()
                || !_TxtNationNoValidate() || !_TxtPhoneValidate())
            {
                validated = false;
            }
            else
            {
                validated = true;
            }

            return validated;
        }

        private bool _TxtNationNoUniqnessValidate()
        {
            bool validated = true;
            if (_Mode == enMode.AddNew)
            {
                if (clsPerson.isPersonExist(txtNationalNo.Text))
                {
                    validated = false;
                    errorProvider1.SetError(txtNationalNo, "National Number is used for another person!");
                }
                else
                {
                    validated = true;
                    errorProvider1.SetError(txtNationalNo, "");
                }
            }

            return validated;
        }

        private bool _TxtNationNoValidate()
        {
            bool validated = true;

            if(string.IsNullOrWhiteSpace(txtNationalNo.Text))
            {
                validated = false;
                errorProvider1.SetError(txtNationalNo, "National No. should have a value!");
            }
            else
            {
                validated = true;
                errorProvider1.SetError(txtNationalNo, "");
            }
            return validated;
        }
        private bool _TxtEmailValidate()
        {
            string email = txtEmail.Text;

            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.com$";
            bool validated = true;

            if (!Regex.IsMatch(email, emailPattern) && !string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                validated = false;
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format!");
            }
            else
            {
                validated = true;
                errorProvider1.SetError(txtEmail, "");
            }
            return validated;
        }


        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if(_TxtNationNoValidate())
            {
                _TxtNationNoUniqnessValidate();
            }
        }



        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {

            _TxtEmailValidate();
        }



        private void lklRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;

            //Delete Image from File that i made in Progress...

            lklRemoveImage.Visible = false;

            _ChangePersonImageWithGender();
        }

        private string _CopyPasteAndRenamePicture(string SourcePath,string PictureName)
        {
            string DestinationFilePath = @"E:\programming advices\course 19\(2)Images\Personal Pictures\" + PictureName;
            File.Copy(SourcePath, DestinationFilePath, true);
           
            Guid newGuid = Guid.NewGuid();
            string guidString = newGuid.ToString();

            SourcePath = DestinationFilePath;
            DestinationFilePath = @"E:\programming advices\course 19\(2)Images\Personal Pictures\" + guidString+".jpg";

            File.Move(SourcePath, DestinationFilePath);
           
            return DestinationFilePath;
        }
        private void lklOpenFileDialog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _isOpenFileDialog = true;
                // Process the selected file
                 _SourcePath = openFileDialog1.FileName;
                //MessageBox.Show("Selected Image is:" + selectedFilePath);
                 _PictureName = openFileDialog1.SafeFileName;
 
                
                pbPersonImage.Load(_SourcePath);
                // ...
                lklRemoveImage.Visible = true;
            }
        }

        private bool _TxtFirstNameValidate()
        {  
            bool validated = true;
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
             validated = false;
                errorProvider1.SetError(txtFirstName, "First Name should have a value!");
            }
            else
            {
                validated = true;
                errorProvider1.SetError(txtFirstName, "");
            }
            return validated;
        }
        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            _TxtFirstNameValidate();
        }

        private bool _TxtSecondNameValidate()
        {
            bool validated = true;
            if (string.IsNullOrWhiteSpace(txtSecondName.Text))
            {
                validated = false;
                errorProvider1.SetError(txtSecondName, "Second Name should have a value!");
            }
            else
            {
                validated = true;
                errorProvider1.SetError(txtSecondName, "");
            }
            return validated;
        }
        private void txtSecondName_Validating(object sender, CancelEventArgs e)
        {
            _TxtSecondNameValidate();
        }

        private bool _TxtLastNameValidate()
        {
            bool validated = true;
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                validated = false;
                errorProvider1.SetError(txtLastName, "Last Name should have a value!");
            }
            else
            {
                validated = true;
                errorProvider1.SetError(txtLastName, "");
            }
            return validated;
        }
        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            
            _TxtLastNameValidate();
        }

        private bool _TxtPhoneValidate()
        {
            bool validated = true;
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                validated = false;
                errorProvider1.SetError(txtPhone, "Phone Number should have a value!");
            }
            else
            {
                validated = true;
                errorProvider1.SetError(txtPhone, "");
            }
            return validated;
        }
        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            _TxtPhoneValidate();
        }

        private bool _TxtAddressValidate()
        {
            bool validated = true;
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                validated = false;
                errorProvider1.SetError(txtAddress, "Address should have a value!");
            }
            else
            {
                validated = true;
                errorProvider1.SetError(txtAddress, "");
            }
            return validated;
        }
        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            _TxtAddressValidate();
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
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
}
