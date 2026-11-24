using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;



namespace DVLDBusinessLayer
{
    public class clsPerson
    {
       
       
        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; }

        }

        public string NationalNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public short Gendor { set; get; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }
        public string ImagePath { get; set; }
        public clsCountries CountryInfo;
        

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        // Default constructor for AddNew mode
        public clsPerson()
        {
            PersonID = -1;
            NationalNo = "";
            FirstName = "";
            SecondName = "";
            ThirdName = "";
            LastName = "";
            DateOfBirth = DateTime.Now;
            Email = "";
            Phone = "";
            Address = "";
            Gendor = 0;
            NationalityCountryID = -1;
            ImagePath = "";
            Mode = enMode.AddNew;
        }

        // Private constructor for loading existing records
        public clsPerson(int personID,string nationalNo, string firstName, string secondName, string thirdName,
                          string lastName, DateTime dateOfBirth, short gender,string email, string phone,
                          string address, int nationalityCountryID, string imagePath)
        {
            this.PersonID = personID;
            this.NationalNo = nationalNo;
            this.FirstName = firstName;
            this.SecondName = secondName;
            this.ThirdName = thirdName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Gendor = gender;
            this.Email = email;
            this.Phone = phone;
            this.Address = address;
            this.NationalityCountryID = nationalityCountryID;
            this.CountryInfo = clsCountries.Find(nationalityCountryID);
            this.ImagePath = imagePath;
            Mode = enMode.Update;
        }

        // Get all people
        public static DataTable GetAllPeople()
        {
            return clsPeopleData.GetAllPeople();
          
            
        }

         
        private bool _AddNewPerson()
        {
            this.PersonID = clsPeopleData.AddNewPerson(this.NationalNo, this.FirstName, this.SecondName, this.ThirdName, this.LastName, this.DateOfBirth,
         this.Gendor, this.Address, this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);



            return (this.PersonID != -1);
        }

        // Update existing person
        private bool _UpdatePerson()
        {
            return clsPeopleData.UpdatePerson(this.PersonID, this.NationalNo, this.FirstName, this.SecondName,
                this.ThirdName, this.LastName, this.DateOfBirth,
                this.Gendor, this.Address, this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);
        }
        
        // Delete person
        public static bool DeletePerson(int personID)
        {
            return clsPeopleData.DeletePerson(personID);
        }

        //Check if a person exists
        public static bool IsPersonExistByPersonID(int personID)
        {
            return clsPeopleData.IsPersonExistByPersonID(personID);
        }

        public static bool IsPersonExistByNationalNO(string NationalNo)
        {
            return clsPeopleData.IsPersonExistByNationalNO(NationalNo);
        }


        public static clsPerson Find(int PersonID)
        {
            string NationalNo = "", FirstName = "", SecondName = "",
            ThirdName = "", LastName = "", Address = "", Phone = "",
            Email = "", ImagePath = "";


            DateTime DateOfBirth = DateTime.Now;
            short Gender = 0;
            int NationalityCountryID = -1;

            if (clsPeopleData.GetPersonInfoByID(PersonID, ref NationalNo, ref FirstName,ref SecondName,ref ThirdName, ref LastName,
                         ref DateOfBirth,ref Gender ,ref Address,ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))
            {

                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Email, Phone,
                           Address, NationalityCountryID, ImagePath);
            }
            else
            {
                return null;
            }
        }

       

        public static clsPerson Find(string nationalNo)
        {
            string  FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            short Gender = 0;
            int NationalityCountryID = -1;
            int PersonID = -1;

            if (clsPeopleData.GetPersonInfoByNationalNo(nationalNo, ref PersonID, ref FirstName, ref SecondName, ref ThirdName, ref LastName,
                         ref DateOfBirth, ref Gender, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))
            {

                return new clsPerson(PersonID, nationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Email, Phone,
                           Address, NationalityCountryID, ImagePath);
            }
            else
            {
                return null;
            }
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdatePerson();

                default:
                    return false;
            }
        }
    }
}
