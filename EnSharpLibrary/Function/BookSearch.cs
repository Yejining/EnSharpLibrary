using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EnSharpLibrary.IO;
using EnSharpLibrary.Data;

namespace EnSharpLibrary.Function
{
    class BookSearch
    {
        Print print = new Print();
        GetValue getValue = new GetValue();
        BookManage bookManage = new BookManage();

        // mode 1: 비회원 도서 검색
        // mode 2 : 회원 도서 검색 및 대출
        // mode 3 : 관리자 도서 검색 및 도서 관리
        public LibraryVO Run(int programMode, int usingMemberNumber, List<BookVO> books, List<MemberVO> members)
        {
            bool isFirstLoop = true;
            LibraryVO library = new LibraryVO(members, books);

            while (true)
            {
                if (isFirstLoop)
                {
                    // 타이틀 및 옵션 출력하고 커서 조절
                    print.BookSearchTitle(programMode);
                    print.BookSearchOption(programMode);

                    print.SetCursorAndChoice(74, 10, '☜');

                    isFirstLoop = false;
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    print.ClearOneLetter(74);
                    if (Console.CursorTop > 11) Console.SetCursorPosition(74, Console.CursorTop - 2);
                    Console.Write('☜');
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    print.ClearOneLetter(74);
                    if ((programMode == 1 || programMode == 2) && Console.CursorTop < 19) Console.SetCursorPosition(74, Console.CursorTop + 2);
                    else if (programMode == 3 && Console.CursorTop < 21) Console.SetCursorPosition(74, Console.CursorTop + 2);
                    Console.Write('☜');
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        switch (Console.CursorTop)
                        {
                            case 11:     // 도서 전체 출력
                                library = AllBooks(programMode, usingMemberNumber, books, members);
                                break;
                            case 13:    // 도서명 검색
                                library = Specifically(programMode, 2, usingMemberNumber, books, members);
                                break;
                            case 15:    // 출판사명 검색
                                library = Specifically(programMode, 3, usingMemberNumber, books, members);
                                break;
                            case 17:    // 저자명 검색
                                library = Specifically(programMode, 4, usingMemberNumber, books, members);
                                break;
                            case 19:    // 뒤로(비회원 : 메뉴, 회원 : 회원 버전 메뉴), 관리자 : 도서 등록
                                if (programMode == 1 || programMode == 2) return library;
                                library = bookManage.AddBook(members, books);
                                break;
                            case 21:    // 관리자 뒤로
                                return library;
                        }

                        books = library.Books;
                        members = library.Members;

                        isFirstLoop = true;
                    }
                    else print.BlockCursorMove(74, "☜ ");             
                }
            }
        }

