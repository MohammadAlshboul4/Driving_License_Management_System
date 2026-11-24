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
    public partial class frmListTestTypes : Form
    {
        public frmListTestTypes()
        {
            InitializeComponent();
        }

        
        DataTable _dtTestTypeList;


        void _RefreshTestTypesList()
        {
            _dtTestTypeList = clsTestType.GetAllTestTypesList();

            dgvTestTypes.DataSource = _dtTestTypeList;

            if (dgvTestTypes.Columns.Count >= 3)
            {
                dgvTestTypes.Columns[0].HeaderText = "ID";
                dgvTestTypes.Columns[0].Width = 110;

                dgvTestTypes.Columns[1].HeaderText = "Title";
                dgvTestTypes.Columns[1].Width = 200;

                dgvTestTypes.Columns[2].HeaderText = "Description";
                dgvTestTypes.Columns[2].Width = 350;

                dgvTestTypes.Columns[3].HeaderText = "Fees";
                dgvTestTypes.Columns[3].Width = 110;
            }
            // 1. Set the default font for all data cells/rows
            dgvTestTypes.DefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);

            // 2. Set the font for the column headers
            dgvTestTypes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            // 3. Set the font for the row headers (the blank gray boxes on the far left)
            dgvTestTypes.RowHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Regular);

            lblRecordsCount.Text = dgvTestTypes.SelectedRows.Count.ToString();
        }
        private void frmListTestTypes_Load(object sender, EventArgs e)
        {

            _RefreshTestTypesList();

            
        }

        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestTypeID = Convert.ToInt32(dgvTestTypes.SelectedRows[0].Cells["TestTypeID"].Value);
            frmEditTestType frm = new frmEditTestType((clsTestType.enTestType)TestTypeID);
            frm.ShowDialog();
            _RefreshTestTypesList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
