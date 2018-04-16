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

        public LibraryVO MemberMode(int usingMemberNumber, int indexOfBook, List<BookVO> books, List<MemberVO> members)
        {
            LibraryVO library = new LibraryVO(members, books);
            MemberVO user = new MemberVO();

            StringBuilder bookInformation1 = new StringBuilder();
            StringBuilder bookInformation2 = new StringBuilder();
            int overdueDays;
            string guide = "나가기(ESC)";
            bool endOfProcess = false;

            print.Title(books[indexOfBook].Name);
            bookInformation1.AppendFormat("▷ {0}/{1}/{2}/{3}", books[indexOfBook].Name, books[indexOfBook].Author,
                books[indexOfBook].Publisher, books[indexOfBook].PublishingYear);
            bookInformation2.AppendFormat("청구기호 : {0}", books[indexOfBook].NumberOfThis);

            foreach (MemberVO member in members) if (member.IdentificationNumber == usingMemberNumber) user = member;

            Console.SetCursorPosition(10, Console.CursorTop);
            Console.WriteLine(bookInformation1);
            Console.SetCursorPosition(13, Console.CursorTop + 1);
            Console.WriteLine(bookInformation2);

            // 대출된 책
            if (books[indexOfBook].NumberOfMember == user.IdentificationNumber)
            {
                StringBuilder content1 = new StringBuilder();
                StringBuilder content2 = new StringBuilder();
                bool isReturn = false;
                bool isRenew = false;
                
                // 반납 예정일 안내
                content1.AppendFormat("나의 반납 예정일 : {0}", books[indexOfBook].ExpectedToReturn.ToString("yy/MM/dd"));
                Console.SetCursorPosition(13, Console.CursorTop + 1);
                Console.WriteLine(content1);

                // 연체일
                overdueDays = getValue.OverdueDate(books[indexOfBook]);
                if (overdueDays > 0)
                {
                    content2.AppendFormat("현재 도서가 {0}일 연체되었습니다.", overdueDays);
                    Console.SetCursorPosition(13, Console.CursorTop + 1);
                    Console.WriteLine(content2);
                }

                // 반납할 것인지?
                Console.SetCursorPosition(10, Console.CursorTop + 1);
                Console.Write("▷ 반납하시겠습니까?");
                isReturn = getValue.YesOrNoAnswer(Console.CursorLeft, Console.CursorTop);
                
                if (isReturn)
                {
                    foreach (MemberVO member in members)
                        if (member.IdentificationNumber == usingMemberNumber)
                            member.ReturnBook(books[indexOfBook].NumberOfThis);
                    
                    books[indexOfBook].SetNonRentalMode();

                    endOfProcess = true;
                }
                else if (overdueDays <= 0 && books[indexOfBook].NumberOfRenew < 2)
                {
                    Console.SetCursorPosition(10, Console.CursorTop + 2);
                    Console.Write("▷ 연장하시겠습니까?");
                    isRenew = getValue.YesOrNoAnswer(Console.CursorLeft, Console.CursorTop);

                    if (isRenew)
                    {
                        books[indexOfBook].NumberOfRenew++;
                        books[indexOfBook].ExpectedToReturn.AddDays(6);
                        Console.SetCursorPosition(10, Console.CursorTop + 4);
                        Console.Write("▷ 연장이 완료되었습니다! 예상 반납일은 {0}입니다.", books[indexOfBook].ExpectedToReturn);
                    }
                }
            }
            else if (books[indexOfBook].BookCondition == 1)
            {
                Console.SetCursorPosition(10, Console.CursorTop + 1);
                Console.Write("▷ 현재 이 도서는 대출중입니다.");
                Console.SetCursorPosition(13, Console.CursorTop + 2);
                Console.Write("이 도서의 반납 예정일은 {0}이며, 도서가 반납될 때까지 기다려주시기 바랍니다.", books[indexOfBook].ExpectedToReturn.ToString("yy/MM/dd"));
            }

            // 대출가능 책
            if (books[indexOfBook].BookCondition == 0 && !endOfProcess)
            {
                string content1 = "▷ 다른 책이 연체되어 이 도서는 대출이 불가능합니다.";
                string content2 = "▷ 이미 대여된 도서와 같은 종류의 책은 대여가 불가능합니다.";
                string content3 = "▷ 현재 4권을 대여하셨습니다. 4권을 초과한 도서 대여는 불가능합니다.";
                string content4 = "▷ 이 책을 대출하시겠습니까?";
                bool isBorrow = false;

                if (getValue.DidIOverdue(user.BorrowedBook, books))
                {
                    Console.SetCursorPosition(10, Console.CursorTop + 1);
                    Console.Write(content1);
                }
                else if (getValue.DidIBorrowedSameBook((int)Math.Floor(books[indexOfBook].NumberOfThis), user.BorrowedBook))
                {
                    Console.SetCursorPosition(10, Console.CursorTop + 1);
                    Console.Write(content2);
                }
                else if (user.BorrowedBook.Count == 4)
                {
                    Console.SetCursorPosition(10, Console.CursorTop + 1);
                    Console.Write(content3);
                }
                else
                {
                    Console.SetCursorPosition(10, Console.CursorTop + 1);
                    Console.Write(content4);
                    isBorrow = getValue.YesOrNoAnswer(Console.CursorLeft, Console.CursorTop);

                    if (isBorrow)
                    {
                        books[indexOfBook].SetRentalMode(DateTime.Now, DateTime.Now.AddDays(6), user.IdentificationNumber);
                        foreach (MemberVO member in members)
                            if (member.IdentificationNumber == usingMemberNumber)
                                member.BorrowedBook.Add(books[indexOfBook].NumberOfThis);

                        Console.SetCursorPosition(10, Console.CursorTop + 4);
                        Console.Write("▷ 도서 대출이 완료되었습니다. 예상 반납일은 {0}입니다.", DateTime.Now.AddDays(6).ToString("yy/MM/dd"));
                    }
                }
            }

            // 분실 및 훼손
            if (books[indexOfBook].BookCondition == 2 || books[indexOfBook].BookCondition == 3 && !endOfProcess)
            {
                Console.SetCursorPosition(10, Console.CursorTop + 1);
                Console.Write("▷ 현재 이 책은 대여가 불가능합니다.");
            }

            Console.SetCursorPosition(55, Console.CursorTop + 4);
            Console.WriteLine(guide);

            while (true)
            {
                ConsoleKeyInfo keyInfo;

                keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Escape) break;
            }

            return library;
        }

        public LibraryVO AddBook(List<MemberVO> members, List<BookVO> books)
        {
            LibraryVO library = new LibraryVO(members, books);

            print.Title("도서 등록");

            return library;
        }

        public LibraryVO AdminMode(int indexOfBook, List<BookVO> books, List<MemberVO> members)
        {
            LibraryVO library = new LibraryVO(members, books);
            int leftCursor, topCursor;
            int mode, mode1;
            int userNumber;
            int index = -1;
            string guide = "나가기(ESC)";

            print.BookCondition(indexOfBook, books, members);
            leftCursor = Console.CursorLeft;
            topCursor = Console.CursorTop;
            
            mode = print.BookManageOption(18);
            if (mode == -1) return library;
            Console.SetCursorPosition(0, 18);
            

            Console.SetCursorPosition(10, 18);
            if (mode == 1) Console.Write("▷ 분실 및 훼손, 발견 등록");
            else Console.Write("▷ 도서 삭제");

            // 반납처리
            userNumber = books[indexOfBook].NumberOfMember;
            if (userNumber != -1)
            {
                for (int i = 0; i < members.Count; i++)
                    if (members[i].IdentificationNumber == userNumber)
                    {
                        index = i;
                        break;
                    }
                members[index].ReturnBook(books[indexOfBook].NumberOfThis);
            }

            if (mode == 2)
            {
                books.RemoveAt(indexOfBook);
                Console.SetCursorPosition(10, 18);
                Console.Write("▷ 도서 삭제가 완료되었습니다.");
            }
            else
            {
                mode1 = print.BookConditionManageOption(Console.CursorLeft + 3, 18);
                if (mode1 == -1) return library;
                books[indexOfBook].SetNonRentalMode();

                Console.SetCursorPosition(10, 18);

                switch (mode1)
                {
                    case 1: // 분실
                        books[indexOfBook].BookCondition = 2;
                        Console.Write("▷ 분실 등록이 완료되었습니다.");
                        break;
                    case 2: // 훼손
                        books[indexOfBook].BookCondition = 3;
                        Console.Write("▷ 훼손 등록이 완료되었습니다.");
                        break;
                    case 3: // 발견
                        books[indexOfBook].BookCondition = 0;
                        Console.Write("▷ 발견 등록이 완료되었습니다.");
                        break;
                }    
            }

            library.Books = books;
            library.Members = members;

            Console.SetCursorPosition(55, Console.CursorTop + 4);
            Console.WriteLine(guide);

            while (true)
            {
                ConsoleKeyInfo keyInfo;

                keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Escape) break;
            }

            return library;
        }
    }
}
