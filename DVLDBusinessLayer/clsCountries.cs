using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;

namespace DVLDBusinessLayer
{
    public class clsCountries
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }

        clsCountries()
        {
            this.CountryID = -1;
            this.CountryName = "";
        }

        clsCountries(int countryID, string countryName)
        {
            this.CountryID = countryID;
            this.CountryName = countryName;
        }

        static public DataTable GetAllCountries()
        {
            return clsCountriesData.GetAllCountries();
        }

        public static clsCountries Find(int CountryID)
        {
            string CountryName = "";



            if (clsCountriesData.GetCountryInfoByID(CountryID, ref CountryName))
            {

                return new clsCountries(CountryID, CountryName);
            }
            else
            {
                return null;
            }
        }


        public static clsCountries Find(string countryName)
        {
            int CountryID = -1;



            if (clsCountriesData.GetCountryInfoByName(countryName, ref CountryID)) 
            {

                return new clsCountries(CountryID, countryName);
            }
            else
            {
                return null;
            }
        }
    }
}
