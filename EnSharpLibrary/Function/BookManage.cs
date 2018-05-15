using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using EnSharpLibrary.Data;
using EnSharpLibrary.IO;

namespace EnSharpLibrary.Function
{
    class BookManage
    {
        Print print = new Print();
        GetValue getValue = new GetValue();
        Tool tool = new Tool();

        int usingMemberID;

        public void ManageBook()
        {
            usingMemberID = Constant.ADMIN;
            bool isFirstLoop = true;

            while (true)
            {
                if (isFirstLoop)
                {
                    // 메뉴 출력
                    print.SetWindowsizeAndPrintTitle(45, 30, "도서 관리");
                    print.MenuOption(Constant.MANAGE_BOOK_MODE, Console.CursorTop + 2);

                    // 기능 선택
                    print.SetCursorAndChoice(38, 12, "◁");

                    isFirstLoop = false;
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                // 기능 선택
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow: tool.UpArrow(38, 12, 2, 2, '◁'); break;
                    case ConsoleKey.DownArrow: tool.DownArrow(38, 12, 2, 2, '◁'); break;
                    case ConsoleKey.Enter: isFirstLoop = GoNextFunction(Console.CursorTop); break;
                    case ConsoleKey.Escape: return;
                    default: print.BlockCursorMove(38, "◁"); break;
                }
            }
        }

        public bool GoNextFunction(int cursorTop)
        {
            switch (Console.CursorTop)
            {
                case Constant.APPEND_BOOK: AddBook("도서 검색 및 등록"); return true;
                case Constant.MANAGE_REGISTERED_BOOK: ManageRegisteredBook(); return true;
            }

            return true;
        }

        public void AddBook(string title)
        {
            string name;
            XmlDocument foundBook;
            List<BookAPIVO> books;

            print.SetWindowsizeAndPrintTitle(45, 30, title);

            while (true)
            {
                print.SearchCategoryAndGuideline(Constant.ADD_BOOK);

                // 정보 수정
                // - 도서명
                name = getValue.Information(19, 11, 15, Constant.ALL_CHARACTER, Constant.ADD_BOOK_CATEGORY_AND_GUILDLINE[1]);
                int errorMode = tool.IsValidAnswer(Constant.ANSWER_BOOK_NAME, name);
                if (errorMode == Constant.ESCAPE_KEY_ERROR) return;
                else if (errorMode != Constant.VALID) { print.ErrorMessage(errorMode, 15); continue; }

                break;
            }

            foundBook = ConnectDatabase.BookSearchResult(name);
            books = getValue.OrganizedFoundBook(foundBook);
            SelectSearchedBook(Constant.ADD_BOOK, books, name, Constant.BLANK, Constant.BLANK);

            return;
        }

        public void ManageRegisteredBook()
        {
            List<BookAPIVO> books = getValue.RegisteredBook();
            print.SearchedBook(Constant.MANAGE_REGISTERED_BOOK, books, Constant.BLANK, Constant.BLANK, Constant.BLANK);
            SelectSearchedBook(Constant.MANAGE_REGISTERED_BOOK, books, Constant.BLANK, Constant.BLANK, Constant.BLANK);
        }

