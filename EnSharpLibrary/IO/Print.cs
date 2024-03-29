﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EnSharpLibrary.Data;
using EnSharpLibrary.Function;

namespace EnSharpLibrary.IO
{
    class Print
    {
        public void SetPointerStartPosition(int cursorLeft, int cursorTop, string pointer)
        {
            Console.SetCursorPosition(0, 0);
            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.Write(pointer);
            Console.SetCursorPosition(cursorLeft, cursorTop);
        }

        public void ChangeUserInformationTitle(int usingMemberID, string name, string address, string phoneNumber, DateTime birthDate)
        {
            if (usingMemberID == Constant.ADMIN)
            {
                SetWindowsizeAndPrintTitle(45, 30, "암호수정");
            }
            else
            {
                SetWindowsizeAndPrintTitle(45, 30, "정보수정");
                UserInformation(name, usingMemberID, address, phoneNumber, birthDate);
            }
        }

        public void UserInformation(string name, int usingMemberID, string address, string phoneNumber, DateTime birthDate)
        {
            SetCursorAndWrite(5, Console.CursorTop + 2, "이    름 | " + name);
            SetCursorAndWrite(5, Console.CursorTop + 2, "학    번 | " + usingMemberID);
            SetCursorAndWrite(5, Console.CursorTop + 2, "주    소 | " + address);
            SetCursorAndWrite(5, Console.CursorTop + 2, "전화번호 | " + phoneNumber);
            SetCursorAndWrite(5, Console.CursorTop + 2, "생    일 | " + birthDate.ToShortDateString());
        }

        public void CompleteToRegisterBook(int cursorLeft, int cursorTop, string message)
        {
            SetCursorAndWrite(cursorLeft, cursorTop, message);
            Console.SetCursorPosition(0, cursorTop + 2);
            ClearCurrentConsoleLine();
            PrintSentence(Constant.COMPLETE_TO_REGISTER, cursorTop + 2, Constant.FOREGROUND_COLOR);
        }

        public void BookInLibrary(List<float> applicationNumber, List<string> bookCondition)
        {
            Console.SetCursorPosition(0, Console.CursorTop + 3);
            PrintSentences(Constant.SEARCHED_BOOK_DETAILED_GUIDLINE, Console.CursorTop);

            for (int index = 0; index < applicationNumber.Count; index++)
            {
                SetCursorAndWrite(43, Console.CursorTop, applicationNumber[index].ToString("n2"));
                SetCursorAndWrite(53, Console.CursorTop, bookCondition[index].ToString());
                Console.WriteLine();
            }
        }

        public void SetCursorAndWrite(int cursorLeft, int cursorTop, string sentence)
        {
            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.Write(sentence);
        }

        public void ClearGuideline(int cursorLocation, int startingLine, int left)
        {
            int length = left - cursorLocation;
            Console.SetCursorPosition(cursorLocation, Console.CursorTop);
            Console.Write(new string(' ', length));
        }

        public void RegisteredBook(List<float> applicationNumber, List<string> bookCondition, List<string> memberID, List<string> dateBorrowed, List<string> dateDeadlineForReturm, List<string> numberOfRenew)
        {
            int countOfBorrowedBook = 0;

            foreach (string guideline in Constant.MANAGE_REGISTERED_BOOK_GUIDLINE) Console.WriteLine(guideline);

            for (int index = 0; index < applicationNumber.Count; index++)
            {
                Console.SetCursorPosition(66, Console.CursorTop);
                Console.Write(applicationNumber[index].ToString("n2"));
                Console.SetCursorPosition(79, Console.CursorTop);
                Console.Write(bookCondition[index]);
                if (string.Compare(bookCondition[index], "대출중") == 0)
                {
                    Console.SetCursorPosition(95, Console.CursorTop);
                    Console.Write(memberID[countOfBorrowedBook]);
                    Console.SetCursorPosition(112, Console.CursorTop);
                    Console.Write(dateBorrowed[countOfBorrowedBook].Remove(10));
                    Console.SetCursorPosition(129, Console.CursorTop);
                    Console.Write(dateDeadlineForReturm[countOfBorrowedBook].Remove(10));
                    Console.SetCursorPosition(148, Console.CursorTop);
                    Console.Write(numberOfRenew[countOfBorrowedBook] + "회");
                    countOfBorrowedBook++;
                }
                Console.WriteLine();
            }
        }

