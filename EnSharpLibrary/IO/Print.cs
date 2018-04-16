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

        public void SpecificallySearchTitle(int programMode, int detailMode)
        {
            StringBuilder title = new StringBuilder();

            if (programMode == 1) title.AppendFormat("비회원 도서검색 -> ");
            else if (programMode == 2) title.AppendFormat("도서 보기 -> ");
            else title.AppendFormat("도서 관리 -> ");

            switch (detailMode)
            {
                case 2:
                    title.AppendFormat("도서명 검색 ");
                    break;
                case 3:
                    title.AppendFormat("출판사 검색");
                    break;
                case 4:
                    title.AppendFormat("저자 검색");
                    break;
            }

            Title(title.ToString());
        }

        public void BookDetailTitle(int mode, int detailMode)
        {
            StringBuilder title = new StringBuilder();
            
            if (mode == 1) title.AppendFormat("비회원 도서검색 -> ");
            else if (mode == 2) title.AppendFormat("도서 보기 -> ");
            else title.AppendFormat("도서 관리 -> ");

            switch (detailMode)
            {
                case 1:
                    title.AppendFormat("전체 도서 목록 -> 도서 상세               ");
                    break;
                case 2:
                    title.AppendFormat("도서명 검색 -> 도서 상세               ");
                    break;
                case 3:
                    title.AppendFormat("출판사 검색 -> 도서 상세               ");
                    break;
                case 4:
                    title.AppendFormat("저자 검색 -> 도서 상세               ");
                    break;
            }

            Title(title.ToString());
        }

        public void MenuOption(int mode)
        {
            string line1 = "┏                     ┓";
            string line2 = "┣                     ┫";
            string line3 = "┗                     ┛";
            string menu1 = "비회원 도서검색     ";
            string menu2 = "로그인   ";
            string menu3 = "회원가입   ";
            string menu4 = "관리자 로그인      ";
            string menu5 = "종료  ";
            string menu6 = "도서 보기  ";
            string menu7 = "대출도서 보기     ";
            string menu8 = "정보수정   ";
            string menu9 = "로그아웃   ";
            string menu10 = "도서 관리   ";
            string menu11 = "회원 관리   ";
            string menu12 = "암호 수정   ";
            string[] member = { menu6, menu7, menu8, menu9, menu5 };
            string[] admin = { menu10, menu11, menu12, menu9, menu5 };
            string[] toPrint = new string[] { menu1, menu2, menu3, menu4, menu5 };

            if (mode == 2) toPrint = member;
            else if (mode == 3) toPrint = admin;
           
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line1.Length / 2)) + "}", line1));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (toPrint[0].Length / 2)) + "}", toPrint[0]));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (toPrint[1].Length / 2)) + "}", toPrint[1]));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (toPrint[2].Length / 2)) + "}", toPrint[2]));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (toPrint[3].Length / 2)) + "}", toPrint[3]));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (toPrint[4].Length / 2)) + "}", toPrint[4]));
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

        public void MemberEditOption()
        {
            string line1 = "┏                     ┓";
            string line2 = "┣                     ┫";
            string line3 = "┗                     ┛";
            string menu1 = "내 정보  ";
            string menu2 = "암호 변경   ";
            string menu3 = "주소 변경   ";
            string menu4 = "전화번호 변경    ";
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

        public void BorrowedBook(int usingMemberNumber, List<BookVO> books, List<float> bookList)
        {
            string categories = " 선택 |             도서            |     저자    |    출판사    | 출판년도 |  대출일  | 반납 예정일 | 청구기호 | 연장 ";
            string line = "-----------------------------------------------------------------------------------------------------------------------";
            string guide = "나가기(ESC)";
            string message = "찾는 도서가 없습니다!         ";
            
            Console.WriteLine(categories);
            Console.WriteLine(line);

            if (bookList.Count == 0)
            {
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (message.Length / 2)) + "}", message));
                Console.SetCursorPosition(0, Console.CursorTop + 2);
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (guide.Length / 2)) + "}", guide));
                return;
            }

            foreach (BookVO book in books)
                foreach (float number in bookList) if (book.NumberOfThis == number)
                    {
                        Console.SetCursorPosition(8, Console.CursorTop);
                        Console.Write(book.Name);
                        Console.SetCursorPosition(37, Console.CursorTop);
                        Console.Write(book.Author);
                        Console.SetCursorPosition(51, Console.CursorTop);
                        Console.Write(book.Publisher);
                        Console.SetCursorPosition(69, Console.CursorTop);
                        Console.Write(book.PublishingYear);
                        Console.SetCursorPosition(78, Console.CursorTop);
                        Console.Write(book.Rental.ToString("yy/MM/dd"));
                        Console.SetCursorPosition(90, Console.CursorTop);
                        Console.Write(book.ExpectedToReturn.ToString("yy/MM/dd"));
                        Console.SetCursorPosition(104, Console.CursorTop);
                        Console.Write(book.NumberOfThis);
                        Console.SetCursorPosition(115, Console.CursorTop);
                        Console.Write(book.NumberOfRenew);
                        Console.SetCursorPosition(0, Console.CursorTop + 1);
                    }

            Console.SetCursorPosition(0, Console.CursorTop + 2);
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (guide.Length / 2)) + "}", guide));
        }

        public void BookDetail(int mode, List<BookVO> books, List<int> indexOfBooks)
        {
            string categories = "  선택  |                  도서                 |       저자      |     출판사     | 출판년도 |  도서상태  | 청구기호  ";
            string line = "-----------------------------------------------------------------------------------------------------------------------";
            string guide = "나가기(ESC)";

            Console.WriteLine(categories);
            Console.WriteLine(line);
            
            for (int order = 0; order < indexOfBooks.Count; order++)
            {
                Console.SetCursorPosition(10, Console.CursorTop);
                Console.Write(books[indexOfBooks[order]].Name);
                Console.SetCursorPosition(50, Console.CursorTop);
                Console.Write(books[indexOfBooks[order]].Author);
                Console.SetCursorPosition(68, Console.CursorTop);
                Console.Write(books[indexOfBooks[order]].Publisher);
                Console.SetCursorPosition(87, Console.CursorTop);
                Console.Write(books[indexOfBooks[order]].PublishingYear);
                Console.SetCursorPosition(97, Console.CursorTop);
                if (books[indexOfBooks[order]].BookCondition == 0) Console.Write("대출가능");
                else if (books[indexOfBooks[order]].BookCondition == 1) Console.Write("대출중");
                if (mode != 3)
                {
                    if (books[indexOfBooks[order]].BookCondition == 2 || books[order].BookCondition == 3)
                        Console.Write("대출불가");
                }
                else
                {
                    if (books[indexOfBooks[order]].BookCondition == 2) Console.Write("분실");
                    else if (books[indexOfBooks[order]].BookCondition == 3) Console.Write("훼손");
                }
                Console.SetCursorPosition(109, Console.CursorTop);
                Console.Write(books[indexOfBooks[order]].NumberOfThis);
                Console.SetCursorPosition(0, Console.CursorTop + 1);
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

        public void ClearSearchBar(int currentLeftCursor, string answer, int searchType)
        {
            Console.SetCursorPosition(currentLeftCursor, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(currentLeftCursor, Console.CursorTop - 1);
            if (searchType != 2) Console.Write(answer);
            else Console.Write(new string('*', answer.Length));
        }

        public void SetCursorAndChoice(int cursorLeft, int numberForTop, char pointer)
        {
            Console.SetCursorPosition(cursorLeft, Console.CursorTop - numberForTop);
            Console.Write(pointer);
            Console.SetCursorPosition(cursorLeft, Console.CursorTop);
        }

        public void BlockCursorMove(int cursorLeft, string pointer)
        {
            Console.SetCursorPosition(cursorLeft, Console.CursorTop);
            Console.Write(pointer);
            Console.SetCursorPosition(cursorLeft, Console.CursorTop);
        }

        public void MemberInformation(MemberVO user)
        {
            string content1 = "▷ 이름 : ";
            string content2 = "▷ 학번 : ";
            string content3 = "▷ 전화번호 : ";
            string content4 = "▷ 주소 : ";
            string guide = "나가기(ESC)";
            
            Title(user.Name);

            Console.SetCursorPosition(45, Console.CursorTop);
            Console.Write(content1);
            Console.Write(user.Name);

            Console.SetCursorPosition(45, Console.CursorTop + 2);
            Console.Write(content2);
            Console.Write(user.IdentificationNumber);

            Console.SetCursorPosition(45, Console.CursorTop + 2);
            Console.Write(content3);
            Console.Write(user.PhoneNumber);

            Console.SetCursorPosition(45, Console.CursorTop + 2);
            Console.Write(content4);
            Console.Write(user.Address);

            Console.SetCursorPosition(55, Console.CursorTop + 4);
            Console.WriteLine(guide);

            while (true)
            {
                ConsoleKeyInfo keyInfo;

                keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Escape) return;
            }
        }
    }
}
