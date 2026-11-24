using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsTestType
    {

        public enum enTestType { VisionTest = 1, WrittenTest = 2, Practical = 3 }

        public clsTestType.enTestType ID { set; get; }
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription { get; set; }
        public decimal TestTypeFees { get; set; }

        public clsTestType()
        {
            this.ID = clsTestType.enTestType.VisionTest;
            this.TestTypeTitle = "";
            this.TestTypeDescription = "";
            this.TestTypeFees = 0;
        }


        public clsTestType(clsTestType.enTestType id, string testTypeTitle, string testTypeDescription, decimal testTypeFees)
        {
            this.ID = id;
            this.TestTypeTitle = testTypeTitle;
            this.TestTypeDescription = testTypeDescription;
            this.TestTypeFees = testTypeFees;

        }


        public static DataTable GetAllTestTypesList()
        {
            return clsTestTypeData.GettAllTestTypesList();
        }



        public static bool UpdateTestTypes(int TestTypeID, string TestTypeTitle, string testTypeDescription, decimal TestTypeFees)
        {

            return clsTestTypeData.UpdateTestTypes(TestTypeID, TestTypeTitle, testTypeDescription, TestTypeFees);

        }


        public static clsTestType FindTestTypeInfoByID(clsTestType.enTestType testTypeID)
        {
            string testTypeTitle = "";
            string testTypeDescription = "";
            decimal testTypeFees = 0;

            if (clsTestTypeData.GetTestTypesInfoByTestTypeID((int)testTypeID, ref testTypeTitle, ref testTypeDescription, ref testTypeFees))
            {

                return new clsTestType(testTypeID, testTypeTitle, testTypeDescription, testTypeFees);
            }
            else
            {
                return null;
            }


        }

        public static clsTestType FindTestTypeInfoByTitle(string TestTypeTitle)
        {
            int ID = 1;
            string testTypeDescription = "";
            decimal testTypeFees = 0;

            if (clsTestTypeData.GetTestTypesInfoByTitle(TestTypeTitle, ref ID, ref testTypeDescription, ref testTypeFees))
            {

                return new clsTestType((clsTestType.enTestType)ID, TestTypeTitle, testTypeDescription, testTypeFees);
            }
            else
            {
                return null;
            }


        }

    }
}