        public void BookDetailInBookAdminMode(int mode, BookAPIVO book, int registeredCount)
        {
            int cursorLeft;

            Console.Clear();
            if (mode == Constant.ADD_BOOK) { Console.SetWindowSize(90, 35); cursorLeft = 90; }
            else if (mode == Constant.BOOK_SEARCH_MODE) { Console.SetWindowSize(90, 35); cursorLeft = 90; }
            else { Console.SetWindowSize(155, 35); cursorLeft = 155;}
            PrintSentences(Constant.ENSHARP_TITLE, 2);
            if (mode == Constant.ADD_BOOK) PrintSentence("도서 검색 및 등록", Console.CursorTop + 1, Constant.FOREGROUND_COLOR);
            else PrintSentence("등록된 도서 관리", Console.CursorTop + 1, Constant.FOREGROUND_COLOR);

            Console.SetCursorPosition(4, Console.CursorTop + 2);
            Console.Write("도  서  명 | ");
            foreach (char title in book.Title)
            {
                Console.Write(title);
                if (Console.CursorLeft >= cursorLeft - 3) Console.SetCursorPosition(17, Console.CursorTop + 1);
            }
            Console.SetCursorPosition(4, Console.CursorTop + 2);
            Console.Write("저      자 | {0}", book.Author);
            Console.SetCursorPosition(4, Console.CursorTop + 2);
            Console.Write("출  판  사 | ");
            foreach (char publisher in book.Publisher)
            {
                Console.Write(publisher);
                if (Console.CursorLeft >= cursorLeft - 3) Console.SetCursorPosition(17, Console.CursorTop + 1);
            }
            Console.SetCursorPosition(4, Console.CursorTop + 2);
            Console.Write("출  판  일 | {0}", book.Pubdate);
            if (mode != Constant.BOOK_SEARCH_MODE)
            {
                Console.SetCursorPosition(4, Console.CursorTop + 2);
                Console.Write("I  S  B  N | {0}", book.Isbn);
                Console.SetCursorPosition(4, Console.CursorTop + 2);
                Console.Write("가      격 | {0}원", book.Price);
                Console.SetCursorPosition(4, Console.CursorTop + 2);
                Console.Write("할  인  가 | {0}원", book.Discount);
            }
            Console.SetCursorPosition(4, Console.CursorTop + 2);
            Console.Write("책  소  개 | ");
            foreach (char description in book.Description)
            {
                Console.Write(description);
                if (Console.CursorLeft >= cursorLeft - 3) Console.SetCursorPosition(17, Console.CursorTop + 1);
            }
            if (mode == Constant.BOOK_SEARCH_MODE) return;
            Console.SetCursorPosition(4, Console.CursorTop + 2);
            if (mode == Constant.MANAGE_REGISTERED_BOOK) return;
            Console.Write("등 록 된  수 량 | {0}권", registeredCount);
            Console.SetCursorPosition(4, Console.CursorTop + 2);
            Console.Write("등 록 할  수 량 | ");
        }

        /// <summary>
        /// 인자로 받은 문자을 가운데정렬하여 출력하는 메소드입니다.
        /// </summary>
        /// <param name="sentence">출력할 문자열</param>
        /// <param name="cursorTop">출력할 줄 정보</param>
        /// <param name="color">출력할 문자열의 색</param>
        public void PrintSentence(string sentence, int cursorTop, ConsoleColor color)
        {
            Console.SetCursorPosition(0, cursorTop);
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

            if (mode == Constant.MEMBER_SEARCH_MODE) space = -41;

            Console.SetCursorPosition(117 + space, 2);
            
            foreach (string title in Constant.ENSHARP_TITLE_IN_SEARCH_MODE)
            {
                Console.WriteLine(title);
                Console.SetCursorPosition(117 + space, Console.CursorTop);
            }

            if (mode == Constant.MANAGE_REGISTERED_BOOK) return;

            // 검색 조건이 '전체'로 설정되어있거나 입력값이 없는 경우
            if (string.Compare(bookName, "") == 0) bookName = string.Copy("전체");
            if (string.Compare(publisher, "") == 0) publisher = string.Copy("전체");
            if (mode == Constant.MEMBER_SEARCH_MODE && string.Compare(publisher, "0") == 0) publisher = string.Copy("전체");
            if (string.Compare(author, "") == 0) author = string.Copy("전체");
            searchingCondition.Add(bookName);
            if (mode != Constant.ADD_BOOK) searchingCondition.Add(publisher);
            if (mode != Constant.ADD_BOOK) searchingCondition.Add(author);

            string[] searchingMenu;
            if (mode == Constant.BOOK_SEARCH_MODE) searchingMenu = Constant.SEARCHING_BOOK_MENU_IN_SEARCHING_MODE;
            else if (mode == Constant.ADD_BOOK) searchingMenu = Constant.SEARCHING_BOOK_MENU_IN_ADDING_MODE;
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
                case Constant.MANAGE_BOOK_MODE: option = Constant.MANAGE_BOOK_OPTION; break;
                case Constant.LOG_MODE: option = Constant.LOG_OPTION; break;
                default: option = Constant.NON_MEMBER_OPTION; break;
            }