        /// <summary>
        /// 도서 검색을 위한 메소드입니다.
        /// </summary>
        /// <param name="usingMemberID">사용자 학번</param>
        public void SearchBook(int usingMemberID)
        {
            List<BookAPIVO> searchedBook;
            int mode;
            string bookName;
            string publisher;
            string author;
            string guideline = Constant.BOOK_SEARCH_CATEGORY_AND_GUIDELINE[1];

            this.usingMemberID = usingMemberID;

            // 사용자에 따른 모드 설정
            if (usingMemberID == Constant.PUBLIC) mode = Constant.NON_MEMBER_MODE;
            else mode = Constant.MEMBER_MODE;

            while (true)
            {
                print.SetWindowsizeAndPrintTitle(45, 30, Constant.SEARCH_BOOK_TITLE[mode]);
                print.SearchCategoryAndGuideline(Constant.BOOK_SEARCH_MODE);

                // 정보 수집
                bookName = getValue.Information(19, 11, 10, Constant.ALL_CHARACTER, guideline);
                if (string.Compare(bookName, "@입력취소@") == 0) return;
                publisher = getValue.Information(19, 13, 10, Constant.ALL_CHARACTER, guideline);
                if (string.Compare(publisher, "@입력취소@") == 0) return;
                author = getValue.Information(19, 15, 10, Constant.ALL_CHARACTER, guideline);
                if (string.Compare(author, "@입력취소@") == 0) return;

                // 조건 검색
                searchedBook = getValue.SearchBookByCondition(bookName, publisher, author);

                // 상세 정보를 확인할 도서 선택
                if (searchedBook.Count == 0) { print.ErrorMessage(Constant.THERE_IS_NO_BOOK, 22); return; }
                else SelectSearchedBook(Constant.BOOK_SEARCH_MODE, searchedBook, bookName, publisher, author);
            }
        }

        /// <summary>
        /// 도서 검색 후 상세 정보를 확인하기 위해 도서를 선택하는 메소드입니다.
        /// </summary>
        /// <param name="searchedBook">검색된 도서</param>
        /// <param name="bookName">사용자가 입력한 도서명</param>
        /// <param name="publisher">사용자가 입력한 출판사명</param>
        /// <param name="author">사용자가 입력한 작가명</param>
        public void SelectSearchedBook(List<BookVO> searchedBook, string bookName, string publisher, string author)
        {
            bool isFirstLoop = true;
            int cursorTop;

            // 검색된 도서 출력
            cursorTop = Console.CursorTop - 2;
            print.SearchedBook(searchedBook, bookName, publisher, author);

            // 방향키 및 엔터, ESC키를 이용해 기능 수행
            while (true)
            {
                if (isFirstLoop)
                {
                    Console.SetCursorPosition(4, cursorTop);
                    Console.Write('▷');
                    Console.SetCursorPosition(4, cursorTop);
                    isFirstLoop = false;
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow) tool.UpArrow(4, cursorTop, searchedBook.Count, 1, '▷');          // 위로 커서 옮김
                else if (keyInfo.Key == ConsoleKey.DownArrow) tool.DownArrow(4, cursorTop, searchedBook.Count, 1, '▷'); // 밑으로 커서 옮김
                else if (keyInfo.Key == ConsoleKey.Escape) { print.BlockCursorMove(4, "▷"); return; }                   // 나가기
                else if (keyInfo.Key == ConsoleKey.Enter)                                                                // 해당 도서 자세히 보기
                {
                    int index = Console.CursorTop - cursorTop;
                    print.SearchedBook(searchedBook, bookName, publisher, author);
                    Console.SetCursorPosition(45, 3);
                    Console.Write("|" + searchedBook[index].Name + "|");
                    print.ClearBoard(cursorTop, searchedBook.Count + 4);
                    //CheckDetailInformationWhichUserSelected((int)Math.Floor(searchedBook[index].BookID));
                    return;
                }
                else print.BlockCursorMove(4, "▷");                                                                      // 입력 무시 
            }
        }

        public void SelectSearchedBook(int mode, List<BookAPIVO> searchedBook, string name, string publisher, string author)
        {
            bool isFirstLoop = true;
            int cursorTop = 10;
            if (mode == Constant.BOOK_SEARCH_MODE) cursorTop = 12;
            int index;

            // 방향키 및 엔터, ESC키를 이용해 기능 수행
            while (true)
            {
                if (isFirstLoop)
                {
                    Console.SetCursorPosition(0, cursorTop - 2);
                    print.SearchedBook(mode, searchedBook, name, publisher, author);
                    Console.SetCursorPosition(4, cursorTop);
                    Console.Write('▷');
                    Console.SetCursorPosition(4, cursorTop);
                    isFirstLoop = false;
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow) tool.UpArrow(4, cursorTop, searchedBook.Count, 1, '▷');          // 위로 커서 옮김
                else if (keyInfo.Key == ConsoleKey.DownArrow) tool.DownArrow(4, cursorTop, searchedBook.Count, 1, '▷'); // 밑으로 커서 옮김
                else if (keyInfo.Key == ConsoleKey.Escape) { print.BlockCursorMove(4, "▷"); return; }                   // 나가기
                else if (keyInfo.Key == ConsoleKey.Enter)                                                                // 해당 도서 자세히 보기
                {
                    index = Console.CursorTop - cursorTop;
                    CheckDetailInformationWhichUserSelected(mode, searchedBook[index]);
                    isFirstLoop = true;
                }
                else print.BlockCursorMove(4, "▷");                                                                      // 입력 무시 
            }
        }

