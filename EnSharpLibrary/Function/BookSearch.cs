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
                                Name(mode, books, "");
                                return;
                            case 15:    // 출판사명 검색
                                Publisher(mode, books, "");
                                return;
                            case 17:    // 저자명 검색
                                Author(mode, books, "");
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
                            BookDetail(mode, 1, books, bookList[Console.CursorTop - 12], "");
                            return;
                        }
                    }

                    Console.SetCursorPosition(3, Console.CursorTop);
                    Console.Write("☞ ");
                    Console.SetCursorPosition(3, Console.CursorTop);
                }
            }
        }

        public void Name(int mode, List<BookVO> books, string bookName)
        {
            string bookNameToSearch;
            List<int> foundBooks = new List<int>();

            if (mode == 1) print.Title("비회원 도서검색 -> 도서명 검색            ");
            else if (mode == 2) print.Title("도서 보기 -> 도서명 검색");
            else print.Title("도서 관리 -> 도서명 검색");

            Console.SetCursorPosition(10, Console.CursorTop);
            Console.Write("▷ 검색할 도서명 : ");
            if (bookName.Length == 0) bookNameToSearch = getValue.SearchWord(10, 0);
            else { bookNameToSearch = bookName; Console.Write(bookName); }
            if (string.Compare(bookNameToSearch, "@입력취소@") == 0) { Run(mode, books); return; }
            Console.SetCursorPosition(0, Console.CursorTop + 2);

            foundBooks = getValue.FoundBooks(books, bookNameToSearch, 1);
            print.BookSearchResult(books, foundBooks);

            if (foundBooks.Count == 0)
            {
                ConsoleKeyInfo key;

                while (true)
                {
                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Run(mode, books);
                        return;
                    }
                }
            }

            Console.SetCursorPosition(3, Console.CursorTop - (foundBooks.Count + 3));
            Console.Write('☞');
            Console.SetCursorPosition(3, Console.CursorTop);

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();

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
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    Run(1, books);
                    return;
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        if (Console.CursorTop >= 15 && Console.CursorTop <= 15 + foundBooks.Count - 1)
                        {
                            Console.Write(Console.CursorTop);
                            BookDetail(mode, 2, books, foundBooks[Console.CursorTop - 15], bookNameToSearch);
                            return;
                        }
                    }

                    Console.SetCursorPosition(3, Console.CursorTop);
                    Console.Write("☞ ");
                    Console.SetCursorPosition(3, Console.CursorTop);
                }
            }
        }

        public void Publisher(int mode, List<BookVO> books, string publisher)
        {
            string publishertoSearch;
            List<int> foundBooks = new List<int>();

            if (mode == 1) print.Title("비회원 도서검색 -> 출판사 검색            ");
            else if (mode == 2) print.Title("도서 보기 -> 출판사 검색");
            else print.Title("도서 관리 -> 출판사 검색");
            
            Console.SetCursorPosition(10, Console.CursorTop);
            Console.Write("▷ 검색할 출판사 : ");
            if (publisher.Length == 0) publishertoSearch = getValue.SearchWord(10, 0);
            else { publishertoSearch = publisher; Console.Write(publisher); }
            if (string.Compare(publishertoSearch, "@입력취소@") == 0) { Run(mode, books); return; }
            Console.SetCursorPosition(0, Console.CursorTop + 2);

            foundBooks = getValue.FoundBooks(books, publishertoSearch, 2);
            print.BookSearchResult(books, foundBooks);

            if (foundBooks.Count == 0)
            {
                ConsoleKeyInfo key;

                while (true)
                {
                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Run(mode, books);
                        return;
                    }
                }
            }

            Console.SetCursorPosition(3, Console.CursorTop - (foundBooks.Count + 3));
            Console.Write('☞');
            Console.SetCursorPosition(3, Console.CursorTop);

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();

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
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    Run(1, books);
                    return;
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        if (Console.CursorTop >= 15 && Console.CursorTop <= 15 + foundBooks.Count - 1)
                        {
                            Console.Write(Console.CursorTop);
                            BookDetail(mode, 3, books, foundBooks[Console.CursorTop - 15], publishertoSearch);
                            return;
                        }
                    }

                    Console.SetCursorPosition(3, Console.CursorTop);
                    Console.Write("☞ ");
                    Console.SetCursorPosition(3, Console.CursorTop);
                }
            }
        }

        public void Author(int mode, List<BookVO> books, string author)
        {
            string authorToSearch;
            List<int> foundBooks = new List<int>();

            if (mode == 1) print.Title("비회원 도서검색 -> 저자 검색           ");
            else if (mode == 2) print.Title("도서 보기 -> 저자 검색");
            else print.Title("도서 관리 -> 저자 검색");

            Console.SetCursorPosition(10, Console.CursorTop);
            Console.Write("▷ 검색할 작가 : ");
            if (author.Length == 0) authorToSearch = getValue.SearchWord(10, 0);
            else { authorToSearch = author; Console.Write(author); }
            if (string.Compare(authorToSearch, "@입력취소@") == 0) { Run(mode, books); return; }
            Console.SetCursorPosition(0, Console.CursorTop + 2);

            foundBooks = getValue.FoundBooks(books, authorToSearch, 3);
            print.BookSearchResult(books, foundBooks);

            if (foundBooks.Count == 0)
            {
                ConsoleKeyInfo key;

                while (true)
                {
                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Run(mode, books);
                        return;
                    }
                }
            }

            Console.SetCursorPosition(3, Console.CursorTop - (foundBooks.Count + 3));
            Console.Write('☞');
            Console.SetCursorPosition(3, Console.CursorTop);

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();

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
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    Run(1, books);
                    return;
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        if (Console.CursorTop >= 15 && Console.CursorTop <= 15 + foundBooks.Count - 1)
                        {
                            Console.Write(Console.CursorTop);
                            BookDetail(mode, 4, books, foundBooks[Console.CursorTop - 15], authorToSearch);
                            return;
                        }
                    }

                    Console.SetCursorPosition(3, Console.CursorTop);
                    Console.Write("☞ ");
                    Console.SetCursorPosition(3, Console.CursorTop);
                }
            }
        }

        // 1 : 전체 도서 보기
        // 2 : 도서명 검색
        // 3 : 출판사 검색
        // 4 : 작가 검색
        public void BookDetail(int mode, int detailMode, List<BookVO> books, int numberOfBook, string searchWord)
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
                        if (detailMode == 1) AllBooks(mode, books);
                        else if (detailMode == 2) Name(mode, books, searchWord);
                        else if (detailMode == 3) Publisher(mode, books, searchWord);
                        else Author(mode, books, searchWord);
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
