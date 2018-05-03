using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EnSharpLibrary.Data;
using EnSharpLibrary.IO;

namespace EnSharpLibrary.Function
{
    class BookManage
    {
        Print print = new Print();
        GetValue getValue = new GetValue();
        Tool tool = new Tool();

        public void ManageBook()
        {

        }

        public void SearchBook(int usingMemberID)
        {
            List<BookVO> searchedBook;
            int mode;
            string bookName;
            string publisher;
            string author;

            if (usingMemberID == Constant.PUBLIC) mode = Constant.NON_MEMBER_MODE;
            else mode = Constant.MEMBER_MODE; 

            while (true)
            {
                print.SetWindowsizeAndPrintTitle(45, 30, Constant.SEARCH_BOOK_TITLE[mode]);
                print.BookSearchCategoryAndGuideline();
                
                // - 정보 수집
                bookName = getValue.Information(19, 11, 10); if (string.Compare(bookName, "@입력취소@") == 0) return;
                publisher = getValue.Information(19, 13, 10); if (string.Compare(publisher, "@입력취소@") == 0) return;
                author = getValue.Information(19, 15, 10); if (string.Compare(author, "@입력취소@") == 0) return;

                // 조건 검색
                searchedBook = getValue.SearchBookByCondition(bookName, publisher, author);

                if (searchedBook.Count == 0) { print.ErrorMessage(Constant.THERE_IS_NO_BOOK, 22); return; }
                else break;
            }

            // 검색된 도서 출력
            print.SearchedBook(searchedBook, bookName, publisher, author);

            if (mode == Constant.NON_MEMBER_MODE) tool.WaitUntilGetEscapeKey();
            else BorrowBook(searchedBook, usingMemberID);

            return;
        }

        public void BorrowBook(List<BookVO> searchedBook, int usingMemberID)
        {

        }

        public void ManageBorrowedBook(int usingMemberID)
        {

        }

        ///// <summary>
        ///// 사용자 모드에서 사용자가 도서를 선택했을 때 실행되는 메소드입니다.
        ///// 도서의 기본정보를 알려주고, 사용자는 대출/반납/연장 등의 기능을 수행할 수 있습니다.
        ///// </summary>
        ///// <param name="usingMemberNumber">로그인한 사용자의 번호</param>
        ///// <param name="indexOfBook">사용자가 선택한 책의 인덱스</param>
        ///// <param name="books">책 정보</param>
        ///// <param name="members">회원 정보</param>
        ///// <returns>변경된 책 정보 및 회원 정보</returns>
        //public LibraryVO MemberMode(int usingMemberNumber, int indexOfBook, List<BookVO> books, List<MemberVO> members)
        //{
        //    LibraryVO library = new LibraryVO(members, books);
        //    MemberVO user = new MemberVO();

        //    StringBuilder bookInformation1 = new StringBuilder();
        //    StringBuilder bookInformation2 = new StringBuilder();
        //    int overdueDays;
        //    string guide = "나가기(ESC)";
        //    bool endOfProcess = false;

        //    // 도서 기본정보 알림
        //    print.Title(books[indexOfBook].Name);
        //    bookInformation1.AppendFormat("▷ {0}/{1}/{2}/{3}", books[indexOfBook].Name, books[indexOfBook].Author,
        //        books[indexOfBook].Publisher, books[indexOfBook].PublishingYear);
        //    bookInformation2.AppendFormat("청구기호 : {0}", books[indexOfBook].NumberOfThis);

        //    foreach (MemberVO member in members) if (member.IdentificationNumber == usingMemberNumber) user = member;

        //    Console.SetCursorPosition(10, Console.CursorTop);
        //    Console.WriteLine(bookInformation1);
        //    Console.SetCursorPosition(13, Console.CursorTop + 1);
        //    Console.WriteLine(bookInformation2);

        //    // 대출된 책
        //    if (books[indexOfBook].NumberOfMember == user.IdentificationNumber)
        //    {
        //        StringBuilder content1 = new StringBuilder();
        //        StringBuilder content2 = new StringBuilder();
        //        bool isReturn = false;
        //        bool isRenew = false;
                
        //        // 반납 예정일 안내
        //        content1.AppendFormat("나의 반납 예정일 : {0}", books[indexOfBook].ExpectedToReturn.ToString("yy/MM/dd"));
        //        Console.SetCursorPosition(13, Console.CursorTop + 1);
        //        Console.WriteLine(content1);

        //        // 연체일
        //        overdueDays = getValue.OverdueDate(books[indexOfBook]);
        //        if (overdueDays > 0)
        //        {
        //            content2.AppendFormat("현재 도서가 {0}일 연체되었습니다.", overdueDays);
        //            Console.SetCursorPosition(13, Console.CursorTop + 1);
        //            Console.WriteLine(content2);
        //        }

        //        // 반납할 것인지?
        //        Console.SetCursorPosition(10, Console.CursorTop + 1);
        //        Console.Write("▷ 반납하시겠습니까?");
        //        isReturn = getValue.YesOrNoAnswer(Console.CursorLeft, Console.CursorTop);
                
