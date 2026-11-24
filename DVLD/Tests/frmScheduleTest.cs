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
    public partial class frmScheduleTest : Form
    {
        public enum enMode { Addnew = 1, Update = 2 }
        private enMode _Mode = enMode.Addnew;

        enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 }
        enTestType _enTestType = enTestType.VisionTest;

        int _LDLAppID = -1;
        int _TestAppointmentID = -1;
        bool _RetakeTest = false;
        public frmScheduleTest(int LDLAppID,int TestType,bool RetakeTest)
        {
            InitializeComponent();
            _LDLAppID = LDLAppID;
            _enTestType = (enTestType)TestType;
            ctrlScheduleTest1.TestType = (int)_enTestType;
            _Mode = enMode.Addnew;
            _RetakeTest = RetakeTest;
            ctrlScheduleTest1.OnSaveComplete += () => this.Close();
        }

        public frmScheduleTest(int LDLAppID, int TestType,int TestAppointmentID)
        {
            InitializeComponent();
            _LDLAppID = LDLAppID;
            _enTestType = (enTestType)TestType;
            ctrlScheduleTest1.TestType = (int)_enTestType;
            _TestAppointmentID= TestAppointmentID;
            _Mode = enMode.Update;
            ctrlScheduleTest1.OnSaveComplete += () => this.Close();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {

            if (_Mode == enMode.Addnew)
            {
                ctrlScheduleTest1.Load_LDL_Info(_LDLAppID, _RetakeTest);
                
            }
            else
            {
                ctrlScheduleTest1._LoadListTestAppointmentsData(_TestAppointmentID);
                
            }
        }
    }
}
