using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLDBusinessLayer;

namespace DVLD
{
    public partial class frmManagePeople : Form
    {
        public frmManagePeople()
        {
            InitializeComponent();

        }
        private static DataTable _dtAllPeople = clsPerson.GetAllPeople();

        public clsPerson PersonInfo;
        private DataTable _dtManagePeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                      "FirstName", "SecondName", "ThirdName", "LastName",
                                                      "GendorCaption", "DateOfBirth", "CountryName",
                                                      "Phone", "Email");


        private void _RefreshPeopleList()
        {

           _dtAllPeople = clsPerson.GetAllPeople();
            _dtManagePeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GendorCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");

            dgvPeople.DataSource = _dtManagePeople;
            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();

        }

        private void ApplyFilter()
        {
            string filterColumn = cbFilterBy.SelectedItem.ToString();
            string searchValue = txtFilterBy.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchValue))
            {
                _RefreshPeopleList();
                return;
            }

            string filterExpression = "";

            switch (filterColumn)
            {
                case "Person ID":
                    filterExpression = $"PersonID = {searchValue}";
                    break;
                case "National No":
                    filterExpression = $"NationalNo LIKE '%{searchValue}%'";
                    break;
                case "First Name":
                    filterExpression = $"FirstName LIKE '%{searchValue}%'";
                    break;
                case "Last Name":
                    filterExpression = $"LastName LIKE '%{searchValue}%'";
                    break;
                case "Gender":
                    filterExpression = $"GendorCaption LIKE '%{searchValue}%'";
                    break;
                case "Country":
                    filterExpression = $"CountryName LIKE '%{searchValue}%'";
                    break;
                case "Phone":
                    filterExpression = $"Phone LIKE '%{searchValue}%'";
                    break;
                case "Email":
                    filterExpression = $"Email LIKE '%{searchValue}%'";
                    break;
                default:
                    filterExpression = "None";
                    break;

            }

            if (txtFilterBy.Text.Trim() == "" || filterExpression == "None")
            {
                _dtManagePeople.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
                return;
            }

            
            _dtManagePeople.DefaultView.RowFilter = filterExpression;

            dgvPeople.DataSource = _dtManagePeople;
            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ApplyFilter();
            txtFilterBy.Visible = (cbFilterBy.Text != "None");

            if (txtFilterBy.Visible)
            {
                txtFilterBy.Text = "";
                txtFilterBy.Focus();

            }
            _RefreshPeopleList();
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.SelectedItem.ToString() == "Person ID")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }


        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            // Initialize filter controls
            cbFilterBy.Items.AddRange(new object[] {"Person ID",
                                                    "National No",
                                                    "First Name",
                                                    "Last Name",
                                                    "Gender",
                                                    "Country",
                                                    "Phone",
                                                    "Email"
                                                   });

            cbFilterBy.SelectedIndex = 0;

            // Set up event handlers
            //cbFilterBy.SelectedIndexChanged += cbFilterBy_SelectedIndexChanged;
            //txtFilterBy.TextChanged += txtFilterBy_TextChanged;
            //txtFilterBy.KeyPress += txtFilterBy_KeyPress;


            _RefreshPeopleList();
        }


        private void btnAddPeople_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            _RefreshPeopleList();
        }

        private void tsmShowDetails_Click(object sender, EventArgs e)
        {
           
            if (dgvPeople.SelectedRows.Count > 0)
            {
                int personID = Convert.ToInt32(dgvPeople.SelectedRows[0].Cells["_PersonID"].Value);
                Form frmShowDetails = new frmShowPersonInfo(personID);
                frmShowDetails.ShowDialog();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Close();
            }
        }

        private void tsmAddNewPerson_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            _RefreshPeopleList();
        }

        private void tsmEdit_Click(object sender, EventArgs e)
        {

            if (dgvPeople.SelectedRows.Count > 0)
            {
                int personID = Convert.ToInt32(dgvPeople.SelectedRows[0].Cells["_PersonID"].Value);
                Form frm = new frmAddUpdatePerson(personID);
                frm.ShowDialog();
                _RefreshPeopleList();
            }
        }

        private void tsmDelete_Click(object sender, EventArgs e)
        {
            int personID = Convert.ToInt32(dgvPeople.SelectedRows[0].Cells["_PersonID"].Value);
            PersonInfo = clsPerson.Find(personID);

            if (PersonInfo == null)
            {
                return;
            }

            string imagePath = PersonInfo.ImagePath;

            if (clsPerson.DeletePerson(personID))
            {
                try
                {
                    string fullOldPath = Path.Combine(@"C:\SavedPersonImages", imagePath);
                    if (File.Exists(fullOldPath))
                    {
                        File.Delete(fullOldPath);
                    }

                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Error deleting old image: {ex.Message}");

                }

            }
            else
            {
                MessageBox.Show("Didnt delete DATA");
            }

            _RefreshPeopleList();
            
        }

       
    }
}