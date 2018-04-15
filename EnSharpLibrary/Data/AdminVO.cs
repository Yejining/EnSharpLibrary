using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnSharpLibrary.Data
{
    class AdminVO
    {
        private string password;

        public AdminVO(string password)
        {
            this.password = password;
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
    }
}