            PrintSentence(Constant.LINE_FOR_OPTION[0], cursorTop, Constant.FOREGROUND_COLOR);
            PrintSentence(option[0], cursorTop + 1, Constant.FOREGROUND_COLOR);
            PrintSentence(Constant.LINE_FOR_OPTION[1], cursorTop + 2, Constant.FOREGROUND_COLOR);
            PrintSentence(option[1], cursorTop + 3, Constant.FOREGROUND_COLOR);
            if (mode == Constant.MANAGE_MEMBER_MODE || mode == Constant.MANAGE_BOOK_MODE)
            { PrintSentence(Constant.LINE_FOR_OPTION[2], cursorTop + 4, Constant.FOREGROUND_COLOR); return; }
            PrintSentence(Constant.LINE_FOR_OPTION[1], cursorTop + 4, Constant.FOREGROUND_COLOR);
            PrintSentence(option[2], cursorTop + 5, Constant.FOREGROUND_COLOR);
            if (mode == Constant.LOG_MODE)
            { PrintSentence(Constant.LINE_FOR_OPTION[2], cursorTop + 6, Constant.FOREGROUND_COLOR); return; }
            PrintSentence(Constant.LINE_FOR_OPTION[1], cursorTop + 6, Constant.FOREGROUND_COLOR);
            PrintSentence(option[3], cursorTop + 7, Constant.FOREGROUND_COLOR);
            PrintSentence(Constant.LINE_FOR_OPTION[1], cursorTop + 8, Constant.FOREGROUND_COLOR);
            PrintSentence(option[4], cursorTop + 9, Constant.FOREGROUND_COLOR);
            if (mode != Constant.ADMIN_MODE) { PrintSentence(Constant.LINE_FOR_OPTION[2], cursorTop + 10, Constant.FOREGROUND_COLOR);  return; }
            PrintSentence(Constant.LINE_FOR_OPTION[1], cursorTop + 10, Constant.FOREGROUND_COLOR);
            PrintSentence(option[5], cursorTop + 11, Constant.FOREGROUND_COLOR);
            PrintSentence(Constant.LINE_FOR_OPTION[2], cursorTop + 12, Constant.FOREGROUND_COLOR);
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
                case Constant.ADD_BOOK: categoryAndGuideline = Constant.ADD_BOOK_CATEGORY_AND_GUILDLINE; break;
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

