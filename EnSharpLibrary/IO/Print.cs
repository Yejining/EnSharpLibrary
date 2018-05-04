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

        /// <summary>
        /// 지금 검색하는 검색 조건의 색을 칠하는 메소드입니다.
        /// 이전 검색 조건은 원래 색으로 바꾸어줍니다.
        /// </summary>
        /// <param name="modeTowhite">원래 색으로 바꿀 검색 조건</param>
        /// <param name="whiteCursorTop">원래 색으로 바꿀 텍스트의 줄 정보</param>
        /// <param name="modeToHighlight">색칠할 검색 조건</param>
        /// <param name="highlightCursorTop">색칠할 텍스트의 줄 정보</param>
        //public void ColorMenu(int modeToGray, int whiteCursorTop, int modeToHighlight, int highlightCursorTop)
        //{
        //    // 원래 색으로 바꿀 검색 조건이 있을 경우 해당 텍스트의 색을 원래 색으로 변환
        //    if (modeToGray != Constant.NONE)
        //    {
        //        Console.ForegroundColor = ConsoleColor.White;
        //        Console.SetCursorPosition(6, whiteCursorTop);
        //        Console.Write(Constant.BOOK_SEARCH_CATEGORY_AND_GUIDELINE[modeToGray - 1]);
        //    }

        //    // 강조할 검색 조건의 텍스트 색 변환
        //    Console.ForegroundColor = ConsoleColor.Yellow;
        //    Console.SetCursorPosition(6, highlightCursorTop);
        //    if (modeToHighlight != Constant.CHECK) Console.Write(Constant.BOOK_SEARCH_CATEGORY_AND_GUIDELINE[modeToHighlight - 1]);
        //    else { Console.SetCursorPosition(10, highlightCursorTop); Console.Write("강의시간표 조회하기(엔터)"); }
        //    Console.ForegroundColor = ConsoleColor.White;
        //}

        /// <summary>
        /// 지금 검색하는 검색 조건의 색을 칠하는 메소드입니다.
        /// 이전 검색 조건은 원래 색으로 바꾸어줍니다.
        /// </summary>
        /// <param name="menu">이전 검색 조건</param>
        /// <param name="whiteCursorTop">원래 색으로 바꿀 텍스트의 줄 정보</param>
        /// <param name="modeToHighlight">색칠할 검색 조건</param>
        /// <param name="highlightCursorTop">색칠할 텍스트의 줄 정보</param>
        //public void ColorMenu(string menu, int whiteCursorTop, int modeToHighlight, int highlightCursorTop)
        //{
        //    // 원래 색으로 바꿀 텍스트의 색을 원래 색으로 변환
        //    Console.ForegroundColor = ConsoleColor.White;
        //    Console.SetCursorPosition(6, whiteCursorTop);
        //    Console.Write(menu);

        //    // 강조할 텍스트가 있을 경우 해당 텍스트 강조
        //    if (modeToHighlight != Constant.NONE)
        //    {
        //        Console.ForegroundColor = ConsoleColor.Yellow;
        //        Console.SetCursorPosition(6, highlightCursorTop);
        //        if (modeToHighlight != Constants.CHECK) Console.Write(Constant.BOOK_SEARCH_CATEGORY_AND_GUIDELINE[modeToHighlight - 1]);
        //        else { Console.SetCursorPosition(10, highlightCursorTop); Console.Write("강의시간표 조회하기(엔터)"); }
        //        Console.ForegroundColor = ConsoleColor.White;
        //    }
        //}

        //public void BookSearchTitle(int mode)
        //{
        //    if (mode == 1) Title("비회원 도서검색     ");
        //    else if (mode == 2) Title("도서 보기");
        //    else Title("도서 관리");
        //}

        //public void BookSearchAllBooksTitle(int mode)
        //{
        //    if (mode == 1) Title("비회원 도서검색 → 전체 도서 목록               ");
        //    else if (mode == 2) Title("도서 보기 → 전체 도서 목록");
        //    else Title("도서 관리 → 전체 도서 목록");
        //}

        //public void SpecificallySearchTitle(int programMode, int detailMode)
        //{
        //    StringBuilder title = new StringBuilder();

        //    if (programMode == 1) title.AppendFormat("비회원 도서검색 -> ");
        //    else if (programMode == 2) title.AppendFormat("도서 보기 -> ");
        //    else title.AppendFormat("도서 관리 -> ");

        //    switch (detailMode)
        //    {
        //        case 2:
        //            title.AppendFormat("도서명 검색 ");
        //            break;
        //        case 3:
        //            title.AppendFormat("출판사 검색");
        //            break;
        //        case 4:
        //            title.AppendFormat("저자 검색");
        //            break;
        //    }

        //    Title(title.ToString());
        //}

        //public void BookDetailTitle(int mode, int detailMode)
        //{
        //    StringBuilder title = new StringBuilder();

        //    if (mode == 1) title.AppendFormat("비회원 도서검색 -> ");
        //    else if (mode == 2) title.AppendFormat("도서 보기 -> ");
        //    else title.AppendFormat("도서 관리 -> ");

        //    switch (detailMode)
        //    {
        //        case 1:
        //            title.AppendFormat("전체 도서 목록 -> 도서 상세               ");
        //            break;
        //        case 2:
        //            title.AppendFormat("도서명 검색 -> 도서 상세               ");
        //            break;
        //        case 3:
        //            title.AppendFormat("출판사 검색 -> 도서 상세               ");
        //            break;
        //        case 4:
        //            title.AppendFormat("저자 검색 -> 도서 상세               ");
        //            break;
        //    }

        //    Title(title.ToString());
        //}

        public void BookSearchOption(int programMode)
        {
            string line1 = "┏                     ┓";
            string line2 = "┣                     ┫";
            string line3 = "┗                     ┛";
            string menu1 = "전체 도서 목록      ";
            string menu2 = "도서명 검색   ";
            string menu3 = "출판사 검색   ";
            string menu4 = "저자 검색   ";
            string menu5 = "뒤로  ";
            string menu8 = "도서 등록   ";

            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line1.Length / 2)) + "}", line1));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu1.Length / 2)) + "}", menu1));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu2.Length / 2)) + "}", menu2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu3.Length / 2)) + "}", menu3));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu4.Length / 2)) + "}", menu4));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            if (programMode != 3)
            {
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu5.Length / 2)) + "}", menu5));
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line3.Length / 2)) + "}", line3));
            }
            else
            {
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu8.Length / 2)) + "}", menu8));
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu5.Length / 2)) + "}", menu5));
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line3.Length / 2)) + "}", line3));
            }
        }

        public void MemberEditOption()
        {
            string line1 = "┏                     ┓";
            string line2 = "┣                     ┫";
            string line3 = "┗                     ┛";
            string menu1 = "내 정보  ";
            string menu2 = "암호 변경   ";
            string menu3 = "주소 변경    ";
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

        public void MemberSearchOption()
        {
            string line1 = "┏                     ┓";
            string line2 = "┣                     ┫";
            string line3 = "┗                     ┛";
            string menu1 = "전체 회원 목록     ";
            string menu2 = "이름 검색   ";
            string menu3 = "학번 검색   ";
            string menu4 = "주소 검색   ";
            string menu7 = "회원 등록   ";
            string menu8 = "뒤로  ";

            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line1.Length / 2)) + "}", line1));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu1.Length / 2)) + "}", menu1));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu2.Length / 2)) + "}", menu2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu3.Length / 2)) + "}", menu3));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu4.Length / 2)) + "}", menu4));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu7.Length / 2)) + "}", menu7));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line2.Length / 2)) + "}", line2));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (menu8.Length / 2)) + "}", menu8));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (line3.Length / 2)) + "}", line3));
        }

        //public void AllBooks(List<BookVO> books)
        //{
        //    string categories = "  선택  |                    도서                   |          저자         |       출판사       |  출판년도  |  수량  ";
        //    string line = "-----------------------------------------------------------------------------------------------------------------------";
        //    string guide = "나가기(ESC)";

        //    Console.WriteLine(categories);
        //    Console.WriteLine(line);

        //    for (int order = 0; order < books.Count; order++)
        //    {
        //        if (books[order].OrderOfBooks == 0)
        //        {
        //            Console.SetCursorPosition(10, Console.CursorTop);
        //            Console.Write(books[order].Name);
        //            Console.SetCursorPosition(54, Console.CursorTop);
        //            Console.Write(books[order].Author);
        //            Console.SetCursorPosition(78, Console.CursorTop);
        //            Console.Write(books[order].Publisher);
        //            Console.SetCursorPosition(102, Console.CursorTop);
        //            Console.Write(books[order].PublishingYear);
        //            Console.SetCursorPosition(114, Console.CursorTop);
        //            Console.Write(books[order].NumberOfBooks);
        //            Console.SetCursorPosition(0, Console.CursorTop + 1);
        //        }
                
        //    }

        //    Console.SetCursorPosition(0, Console.CursorTop + 2);
        //    Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (guide.Length / 2)) + "}", guide));
        //}

        //public void AllMembers(List<BookVO> books, List<MemberVO> members)
        //{
        //    string categories = "  선택  |    이름    |   학번   |              주소               |    전화번호    |  대출도서 번호  |  연체도서 번호  ";
        //    string line = "-----------------------------------------------------------------------------------------------------------------------";
        //    string guide = "나가기(ESC)";

        //    List<float> overdueBooks = new List<float>();
        //    TimeSpan date;

        //    Console.WriteLine(categories);
        //    Console.WriteLine(line);

        //    for (int order = 0; order < members.Count; order++)
        //    {
        //        Console.SetCursorPosition(10, Console.CursorTop);
        //        Console.Write(members[order].Name);
        //        Console.SetCursorPosition(23, Console.CursorTop);
        //        Console.Write(members[order].IdentificationNumber);
        //        Console.SetCursorPosition(34, Console.CursorTop);
        //        Console.Write(members[order].Address);
        //        Console.SetCursorPosition(68, Console.CursorTop);
        //        Console.Write(members[order].PhoneNumber);
        //        Console.SetCursorPosition(85, Console.CursorTop);
        //        for (int i = 0; i < members[order].BorrowedBook.Count; i++)
        //        {
        //            if (i == 0) Console.Write("{0}", members[order].BorrowedBook[i]);
        //            else Console.Write(",{0}", members[order].BorrowedBook[i]);
        //        }
        //        Console.SetCursorPosition(103, Console.CursorTop);
                


        //        overdueBooks.Clear();
        //        for (int book = 0; book < members[order].BorrowedBook.Count; book++)
        //        {
        //            for (int j = 0; j < books.Count; j++)
        //            {
        //                if (members[order].BorrowedBook[book] == books[j].NumberOfThis)
        //                {
        //                    date = DateTime.Now - books[book].ExpectedToReturn;
        //                    if (date.Days > 0) overdueBooks.Add(books[j].NumberOfThis);
        //                }
        //            }
        //        }

        //        for (int i = 0; i < overdueBooks.Count; i++)
        //        {
        //            if (i == 0) Console.Write("{0}", overdueBooks[i]);
        //            else Console.Write(",{0}", overdueBooks[i]);
        //        }
        //        Console.SetCursorPosition(0, Console.CursorTop + 1);
        //    }

        //    Console.SetCursorPosition(0, Console.CursorTop + 2);
        //    Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (guide.Length / 2)) + "}", guide));
        //}

        // 1 : 도서명 검색
        // 2 : 출판사 검색
        // 3 : 저자 검색
        //public void BookSearchResult(List<BookVO> books, List<int>foundBooks)
        //{
        //    List<int> indexesOfSearchResult = foundBooks;

        //    string categories = "  선택  |                    도서                   |          저자         |       출판사       |  출판년도  |  수량  ";
        //    string line = "-----------------------------------------------------------------------------------------------------------------------";
        //    string guide = "나가기(ESC)";
        //    string message = "찾는 도서가 없습니다!         ";
            
        //    Console.SetCursorPosition(0, Console.CursorTop + 1);
        //    Console.WriteLine(categories);
        //    Console.WriteLine(line);

        //    if (indexesOfSearchResult.Count == 0)
        //    {
        //        Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (message.Length / 2)) + "}", message));
        //        Console.SetCursorPosition(0, Console.CursorTop + 2);
        //        Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (guide.Length / 2)) + "}", guide));
        //        return;
        //    }

        //    for (int inList = 0; inList < indexesOfSearchResult.Count; inList++)
        //    {
        //        for (int index = 0; index < books.Count; index++)
        //        {
        //            if (indexesOfSearchResult[inList] == books[index].NumberOfThis)
        //            {
        //                Console.SetCursorPosition(10, Console.CursorTop);
        //                Console.Write(books[index].Name);
        //                Console.SetCursorPosition(54, Console.CursorTop);
        //                Console.Write(books[index].Author);
        //                Console.SetCursorPosition(78, Console.CursorTop);
        //                Console.Write(books[index].Publisher);
        //                Console.SetCursorPosition(102, Console.CursorTop);
        //                Console.Write(books[index].PublishingYear);
        //                Console.SetCursorPosition(114, Console.CursorTop);
        //                Console.Write(books[index].NumberOfBooks);
        //                Console.SetCursorPosition(0, Console.CursorTop + 1);
        //            }
        //        }
        //    }

        //    Console.SetCursorPosition(0, Console.CursorTop + 2);
        //    Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (guide.Length / 2)) + "}", guide));
        //}

        //public void MemberSearchResult(List<MemberVO> members, List<BookVO> books, List<int> foundMembers)
        //{
        //    List<int> indexesOfSearchResult = foundMembers;

        //    string categories = "  선택  |    이름    |   학번   |              주소               |    전화번호    |  대출도서 번호  |  연체도서 번호  ";
        //    string line = "-----------------------------------------------------------------------------------------------------------------------";
        //    string guide = "나가기(ESC)";
        //    string message = "찾는 도서가 없습니다!         ";

        //    Console.SetCursorPosition(0, Console.CursorTop + 1);
        //    Console.WriteLine(categories);
        //    Console.WriteLine(line);

        //    List<float> overdueBooks = new List<float>();
        //    TimeSpan date;

        //    if (indexesOfSearchResult.Count == 0)
        //    {
        //        Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (message.Length / 2)) + "}", message));
        //        Console.SetCursorPosition(0, Console.CursorTop + 2);
        //        Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (guide.Length / 2)) + "}", guide));
        //        return;
        //    }

        //    for (int inList = 0; inList < indexesOfSearchResult.Count; inList++)
        //    {
        //        for (int index = 0; index < members.Count; index++)
        //        {
        //            if (indexesOfSearchResult[inList] == members[index].IdentificationNumber)
        //            {
        //                Console.SetCursorPosition(10, Console.CursorTop);
        //                Console.Write(members[index].Name);
        //                Console.SetCursorPosition(23, Console.CursorTop);
        //                Console.Write(members[index].IdentificationNumber);
        //                Console.SetCursorPosition(34, Console.CursorTop);
        //                Console.Write(members[index].Address);
        //                Console.SetCursorPosition(68, Console.CursorTop);
        //                Console.Write(members[index].PhoneNumber);
        //                Console.SetCursorPosition(85, Console.CursorTop);
        //                for (int i = 0; i < members[index].BorrowedBook.Count; i++)
        //                {
        //                    if (i == 0) Console.Write("{0}", members[index].BorrowedBook[i]);
        //                    else Console.Write(",{0} ", members[index].BorrowedBook[i]);
        //                }
        //                Console.SetCursorPosition(103, Console.CursorTop);

        //                overdueBooks.Clear();
        //                foreach (BookVO book in books)
        //                    foreach (float number in members[index].BorrowedBook)
        //                        if (book.NumberOfThis == number)
        //                        {
        //                            date = DateTime.Now - book.ExpectedToReturn;
        //                            if (date.Days > 0) overdueBooks.Add(book.NumberOfThis);
        //                        }

        //                for (int i = 0; i < overdueBooks.Count; i++)
        //                {
        //                    if (i == 0) Console.Write("{0}", overdueBooks[i]);
        //                    else Console.Write(",{0} ", overdueBooks[i]);
        //                }
        //                Console.SetCursorPosition(0, Console.CursorTop + 1);
        //            }
        //        }
        //    }

        //    Console.SetCursorPosition(0, Console.CursorTop + 2);
        //    Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (guide.Length / 2)) + "}", guide));
        //}

        //public void BorrowedBook(int usingMemberNumber, List<BookVO> books, List<float> bookList)
        //{
        //    string categories = " 선택 |             도서            |     저자    |    출판사    | 출판년도 |  대출일  | 반납 예정일 | 청구기호 | 연장 ";
        //    string line = "-----------------------------------------------------------------------------------------------------------------------";
        //    string guide = "나가기(ESC)";
        //    string message = "찾는 도서가 없습니다!         ";
            
        //    Console.WriteLine(categories);
        //    Console.WriteLine(line);

        //    if (bookList.Count == 0)
        //    {
        //        Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (message.Length / 2)) + "}", message));
        //        Console.SetCursorPosition(0, Console.CursorTop + 2);
        //        Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (guide.Length / 2)) + "}", guide));
        //        return;
        //    }

        //    foreach (BookVO book in books)
        //        foreach (float number in bookList) if (book.NumberOfThis == number)
        //            {
        //                Console.SetCursorPosition(8, Console.CursorTop);
        //                Console.Write(book.Name);
        //                Console.SetCursorPosition(37, Console.CursorTop);
        //                Console.Write(book.Author);
        //                Console.SetCursorPosition(51, Console.CursorTop);
        //                Console.Write(book.Publisher);
        //                Console.SetCursorPosition(69, Console.CursorTop);
        //                Console.Write(book.PublishingYear);
        //                Console.SetCursorPosition(78, Console.CursorTop);
        //                Console.Write(book.Rental.ToString("yy/MM/dd"));
        //                Console.SetCursorPosition(90, Console.CursorTop);
        //                Console.Write(book.ExpectedToReturn.ToString("yy/MM/dd"));
        //                Console.SetCursorPosition(104, Console.CursorTop);
        //                Console.Write(book.NumberOfThis);
        //                Console.SetCursorPosition(115, Console.CursorTop);
        //                Console.Write(book.NumberOfRenew);
        //                Console.SetCursorPosition(0, Console.CursorTop + 1);
        //            }

        //    Console.SetCursorPosition(0, Console.CursorTop + 2);
        //    Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (guide.Length / 2)) + "}", guide));
        //}

        //public void BookDetail(int mode, List<BookVO> books, List<int> indexOfBooks)
        //{
        //    string categories = "  선택  |                  도서                 |       저자      |     출판사     | 출판년도 |  도서상태  | 청구기호  ";
        //    string line = "-----------------------------------------------------------------------------------------------------------------------";
        //    string guide = "나가기(ESC)";

        //    Console.WriteLine(categories);
        //    Console.WriteLine(line);
            
        //    for (int order = 0; order < indexOfBooks.Count; order++)
        //    {
        //        Console.SetCursorPosition(10, Console.CursorTop);
        //        Console.Write(books[indexOfBooks[order]].Name);
        //        Console.SetCursorPosition(50, Console.CursorTop);
        //        Console.Write(books[indexOfBooks[order]].Author);
        //        Console.SetCursorPosition(68, Console.CursorTop);
        //        Console.Write(books[indexOfBooks[order]].Publisher);
        //        Console.SetCursorPosition(87, Console.CursorTop);
        //        Console.Write(books[indexOfBooks[order]].PublishingYear);
        //        Console.SetCursorPosition(97, Console.CursorTop);
        //        if (books[indexOfBooks[order]].BookCondition == 0) Console.Write("대출가능");
        //        else if (books[indexOfBooks[order]].BookCondition == 1) Console.Write("대출중");
        //        if (mode != 3)
        //        {
        //            if (books[indexOfBooks[order]].BookCondition == 2 || books[order].BookCondition == 3)
        //                Console.Write("대출불가");
        //        }
        //        else
        //        {
        //            if (books[indexOfBooks[order]].BookCondition == 2) Console.Write("분실");
        //            else if (books[indexOfBooks[order]].BookCondition == 3) Console.Write("훼손");
        //        }
        //        Console.SetCursorPosition(109, Console.CursorTop);
        //        Console.Write(books[indexOfBooks[order]].NumberOfThis);
        //        Console.SetCursorPosition(0, Console.CursorTop + 1);
        //    }

        //    Console.SetCursorPosition(0, Console.CursorTop + 2);
        //    Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (guide.Length / 2)) + "}", guide));
        //}

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

        // 1 : 내정보
        // 2 : 회원 정보
        //public void MemberInformation(MemberVO user, int mode)
        //{
        //    string content1 = "▷ 이름 : ";
        //    string content2 = "▷ 학번 : ";
        //    string content3 = "▷ 전화번호 : ";
        //    string content4 = "▷ 주소 : ";
        //    string guide = "나가기(ESC)";
            
        //    Title(user.Name);

        //    Console.SetCursorPosition(45, Console.CursorTop);
        //    Console.Write(content1);
        //    Console.Write(user.Name);

        //    Console.SetCursorPosition(45, Console.CursorTop + 2);
        //    Console.Write(content2);
        //    Console.Write(user.IdentificationNumber);

        //    Console.SetCursorPosition(45, Console.CursorTop + 2);
        //    Console.Write(content3);
        //    Console.Write(user.PhoneNumber);

        //    Console.SetCursorPosition(45, Console.CursorTop + 2);
        //    Console.Write(content4);
        //    Console.Write(user.Address);

        //    if (mode == 1)
        //    {
        //        Console.SetCursorPosition(55, Console.CursorTop + 4);
        //        Console.WriteLine(guide);
        //    }

        //    while (mode == 1)
        //    {
        //        ConsoleKeyInfo keyInfo;

        //        keyInfo = Console.ReadKey();
        //        if (keyInfo.Key == ConsoleKey.Escape) return;
        //    }
        //}

        //public void BookCondition(int indexOfBook, List<BookVO> books, List<MemberVO> members)
        //{
        //    StringBuilder bookInformation1 = new StringBuilder();
        //    StringBuilder bookInformation2 = new StringBuilder();
        //    StringBuilder bookInformation3 = new StringBuilder();
        //    StringBuilder bookInformation4 = new StringBuilder();

        //    Title(books[indexOfBook].Name);
        //    bookInformation1.AppendFormat("▷ {0}/{1}/{2}/{3}", books[indexOfBook].Name, books[indexOfBook].Author,
        //        books[indexOfBook].Publisher, books[indexOfBook].PublishingYear);
        //    bookInformation2.AppendFormat("청구기호 : {0}", books[indexOfBook].NumberOfThis);
        //    if (books[indexOfBook].BookCondition == 0) bookInformation3.AppendFormat("도서상태 : 대출 가능");
        //    else if (books[indexOfBook].BookCondition == 1)
        //    {
        //        bookInformation3.AppendFormat("도서상태 : 대출중({0})", books[indexOfBook].NumberOfMember);
        //        bookInformation4.AppendFormat("대출일 : {0} / 반납 예정일 : {1}", books[indexOfBook].Rental.ToString("yy/MM/dd"), books[indexOfBook].ExpectedToReturn.ToString("yy/MM/dd"));
        //    }
        //    else if (books[indexOfBook].BookCondition == 2) bookInformation3.AppendFormat("도서상태 : 분실");
        //    else bookInformation3.AppendFormat("도서상태 : 훼손");

        //    Console.SetCursorPosition(10, Console.CursorTop);
        //    Console.WriteLine(bookInformation1);
        //    Console.SetCursorPosition(13, Console.CursorTop + 1);
        //    Console.WriteLine(bookInformation2);
        //    Console.SetCursorPosition(13, Console.CursorTop + 1);
        //    Console.WriteLine(bookInformation3);
        //    if (books[indexOfBook].BookCondition == 1)
        //    {
        //        Console.SetCursorPosition(13, Console.CursorTop + 1);
        //        Console.WriteLine(bookInformation4);
        //    }
        //    else Console.SetCursorPosition(13, Console.CursorTop + 2);
        //}

        public int BookManageOption(int topCursor)
        {
            string option1 = "▷ 분실 및 훼손, 발견 등록";
            string option2 = "▷ 도서 삭제";
            string option3 = "▷ 뒤로";
            bool isFirstLoop = true;
            int cursor;

            Console.SetCursorPosition(10, topCursor);
            Console.WriteLine(option1);
            Console.SetCursorPosition(10, topCursor + 2);
            Console.WriteLine(option2);
            Console.SetCursorPosition(10, topCursor + 4);
            Console.WriteLine(option3);

            while (true)
            {
                if(isFirstLoop)
                {
                    SetCursorAndChoice(40, 5, "☜");
                    isFirstLoop = false;
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    ClearOneLetter(40);
                    if (Console.CursorTop > 18) Console.SetCursorPosition(40, Console.CursorTop - 2);
                    Console.Write("☜");
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    ClearOneLetter(40);
                    if (Console.CursorTop < 22) Console.SetCursorPosition(40, Console.CursorTop + 2);
                    Console.Write("☜");
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        cursor = Console.CursorTop;

                        for (int i = 18; i <= 22; i++)
                        {
                            ClearCurrentConsoleLine();
                            Console.SetCursorPosition(0, Console.CursorTop + 1);
                        }

                        switch (cursor)
                        {
                            case 18:
                                return 1;
                            case 20:
                                return 2;
                            case 22:
                                return -1;
                        }

                        isFirstLoop = true;
                    }
                    else
                    {
                        Console.SetCursorPosition(40, Console.CursorTop);
                        Console.Write("☜ ");
                        Console.SetCursorPosition(40, Console.CursorTop);
                    }
                }
            }
        }

        public int BookConditionManageOption(int cursorLeft, int cursorTop)
        {
            string option1 = "▷ 분실";
            string option2 = "▷ 훼손";
            string option3 = "▷ 발견";
            string option4 = "▷ 뒤로";
            bool isFirstLoop = true;
            int cursor;

            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.WriteLine(option1);
            Console.SetCursorPosition(cursorLeft, cursorTop + 2);
            Console.WriteLine(option2);
            Console.SetCursorPosition(cursorLeft, cursorTop + 4);
            Console.WriteLine(option3);
            Console.SetCursorPosition(cursorLeft, cursorTop + 6);
            Console.WriteLine(option4);

            while (true)
            {
                if (isFirstLoop)
                {
                    SetCursorAndChoice(cursorLeft + 10, 5, "☜");
                    isFirstLoop = false;
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    ClearOneLetter(cursorLeft + 10);
                    if (Console.CursorTop > 18) Console.SetCursorPosition(cursorLeft + 10, Console.CursorTop - 2);
                    Console.Write("☜");
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    ClearOneLetter(cursorLeft + 10);
                    if (Console.CursorTop < 24) Console.SetCursorPosition(cursorLeft + 10, Console.CursorTop + 2);
                    Console.Write("☜");
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        cursor = Console.CursorTop;

                        for (int i = 18; i <= 24; i++)
                        {
                            ClearCurrentConsoleLine();
                            Console.SetCursorPosition(0, Console.CursorTop + 1);
                        }

                        switch (cursor)
                        {
                            case 18:
                                return 1;
                            case 20:
                                return 2;
                            case 22:
                                return 3;
                            case 24:
                                return -1;
                        }

                        isFirstLoop = true;
                    }
                    else
                    {
                        Console.SetCursorPosition(cursorLeft + 10, Console.CursorTop);
                        Console.Write("☜ ");
                        Console.SetCursorPosition(cursorLeft + 10, Console.CursorTop);
                    }
                }
            }
        }

        public void MemberManageOption(int cursorLeft, int cursorTop)
        {
            string option1 = "▶ 회원 삭제";
            string option2 = "▶ 주소 변경";
            string option3 = "▶ 전화번호 변경";
            string option4 = "▶ 뒤로";

            Console.SetCursorPosition(cursorLeft, cursorTop + 2);
            Console.Write(option1);
            Console.SetCursorPosition(cursorLeft, cursorTop + 4);
            Console.Write(option2);
            Console.SetCursorPosition(cursorLeft, cursorTop + 6);
            Console.Write(option3);
            Console.SetCursorPosition(cursorLeft, cursorTop + 8);
            Console.Write(option4);

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