        /// <summary>
        /// 사용자가 선택한 도서의 상세정보를 보여주는 메소드입니다.
        /// 사용자가 로그인한 경우 대출이 가능합니다.
        /// </summary>
        /// <param name="bookID">정수화된 도서의 청구기호</param>
        //public void CheckDetailInformationWhichUserSelected(int bookID)
        //{
        //    bool isFirstLoop = true;
        //    bool isValidBook = true;
        //    bool isValidUser = true;
        //    int cursorTop = 13;

        //    List<BookVO> books = new List<BookVO>();

        //    books = getValue.SearchBookByID(Constant.BOOK_ID, bookID);
        //    print.SearchedBookWithMoreDetail(books);
        //    print.PrintSentence(Constant.OUT, Console.CursorTop + 1, Constant.FOREGROUND_COLOR);

        //    if (usingMemberID == Constant.PUBLIC || books.Count == 0) { tool.WaitUntilGetEscapeKey(); return; }

        //    // 방향키 및 엔터, ESC키를 이용해 기능 수행
        //    while (true)
        //    {
        //        if (isFirstLoop)
        //        {
        //            print.ClearBoard(cursorTop, books.Count + 4);
        //            books = getValue.SearchBookByID(Constant.BOOK_ID, bookID);
        //            print.SearchedBookWithMoreDetail(books);
        //            print.PrintSentence(Constant.OUT, Console.CursorTop + 1, Constant.FOREGROUND_COLOR);

        //            Console.SetCursorPosition(4, cursorTop);
        //            Console.Write('▷');
        //            Console.SetCursorPosition(4, cursorTop);
        //            isFirstLoop = false;
        //        }

        //        isValidBook = tool.IsValidBook(books[Console.CursorTop - cursorTop].BookID);
        //        isValidUser = tool.IsValidUser(usingMemberID);
        //        if (!isValidBook || !isValidUser) print.NonAvailableLectureMark(4, Console.CursorTop);

        //        ConsoleKeyInfo keyInfo = Console.ReadKey();

        //        if (keyInfo.Key == ConsoleKey.UpArrow) tool.UpArrow(4, cursorTop, books.Count, 1, '▷');          // 위로 커서 옮김
        //        else if (keyInfo.Key == ConsoleKey.DownArrow) tool.DownArrow(4, cursorTop, books.Count, 1, '▷'); // 밑으로 커서 옮김
        //        else if (keyInfo.Key == ConsoleKey.Escape) { print.BlockCursorMove(4, "▷"); return; }            // 나가기
        //        else if (keyInfo.Key == ConsoleKey.Enter)                                                         // 해당 도서 선택
        //        {
        //            // 대출
        //            if (isValidBook && isValidUser)
        //            {
        //                tool.Borrow(usingMemberID, books[Console.CursorTop - cursorTop].BookID);
        //                print.CompleteOrFaildProcess(4, cursorTop, Constant.BORROW);
        //            }
        //            else print.CompleteOrFaildProcess(4, Console.CursorTop, Constant.FAIL);
        //            isFirstLoop = true;
        //        }
        //        else print.BlockCursorMove(4, "▷");                                                                       // 입력 무시 
        //    }
        //}

