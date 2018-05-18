using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnSharpLibrary.Data
{
    class MemberVO
    {
        private int memberID;                   // 회원 학번
        private string name;                    // 이름
        private DateTime birthdate;             // 생일
        private string address;                 // 주소
        private string phoneNumber;             // 전화번호
        private string password;                // 암호
        private int accumulatedOverdueNumber;   // 누적 연체 횟수
        private int overdueNumber;              // 연체 횟수

        /// <summary>
        /// 회원의 정보를 저장하는 MemberVO의 생성자입니다.
        /// 일반 도서 모드로 만들어줍니다.
        /// </summary>
        public MemberVO()
        {
            memberID = -1;
            accumulatedOverdueNumber = 0;
            overdueNumber = 0;
        }

        /// <summary>
        /// MemberVO의 생성자입니다. 회원의 학번과 이름, 암호를 저장합니다.
        /// </summary>
        /// <param name="number">학번</param>
        /// <param name="name">이름</param>
        /// <param name="password">암호</param>
        public MemberVO(int number, string name, string password)
        {
            memberID = number;
            this.name = name;
            this.password = password;
        }

        /// <summary>
        /// MemberVO의 정보를 더해주는 메소드입니다. 주소와 전화번호, 생일을 저장합니다.
        /// </summary>
        /// <param name="address">주소</param>
        /// <param name="phoneNumber">전화번호</param>
        /// <param name="birthdate">생일</param>
        public void AppendInformation(string address, string phoneNumber, DateTime birthdate)
        {
            this.address = address;
            this.phoneNumber = phoneNumber;
            this.birthdate = birthdate;
        }

        /// <summary>
        /// MemberVO의 정보를 더해주는 메소드입니다. 누적연체횟수와 연체횟수를 저장합니다.
        /// </summary>
        /// <param name="accumulatedOverdueNumber">누적연체횟수</param>
        /// <param name="overdueNumber">연체횟수</param>
        public void AppendInformation(int accumulatedOverdueNumber, int overdueNumber)
        {
            this.accumulatedOverdueNumber = accumulatedOverdueNumber;
            this.overdueNumber = overdueNumber;
        }

        public int MemberID
        {
            get { return memberID; }
            set { memberID = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public DateTime Birthdate
        {
            get { return birthdate; }
            set { birthdate = value; }
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
