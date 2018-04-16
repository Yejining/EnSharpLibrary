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

        /// <summary>
        /// 프로그램의 회원 정보와 책 정보를 저장한 LibraryVO의 생성자입니다.
        /// </summary>
        /// <param name="members">회원 정보</param>
        /// <param name="books">책 정보</param>
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
