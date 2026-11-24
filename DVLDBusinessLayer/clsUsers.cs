using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;

namespace DVLDBusinessLayer
{
    public class clsUsers
    {
        public int UserID { get; set; }
        public int PersonID { set; get; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        private clsPerson _PersonInfo = null;

        public clsPerson PersonInfo
        {
            get
            {
                if (_PersonInfo == null && this.PersonID != -1)
                {
                    _PersonInfo = clsPerson.Find(this.PersonID);
                }

                return _PersonInfo;
            }

        }

        

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public clsUsers()
        {
            this.UserID = -1;
            this.PersonID = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = true;
            this._PersonInfo = null;
            Mode = enMode.AddNew;

        }

        public clsUsers(int userID, int personID, string userName, string password, bool isActive)
        {
            this.UserID = userID;
            this.PersonID = personID;
            this.UserName = userName;
            this.Password = password;
            this.IsActive = isActive;

            this._PersonInfo = null;

            Mode = enMode.Update;
        }



        public static clsUsers Login(string username, string password)
        {
            int userID = 0;
            int personID = 0;
            bool isActive = false;

            bool found = clsUsersData.GetUserInfoByUsernameAndPassword(username, password,
                                                                   ref userID, ref personID, ref isActive);
            if (!found)
                return null;

            clsUsers user = new clsUsers
            {
                UserID = userID,
                PersonID = personID,       // ✅ now assigned
                UserName = username,
                Password = password,
                IsActive = isActive,       // ✅ no redundant ternary
                _PersonInfo = clsPerson.Find(personID),
                Mode = enMode.Update       // ✅ user exists in DB
            };

            return user;
        }


        public static DataTable GetAllUsers()
        {
            return clsUsersData.GetAllUsers();
        }


        private bool _AddNewUser()
        {
            this.UserID = clsUsersData.AddNewUser(this.PersonID, this.UserName, this.Password, this.IsActive);

            return (this.UserID != -1);
        }

        private bool _UpdateUser()
        {

            return clsUsersData.UpdateUser(this.UserID, this.PersonID, this.UserName, this.Password, this.IsActive);

        }

        public static bool DeleteUser(int UserID)
        {
            return clsUsersData.DeleteUser(UserID);
        }
        public static clsUsers FindUser(int UserID)
        {
            int personid = -1; string username = "";  string password = ""; bool isactive = false;

            if (clsUsersData.GetUserInfoByUserID(UserID, ref personid, ref username, ref password, ref isactive))
            {
                return new clsUsers(UserID, personid, username, password, isactive);
            }
            else
            {
                return null;
            }
        }

        public static bool IsUserExistByUserID(int userID)
        {
            return clsUsersData.IsUserExistByUserID(userID);
        }


        public static bool IsUserExistByPersonID(int personID)
        {
            return clsUsersData.IsUserExistByPersonID(personID);
        }

        public static bool IsUserExistByUserName(string UserName)
        {
            return clsUsersData.IsUserExistByUserName(UserName);
        }

        public static bool ChangePasswordforUser(int UserID,string Password)
        {
            return clsUsersData.ChangePasswordForUser(UserID, Password);
        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateUser();

                default: return false;
            }
        }

    }
}
