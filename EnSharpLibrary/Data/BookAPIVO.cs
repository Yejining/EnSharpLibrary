using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnSharpLibrary.Data
{
    class BookAPIVO
    {
        private string title;
        private string author;
        private int price;
        private int discount;
        private string publisher;
        public string pubdate;
        public string isbn;
        public string decription;

        public BookAPIVO(string title, string publisher, string pubdate)
        {
            this.title = title;
            this.publisher = publisher;
            this.pubdate = pubdate;
        }

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
    }
}