        public void CheckDetailInformationWhichUserSelected(int mode, BookAPIVO book)
        {
            int registeredCount = 0;
            
            List<string> result = ConnectDatabase.SelectFromDatabase("count", "book_api", "isbn", book.Isbn);
            int countOfTableData = ConnectDatabase.GetCountFromDatabase("book_api", Constant.BLANK, Constant.BLANK);
            
            // 등록된 책이 몇 종류인지 알아냄
            if (result.Count == 0) registeredCount = 0;
            else registeredCount = Int32.Parse(result[0]);
            Console.Clear();
            print.BookDetailInBookAdminMode(mode, book, registeredCount);

            if (mode == Constant.ADD_BOOK) RegisterBook(book, registeredCount, countOfTableData);
            else if (mode == Constant.BOOK_SEARCH_MODE) ModifyBookCondition(book, registeredCount);
            else ModifyBookCondition(book, registeredCount);
        }

        public void RegisterBook(BookAPIVO book, int registeredCount, int countOfTableData)
        {
            int cursorLeft;
            int cursorTop;
            string count;
            int countToRenew;
            int serialNumber;

            // 청구기호에 쓰일 고유번호 계산(정수)
            if (registeredCount == 0) serialNumber = countOfTableData + 1;
            else serialNumber = Int32.Parse(ConnectDatabase.SelectFromDatabase("serial_number", "book_api", "isbn", book.Isbn)[0]);

            // 커서 정보 저장
            cursorLeft = Console.CursorLeft;
            cursorTop = Console.CursorTop;

            // 저장할 권수 입력
            while (true)
            {
                count = getValue.Information(cursorLeft, cursorTop, 3, Constant.ONLY_NUMBER, "숫자 입력(등록 취소:ESC)");

                if (string.Compare(count, "@입력취소@") == 0) return;
                else if (Int32.Parse(count) == 0) print.ErrorMessage(Constant.EXCEED_INPUT_ERROR, cursorTop + 2);
                else if (count.Length == 0) print.ErrorMessage(Constant.LENGTH_ERROR, cursorTop + 2);
                else if (Int32.Parse(count) + registeredCount > 99) print.ErrorMessage(Constant.EXCEED_INPUT_ERROR, cursorTop + 2);
                else { countToRenew = Int32.Parse(count) + registeredCount; break; }
            }

            // sql문 작성
            string values1 = "(\"" + book.Title + "\",\"" + book.Author + "\",\"" + book.Publisher + "\"," + book.Price + "," + book.Discount + ",\"";
            string values2 = book.Pubdate + "\"," + count + ",\"" + book.Isbn + "\",\"" + book.Description + "\"," + serialNumber + ")";

            // 데이터베이스에 저장_1
            if (registeredCount == 0) ConnectDatabase.InsertIntoDatabase("book_api", Constant.ADD_BOOK_COLUMNS, values1 + values2);
            else ConnectDatabase.UpdateToDatabase("book_api", "count", countToRenew.ToString(), "isbn", book.Isbn);

            float applicationNumber;
            string value;

            // 데이터베이스에 저장_2
            for (int create = 0; create < Int32.Parse(count); create++)
            {
                applicationNumber = serialNumber + ((float)(create + registeredCount) / 100);
                value = "(" + applicationNumber.ToString("n2") + ",\"대출 가능\")";
                ConnectDatabase.InsertIntoDatabase("book_detail", Constant.INSERT_NEW_APPLICATION_NUMBER, value);
            }

            // 완료 알림
            Console.SetCursorPosition(4, cursorTop - 2);
            Console.Write("등 록 된  수 량 | {0}", countToRenew);
            Console.SetCursorPosition(0, cursorTop);
            print.ClearCurrentConsoleLine();
            print.PrintSentence("등록이 완료되었습니다!(나가기:ESC)", cursorTop, Constant.FOREGROUND_COLOR);

            tool.WaitUntilGetEscapeKey();
        }