        //        if (isReturn)
        //        {
        //            foreach (MemberVO member in members)
        //                if (member.IdentificationNumber == usingMemberNumber)
        //                    member.ReturnBook(books[indexOfBook].NumberOfThis);
                    
        //            books[indexOfBook].SetNonRentalMode();

        //            endOfProcess = true;
        //        }
        //        else if (overdueDays <= 0 && books[indexOfBook].NumberOfRenew < 2)
        //        {
        //            Console.SetCursorPosition(10, Console.CursorTop + 2);
        //            Console.Write("▷ 연장하시겠습니까?");
        //            isRenew = getValue.YesOrNoAnswer(Console.CursorLeft, Console.CursorTop);

        //            if (isRenew)
        //            {
        //                books[indexOfBook].NumberOfRenew++;
        //                books[indexOfBook].ExpectedToReturn.AddDays(6);
        //                Console.SetCursorPosition(10, Console.CursorTop + 4);
        //                Console.Write("▷ 연장이 완료되었습니다! 예상 반납일은 {0}입니다.", books[indexOfBook].ExpectedToReturn);
        //            }
        //        }
        //    }
        //    else if (books[indexOfBook].BookCondition == 1)
        //    {
        //        Console.SetCursorPosition(10, Console.CursorTop + 1);
        //        Console.Write("▷ 현재 이 도서는 대출중입니다.");
        //        Console.SetCursorPosition(13, Console.CursorTop + 2);
        //        Console.Write("이 도서의 반납 예정일은 {0}이며, 도서가 반납될 때까지 기다려주시기 바랍니다.", books[indexOfBook].ExpectedToReturn.ToString("yy/MM/dd"));
        //    }

        //    // 대출가능 책
        //    if (books[indexOfBook].BookCondition == 0 && !endOfProcess)
        //    {
        //        string content1 = "▷ 다른 책이 연체되어 이 도서는 대출이 불가능합니다.";
        //        string content2 = "▷ 이미 대여된 도서와 같은 종류의 책은 대여가 불가능합니다.";
        //        string content3 = "▷ 현재 4권을 대여하셨습니다. 4권을 초과한 도서 대여는 불가능합니다.";
        //        string content4 = "▷ 이 책을 대출하시겠습니까?";
        //        bool isBorrow = false;

        //        if (getValue.DidIOverdue(user.BorrowedBook, books))
        //        {
        //            Console.SetCursorPosition(10, Console.CursorTop + 1);
        //            Console.Write(content1);
        //        }
        //        else if (getValue.DidIBorrowedSameBook((int)Math.Floor(books[indexOfBook].NumberOfThis), user.BorrowedBook))
        //        {
        //            Console.SetCursorPosition(10, Console.CursorTop + 1);
        //            Console.Write(content2);
        //        }
        //        else if (user.BorrowedBook.Count == 4)
        //        {
        //            Console.SetCursorPosition(10, Console.CursorTop + 1);
        //            Console.Write(content3);
        //        }
        //        else
        //        {
        //            Console.SetCursorPosition(10, Console.CursorTop + 1);
        //            Console.Write(content4);
        //            isBorrow = getValue.YesOrNoAnswer(Console.CursorLeft, Console.CursorTop);

        //            if (isBorrow)
        //            {
        //                books[indexOfBook].SetRentalMode(DateTime.Now, DateTime.Now.AddDays(6), user.IdentificationNumber);
        //                foreach (MemberVO member in members)
        //                    if (member.IdentificationNumber == usingMemberNumber)
        //                        member.BorrowedBook.Add(books[indexOfBook].NumberOfThis);

        //                Console.SetCursorPosition(10, Console.CursorTop + 4);
        //                Console.Write("▷ 도서 대출이 완료되었습니다. 예상 반납일은 {0}입니다.", DateTime.Now.AddDays(6).ToString("yy/MM/dd"));
        //            }
        //        }
        //    }

        //    // 분실 및 훼손
        //    if (books[indexOfBook].BookCondition == 2 || books[indexOfBook].BookCondition == 3 && !endOfProcess)
        //    {
        //        Console.SetCursorPosition(10, Console.CursorTop + 1);
        //        Console.Write("▷ 현재 이 책은 대여가 불가능합니다.");
        //    }

        //    Console.SetCursorPosition(55, Console.CursorTop + 4);
        //    Console.WriteLine(guide);

        //    while (true)
        //    {
        //        ConsoleKeyInfo keyInfo;

        //        keyInfo = Console.ReadKey();
        //        if (keyInfo.Key == ConsoleKey.Escape) break;
        //    }

        //    return library;
        //}


