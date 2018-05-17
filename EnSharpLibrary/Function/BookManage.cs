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

        public int UsingMemberID
        {
            set { usingMemberID = value; }
        }

        public void SearchInRegisteredBook()
        {
            string name;
            string publisher;
            string author;
            
            List<BookAPIVO> searchedBook;
            BookAPIVO book;

            GetSearchWordAndSearchInRegisteredBook(out searchedBook, out name, out publisher, out author);
            if (getValue.IsCanceled(name) || getValue.IsCanceled(publisher) || getValue.IsCanceled(author)) return;

            book = ListBooksAndChooseOneBook(Constant.BOOK_SEARCH_MODE, searchedBook, name, publisher, author);
            if (book == null) return;
        }

        public void ExtendOrReturnBook()
        {
            List<string> bookID;
            List<BookAPIVO> borrowedBook;
            List<HistoryVO> histories;

            ConsoleKeyInfo keyInfo;
            bool isFirstLoop = true;
            int cursorTop = 8;

            // 회원이 빌린 책의 청구기호와 책 정보, 대출 기록 불러옴
            getValue.InformationAboutBorrowedBookFromMember(usingMemberID, out bookID, out borrowedBook, out histories);

            print.ManageBorrowedBookTitle();

            while (true)
            {
                if (isFirstLoop)
                {
                    // 대출한 책 목록 출력
                    getValue.InformationAboutBorrowedBookFromMember(usingMemberID, out bookID, out borrowedBook, out histories);
                    print.BorrowedBook(cursorTop, borrowedBook, histories, bookID);
                    tool.SetPointerStartPosition(4, cursorTop, "▷");

                    if (borrowedBook.Count == 0) { tool.WaitUntilGetEscapeKey(); return; }
                    
                    isFirstLoop = false;
                }

                keyInfo = Console.ReadKey();

                // 키 입력을 통해 도서 연장/반납 등의 기능 수행
                if (keyInfo.Key == ConsoleKey.UpArrow) tool.UpArrow(4, cursorTop, borrowedBook.Count, 1, "▷");
                else if (keyInfo.Key == ConsoleKey.DownArrow) tool.DownArrow(4, cursorTop, borrowedBook.Count, 1, "▷");
                else if (keyInfo.Key == ConsoleKey.Q) isFirstLoop = Extend(histories[Console.CursorTop - cursorTop], bookID[Console.CursorTop - cursorTop]);
                else if (keyInfo.Key == ConsoleKey.W) isFirstLoop = Return(bookID[Console.CursorTop - cursorTop]);
                else if (keyInfo.Key == ConsoleKey.Escape) return;
                else print.BlockCursorMove(4, "▷");                                                               
            }
        }

        public void AddBookThroughNaverAPI()
        {
            string name;
            List<BookAPIVO> books;
            BookAPIVO bookToRegister;

            print.SetWindowsizeAndPrintTitle(45, 30, "도서 검색 및 등록");

            // 검색할 도서명 입력
            name = getValue.BookNameToSearch();
            if (getValue.IsCanceled(name)) return;

            // 네이버에서 검색어와 일치하는 도서 검색, 등록할 책 선택 후 등록
            books = getValue.MatchedBooksFromSearchWord(name);
            bookToRegister = ListBooksAndChooseOneBook(Constant.ADD_BOOK, books, name, Constant.BLANK, Constant.BLANK);
            if (bookToRegister != null) RegisterBook(bookToRegister);

            return;
        }

        public bool Extend(HistoryVO history, string bookID)
        {
            if (history.NumberOfRenew != 2)
            {
                ConnectDatabase.Extend(bookID);
                print.CompleteOrFaildProcess(4, Console.CursorTop, Constant.EXTEND);
            }
            else print.CompleteOrFaildProcess(4, Console.CursorTop, Constant.FAIL);

            return true;
        }

        public bool Return(string bookID)
        {
            ConnectDatabase.Return(bookID);
            print.CompleteOrFaildProcess(4, Console.CursorTop, Constant.RETURN);

            return true;
        }
        
        public void GetSearchWordAndSearchInRegisteredBook(out List<BookAPIVO> searchedBook, out string bookName, out string publisher, out string author)
        {
            string guideline = Constant.BOOK_SEARCH_CATEGORY_AND_GUIDELINE[1];

            searchedBook = null;
            bookName = Constant.BLANK;
            publisher = Constant.BLANK;
            author = Constant.BLANK;

            print.SetWindowsizeAndPrintTitle(45, 30, Constant.SEARCH_BOOK_TITLE[0]);
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
        }

        public BookAPIVO ListBooksAndChooseOneBook(int mode, List<BookAPIVO> searchedBook, string name, string publisher, string author)
        {
            BookAPIVO book;
            bool isFirstLoop = true;
            int registeredCount;
            int cursorTop = 10;
            if (mode == Constant.BOOK_SEARCH_MODE) cursorTop = 12;

            // 방향키 및 엔터, ESC키를 이용해 기능 수행
            while (true)
            {
                if (isFirstLoop)
                {
                    print.SearchedBook(mode, cursorTop, searchedBook, name, publisher, author);
                    tool.SetPointerStartPosition(4, cursorTop, "▷");
                    isFirstLoop = false;
                }

                if (searchedBook.Count == 0) { tool.WaitUntilGetEscapeKey(); return null; }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow) tool.UpArrow(4, cursorTop, searchedBook.Count, 1, "▷");          // 위로 커서 옮김
                else if (keyInfo.Key == ConsoleKey.DownArrow) tool.DownArrow(4, cursorTop, searchedBook.Count, 1, "▷"); // 밑으로 커서 옮김
                else if (keyInfo.Key == ConsoleKey.Escape) return null;                                                  // 나가기
                else if (keyInfo.Key == ConsoleKey.Enter)                                                                // 해당 도서 자세히 보기
                {
                    book = searchedBook[Console.CursorTop - cursorTop];
                    registeredCount = getValue.RegisteredCount(book.Isbn);
                    print.BookDetailInBookAdminMode(mode, book, registeredCount);
                    if (usingMemberID != Constant.ADMIN) SelectBookToBorrow(book);
                    else return book;
                    isFirstLoop = true;
                }
                else print.BlockCursorMove(4, "▷");                                                                      // 입력 무시 
            }
        }
        
        public void RegisterBook(BookAPIVO book)
        {
            int cursorTop;
            string countToRegister;
            int registeredCount;
            int serialNumber;

            // 데이터베이스에 등록된 책의 숫자와 청구기호 구하기
            getValue.SerialNumberAndRegisteredCount(book.Isbn, out serialNumber, out registeredCount);

            cursorTop = Console.CursorTop;

            // 새로 등록할 수량 입력
            countToRegister = getValue.BookCountToRegister(Console.CursorLeft, cursorTop, registeredCount);
            if (getValue.IsCanceled(countToRegister)) return;

            // 데이터베이스에 책 등록
            ConnectDatabase.RegisterBookToDatabase(book, registeredCount, countToRegister, serialNumber);
            print.CompleteToRegisterBook(4, cursorTop - 2, "등 록 된  수 량 | " + (registeredCount + Int32.Parse(countToRegister)));
            tool.WaitUntilGetEscapeKey();
        }

        public void ChangeBookCondition(BookAPIVO book)
        {
            string guide;
            int cursorTop = Console.CursorTop + 2;
            int registeredCount;
            int index;
            string currentCondition;
            List<float> bookID;
            List<string> condition;
            List<string> memberID;
            List<string> dateBorrowed;
            List<string> deadline;
            List<string> numberOfRenew;
            
            // 도서 정보 구하기
            registeredCount = getValue.RegisteredCount(book.Isbn);
            getValue.BookCondition(usingMemberID, registeredCount, book, out bookID, out condition);
            getValue.BookCondition(registeredCount, book, bookID, condition, out memberID, out dateBorrowed, out deadline, out numberOfRenew);

            // 도서 정보 출력
            print.RegisteredBook(bookID, condition, memberID, dateBorrowed, deadline, numberOfRenew);
            Console.SetCursorPosition(4, cursorTop);

            // 방향키 및 엔터, ESC키를 이용해 기능 수행
            while (true)
            {
                index = Console.CursorTop - cursorTop;
                currentCondition = condition[index];

                guide = getValue.GuideForModifyingBookCondition(bookID[index]);
                Console.Write(guide);

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow) tool.UpArrow(4, cursorTop, registeredCount, 1, Console.CursorLeft);          // 위로 커서 옮김
                else if (keyInfo.Key == ConsoleKey.DownArrow) tool.DownArrow(4, cursorTop, registeredCount, 1, Console.CursorLeft); // 밑으로 커서 옮김
                else if (keyInfo.Key == ConsoleKey.Escape) return;                                                                  // 나가기
                else if (getValue.IsQualifiedToBeNormal(keyInfo.Key, currentCondition)) ChangeBookCondition(cursorTop, bookID[index], "대출 가능", currentCondition);
                else if (getValue.IsQualifiedToBeLost(keyInfo.Key, currentCondition)) ChangeBookCondition(cursorTop, bookID[index], "분실", currentCondition);
                else if (getValue.IsQualifiedToBeDamaged(keyInfo.Key, currentCondition)) ChangeBookCondition(cursorTop, bookID[index], "훼손", currentCondition);
                else if (getValue.IsQualifiedToBeSaved(keyInfo.Key, currentCondition)) ChangeBookCondition(cursorTop, bookID[index], "보관도서", currentCondition);
                else if (getValue.IsQualiiedToBeDeleted(keyInfo.Key, currentCondition)) ChangeBookCondition(cursorTop, bookID[index], "삭제", currentCondition);
                else print.BlockCursorMove(4, guide);
            }
        }

        public void ChangeBookCondition(int cursorTop, float bookID, string condition, string currentCondition)
        {
            ConnectDatabase.UpdateToDatabase("book_detail", "book_condition", condition, "application_number", bookID.ToString("n2"));
            print.ClearGuideline(4, cursorTop, Console.CursorLeft);
            print.ClearGuideline(79, cursorTop, 89);
            print.CompleteOrFaildProcess(4, Console.CursorTop, Constant.SUCCESS);
            print.SetCursorAndWrite(79, Console.CursorTop, condition);

            if (tool.IsBorrowed(currentCondition))
            {
                ConnectDatabase.UpdateToDatabase("history", "date_return", "NOW()", "book_id", bookID.ToString("n2"), "date_return");
                print.SetCursorAndWrite(95, Console.CursorTop, new string(' ', Console.WindowWidth - 96));
            }
        }

        public void SelectBookToBorrow(BookAPIVO book)
        {
            int index;
            int cursorTop = Console.CursorTop + 5;
            int registeredCount;

            List<float> bookID;
            List<string> condition;

            // 도서 정보 구하기
            registeredCount = getValue.RegisteredCount(book.Isbn);
            getValue.BookCondition(usingMemberID, registeredCount, book, out bookID, out condition);

            // 도서 정보 출력
            print.BookInLibrary(bookID, condition);

            if (usingMemberID == Constant.PUBLIC) { tool.WaitUntilGetEscapeKey(); return; }

            print.SetCursorAndChoice(30, cursorTop, "▷");
            while (true)
            {
                index = Console.CursorTop - cursorTop;
                if (!tool.IsNormal(condition[index], usingMemberID, bookID[index])) print.NonAvailableBookMark(30, Console.CursorTop);

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow) tool.UpArrow(30, cursorTop, condition.Count, 1, "▷");          // 위로 커서 옮김
                else if (keyInfo.Key == ConsoleKey.DownArrow) tool.DownArrow(30, cursorTop, condition.Count, 1, "▷"); // 밑으로 커서 옮김
                else if (keyInfo.Key == ConsoleKey.Escape) { print.BlockCursorMove(30, "▷"); return; }                // 나가기
                else if (keyInfo.Key == ConsoleKey.Enter) BorrowBook(condition[index], usingMemberID, bookID[index]);  // 대출
                else print.BlockCursorMove(30, "▷");                                                                  // 입력 무시
            }
        }

        public void BorrowBook(string condition, int usingMemberID, float bookID)
        {
            if (tool.IsNormal(condition, usingMemberID, bookID))
            {
                ConnectDatabase.BorrowBook(usingMemberID, bookID.ToString("n2"));
                print.CompleteOrFaildProcess(30, Console.CursorTop, Constant.BORROW);
            }
            else print.CompleteOrFaildProcess(30, Console.CursorTop, Constant.FAIL);
        }
    }
}