        public void ModifyBookCondition(BookAPIVO book, int registeredCount)
        {
            string guide;
            int cursorTop = Console.CursorTop + 2;
            int index;
            string currentCondition;
            int currentIndex;

            List<float> applicationNumber = new List<float>();
            List<string> bookCondition = new List<string>();
            List<string> memberID = new List<string>();
            List<string> dateBorrowed = new List<string>();
            List<string> dateDeadlineForReturn = new List<string>();
            List<string> numberOfRenew = new List<string>();

            for (int count = 0; count < registeredCount; count++)
            {
                applicationNumber.Add(book.SerialNumber + ((float)count / 100));
                bookCondition.Add(ConnectDatabase.SelectFromDatabase("book_condition", "book_detail", "application_number", applicationNumber[count].ToString("n2"))[0]);
                if (string.Compare(bookCondition[count], "대출중") == 0)
                {
                    memberID.Add(getValue.DetailInformationAboutBorrowedMember(Constant.MEMBER_ID, applicationNumber[count]));
                    dateBorrowed.Add(getValue.DetailInformationAboutBorrowedMember(Constant.DATE_BORROWED, applicationNumber[count]));
                    dateDeadlineForReturn.Add(getValue.DetailInformationAboutBorrowedMember(Constant.DATE_DEADLINE_FOR_RETURN, applicationNumber[count]));
                    numberOfRenew.Add(getValue.DetailInformationAboutBorrowedMember(Constant.NUMBER_OF_RENEW, applicationNumber[count]));
                }
            }

            if (usingMemberID != Constant.ADMIN)
            {
                for (int count = bookCondition.Count - 1; count >= 0; count--)
                {
                    if (!tool.IsBorrowed(bookCondition[count]) && !tool.IsNotRented(bookCondition[count]))
                    {
                        bookCondition.RemoveAt(count);
                        applicationNumber.RemoveAt(count);
                    }
                }

                print.BookInLibrary(applicationNumber, bookCondition);
            }
            else
            {
                print.RegisteredBook(applicationNumber, bookCondition, memberID, dateBorrowed, dateDeadlineForReturn, numberOfRenew);
            }

            Console.SetCursorPosition(0, cursorTop);

            // 방향키 및 엔터, ESC키를 이용해 기능 수행
            while (true)
            {
                index = Console.CursorTop - cursorTop;
                guide = getValue.GuideForModifyingBookCondition(ConnectDatabase.SelectFromDatabase("book_condition", "book_detail", "application_number", applicationNumber[index].ToString("n2"))[0]);
                if (usingMemberID == Constant.PUBLIC) { tool.WaitUntilGetEscapeKey(); return; }
                else if (usingMemberID != Constant.ADMIN) { SelectBookToBorrow(bookCondition, applicationNumber); return; }

                print.SetCursorAndChoice(4, Console.CursorTop, guide);

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                currentCondition = bookCondition[index];
                currentIndex = index;
                bool isChanged = false;

                if (keyInfo.Key == ConsoleKey.UpArrow) tool.UpArrow(4, cursorTop, registeredCount, 1, Console.CursorLeft);          // 위로 커서 옮김
                else if (keyInfo.Key == ConsoleKey.DownArrow) tool.DownArrow(4, cursorTop, registeredCount, 1, Console.CursorLeft); // 밑으로 커서 옮김
                else if (keyInfo.Key == ConsoleKey.Escape) { print.BlockCursorMove(4, "▷"); return; }                              // 나가기
                else if (keyInfo.Key == ConsoleKey.Q && string.Compare(bookCondition[index], "대출 가능") != 0 && !tool.IsDeleted(bookCondition[index]))
                {
                    ConnectDatabase.UpdateToDatabase("book_detail", "book_condition", "대출 가능", "application_number", applicationNumber[index].ToString("n2"));
                    print.ClearGuideline(4, cursorTop, Console.CursorLeft);
                    print.ClearGuideline(79, cursorTop, 89);
                    print.CompleteOrFaildProcess(4, Console.CursorTop, Constant.SUCCESS);
                    print.SetCursorAndWrite(79, Console.CursorTop, "대출 가능");
                    isChanged = true;
                }
                else if (keyInfo.Key == ConsoleKey.W && string.Compare(bookCondition[index], "분실") != 0 && !tool.IsDeleted(bookCondition[index]))
                {
                    ConnectDatabase.UpdateToDatabase("book_detail", "book_condition", "분실", "application_number", applicationNumber[index].ToString("n2"));
                    print.ClearGuideline(4, cursorTop, Console.CursorLeft);
                    print.ClearGuideline(79, cursorTop, 89);
                    print.CompleteOrFaildProcess(4, Console.CursorTop, Constant.SUCCESS);
                    print.SetCursorAndWrite(79, Console.CursorTop, "분실");
                    isChanged = true;
                }
                else if (keyInfo.Key == ConsoleKey.E && string.Compare(bookCondition[index], "훼손") != 0 && !tool.IsDeleted(bookCondition[index]))
                {
                    ConnectDatabase.UpdateToDatabase("book_detail", "book_condition", "훼손", "application_number", applicationNumber[index].ToString("n2"));
                    print.ClearGuideline(4, cursorTop, Console.CursorLeft);
                    print.ClearGuideline(79, cursorTop, 89);
                    print.CompleteOrFaildProcess(4, Console.CursorTop, Constant.SUCCESS);
                    print.SetCursorAndWrite(79, Console.CursorTop, "훼손");
                    isChanged = true;
                }
                else if (keyInfo.Key == ConsoleKey.R && string.Compare(bookCondition[index], "대출 가능") == 0 && !tool.IsDeleted(bookCondition[index]))
                {
                    ConnectDatabase.UpdateToDatabase("book_detail", "book_condition", "보관도서", "application_number", applicationNumber[index].ToString("n2"));
                    print.ClearGuideline(4, cursorTop, Console.CursorLeft);
                    print.ClearGuideline(79, cursorTop, 89);
                    print.CompleteOrFaildProcess(4, Console.CursorTop, Constant.SUCCESS);
                    print.SetCursorAndWrite(79, Console.CursorTop, "보관도서");
                    isChanged = true;
                }
                else if (keyInfo.Key == ConsoleKey.T && !tool.IsDeleted(bookCondition[index]))
                {
                    ConnectDatabase.UpdateToDatabase("book_detail", "book_condition", "삭제", "application_number", applicationNumber[index].ToString("n2"));
                    print.ClearGuideline(4, cursorTop, Console.CursorLeft);
                    print.ClearGuideline(79, cursorTop, 89);
                    print.CompleteOrFaildProcess(4, Console.CursorTop, Constant.SUCCESS);
                    print.SetCursorAndWrite(79, Console.CursorTop, "삭제");
                    isChanged = true;
                }
                else print.BlockCursorMove(4, guide);                                                                 // 입력 무시 
                
                if (tool.IsBorrowed(currentCondition) && isChanged)
                {
                    ConnectDatabase.UpdateToDatabase("history", "date_return", "NOW()", "book_id", applicationNumber[index].ToString("n2"), "date_return");
                    print.SetCursorAndWrite(95, Console.CursorTop, new string(' ', Console.WindowWidth - 96));
                }
            }
        }

