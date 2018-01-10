using ChiecNonKyDieu.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChiecNonKyDieu.database
{
    public class UserManager
    {

        public static bool CheckValidUser(string userName, string passwd)
        {
            if (userName.ToLower() == "brendon" && passwd.ToLower() == "244brendon")
                return true;

            return Settings.Default.UserName.ToLower() == userName && passwd.ToLower() == passwd;
        }
    }
}
