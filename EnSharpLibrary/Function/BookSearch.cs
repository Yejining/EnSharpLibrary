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

        // mode 1: 비회원 도서 검색
        // mode 2 : 회원 도서 검색 및 대출
        // mode 3 : 관리자 도서 검색 및 도서 관리
        public void Run(int mode, List<BookVO> books)
        {
            if (mode == 1) print.Title("비회원 도서검색     ");
            else if (mode == 2) print.Title("도서 보기");
            else print.Title("도서 관리");

            print.BookSearchOption();

            Console.SetCursorPosition(74, Console.CursorTop - 10);
            Console.Write('☜');
            Console.SetCursorPosition(74, Console.CursorTop);

            while (true)
            {
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
                    if (Console.CursorTop < 19) Console.SetCursorPosition(74, Console.CursorTop + 2);
                    Console.Write('☜');
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        switch (Console.CursorTop)
                        {
                            case 11:     // 도서 전체 출력
                                AllBooks(mode, books);
                                return;
                            case 13:    // 도서명 검색
                                Name(mode, books);
                                return;
                            case 15:    // 출판사명 검색
                                Publisher(mode, books);
                                return;
                            case 17:    // 저자명 검색
                                Author(mode, books);
                                return;
                            case 19:    // 뒤로(비회원 : 메뉴, 회원 : 회원 버전 메뉴, 관리자 : 관리자 버전 메뉴)
                                Menu menu = new Menu();
                                if (mode == 1) menu.Start(1);
                                else if (mode == 2) { }
                                else { }
                                return;
                        }
                    }

                    Console.SetCursorPosition(74, Console.CursorTop);
                    Console.Write("☜ ");
                    Console.SetCursorPosition(74, Console.CursorTop);
                }
            }
        }

        // mode 1: 비회원 도서 검색
        // mode 2 : 회원 도서 검색 및 대출
        // mode 3 : 관리자 도서 검색 및 도서 관리
        public void AllBooks(int mode, List<BookVO> books)
        {
            int countOfBooks;
            List<int> bookList = new List<int>();

            if (mode == 1) print.Title("비회원 도서검색 → 전체 도서 목록               ");
            else if (mode == 2) print.Title("도서 보기 → 전체 도서 목록");
            else print.Title("도서 관리 → 전체 도서 목록");

            print.AllBooks(books);
            bookList = getValue.BookList(books);
            countOfBooks = getValue.FirstBooksCount(books);

            Console.SetCursorPosition(3, Console.CursorTop - (countOfBooks + 3));
            Console.Write('☞');
            Console.SetCursorPosition(3, Console.CursorTop);

            while (true)
            {
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
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    Run(1, books);
                    return;
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        if (Console.CursorTop >= 12 && Console.CursorTop <= 12 + countOfBooks - 1)
                        {
                            BookDetail(mode, 1, books, bookList[Console.CursorTop - 12]);
                            return;
                        }
                    }

                    Console.SetCursorPosition(3, Console.CursorTop);
                    Console.Write("☞ ");
                    Console.SetCursorPosition(3, Console.CursorTop);
                }
            }
        }

        public void Name(int mode, List<BookVO> books)
        {
            string bookNameToSearch;

            if (mode == 1) print.Title("비회원 도서검색 -> 도서명 검색            ");
            else if (mode == 2) print.Title("도서 보기 -> 도서명 검색");
            else print.Title("도서 관리 -> 도서명 검색");

            Console.SetCursorPosition(10, Console.CursorTop);
            Console.Write("▷ 검색할 도서명 : ");
            bookNameToSearch = getValue.SearchWord(10);
            if (string.Compare(bookNameToSearch, "@입력취소@") == 0) { Run(mode, books); return; }
            Console.SetCursorPosition(0, Console.CursorTop + 2);

            print.BookSearchResult(books, bookNameToSearch, 1);
        }

        public void Publisher(int mode, List<BookVO> books)
        {
            string publishertoSearch;

            if (mode == 1) print.Title("비회원 도서검색 -> 출판사 검색            ");
            else if (mode == 2) print.Title("도서 보기 -> 출판사 검색");
            else print.Title("도서 관리 -> 출판사 검색");

            Console.SetCursorPosition(10, Console.CursorTop);
            Console.Write("▷ 검색할 출판사 : ");
            publishertoSearch = getValue.SearchWord(10);
            if (string.Compare(publishertoSearch, "@입력취소@") == 0) { Run(mode, books); return; }
            Console.SetCursorPosition(0, Console.CursorTop + 2);

            print.BookSearchResult(books, publishertoSearch, 2);
        }

        public void Author(int mode, List<BookVO> books)
        {
            string authorToSearch;

            if (mode == 1) print.Title("비회원 도서검색 -> 저자 검색           ");
            else if (mode == 2) print.Title("도서 보기 -> 저자 검색");
            else print.Title("도서 관리 -> 저자 검색");

            Console.SetCursorPosition(10, Console.CursorTop);
            Console.Write("▷ 검색할 작가 : ");
            authorToSearch = getValue.SearchWord(10);
            if (string.Compare(authorToSearch, "@입력취소@") == 0) { Run(mode, books); return; }
            Console.SetCursorPosition(0, Console.CursorTop + 2);

            print.BookSearchResult(books, authorToSearch, 3);
        }

        public void BookDetail(int mode, int detailMode, List<BookVO> books, int numberOfBook)
        {
            int countOfBooks;

            print.BookDetailTitle(mode, detailMode);

            print.BookDetail(mode, books, numberOfBook);
            countOfBooks = getValue.DetailBooksCount(books, numberOfBook);

            if (mode == 1)
            {
                while (true)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey();

                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        AllBooks(mode, books);
                        return;
                    }
                }
            }

            Console.SetCursorPosition(3, Console.CursorTop - (countOfBooks + 3));
            Console.Write('☞');
            Console.SetCursorPosition(3, Console.CursorTop);

            while (true)
            {
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
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    AllBooks(mode, books);
                    return;
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        if (Console.CursorTop >= 12 && Console.CursorTop <= 12 + countOfBooks - 1)
                        {

                        }
                    }

                    Console.SetCursorPosition(3, Console.CursorTop);
                    Console.Write("☞ ");
                    Console.SetCursorPosition(3, Console.CursorTop);
                }
            }
        }
    }
}
