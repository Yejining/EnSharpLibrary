using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnSharpLibrary.Data
{
    class BookVO
    {
        // 전체 공개 가능 변수
        private string name;
        private string author;
        private string publisher;
        private int publishingYear;
        private int bookCondition;
        private int numberOfMember;
        private int numberOfBooks;  // 같은 책들의 숫자
        private int orderOfBooks;   // 같은 책들 중 자신의 순서, 0부터 시작
        private float numberOfThis; // 자신의 청구기호. 고유기호.orderOfBooks

        // 관리자 및 대출자 공개 가능 변수
        private DateTime rental;
        private DateTime expectedToReturn;
        private int numberOfRenew;

        /// <summary>
        /// BookVO의 생성자입니다.
        /// 책의 이름과 저자, 출판사, 출판년도를 저장합니다.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="author"></param>
        /// <param name="publisher"></param>
        /// <param name="publishingYear"></param>
        public BookVO(string name, string author, string publisher, int publishingYear)
        {
            this.name = name;
            this.author = author;
            this.publisher = publisher;
            this.publishingYear = publishingYear;
            bookCondition = 0;
            numberOfMember = -1;
            numberOfBooks = 1;
            orderOfBooks = 0;
            numberOfThis = 0;

            rental = DateTime.Parse("1980/01/01");
            expectedToReturn = DateTime.Parse("1980/01/01");
            numberOfRenew = 0;
        }

        /// <summary>
        /// 도서의 상태를 대출모드로 바꾸어줍니다.
        /// </summary>
        /// <param name="rental">대출일</param>
        /// <param name="expectedToReturn">반납 예정일</param>
        /// <param name="memberNumber">대출한 회원의 번호</param>
        public void SetRentalMode(DateTime rental, DateTime expectedToReturn, int memberNumber)
        {
            this.rental = rental;
            this.expectedToReturn = expectedToReturn;
            numberOfMember = memberNumber;
            bookCondition = 1;
        }

        public string Name
        {
            get { return name; }
        }

        public string Author
        {
            get { return author; }
        }

        public string Publisher
        {
            get { return publisher; }
        }

        public int PublishingYear
        {
            get { return publishingYear; }
        }

        public int BookCondition
        {
            get { return bookCondition; }
            set { bookCondition = value; }
        }

        public int NumberOfMember
        {
            get { return numberOfMember; }
            set { numberOfMember = value; }
        }

        public int NumberOfBooks
        {
            get { return numberOfBooks; }
            set { numberOfBooks = value; }
        }

        public int OrderOfBooks
        {
            get { return orderOfBooks; }
            set { orderOfBooks = value; }
        }

        public float NumberOfThis
        {
            get { return numberOfThis; }
            set { numberOfThis = value; }
        }

        public DateTime Rental
        {
            get { return rental; }
            set { rental = value; }
        }

        public DateTime ExpectedToReturn
        {
            get { return expectedToReturn; }
            set { expectedToReturn = value; }
        }

        public int NumberOfRenew
        {
            get { return numberOfRenew; }
            set { numberOfRenew = value; }
        }

        /// <summary>
        /// 대출도서를 반납모드로 바꾸어줍니다.
        /// </summary>
        public void SetNonRentalMode()
        {
            bookCondition = 0;
            expectedToReturn = DateTime.Parse("1980/01/01");
            rental = DateTime.Parse("1980/01/01");
            numberOfMember = -1;
        }
    }
}