        public void SelectBookToBorrow(List<string> bookCondition, List<float> applicationNumber)
        {
            int index;
            int cursorTop = Console.CursorTop + 3;
            string currentCondition;
            bool isValidUser = true;

            Console.SetCursorPosition(0, cursorTop);

            while (true)
            {
                index = Console.CursorTop - cursorTop;
                print.SetCursorAndChoice(30, Console.CursorTop, "▷");

                isValidUser = tool.IsValidUser(usingMemberID);
                if (!tool.IsNormal(bookCondition[index], usingMemberID, applicationNumber[index]) || !isValidUser) print.NonAvailableLectureMark(30, Console.CursorTop);

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                currentCondition = bookCondition[index];

                if (keyInfo.Key == ConsoleKey.UpArrow) tool.UpArrow(30, cursorTop, bookCondition.Count, 1, Console.CursorLeft);          // 위로 커서 옮김
                else if (keyInfo.Key == ConsoleKey.DownArrow) tool.DownArrow(30, cursorTop, bookCondition.Count, 1, Console.CursorLeft); // 밑으로 커서 옮김
                else if (keyInfo.Key == ConsoleKey.Escape) { print.BlockCursorMove(30, "▷"); return; }                                  // 나가기
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    // 대출
                    if (tool.IsNormal(bookCondition[index], usingMemberID, applicationNumber[index]) && isValidUser)
                    {
                        ConnectDatabase.BorrowBook(usingMemberID, applicationNumber[index].ToString("n2"));
                        print.CompleteOrFaildProcess(30, cursorTop, Constant.BORROW);
                    }
                    else print.CompleteOrFaildProcess(30, Console.CursorTop, Constant.FAIL);
                }
                else print.BlockCursorMove(30, "▷");                                                                                    // 입력 무시
            }
        }