        public void Log(List<string> time,  List<string> member, List<string> content)
        {
            int cursorTop = Console.CursorTop;

            for (int log = 0; log < time.Count; log++)
            {
                SetCursorAndWrite(8, cursorTop + log, time[log]);
                SetCursorAndWrite(38, cursorTop + log, member[log]);
                SetCursorAndWrite(67, cursorTop + log, content[log]);
            }

            PrintSentence(Constant.OUT, Console.CursorTop + 2, Constant.FOREGROUND_COLOR);
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

        public void SearchedBook(int mode, int cursorTop, List<BookAPIVO> searchedBook, string bookName, string publisher, string author)
        {
            string[] guideline;

            if (mode == Constant.ADD_BOOK) guideline = Constant.ADD_NEW_BOOK_GUIDELINE;
            else if (mode == Constant.MANAGE_REGISTERED_BOOK) guideline = Constant.ADD_NEW_BOOK_GUIDELINE;
            else guideline = Constant.SEARCHED_BOOK_GUIDELINE;

            Console.SetWindowSize(155, 35);
            Console.Clear();

            Console.SetCursorPosition(0, cursorTop);
            SearchedTitle(mode, bookName, publisher, author);
            Console.SetCursorPosition(0, Console.CursorTop + 3);
            if (mode == Constant.ADD_BOOK) Console.SetCursorPosition(0, Console.CursorTop + 2);
            foreach (string guide in guideline) Console.WriteLine(guide);

            Books(mode, searchedBook, Console.CursorTop);
            PrintSentence(Constant.OUT, Console.CursorTop + 1, Constant.FOREGROUND_COLOR);
        }

        public void BorrowedBook(int cursorTop, List<BookAPIVO> books, List<HistoryVO> histories, List<string> numbers)
        {
            ClearBoard(cursorTop, Console.WindowHeight);
            Console.SetCursorPosition(0, cursorTop);

            for (int order = 0; order < books.Count; order++)
            {
                Console.SetCursorPosition(18, Console.CursorTop);
                Console.Write(books[order].Title);
                Console.SetCursorPosition(58, Console.CursorTop);
                Console.Write(books[order].Author);
                Console.SetCursorPosition(80, Console.CursorTop);
                Console.Write(histories[order].DateBorrowed);
                Console.SetCursorPosition(95, Console.CursorTop);
                Console.Write(histories[order].NumberOfRenew + "회");
                Console.SetCursorPosition(104, Console.CursorTop);
                Console.Write(histories[order].DateDeadlineForReturn);
                Console.SetCursorPosition(118, Console.CursorTop);
                Console.WriteLine(numbers[order]);
            }

            PrintSentence(Constant.OUT, Console.CursorTop + 1, Constant.FOREGROUND_COLOR);
        }

        public void SearchedMember(List<MemberVO> searchedMember, List<string> borrowedBookForEachMember, string name, string address)
        {
            Console.SetWindowSize(114, 35);
            Console.Clear();

            SearchedTitle(Constant.MEMBER_SEARCH_MODE, name, "", address);
            Console.SetCursorPosition(0, 11);
            foreach (string guidline in Constant.SEARCHED_MEMBER_GUIDELINE) Console.WriteLine(guidline);

            if (searchedMember.Count != 0) Members(searchedMember, borrowedBookForEachMember, Console.CursorTop);
            PrintSentence(Constant.OUT, Console.CursorTop + 2, Constant.FOREGROUND_COLOR);
        }

        public string ShortenKeyword(string keyword, int limit)
        {
            int cursorTop = Console.CursorTop;
            int cursorLeft = Console.CursorLeft;

            while (limit != 0)
            {
                Console.Write(keyword);
                if (Console.CursorLeft > cursorLeft + limit)
                {
                    keyword = keyword.Remove(keyword.Length - 5);
                    keyword = keyword.Insert(keyword.Length, "...");
                }
                else
                {
                    ClearCurrentConsoleLine();
                    Console.SetCursorPosition(0, cursorTop);
                    break;
                }

                ClearCurrentConsoleLine();
                Console.SetCursorPosition(cursorLeft, cursorTop);
            }

            return keyword;
        }

        public void Books(int mode, List<BookAPIVO> books, int cursorTop)
        {
            string title;
            string author;
            string publisher;

            foreach (BookAPIVO book in books)
            {
                title = ShortenKeyword(book.Title, 60);
                author = ShortenKeyword(book.Author, 20);
                publisher = ShortenKeyword(book.Publisher, 18);

                Console.SetCursorPosition(10, Console.CursorTop);
                Console.Write(title);
                Console.SetCursorPosition(73, Console.CursorTop);
                Console.Write(author);
                Console.SetCursorPosition(96, Console.CursorTop);
                Console.Write(publisher);
                Console.SetCursorPosition(115, Console.CursorTop);
                Console.Write(book.Pubdate);
                Console.SetCursorPosition(126, Console.CursorTop);
                if (mode == Constant.MANAGE_REGISTERED_BOOK || mode == Constant.ADD_BOOK) Console.WriteLine(book.Isbn);
                else Console.WriteLine("          " + book.SerialNumber);
            }
        }

        public void Members(List<MemberVO> members, List<string> borrowedBookForEachMember, int cursorTop)
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
                Console.Write(borrowedBookForEachMember[order]);
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

        public void NonAvailableBookMark(int cursorLeft, int cursorTop)
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
            else if (mode == Constant.SUCCESS) Console.Write("상태 변경 완료");
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
        }

        public void BlockCursorMove(int cursorLeft, string pointer)
        {
            Console.SetCursorPosition(cursorLeft, Console.CursorTop);
            Console.Write(pointer + " ");
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
