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
        private int usingMemberNumber;
        private int programMode;

        public void Start(int mode)
        {
            bool isFirtLoop = true;
            LibraryVO library = new LibraryVO(members, books);

            if (mode == 0)
            {
                InitBooks();
                InitMembers();
                usingMemberNumber = -1;
                programMode = 1;
                mode = 1;
            }
            
            while (true)
            {
                if (isFirtLoop)
                {
                    // 타이틀 및 옵션 출력하고 커서 조절
                    print.Title("");
                    print.MenuOption(programMode);

                    print.SetCursorAndChoice(74, 10, '☜');
                    
                    isFirtLoop = false;
                }

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
                            case 8:     // 비회원 도서검색, 도서보기. 도서관리
                                library = bookSearch.Run(programMode, usingMemberNumber, books, members);
                                break;
                            case 10:    // 로그인, 대출도서 보기, 회원관리
                                if (programMode == 0 || programMode == 1)
                                {
                                    usingMemberNumber = logIn.Member(admin, members, books);
                                    if (usingMemberNumber != -1) programMode = 2;
                                    else programMode = 1;
                                }
                                else if (programMode == 2) library = bookSearch.BorrowedBook(usingMemberNumber, members, books);
                                //else 회원관리
                                break;
                            case 12:    // 회원가입, 정보수정, 암호수정
                                if (programMode == 0 || programMode == 1) logIn.Join(members);
                                //else if (programMode == 2) 정보수정
                                //else 암호수정
                                break;
                            case 14:    // 관리자 로그인, 로그아웃, 로그아웃
                                if (programMode == 0 || programMode == 1) programMode = logIn.Admin(admin);
                                else programMode = 1;
                                break;
                            case 16:    // 종료
                                return;
                        }

                        books = library.Books;
                        members = library.Members;

                        isFirtLoop = true;
                    }
                    else
                    {
                        Console.SetCursorPosition(74, Console.CursorTop);
                        Console.Write("☜ ");
                        Console.SetCursorPosition(74, Console.CursorTop);
                    }
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
            member1.BorrowedBook.Add((float)3.01);

            member2.SetMember(16011000, "김예진", "970106");
            member2.SetMember("경기도 남양주시 진접읍", "010-5110-1996");
            member2.BorrowedBook.Add(2);
            member2.BorrowedBook.Add(3);

            member3.SetMember(16011020, "이다인", "970820");
            member3.SetMember("경기도 남양주시 진접읍", "010-8215-8282");
            member3.BorrowedBook.Add(5);

            member4.SetMember(16011001, "박수오", "970411");
            member4.SetMember("경기도 의정부시 녹양동", "010-6357-0495");
            member4.BorrowedBook.Add((float)6.01);
            member4.AccumulatedOverdueNumber = 3;
            member4.OverdueNumber = 3;

            members.Add(member1);
            members.Add(member2);
            members.Add(member3);
            members.Add(member4);
        }
    }
}
