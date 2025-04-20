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
    public partial class frmManageApplicationTypes : Form
    {
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }
        private void _RefreshRecordsCount()
        {
            int numberOfDataRows = dgvApplicationTypes.Rows.Count;

            lblCount.Text = numberOfDataRows.ToString();
        }

        private void _RefreshApplicationTypeList()
        {
            dgvApplicationTypes.DataSource = clsApplicationTypes.GetAllApplicationTypes();
            dgvApplicationTypes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvApplicationTypes.Columns[0].Width = 100;
            dgvApplicationTypes.Columns[1].Width = 250;
            _RefreshRecordsCount();

        }
        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            _RefreshApplicationTypeList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUpdateApplicationType frm = new frmUpdateApplicationType((int)dgvApplicationTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshApplicationTypeList();
        }
    }
}
