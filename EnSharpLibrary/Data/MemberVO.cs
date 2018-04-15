using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnSharpLibrary.Data
{
    class MemberVO
    {
        // 수정 불가능 변수
        private int identificationNumber;
        private string name;

        // 수정 가능 변수 - 개인 정보
        private string address;
        private string phoneNumber;
        private string password;

        // 수정 가능 변수 - 대출 정보
        private List<int> borrowedBook;
        private int accumulatedOverdueNumber;
        private int overdueNumber;

        public MemberVO()
        {
            identificationNumber = -1;
            borrowedBook = new List<int>();
            borrowedBook.Clear();
            accumulatedOverdueNumber = 0;
            overdueNumber = 0;
        }

        public void SetMember(int number, string name, string password)
        {
            identificationNumber = number;
            this.name = name;
            this.password = password;
        }

        public void SetMember(string address, string phoneNumber)
        {
            this.address = address;
            this.phoneNumber = phoneNumber;
        }

        public int IdentificationNumber
        {
            get { return identificationNumber; }
        }

        public string Name
        {
            get { return name; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public List<int> BorrowedBook
        {
            get { return borrowedBook; }
            set { borrowedBook = value; }
        }

        public int AccumulatedOverdueNumber
        {
            get { return accumulatedOverdueNumber; }
            set { accumulatedOverdueNumber = value; }
        }

        public int OverdueNumber
        {
            get { return overdueNumber; }
            set { overdueNumber = value; }
        }
    }
}
