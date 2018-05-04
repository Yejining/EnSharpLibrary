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
        /// <summary>
        /// 인자로 받은 문자을 가운데정렬하여 출력하는 메소드입니다.
        /// </summary>
        /// <param name="sentence">출력할 문자열</param>
        /// <param name="cursorTop">출력할 줄 정보</param>
        /// <param name="color">출력할 문자열의 색</param>
        public void PrintSentence(string sentence, int cursorTop, ConsoleColor color)
        {
            int leftCursor = GetLeftCursorForCenterArrangeMent(sentence);
            Console.SetCursorPosition(leftCursor, cursorTop);
            Console.ForegroundColor = color;
            Console.WriteLine(sentence);
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// 인자로 받은 문자열 배열들을 가운데정렬하여 출력하는 메소드입니다.
        /// </summary>
        /// <param name="sentences">출력할 문자열 배열</param>
        /// <param name="cursorTop">출력할 줄 정보</param>
        public void PrintSentences(string[] sentences, int cursorTop)
        {
            foreach(string sentence in sentences) { PrintSentence(sentence, cursorTop, Constant.FOREGROUND_COLOR); cursorTop++; }
        }

        /// <summary>
        /// 윈도우 사이즈를 설정하고 타이틀을 출력하는 메소드입니다.
        /// </summary>
        /// <param name="windowWidth">윈도우 너비</param>
        /// <param name="windowHeight">윈도우 높이</param>
        /// <param name="title">타이틀</param>
        public void SetWindowsizeAndPrintTitle(int windowWidth, int windowHeight, string title)
        {
            // 윈도우 설정
            Console.SetWindowSize(windowWidth, windowHeight);
            Console.Clear();

            // 타이틀 출력
            PrintSentences(Constant.ENSHARP_TITLE, 4);
            if (string.Compare(title, "") > 0) PrintSentence(title, Console.CursorTop + 1, Constant.FOREGROUND_COLOR);
        }

        /// <summary>
        /// 도서 검색 결과를 나타낼 때 배경을 출력해주는 메소드입니다.
        /// </summary>
        /// <param name="bookName">사용자가 검색시 입력한 도서명</param>
        /// <param name="publisher">사용자가 검색시 입력한 출판사</param>
        /// <param name="author">사용자가 검색시 입력한 작가</param>
        public void SearchedTitle(int mode, string bookName, string publisher, string author)
        {
            List<string> searchingCondition = new List<string>();
            int space = 0;

            if (mode == Constant.MEMBER_SEARCH_MODE) space = -23;

            Console.SetCursorPosition(85 + space, 2);
            
            foreach (string title in Constant.ENSHARP_TITLE_IN_SEARCH_MODE)
            {
                Console.WriteLine(title);
                Console.SetCursorPosition(85 + space, Console.CursorTop);
            }

            // 검색 조건이 '전체'로 설정되어있거나 입력값이 없는 경우
            if (string.Compare(bookName, "") == 0) bookName = string.Copy("전체");
            if (string.Compare(publisher, "") == 0) publisher = string.Copy("전체");
            if (mode == Constant.MEMBER_SEARCH_MODE && string.Compare(publisher, "0") == 0) publisher = string.Copy("전체");
            if (string.Compare(author, "") == 0) author = string.Copy("전체");
            searchingCondition.Add(bookName);
            searchingCondition.Add(publisher);
            searchingCondition.Add(author);

            string[] searchingMenu;
            if (mode == Constant.BOOK_SEARCH_MODE) searchingMenu = Constant.SEARCHING_BOOK_MENU_IN_SEARCHING_MODE;
            else searchingMenu = Constant.SEARCHING_MEMBER_MENU_IN_SEARCHING_MODE;

            // 배경 출력
            Console.SetCursorPosition(7, 1);
            for (int item = 1; item < searchingMenu.Count(); item++)
            {
                Console.SetCursorPosition(7, Console.CursorTop + 2);
                Console.Write(searchingMenu[item]);
                Console.Write(searchingCondition[item - 1]);
            }
        }

        public void ManageBorrowedBookTitle()
        {
            Console.SetWindowSize(128, 30);
            Console.Clear();

            Console.SetCursorPosition(85, 2);

            foreach (string title in Constant.ENSHARP_TITLE_IN_SEARCH_MODE)
            {
                Console.WriteLine(title);
                Console.SetCursorPosition(85, Console.CursorTop);
            }

            Console.SetCursorPosition(0, 6);
            foreach (string guideline in Constant.MANAGE_BORROWED_BOOK_GUIDELINE) Console.WriteLine(guideline);
        }

        /// <summary>
        /// Menu Class에서 옵션을 출력하는 메소드입니다.
        /// </summary>
        /// <param name="mode">프로그램 사용 모드</param>
        /// <param name="cursorTop">출력할 줄 정보</param>
        public void MenuOption(int mode, int cursorTop)
        {
            string[] option;

            switch (mode)
            {
                case Constant.NON_MEMBER_MODE: option = Constant.NON_MEMBER_OPTION; break;
                case Constant.MEMBER_MODE: option = Constant.MEMBER_OPTION; break;
                case Constant.ADMIN_MODE: option = Constant.ADMIN_OPTION; break;
                case Constant.MANAGE_MEMBER_MODE: option = Constant.MANAGE_MEMBER_OPTION; break;
                default: option = Constant.NON_MEMBER_OPTION; break;
            }

            PrintSentence(Constant.LINE_FOR_OPTION[0], cursorTop, Constant.FOREGROUND_COLOR);
            PrintSentence(option[0], cursorTop + 1, Constant.FOREGROUND_COLOR);
            PrintSentence(Constant.LINE_FOR_OPTION[1], cursorTop + 2, Constant.FOREGROUND_COLOR);
            PrintSentence(option[1], cursorTop + 3, Constant.FOREGROUND_COLOR);
            if (mode == Constant.MANAGE_MEMBER_MODE) { PrintSentence(Constant.LINE_FOR_OPTION[2], cursorTop + 4, Constant.FOREGROUND_COLOR); return; }
            PrintSentence(Constant.LINE_FOR_OPTION[1], cursorTop + 4, Constant.FOREGROUND_COLOR);
            PrintSentence(option[2], cursorTop + 5, Constant.FOREGROUND_COLOR);
            PrintSentence(Constant.LINE_FOR_OPTION[1], cursorTop + 6, Constant.FOREGROUND_COLOR);
            PrintSentence(option[3], cursorTop + 7, Constant.FOREGROUND_COLOR);
            PrintSentence(Constant.LINE_FOR_OPTION[1], cursorTop + 8, Constant.FOREGROUND_COLOR);
            PrintSentence(option[4], cursorTop + 9, Constant.FOREGROUND_COLOR);
            PrintSentence(Constant.LINE_FOR_OPTION[2], cursorTop + 10, Constant.FOREGROUND_COLOR);
        }

        /// <summary>
        /// 검색 항목과 항목별 입력 조건을 출력해주는 메소드입니다.
        /// </summary>
        public void SearchCategoryAndGuideline(int mode)
        {
            string[] categoryAndGuideline;

            Console.SetCursorPosition(0, 11);
            for (int count = 0; count < Console.WindowHeight - 12; count++)
            {
                ClearCurrentConsoleLine();
                Console.SetCursorPosition(0, Console.CursorTop + 1);
            }

            switch (mode)
            {
                case Constant.BOOK_SEARCH_MODE: categoryAndGuideline = Constant.BOOK_SEARCH_CATEGORY_AND_GUIDELINE; break;
                case Constant.LOG_IN_MODE: categoryAndGuideline = Constant.LOGIN_SEARCH_CATEGORY_AND_GUIDELINE; break;
                case Constant.JOIN_IN: categoryAndGuideline = Constant.JOIN_SEARCH_CATEGORY_AND_GUIDELINE; break;
                case Constant.MEMBER_SEARCH_MODE: categoryAndGuideline = Constant.MEMBER_SEARCH_CATEGORY_AND_GUIDELINE; break;
                default: categoryAndGuideline = Constant.BOOK_SEARCH_CATEGORY_AND_GUIDELINE; break;
            }

            Console.SetCursorPosition(10, 11);

            for (int sentence = 0; sentence < categoryAndGuideline.Length; sentence++)
            {
                if (sentence % 2 == 0)  // 조건 출력
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(categoryAndGuideline[sentence]);
                }
                else                     // 가이드라인 출력
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(categoryAndGuideline[sentence]);
                    Console.SetCursorPosition(10, Console.CursorTop + 1);
                }
            }
        }

        /// <summary>
        /// 검색창이 비었을 때 안내멘트를 출력해주는 메소드입니다.
        /// </summary>
        /// <param name="guideline">안내멘트</param>
        /// <param name="cursorLeft">커서 설정 변수(들여쓰기)</param>
        /// <param name="cursorTop">커서 설정 변수(줄)</param>
        public void GuidelineForSearch(string guideline, int cursorLeft, int cursorTop)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.Write(guideline);
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(cursorLeft, cursorTop);
        }

        /// <summary>
        /// 사용자가 검색창에 검색어를 입력하면 안내문을 지워주는 메소드입니다.
        /// </summary>
        /// <param name="cursorLeft">커서 설정 변수(들여쓰기)</param>
        /// <param name="isNumber">사용자가 입력한 문자가 유효한지 검사</param>
        /// <param name="letter">사용자가 입력한 문자</param>
        public void DeleteGuideLine(int cursorLeft, bool isValid, ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.Enter || keyInfo.Key == ConsoleKey.Tab) return;

            Console.Write(new string(' ', Console.WindowWidth - cursorLeft));
            Console.SetCursorPosition(cursorLeft, Console.CursorTop);
            if (isValid) Console.Write(keyInfo.KeyChar);
        }

        /// <summary>
        /// 도서 검색 결과를 출력하는 메소드입니다.
        /// </summary>
        /// <param name="searchedBook">검색된 도서</param>
        /// <param name="bookName">사용자가 입력한 도서명</param>
        /// <param name="publisher">사용자가 입력한 출판사</param>
        /// <param name="author">사용자가 입력한 저자</param>
        public void SearchedBook(List<BookVO> searchedBook, string bookName, string publisher, string author)
        {
            Console.SetWindowSize(130, 35);
            Console.Clear();

            SearchedTitle(Constant.BOOK_SEARCH_MODE, bookName, publisher, author);
            Console.SetCursorPosition(0, 11);
            foreach (string guideline in Constant.SEARCHED_BOOK_GUIDELINE) Console.WriteLine(guideline);
            
            Books(searchedBook, Console.CursorTop);
        }

        public void SearchedBookWithMoreDetail(List<BookVO> books)
        {
            Console.SetCursorPosition(0, 11);
            foreach (string guideline in Constant.SEARCHED_BOOK_DETAILED_GUIDLINE) Console.WriteLine(guideline);
            Console.SetCursorPosition(0, 13);

            for (int order = 0; order < books.Count; order++)
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
                Console.Write(books[order].BookCondition);
                Console.SetCursorPosition(109, Console.CursorTop);
                Console.Write(books[order].BookID);
                Console.SetCursorPosition(120, Console.CursorTop);
                Console.Write(books[order].Price + "원");
                Console.SetCursorPosition(0, Console.CursorTop + 1);
            }
        }

        public void BorrowedBook(List<BookVO> books, List<HistoryVO> histories)
        {
            for (int order = 0; order < books.Count; order++)
            {
                Console.SetCursorPosition(18, Console.CursorTop);
                Console.Write(books[order].Name);
                Console.SetCursorPosition(58, Console.CursorTop);
                Console.Write(books[order].Author);
                Console.SetCursorPosition(80, Console.CursorTop);
                Console.Write(histories[order].DateBorrowed.ToShortDateString());
                Console.SetCursorPosition(95, Console.CursorTop);
                Console.Write(histories[order].NumberOfRenew + "회");
                Console.SetCursorPosition(104, Console.CursorTop);
                Console.Write(histories[order].DateDeadlineForReturn.ToShortDateString());
                Console.SetCursorPosition(118, Console.CursorTop);
                Console.Write(books[order].BookID);
                Console.SetCursorPosition(0, Console.CursorTop + 1);
            }
        }

        public void SearchedMember(List<MemberVO> searchedMember, string name, string age, string address)
        {
            Console.SetWindowSize(100, 35);
            Console.Clear();

            SearchedTitle(Constant.MEMBER_SEARCH_MODE, name, age, address);
            Console.SetCursorPosition(0, 11);
            foreach (string guidline in Constant.SEARCHED_MEMBER_GUIDELINE) Console.WriteLine(guidline);

            if (searchedMember.Count != 0) Members(searchedMember, Console.CursorTop);
            PrintSentence(Constant.OUT, Console.CursorTop + 2, Constant.FOREGROUND_COLOR);
        }

        public void Books(List<BookVO> books, int cursorTop)
        {
            Console.SetCursorPosition(0, cursorTop);
            
            for (int order = 0; order < books.Count; order++)
            {
                if ((int)(books[order].BookID - (int)books[order].BookID) * 100 == 0)
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
                    Console.SetCursorPosition(120, Console.CursorTop);
                    Console.Write(books[order].Price + "원");
                    Console.SetCursorPosition(0, Console.CursorTop + 1);
                }
            }
            
            PrintSentence(Constant.OUT, Console.CursorTop + 2, Constant.FOREGROUND_COLOR);
        }

        public void Members(List<MemberVO> members, int cursorTop)
        {
            Console.SetCursorPosition(0, cursorTop);

            for (int order = 0; order < members.Count; order++)
            {
                Console.SetCursorPosition(10, Console.CursorTop);
                Console.Write(members[order].Name);
                Console.SetCursorPosition(23, Console.CursorTop);
                Console.Write(members[order].MemberID);
                Console.SetCursorPosition(34, Console.CursorTop);
                Console.Write(members[order].Address);
                Console.SetCursorPosition(68, Console.CursorTop);
                Console.Write(members[order].PhoneNumber);
                Console.SetCursorPosition(85, Console.CursorTop);
                Console.SetCursorPosition(0, Console.CursorTop + 1);
            }
        }

        /// <summary>
        /// 에러 메시지를 출력하는 메소드입니다.
        /// 에러 메시지 출력 후 1초 후에 메시지는 사라집니다.
        /// </summary>
        /// <param name="mode">에러 종류</param>
        /// <param name="cursorTop">커서 정보(줄)</param>
        public void ErrorMessage(int mode, int cursorTop)
        {
            Console.SetCursorPosition(0, cursorTop);
            ClearCurrentConsoleLine();
            PrintSentence(Constant.ERROR_MESSAGE[mode], cursorTop, ConsoleColor.Red);
            System.Threading.Thread.Sleep(2000);
            Console.SetCursorPosition(0, cursorTop);
            ClearCurrentConsoleLine();
        }

        public void NonAvailableLectureMark(int cursorLeft, int cursorTop)
        {
            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.Write(" X");
            Console.SetCursorPosition(cursorLeft + 1, cursorTop);
        }

        public void ClearBoard(int cursorTop, int countOfConsoleLine)
        {
            Console.SetCursorPosition(0, cursorTop);
            for (int clear = 1; clear <= countOfConsoleLine; clear++)
            {
                ClearCurrentConsoleLine();
                Console.SetCursorPosition(0, cursorTop + clear);
            }
        }

        /// <summary>
        /// 콘솔에서 한 글자만을 삭제하는 메소드입니다.
        /// </summary>
        /// <param name="spaces">삭제할 글자의 왼쪽 커서 위치</param>
        public void ClearOneLetter(int spaces)
        {
            Console.SetCursorPosition(spaces, Console.CursorTop);
            Console.Write(' ');
            Console.SetCursorPosition(spaces, Console.CursorTop);
        }

        /// <summary>
        /// 콘솔에서 현재 커서가 위치한 줄을 비워줍니다.
        /// </summary>
        public void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        /// <summary>
        /// 사용자가 유효하지 않은 문자를 입력한 경우 그 문자를 콘솔창에서 지워주는 메소드입니다.
        /// </summary>
        /// <param name="currentCursor">커서 설정 변수(들여쓰기)</param>
        /// <param name="cursorTop">커서 설정 변수(줄)</param>
        public void InvalidInput(ConsoleKeyInfo keyInfo, int currentCursor, int cursorTop)
        {
            string space;

            if (System.Text.RegularExpressions.Regex.IsMatch(keyInfo.KeyChar.ToString(), Constant.KOREAN_PATTERN))
                space = "  ";
            else space = " ";

            Console.SetCursorPosition(currentCursor, cursorTop);
            Console.Write(space);
            Console.SetCursorPosition(currentCursor, cursorTop);
        }

        public void CompleteOrFaildProcess(int cursorLeft, int cursorTop, int mode)
        {
            Console.SetCursorPosition(cursorLeft, cursorTop);
            if (mode == Constant.BORROW) Console.Write("대출");
            else if (mode == Constant.EXTEND) Console.Write("연장");
            else if (mode == Constant.RETURN) Console.Write("반납");
            else Console.Write(" X");
            System.Threading.Thread.Sleep(500);
            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.Write(new string(' ', 4));
            Console.SetCursorPosition(cursorLeft + 1, cursorTop);
            Console.Write('▷');
        }

        public void Announce(string content, int cursorTop)
        {
            // 메시지 출력
            PrintSentence(content, cursorTop, Constant.FOREGROUND_COLOR);

            // 메시지 출력 2초 후 콘솔에서 지우기
            System.Threading.Thread.Sleep(2000);
            ClearCurrentConsoleLine();
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }

        public void ClearSearchBar(int currentLeftCursor, string answer, int searchType)
        {
            Console.SetCursorPosition(currentLeftCursor, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(currentLeftCursor, Console.CursorTop - 1);
            if (searchType != 2) Console.Write(answer);
            else Console.Write(new string('*', answer.Length));
        }

        public void SetCursorAndChoice(int cursorLeft, int cursorTop, string pointer)
        {
            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.Write(pointer);
            Console.SetCursorPosition(cursorLeft, cursorTop);
        }

        public void BlockCursorMove(int cursorLeft, string pointer)
        {
            Console.SetCursorPosition(cursorLeft, Console.CursorTop);
            Console.Write(pointer);
            Console.SetCursorPosition(cursorLeft, Console.CursorTop);
        }

        /// <summary>
        /// 가운데 정렬로 충렬하기 위해 들여쓰기할 공간을 계산하는 메소드입니다.
        /// </summary>
        /// <param name="sentence">출력할 문장</param>
        /// <returns>가운데 정렬을 위한 들여쓰기 공백</returns>
        public int GetLeftCursorForCenterArrangeMent(string sentence)
        {
            int leftCursor;

            if (sentence.Length == 0) return 0;

            Console.Write(sentence);
            leftCursor = Console.WindowWidth / 2 - Console.CursorLeft / 2;
            ClearCurrentConsoleLine();

            return leftCursor;
        }
    }
}