        //public LibraryVO AddBook(List<MemberVO> members, List<BookVO> books)
        //{
        //    LibraryVO library = new LibraryVO(members, books);
        //    string content1 = "▷ 도서명 : ";
        //    string content2 = "▷ 출판사 : ";
        //    string content3 = "▷ 저자 : ";
        //    string content4 = "▷ 출판년도 : ";
        //    string name;
        //    string publisher;
        //    string author;
        //    string publishingYear;
        //    int cursor = 0;

        //    print.Title("도서 등록");

        //    // 도서명
        //    Console.SetCursorPosition(40, Console.CursorTop);
        //    Console.Write(content1);
        //    name = getValue.SearchWord(23, 0);
        //    if (string.Compare(name, "@입력취소@") == 0) return library;

        //    // 출판사
        //    Console.SetCursorPosition(40, Console.CursorTop + 2);
        //    Console.Write(content2);
        //    publisher = getValue.SearchWord(15, 0);
        //    if (string.Compare(publisher, "@입력취소@") == 0) return library;

        //    // 저자
        //    Console.SetCursorPosition(40, Console.CursorTop + 2);
        //    Console.Write(content3);
        //    author = getValue.SearchWord(10, 0);
        //    if (string.Compare(author, "@입력취소@") == 0) return library;

        //    // 출판년도
        //    Console.SetCursorPosition(40, Console.CursorTop + 2);
        //    Console.Write(content4);
        //    publishingYear = getValue.SearchWord(6, 1);
        //    if (string.Compare(publishingYear, "@입력취소@") == 0) return library;

        //    while (true)
        //    {
        //        int year = DateTime.Now.Year;
        //        Console.SetCursorPosition(40, Console.CursorTop + 1);
        //        if (Int32.Parse(publishingYear) < year) print.Announce("잘못된 출판년도입니다!");
        //        Console.SetCursorPosition(40, cursor);
        //        print.ClearCurrentConsoleLine();
        //        Console.SetCursorPosition(40, cursor);
        //        Console.Write(content4);
        //        publishingYear = getValue.SearchWord(6, 1);
        //        if (string.Compare(publishingYear, "@입력취소@") == 0) return library;
        //        else break;

        //    }

        //    BookVO newBook = new BookVO(name, author, publisher, Int32.Parse(publishingYear));
        //    books.Add(newBook);

        //    library.Books = books;

        //    return library;
        //}

        //public LibraryVO AdminMode(int indexOfBook, List<BookVO> books, List<MemberVO> members)
        //{
        //    LibraryVO library = new LibraryVO(members, books);
        //    int leftCursor, topCursor;
        //    int mode, mode1;
        //    int userNumber;
        //    int index = -1;
        //    string guide = "나가기(ESC)";

        //    print.BookCondition(indexOfBook, books, members);
        //    leftCursor = Console.CursorLeft;
        //    topCursor = Console.CursorTop;
            
        //    mode = print.BookManageOption(18);
        //    if (mode == -1) return library;
        //    Console.SetCursorPosition(0, 18);
            

        //    Console.SetCursorPosition(10, 18);
        //    if (mode == 1) Console.Write("▷ 분실 및 훼손, 발견 등록");
        //    else Console.Write("▷ 도서 삭제");

        //    // 반납처리
        //    userNumber = books[indexOfBook].NumberOfMember;
        //    if (userNumber != -1)
        //    {
        //        for (int i = 0; i < members.Count; i++)
        //            if (members[i].IdentificationNumber == userNumber)
        //            {
        //                index = i;
        //                break;
        //            }
        //        members[index].ReturnBook(books[indexOfBook].NumberOfThis);
        //    }

        //    if (mode == 2)
        //    {
        //        books.RemoveAt(indexOfBook);
        //        Console.SetCursorPosition(10, 18);
        //        Console.Write("▷ 도서 삭제가 완료되었습니다.");
        //    }
        //    else
        //    {
        //        mode1 = print.BookConditionManageOption(Console.CursorLeft + 3, 18);
        //        if (mode1 == -1) return library;
        //        books[indexOfBook].SetNonRentalMode();

        //        Console.SetCursorPosition(10, 18);

        //        switch (mode1)
        //        {
        //            case 1: // 분실
        //                books[indexOfBook].BookCondition = 2;
        //                Console.Write("▷ 분실 등록이 완료되었습니다.");
        //                break;
        //            case 2: // 훼손
        //                books[indexOfBook].BookCondition = 3;
        //                Console.Write("▷ 훼손 등록이 완료되었습니다.");
        //                break;
        //            case 3: // 발견
        //                books[indexOfBook].BookCondition = 0;
        //                Console.Write("▷ 발견 등록이 완료되었습니다.");
        //                break;
        //        }    
        //    }

        //    library.Books = books;
        //    library.Members = members;

        //    Console.SetCursorPosition(55, Console.CursorTop + 4);
        //    Console.WriteLine(guide);

        //    while (true)
        //    {
        //        ConsoleKeyInfo keyInfo;

        //        keyInfo = Console.ReadKey();
        //        if (keyInfo.Key == ConsoleKey.Escape) break;
        //    }

        //    return library;
        //}
    }
}
