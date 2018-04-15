using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EnSharpLibrary.Data;
using EnSharpLibrary.Function;

namespace EnSharpLibrary.IO
{
    class Print
    {
        //GetValue getValueInPrintClass = new GetValue();

        public void Title(string title)
        {
            string title1 = "┏                     ┓";
            string title2 = "  En# Library";
            string title3 = "┗                     ┛";

            Console.SetWindowSize(120, 30);
            Console.Clear();
            Console.SetCursorPosition(0, 0);

            Console.WriteLine(String.Format("\n\n{0," + ((Console.WindowWidth / 2) + (title1.Length / 2)) + "}", title1));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (title2.Length / 2)) + "}", title2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (title3.Length / 2)) + "}\n\n", title3));

            if (string.Compare(title, "") > 0) Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (title.Length / 2)) + "}\n\n", title));
        }

        public void BookSearchTitle(int mode)
        {
            if (mode == 1) Title("비회원 도서검색     ");
            else if (mode == 2) Title("도서 보기");
            else Title("도서 관리");
        }

        public void BookSearchAllBooksTitle(int mode)
        {
            if (mode == 1) Title("비회원 도서검색 → 전체 도서 목록               ");
            else if (mode == 2) Title("도서 보기 → 전체 도서 목록");
            else Title("도서 관리 → 전체 도서 목록");
        }

        public void BookDetailTitle(int mode, int detailMode)
        {
            switch (mode)
            {
                case 1:
                    if (detailMode == 1) Title("비회원 도서검색 -> 전체 도서 목록 -> 도서 상세               ");
                    else if (detailMode == 2) Title("비회원 도서검색 -> 도서명 검색 -> 도서 상세               ");
                    else if (detailMode == 3) Title("비회원 도서검색 -> 출판사 검색 -> 도서 상세               ");
                    else Title("비회원 도서검색 -> 저자 검색 -> 도서 상세               ");
                    break;
                case 2:
                    if (detailMode == 1) Title("도서 보기 -> 전체 도서 목록 -> 도서 상세               ");
                    else if (detailMode == 2) Title("도서 보기 -> 도서명 검색 -> 도서 상세               ");
                    else if (detailMode == 3) Title("도서 보기 -> 출판사 검색 -> 도서 상세               ");
                    else Title("도서 보기 -> 저자 검색 -> 도서 상세               ");
                    break;
                case 3:
                    if (detailMode == 1) Title("도서 관리 -> 전체 도서 목록 -> 도서 상세               ");
                    else if (detailMode == 2) Title("도서 관리 -> 도서명 검색 -> 도서 상세               ");
                    else if (detailMode == 3) Title("도서 관리 -> 출판사 검색 -> 도서 상세               ");
                    else Title("도서 관리 -> 저자 검색 -> 도서 상세               ");
                    break;
            }
        }

        public void NonMemberMenuOption()
        {
            string line1 = "┏                     ┓";
            string line2 = "┣                     ┫";
            string line3 = "┗                     ┛";
            string menu1 = "비회원 도서검색     ";
            string menu2 = "로그인   ";
            string menu3 = "회원가입   ";
            string menu4 = "관리자 로그인      ";
            string menu5 = "종료  ";

            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line1.Length / 2)) + "}", line1));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu1.Length / 2)) + "}", menu1));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu2.Length / 2)) + "}", menu2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu3.Length / 2)) + "}", menu3));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu4.Length / 2)) + "}", menu4));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu5.Length / 2)) + "}", menu5));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line3.Length / 2)) + "}", line3));
        }

        public void BookSearchOption()
        {
            string line1 = "┏                     ┓";
            string line2 = "┣                     ┫";
            string line3 = "┗                     ┛";
            string menu1 = "전체 도서 목록      ";
            string menu2 = "도서명 검색   ";
            string menu3 = "출판사 검색   ";
            string menu4 = "저자 검색   ";
            string menu5 = "뒤로  ";

            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line1.Length / 2)) + "}", line1));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu1.Length / 2)) + "}", menu1));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu2.Length / 2)) + "}", menu2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu3.Length / 2)) + "}", menu3));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu4.Length / 2)) + "}", menu4));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu5.Length / 2)) + "}", menu5));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line3.Length / 2)) + "}", line3));
        }

        public void AllBooks(List<BookVO> books)
        {
            string categories = "  선택  |                    도서                   |          저자         |       출판사       |  출판년도  |  수량  ";
            string line = "-----------------------------------------------------------------------------------------------------------------------";
            string guide = "나가기(ESC)";

            Console.WriteLine(categories);
            Console.WriteLine(line);

            for (int order = 0; order < books.Count; order++)
            {
                if (books[order].OrderOfBooks == 0)
                {
                    Console.SetCursorPosition(10, Console.CursorTop);
                    Console.Write(books[order].Name);
                    Console.SetCursorPosition(54, Console.CursorTop);
                    Console.Write(books[order].Author);
                    Console.SetCursorPosition(78, Console.CursorTop);
                    Console.Write(books[order].Publisher);
                    Console.SetCursorPosition(102, Console.CursorTop);
                    Console.Write(books[order].PublishingYear);
                    Console.SetCursorPosition(114, Console.CursorTop);
                    Console.Write(books[order].NumberOfBooks);
                    Console.SetCursorPosition(0, Console.CursorTop + 1);
                }
            }

            Console.SetCursorPosition(0, Console.CursorTop + 2);
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (guide.Length / 2)) + "}", guide));
        }

        // 1 : 도서명 검색
        // 2 : 출판사 검색
        // 3 : 저자 검색
        public void BookSearchResult(List<BookVO> books, List<int>foundBooks)
        {
            List<int> indexesOfSearchResult = foundBooks;

            string categories = "  선택  |                    도서                   |          저자         |       출판사       |  출판년도  |  수량  ";
            string line = "-----------------------------------------------------------------------------------------------------------------------";
            string guide = "나가기(ESC)";
            string message = "찾는 도서가 없습니다!         ";
            
            Console.SetCursorPosition(0, Console.CursorTop + 1);
            Console.WriteLine(categories);
            Console.WriteLine(line);

            if (indexesOfSearchResult.Count == 0)
            {
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (message.Length / 2)) + "}", message));
                Console.SetCursorPosition(0, Console.CursorTop + 2);
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (guide.Length / 2)) + "}", guide));
                return;
            }

            for (int inList = 0; inList < indexesOfSearchResult.Count; inList++)
            {
                for (int index = 0; index < books.Count; index++)
                {
                    if (indexesOfSearchResult[inList] == books[index].NumberOfThis)
                    {
                        Console.SetCursorPosition(10, Console.CursorTop);
                        Console.Write(books[index].Name);
                        Console.SetCursorPosition(54, Console.CursorTop);
                        Console.Write(books[index].Author);
                        Console.SetCursorPosition(78, Console.CursorTop);
                        Console.Write(books[index].Publisher);
                        Console.SetCursorPosition(102, Console.CursorTop);
                        Console.Write(books[index].PublishingYear);
                        Console.SetCursorPosition(114, Console.CursorTop);
                        Console.Write(books[index].NumberOfBooks);
                        Console.SetCursorPosition(0, Console.CursorTop + 1);
                    }
                }
            }

            Console.SetCursorPosition(0, Console.CursorTop + 2);
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (guide.Length / 2)) + "}", guide));
        }

        public void BookDetail(int mode, List<BookVO> books, int numberOfBook)
        {
            string categories = "  선택  |                  도서                 |       저자      |     출판사     | 출판년도 |  도서상태  | 청구기호  ";
            string line = "-----------------------------------------------------------------------------------------------------------------------";
            string guide = "나가기(ESC)";

            Console.WriteLine(categories);
            Console.WriteLine(line);

            for (int order = 0; order < books.Count; order++)
            {
                if ((int)Math.Floor(books[order].NumberOfThis) == numberOfBook)
                {
                    Console.SetCursorPosition(10, Console.CursorTop);
                    Console.Write(books[order].Name);
                    Console.SetCursorPosition(50, Console.CursorTop);
                    Console.Write(books[order].Author);
                    Console.SetCursorPosition(68, Console.CursorTop);
                    Console.Write(books[order].Publisher);
                    Console.SetCursorPosition(87, Console.CursorTop);
                    Console.Write(books[order].PublishingYear);
                    Console.SetCursorPosition(97, Console.CursorTop);
                    if (books[order].BookCondition == 0) Console.Write("대출가능");
                    else if (books[order].BookCondition == 1) Console.Write("대출중");
                    if (mode != 3)
                    {
                        if (books[order].BookCondition == 2 || books[order].BookCondition == 3)
                            Console.Write("대출불가");
                    }
                    else
                    {
                        if (books[order].BookCondition == 2) Console.Write("분실");
                        else if (books[order].BookCondition == 3) Console.Write("훼손");
                    }
                    Console.SetCursorPosition(109, Console.CursorTop);
                    Console.Write(books[order].NumberOfThis);
                    Console.SetCursorPosition(0, Console.CursorTop + 1);
                }
            }

            Console.SetCursorPosition(0, Console.CursorTop + 2);
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (guide.Length / 2)) + "}", guide));
        }

        public void Announce(string content)
        {
            // 에러 메시지 출력
            Console.Write(String.Format("\n{0," + ((Console.WindowWidth / 2) + ((content.Length - 3) / 2)) + "}", content));

            // 메시지 출력 1초 후 콘솔에서 지우기
            System.Threading.Thread.Sleep(1000);
            ClearCurrentConsoleLine();
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }

        public void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public void ClearOneLetter(int spaces)
        {
            Console.SetCursorPosition(spaces, Console.CursorTop);
            Console.Write(' ');
            Console.SetCursorPosition(spaces, Console.CursorTop);
        }

        public void ClearSearchBar(int currentLeftCursor, string answer)
        {
            Console.SetCursorPosition(currentLeftCursor, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(currentLeftCursor, Console.CursorTop - 1);
            Console.Write(answer);
        }
    }
}
