using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnSharpLibrary.Data
{
    class BookAPIVO
    {
        private string title;       // 도서명
        private string author;      // 작가
        private int price;          // 금액
        private int discount;       // 할인가
        private string publisher;   // 출판사
        private string pubdate;     // 출판일
        private string isbn;        // isbn
        private string decription;  // 책 소개
        private int count;          // 수량
        private int serialNumber;   // 청구기호

        /// <summary>
        /// BookAPIVO의 생성자 메소드입니다.
        /// </summary>
        public BookAPIVO()
        {

        }

        /// <summary>
        /// BookAPIVO의 생성사 메소드입니다.
        /// mode에 따라 도서명, 출판사, 출판일을 초기화하거나 도서명, 출판사, 작가를 초기화합니다.
        /// 수량과 청구기호는 각각 0으로 초기화합니다.
        /// </summary>
        /// <param name="mode">사용자 모드</param>
        /// <param name="title">도서명</param>
        /// <param name="publisher">출판사명</param>
        /// <param name="value">출판일 또는 작가</param>
        public BookAPIVO(int mode, string title, string publisher, string value)
        {
            this.title = title;
            this.publisher = publisher;

            if (mode == Constant.ADMIN_MODE) pubdate = value;
            else author = value;

            count = 0;
            serialNumber = 0;
        }

        /// <summary>
        /// mode에 따라 BookAPIVO의 변수값을 바꾸어줍니다.
        /// </summary>
        /// <param name="mode">바꿀 변수</param>
        /// <param name="detail">내용</param>
        public void SaveDetail(int mode, string detail)
        {
            if (detail.Length == 0)
            {
                if (mode == Constant.AUTHOR) author = "작자미상";
                else if (mode == Constant.ISBN) isbn = "없음";
                else decription = title;
            }
            else
            {
                if (mode == Constant.AUTHOR) author = detail;
                else if (mode == Constant.ISBN) isbn = detail;
                else decription = detail;
            }
        }

        /// <summary>
        /// 가격 정보를 저장하는 메소드입니다,
        /// mode에 따라 price 혹은 discount의 가격을 저장합니다.
        /// </summary>
        /// <param name="mode">바꿀 변수</param>
        /// <param name="price">금액</param>
        public void SavePrice(int mode, string price)
        {
            if (price.Length == 0)
            {
                if (mode == Constant.PRICE) this.price = 0;
                else discount = 0;
            }
            else 
            {
                if (mode == Constant.PRICE) this.price = (int)float.Parse(price);
                else discount = Int32.Parse(price);
            }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Author
        {
            get { return author;}
            set { author = value; }
        }

        public int Price
        {
            get { return price; }
            set { price = value; }
        }

        public int Discount
        {
            get { return discount; }
            set { discount = value; }
        }

        public string Publisher
        {
            get { return publisher; }
            set { publisher = value; }
        }

        public string Pubdate
        {
            get { return pubdate; }
            set { pubdate = value; }
        }

        public string Isbn
        {
            get { return isbn; }
            set { isbn = value; }
        }

        public string Description
        {
            get { return decription; }
            set { decription = value; }
        }

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public int SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; }
        }
    }
}
