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
        private List<float> borrowedBook;
        private int accumulatedOverdueNumber;
        private int overdueNumber;

        /// <summary>
        /// 회원의 정보를 저장하는 MemberVO의 생성자입니다.
        /// 일반 도서 모드로 만들어줍니다.
        /// </summary>
        public MemberVO()
        {
            identificationNumber = -1;
            borrowedBook = new List<float>();
            borrowedBook.Clear();
            accumulatedOverdueNumber = 0;
            overdueNumber = 0;
        }

        /// <summary>
        /// 회원의 번호와 이름, 암호를 저장하는 메소드입니다.
        /// </summary>
        /// <param name="number">회원 번호</param>
        /// <param name="name">회원 이름</param>
        /// <param name="password">암호</param>
        public void SetMember(int number, string name, string password)
        {
            identificationNumber = number;
            this.name = name;
            this.password = password;
        }

        /// <summary>
        /// 회원의 주소와 전화번호를 저장하는 메소드입니다.
        /// </summary>
        /// <param name="address">주소</param>
        /// <param name="phoneNumber">전화번호</param>
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

        public List<float> BorrowedBook
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

        /// <summary>
        /// 도서를 반납하는 메소드입니다.
        /// 반납할 책의 청구기호를 받아 대여목록에서 삭제합니다.
        /// </summary>
        /// <param name="numberOfBook">반납할 책의 청구기호</param>
        public void ReturnBook(float numberOfBook)
        {
            for (int index = 0; index < borrowedBook.Count; index++)
                if (borrowedBook[index] == numberOfBook)
                {
                    borrowedBook.RemoveAt(index);
                    return;
                }
        }
    }
}
