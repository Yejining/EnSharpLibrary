using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnSharpLibrary.Data
{
    class BookVO
    {
        // 수정 불가능 변수
        private string name;
        private string author;
        private string publisher;
        private int publishingYear;
        private float bookID;           // 청구기호

        // 수정 가능 변수
        private int bookCondition;
        private int borrowedMemberID;
        private int price; 

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
            borrowedMemberID = -1;
            bookID = 0;
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

        public float BookID
        {
            get { return bookID; }
            set { bookID = value; }
        }

        public int BookCondition
        {
            get { return bookCondition; }
            set { bookCondition = value; }
        }

        public int BorrowedMemberID
        {
            get { return borrowedMemberID; }
            set { borrowedMemberID = value; }
        }

        public int Price
        {
            get { return price; }
            set { price = value; }
        }

        /// <summary>
        /// 도서의 상태를 대출모드로 바꾸어줍니다.
        /// </summary>
        /// <param name="rental">대출일</param>
        /// <param name="expectedToReturn">반납 예정일</param>
        /// <param name="memberNumber">대출한 회원의 번호</param>
        public void SetRentalMode(DateTime rental, DateTime expectedToReturn, int memberNumber)
        {

        }

        /// <summary>
        /// 대출도서를 반납모드로 바꾸어줍니다.
        /// </summary>
        public void SetNonRentalMode()
        {

        }
    }
}
