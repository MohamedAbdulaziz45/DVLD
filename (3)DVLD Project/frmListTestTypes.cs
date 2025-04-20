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
    public partial class frmManageTestTypes : Form
    {
        public frmManageTestTypes()
        {
            InitializeComponent();
        }
        private void _RefreshRecordsCount()
        {
            int numberOfDataRows = dgvTestTypes.Rows.Count;

            lblCount.Text = numberOfDataRows.ToString();
        }
        private void _RefreshApplicationTypeList()
        {
            dgvTestTypes.DataSource = clsTestTypes.GetAllTestTypes();
            dgvTestTypes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTestTypes.Columns[0].Width = 75;
            dgvTestTypes.Columns[1].Width = 150;
            dgvTestTypes.Columns[2].Width = 400;
            _RefreshRecordsCount();

        }
        private void frmManageTestTypes_Load(object sender, EventArgs e)
        {
            _RefreshApplicationTypeList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUpdateTestType frm = new frmUpdateTestType((int)dgvTestTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshApplicationTypeList();
        }
    }
}