        // mode 1: 비회원 도서 검색
        // mode 2 : 회원 도서 검색 및 대출
        // mode 3 : 관리자 도서 검색 및 도서 관리
        public LibraryVO AllBooks(int programMode, int usingMemberNumber, List<BookVO> books, List<MemberVO> members)
        {
            List<float> bookList = new List<float>();
            int countOfBooks;
            bool isFirstLoop = true;
            LibraryVO library = new LibraryVO(members, books);

            bookList = getValue.BookList(books, 1, -1);
            countOfBooks = bookList.Count;
       
            while (true)
            {
                if (isFirstLoop)
                {
                    bookList = getValue.BookList(books, 1, -1);
                    countOfBooks = bookList.Count;

                    print.BookSearchAllBooksTitle(programMode);
                    print.AllBooks(books);

                    print.SetCursorAndChoice(3, countOfBooks + 3, '☞');

                    isFirstLoop = false;
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    print.ClearOneLetter(3);
                    if (Console.CursorTop > 12) Console.SetCursorPosition(3, Console.CursorTop - 1);
                    Console.Write('☞');
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    print.ClearOneLetter(3);
                    if (Console.CursorTop < 12 + countOfBooks - 1) Console.SetCursorPosition(3, Console.CursorTop + 1);
                    Console.Write('☞');
                }
                else if (keyInfo.Key == ConsoleKey.Escape) return library;
                else
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        if (Console.CursorTop >= 12 && Console.CursorTop <= 12 + countOfBooks - 1)
                            library = BookDetail(programMode, 1, books, (int)bookList[Console.CursorTop - 12], usingMemberNumber, members);

                        books = library.Books;
                        members = library.Members;

                        isFirstLoop = true;
                    }
                    else print.BlockCursorMove(3, "☞ ");
                }
            }
        }

        // 2 : 도서명 검색
        // 3 : 출판사 검색
        // 4 : 작가 검색
        public LibraryVO Specifically(int programMode, int detailMode, int usingMemberNumber, List<BookVO> books, List<MemberVO> members)
        {
            List<int> foundBooks = new List<int>();
            string keywordToSearch;
            ConsoleKeyInfo keyInfo;
            StringBuilder title = new StringBuilder();
            bool isFirstLoop = false;
            LibraryVO library = new LibraryVO(members, books);
            
            if (detailMode == 2) title.AppendFormat("▷ 검색할 도서명 : ");
            else if (detailMode == 3) title.AppendFormat("▷ 검색할 출판사 : ");
            else title.AppendFormat("▷ 검색할 저자 : ");

            print.SpecificallySearchTitle(programMode, detailMode);
            Console.SetCursorPosition(10, Console.CursorTop);
            Console.Write(title);
            keywordToSearch = getValue.SearchWord(10, 0);
            title.Append(keywordToSearch);            

            if (string.Compare(keywordToSearch, "@입력취소@") == 0) return library;

            Console.SetCursorPosition(0, Console.CursorTop + 2);

            foundBooks = getValue.FoundBooks(books, keywordToSearch, detailMode);
            print.BookSearchResult(books, foundBooks);

            while (foundBooks.Count == 0)
            {
                keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Escape) return library;
            }

            print.SetCursorAndChoice(3, foundBooks.Count + 3, '☞');

            while (true)
            {
                if (isFirstLoop)
                {
                    print.SpecificallySearchTitle(programMode, detailMode);
                    Console.SetCursorPosition(10, Console.CursorTop);
                    Console.Write(title);

                    foundBooks = getValue.FoundBooks(books, keywordToSearch, detailMode);

                    while (foundBooks.Count == 0)
                    {
                        keyInfo = Console.ReadKey();
                        if (keyInfo.Key == ConsoleKey.Escape) return library;
                    }

                    Console.SetCursorPosition(0, Console.CursorTop + 2);
                    print.BookSearchResult(books, foundBooks);

                    isFirstLoop = false;
                }

                keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    print.ClearOneLetter(3);
                    if (Console.CursorTop > 15) Console.SetCursorPosition(3, Console.CursorTop - 1);
                    Console.Write('☞');
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    print.ClearOneLetter(3);
                    if (Console.CursorTop < 15 + foundBooks.Count - 1) Console.SetCursorPosition(3, Console.CursorTop + 1);
                    Console.Write('☞');
                }
                else if (keyInfo.Key == ConsoleKey.Escape) return library;
                else
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        if (Console.CursorTop >= 15 && Console.CursorTop <= 15 + foundBooks.Count - 1)
                        {
                            library = BookDetail(programMode, detailMode, books, foundBooks[Console.CursorTop - 15], usingMemberNumber, members);
                        }

                        books = library.Books;
                        members = library.Members;

                        isFirstLoop = true;
                    }
                    else print.BlockCursorMove(3, "☞ ");
                }
            }
        }

        // 1 : 전체 도서 보기 -> 도서 상세
        // 2 : 도서명 검색 -> 도서 상세
        // 3 : 출판사 검색 -> 도서 상세
        // 4 : 작가 검색 -> 도서 상세
        public LibraryVO BookDetail(int mode, int detailMode, List<BookVO> books, int numberOfBook, int usingMemberNumber, List<MemberVO> members)
        {
            int countOfBooks;
            bool isFirstLoop = true;
            List<int> indexOfBooks = new List<int>();
            LibraryVO library = new LibraryVO(members, books);

            countOfBooks = getValue.DetailBooksCount(books, numberOfBook);
            indexOfBooks = getValue.IndexOfBooks(books, numberOfBook);

            while (mode == 0 || mode == 1)
            {
                print.BookDetailTitle(mode, detailMode);
                print.BookDetail(mode, books, indexOfBooks);

                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Escape) return library;
            }

            while (true)
            {
                if (isFirstLoop)
                {
                    countOfBooks = getValue.DetailBooksCount(books, numberOfBook);
                    indexOfBooks = getValue.IndexOfBooks(books, numberOfBook);

                    print.BookDetailTitle(mode, detailMode);
                    print.BookDetail(mode, books, indexOfBooks);

                    print.SetCursorAndChoice(3, countOfBooks + 3, '☞');

                    isFirstLoop = false;
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    print.ClearOneLetter(3);
                    if (Console.CursorTop > 12) Console.SetCursorPosition(3, Console.CursorTop - 1);
                    Console.Write('☞');
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    print.ClearOneLetter(3);
                    if (Console.CursorTop < 12 + countOfBooks - 1) Console.SetCursorPosition(3, Console.CursorTop + 1);
                    Console.Write('☞');
                }
                else if (keyInfo.Key == ConsoleKey.Escape) return library;
                else
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        if (Console.CursorTop >= 12 && Console.CursorTop <= 12 + countOfBooks - 1)
                        {
                            if (mode == 2)
                            {
                                library = bookManage.MemberMode(usingMemberNumber, indexOfBooks[Console.CursorTop - 12], books, members);
                            }
                            else
                                library = bookManage.AdminMode(indexOfBooks[Console.CursorTop - 12], books, members);
                        }
                        isFirstLoop = true;

                        members = library.Members;
                        books = library.Books;
                    }
                    else print.BlockCursorMove(3, "☞ ");
                }
            }
        }

        // usingMemberNumber -1 : 관리자모드 대출된 도서 보기
        // usingMemberNumber -2 : 관리자모드 연체된 도서 보기
        public LibraryVO BorrowedBook(int usingMemberNumber, List<MemberVO> members, List<BookVO> books)
        {
            List<float> bookList = new List<float>();
            int countOfBooks = 0;
            bool isFirstLoop = true;
            LibraryVO library = new LibraryVO(members, books);

            while (true)
            {
                if (isFirstLoop)
                {
                    // 모드마다 다름
                    bookList = getValue.BookList(books, 2, usingMemberNumber);
                    countOfBooks = bookList.Count;

                    // 모드마다 다름
                    print.Title("대출도서 보기");
                    print.BorrowedBook(usingMemberNumber, books, bookList);

                    print.SetCursorAndChoice(2, countOfBooks + 3, '☞');

                    isFirstLoop = false;
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    print.ClearOneLetter(2);
                    if (Console.CursorTop > 12) Console.SetCursorPosition(2, Console.CursorTop - 1);
                    Console.Write('☞');
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    print.ClearOneLetter(2);
                    if (Console.CursorTop < 12 + countOfBooks - 1) Console.SetCursorPosition(2, Console.CursorTop + 1);
                    Console.Write('☞');
                }
                else if (keyInfo.Key == ConsoleKey.Escape) return library;
                else
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        if (Console.CursorTop >= 12 && Console.CursorTop <= 12 + countOfBooks - 1)
                            library = bookManage.MemberMode(usingMemberNumber, getValue.GetIndex(bookList[Console.CursorTop - 12], books), books, members);
                        
                        books = library.Books;
                        members = library.Members;

                        isFirstLoop = true;
                    }
                    else print.BlockCursorMove(2, "☞ ");
                }
            }
        }
    }
}
