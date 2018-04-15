using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EnSharpLibrary.IO;
using EnSharpLibrary.Data;

namespace EnSharpLibrary.Function
{
    class Menu
    {
        Print print = new Print();
        GetValue getValue = new GetValue();
        BookSearch bookSearch = new BookSearch();
        LogIn logIn = new LogIn();

        private AdminVO admin = new AdminVO("970106");
        private List<BookVO> books = new List<BookVO>();
        private List<MemberVO> members = new List<MemberVO>();

        public void Start(int mode)
        {
            if (mode == 0)
            {
                InitBooks();
                InitMembers();
            }

            print.Title("");
            print.NonMemberMenuOption();

            Console.SetCursorPosition(74, Console.CursorTop - 10);
            Console.Write('☜');
            Console.SetCursorPosition(74, Console.CursorTop);

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    print.ClearOneLetter(74);
                    if (Console.CursorTop > 9) Console.SetCursorPosition(74, Console.CursorTop - 2);
                    Console.Write('☜');
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    print.ClearOneLetter(74);
                    if (Console.CursorTop < 15) Console.SetCursorPosition(74, Console.CursorTop + 2);
                    Console.Write('☜');
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        switch (Console.CursorTop)
                        {
                            case 8:     // 비회원 도서검색
                                bookSearch.Run(1, books);
                                return;
                            case 10:    // 로그인
                                logIn.Member(members);
                                return;
                            case 12:    // 회원가입
                                logIn.Join(members);
                                return;
                            case 14:    // 관리자 로그인
                                logIn.Admin(admin);
                                return;
                            case 16:    // 종료
                                return;
                        }
                    }

                    Console.SetCursorPosition(74, Console.CursorTop);
                    Console.Write("☜ ");
                    Console.SetCursorPosition(74, Console.CursorTop);
                }
            }
        }

        public void InitBooks()
        {
            BookVO book1 = new BookVO("(리더십의 고전)목민심서", "정약용", "고려원북스", 2006);

            BookVO book2 = new BookVO("걸리버 여행기", "조나단 스위프트", "문학수첩", 1992);
            book2.SetRentalMode(DateTime.Parse("2018/04/08"), DateTime.Parse("2018/04/14"), 16011000);

            BookVO book3 = new BookVO("자존감 수업", "윤홍균", "심플라이프", 2016);
            book3.SetRentalMode(DateTime.Parse("2018/04/09"), DateTime.Parse("2018/04/15"), 16011000);
            book3.NumberOfBooks = 2;

            BookVO book4 = new BookVO("자존감 수업", "윤홍균", "심플라이프", 2016);
            book4.SetRentalMode(DateTime.Parse("2018/04/02"), DateTime.Parse("2018/04/08"), 14010998);
            book4.NumberOfBooks = 2;
            book4.OrderOfBooks = 1;

            BookVO book5 = new BookVO("농담", "밀란 쿤데라", "민음사", 1999);
            book5.SetRentalMode(DateTime.Parse("2018/04/10"), DateTime.Parse("2018/04/16"), 16011020);

            BookVO book6 = new BookVO("82년생 김지영", "조남주", "민음사", 2016);
            book6.NumberOfBooks = 2;

            BookVO book7 = new BookVO("82년생 김지영", "조남주", "민음사", 2016);
            book7.SetRentalMode(DateTime.Parse("2018/02/05"), DateTime.Parse("2018/02/25"), 16011001);
            book7.NumberOfBooks = 2;
            book7.OrderOfBooks = 1;
            book7.NumberOfRenew = 2;

            AddBook(book1);
            AddBook(book2);
            AddBook(book3);
            AddBook(book4);
            AddBook(book5);
            AddBook(book6);
            AddBook(book7);
        }

        public void AddBook(BookVO book)
        {
            int numberOfSameBook = getValue.NumberOfSameBook(books, book.Name, book.Author, book.Publisher, book.PublishingYear);
            float orderOfBooksToFloat = 0;

            if (numberOfSameBook != -1)
            {
                orderOfBooksToFloat = (float)book.OrderOfBooks / 100;
                book.NumberOfThis = numberOfSameBook + orderOfBooksToFloat;
            }
            else
            {
                book.NumberOfThis = books.Count + 1;
            }
            books.Add(book);
        }

        public void InitMembers()
        {
            MemberVO member1 = new MemberVO();
            MemberVO member2 = new MemberVO();
            MemberVO member3 = new MemberVO();
            MemberVO member4 = new MemberVO();

            member1.SetMember(14010998, "허진규", "940209");
            member1.SetMember("서울특별시 광진구 군자동", "010-4701-8554");
            member1.BorrowedBook.Add(4);

            member2.SetMember(16011000, "김예진", "970106");
            member2.SetMember("경기도 남양주시 진접읍", "010-5110-1996");
            member2.BorrowedBook.Add(2);
            member2.BorrowedBook.Add(3);

            member3.SetMember(1601120, "이다인", "970820");
            member3.SetMember("경기도 남양주시 진접읍", "010-8215-8282");
            member3.BorrowedBook.Add(5);

            member4.SetMember(16011001, "박수오", "970411");
            member4.SetMember("경기도 의정부시 녹양동", "010-6357-0495");
            member4.BorrowedBook.Add(7);
            member4.AccumulatedOverdueNumber = 3;
            member4.OverdueNumber = 3;

            members.Add(member1);
            members.Add(member2);
            members.Add(member3);
            members.Add(member4);
        }
    }
}