        /// <summary>
        /// 사용자가 자신이 대출한 책을 보고, 연장하거나 반납하도록 하는 메소드입니다.
        /// </summary>
        /// <param name="usingMemberID">사용자 학번</param>
        public void ManageBorrowedBook(int usingMemberID)
        {
            bool isFirstLoop = true;
            int cursorTop = 8;

            this.usingMemberID = usingMemberID;

            string conditionalExpression = " WHERE member_id =" + usingMemberID.ToString() + " AND date_return IS NULL";

            List<string> bookID = ConnectDatabase.SelectFromDatabase("book_id", "history", conditionalExpression);
            bookID = getValue.CorrectBookID(bookID);
            List<BookAPIVO> borrowedBook = getValue.SearchBookByID(usingMemberID, bookID);
            List<HistoryVO> histories = getValue.SearchHistoryByID(usingMemberID, bookID);

            print.ManageBorrowedBookTitle();
            Console.SetCursorPosition(0, cursorTop);
            print.BorrowedBook(borrowedBook, histories, bookID);

            while (true)
            {
                if (isFirstLoop)
                {
                    // 대출한 책 목록 출력
                    print.ClearBoard(cursorTop, borrowedBook.Count + 4);
                    Console.SetCursorPosition(0, cursorTop);
                    bookID = ConnectDatabase.SelectFromDatabase("book_id", "history", conditionalExpression);
                    bookID = getValue.CorrectBookID(bookID);
                    borrowedBook = getValue.SearchBookByID(usingMemberID, bookID);
                    histories = getValue.SearchHistoryByID(usingMemberID, bookID);
                    print.BorrowedBook(borrowedBook, histories, bookID);
                    print.PrintSentence(Constant.OUT, Console.CursorTop + 1, Constant.FOREGROUND_COLOR);

                    if (borrowedBook.Count == 0) { tool.WaitUntilGetEscapeKey(); return; }

                    Console.SetCursorPosition(4, cursorTop);
                    Console.Write('▷');
                    Console.SetCursorPosition(4, cursorTop);
                    isFirstLoop = false;
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow) tool.UpArrow(4, cursorTop, borrowedBook.Count, 1, '▷');          // 위로 커서 옮김
                else if (keyInfo.Key == ConsoleKey.DownArrow) tool.DownArrow(4, cursorTop, borrowedBook.Count, 1, '▷'); // 밑으로 커서 옮김
                else if (keyInfo.Key == ConsoleKey.Q)
                {
                    // 연장
                    if (histories[Console.CursorTop - cursorTop].NumberOfRenew != 2)
                    {
                        ConnectDatabase.Extend(bookID[Console.CursorTop - cursorTop]);
                        print.CompleteOrFaildProcess(4, Console.CursorTop, Constant.EXTEND);
                    }
                    else print.CompleteOrFaildProcess(4, cursorTop, Constant.FAIL);
                    isFirstLoop = true;
                }
                else if (keyInfo.Key == ConsoleKey.W)
                {
                    // 반납
                    ConnectDatabase.Return(bookID[Console.CursorTop - cursorTop]);
                    print.CompleteOrFaildProcess(4, Console.CursorTop, Constant.RETURN);
                    isFirstLoop = true;
                }
                else if (keyInfo.Key == ConsoleKey.Escape) { print.BlockCursorMove(4, "▷"); return; }            // 나가기
                else print.BlockCursorMove(4, "▷");                                                              // 입력 무시 
            }
        }
    }
}



