using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmApplicationsTypeslList : Form
    {
       
        private DataTable dtAllApplicationTypes;

        public frmApplicationsTypeslList()
        {
            InitializeComponent();
        }

        private void _RefreshApplicationTypesList()
        {
            // 1. Fetch ALL data from Business Layer
             dtAllApplicationTypes = clsApplicationsType.GetAllApplicationTypes();

           
            dgvApplicationTypes.DataSource = dtAllApplicationTypes.DefaultView;

           
            lblRecordsCount.Text = dgvApplicationTypes.RowCount.ToString();
        }

        private void frmManageApplicationsTypes_Load(object sender, EventArgs e)
        {
            // Call the refresh method to load and bind data cleanly
            _RefreshApplicationTypesList();

            // Setup column styling once during load
            if (dgvApplicationTypes.Columns.Count >= 3)
            {
                dgvApplicationTypes.Columns[0].HeaderText = "ID";
                dgvApplicationTypes.Columns[0].Width = 110;

                dgvApplicationTypes.Columns[1].HeaderText = "Title";
                dgvApplicationTypes.Columns[1].Width = 350;

                dgvApplicationTypes.Columns[2].HeaderText = "Fees";
                dgvApplicationTypes.Columns[2].Width = 110;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ApplicationTypeID = Convert.ToInt32(dgvApplicationTypes.SelectedRows[0].Cells["ApplicationTypeID"].Value);
            frmUpdateApplicationType frm = new frmUpdateApplicationType(ApplicationTypeID);
            frm.ShowDialog();
            _RefreshApplicationTypesList();
        }
    }
}
