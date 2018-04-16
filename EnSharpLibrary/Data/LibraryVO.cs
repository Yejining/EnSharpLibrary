using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnSharpLibrary.Data
{
    class LibraryVO
    {
        private List<MemberVO> members;
        private List<BookVO> books;

        public LibraryVO(List<MemberVO> members, List<BookVO> books)
        {
            this.members = members;
            this.books = books;
        }

        public List<MemberVO> Members
        {
            get { return members; }
            set { members = value; }
        }

        public List<BookVO> Books
        {
            get { return books; }
            set { books = value; }
        }
    }
}
