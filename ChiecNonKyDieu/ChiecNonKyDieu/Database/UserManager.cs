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
            return userName.ToLower() == "brendon" && passwd.ToLower() == "244brendon";
        }
    }
}
