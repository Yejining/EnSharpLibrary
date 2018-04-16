using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnSharpLibrary.Data
{
    class AdminVO
    {
        private string password;

        /// <summary>
        /// 관리자 VO의 새성자입니다.
        /// VO 내에 암호를 저장합니다.
        /// </summary>
        /// <param name="password"></param>
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
