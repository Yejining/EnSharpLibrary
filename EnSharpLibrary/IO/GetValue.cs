using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Xml;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using EnSharpLibrary.Data;
using EnSharpLibrary.Function;

namespace EnSharpLibrary.IO
{
    class GetValue
    {
        Print print = new Print();
        Tool tool = new Tool();

        public string Address(int cursorLeft, int cursorTop)
        {
            int address1;
            int address2;
            string address;

            // 도단위 주소 입력
            address1 = DropBox(cursorLeft, cursorTop, Constant.ANSWER_ADDRESS_INCLUDE_ALL_OPTION);
            if (address1 == -1) return Constant.CANCELED_INPUT;
            print.SetCursorAndWrite(cursorLeft, cursorTop, Constant.DISTRICT_INCLUDE_ALL_OPTION[address1]);

            address = Constant.DISTRICT_INCLUDE_ALL_OPTION[address1];

            // 시/군/구단위 주소 입력
            if (address1 != 0)
            {
                address2 = DropBox(Console.CursorLeft + 1, cursorTop, Constant.ANSWER_ADDRESS_DEEPLY + address1 - 1);
                if (address2 == -1) return Constant.CANCELED_INPUT;
                address += " " + Constant.DISTRICT[address1][address2];
            }

            return address;
        }
 
        public int GetDataTypeFromUser(int usingMemberID, out int cursorLeft, out int cursorTop)
        {
            int dataType;

            if (usingMemberID != Constant.ADMIN)
            {
                print.SetCursorAndWrite(5, Console.CursorTop + 2, "수정할 정보 | ");
                dataType = DropBox(Console.CursorLeft, Console.CursorTop, Constant.ANSWER_WHAT_TO_EDIT);
            }
            else
            {
                dataType = Constant.EDIT_PASSWORD;
            }
            
            Console.SetCursorPosition(5, Console.CursorTop + 2);
            if (dataType == Constant.EDIT_PASSWORD) Console.Write("현재 ");
            Console.Write(Constant.MEMBER_EDIT_OPTION[dataType] + " | ");

            cursorLeft = Console.CursorLeft;
            cursorTop = Console.CursorTop;

            return dataType;
        }

        public bool IsPasswordCorrespond(string userInputMemberID, string userInputPassword)
        {
            string password = ConnectDatabase.SelectFromDatabase("password", "member", "member_id", userInputMemberID)[0];

            if (string.Compare(password, userInputPassword) == 0) return true;
            else return false;
        }

        public DateTime StringToDateTime(string date)
        {
            int year, day;
            string month;

            year = Int32.Parse(date.Remove(4));
            month = date.Remove(7);
            month = month.Remove(0, 5);
            day = Int32.Parse(date.Remove(0, 8));

            return new DateTime(year, Int32.Parse(month), day);
        }

        public void UserInformationFromDatabase(int memberID, out string name, out string address, out string phoneNumber, out DateTime birthDate)
        {
            string table = "member";
            string conditionalExpression = " WHERE member_id=" + memberID;
            string date;

            name = ConnectDatabase.SelectFromDatabase("name", table, conditionalExpression)[0];
            address = ConnectDatabase.SelectFromDatabase("address", table, conditionalExpression)[0];
            phoneNumber = ConnectDatabase.SelectFromDatabase("phone_number", table, conditionalExpression)[0];
            date = ConnectDatabase.SelectFromDatabase("birthdate", table, conditionalExpression)[0].Remove(10);
            birthDate = StringToDateTime(date);

            return;
        }

        public void UserInformationFromUser(out string name, out string userID, out string password, out string address, out string phoneNumber, out DateTime birthdate)
        {
            int errorMode;
            int address1, address2;
            int year, month, day;

            // 초기화
            name = Constant.BLANK;
            userID = Constant.BLANK;
            password = Constant.BLANK;
            address = Constant.BLANK;
            phoneNumber = Constant.BLANK;
            birthdate = new DateTime(1980, 1, 1);

            // - 이름
            while (true)
            {
                name = Information(17, 11, 5, Constant.ONLY_KOREAN, Constant.JOIN_SEARCH_CATEGORY_AND_GUIDELINE[1]);
                errorMode = tool.IsValidAnswer(Constant.ANSWER_NAME, name);
                if (errorMode == Constant.ESCAPE_KEY_ERROR) return;
                else if (errorMode != Constant.VALID) { print.ErrorMessage(errorMode, 23); continue; }
                else break;
            }

            // - 학번
            while (true)
            {
                userID = Information(17, 13, 8, Constant.ONLY_NUMBER, Constant.JOIN_SEARCH_CATEGORY_AND_GUIDELINE[3]);
                errorMode = tool.IsValidAnswer(Constant.ANSWER_USER_ID, userID);
                if (errorMode == Constant.ESCAPE_KEY_ERROR) return;
                else if (errorMode != Constant.VALID) { print.ErrorMessage(errorMode, 23); continue; }
                else break;
            }

            // - 암호
            while (true)
            {
                password = Information(17, 15, 15, Constant.NO_KOREAN, Constant.JOIN_SEARCH_CATEGORY_AND_GUIDELINE[5]);
                errorMode = tool.IsValidAnswer(Constant.ANSWER_PASSWORD, password);
                if (errorMode == Constant.ESCAPE_KEY_ERROR) return;
                else if (errorMode != Constant.VALID) { print.ErrorMessage(errorMode, 23); continue; }
                else break;
            }
            
            // - 주소
            while (true)
            {
                address1 = DropBox(17, 17, Constant.ANSWER_ADDRESS);
                if (address1 == -1) return;
                Console.SetCursorPosition(17, 17);
                Console.Write(Constant.DISTRICT[0][address1]);
                address2 = DropBox(Console.CursorLeft + 1, 17, Constant.ANSWER_ADDRESS_DEEPLY + address1);
                if (address2 == 2) return;
                address = Constant.DISTRICT[0][address1] + " " + Constant.DISTRICT[address1 + 1][address2];
                break;
            }
            
            // 전화번호
            while (true)
            {
                phoneNumber = PhoneNumber(21, 19);
                if (IsCanceled(phoneNumber)) return;
                else break;
            }

            // 생일
            while (true)
            {
                year = DropBox(17, 21, Constant.ANSWER_BIRTHDATE_YEAR); if (year == -1) return;
                month = DropBox(24, 21, Constant.ANSWER_BIRTHDATE_MONTH); if (month == -1) return;
                day = DropBox(29, 21, Constant.ANSWER_BIRTHDATE_DAY); if (day == -1) return;
                birthdate = Birthdate(year, month, day);
                break;
            }
        }

        public string[] YEAR(int mode)
        {
            string[] years;

            if (mode == Constant.INCLUDE_ALL_OPTION)
            {
                years = new string[DateTime.Now.Year - 1990 + 2];
                years.SetValue("전체", 0);
            }
            else years = new string[DateTime.Now.Year - 1990 + 1];

            for (int year = 1990; year <= DateTime.Now.Year; year++)
            {
                if (mode == Constant.GENERAL_MODE) years.SetValue(year + "년", year - 1990);
                else years.SetValue(year + "년", year - 1989);
            }

            return years;
        }


        public DateTime Birthdate(int yearIndex, int monthIndex, int dayIndex)
        {
            string year = YEAR(Constant.GENERAL_MODE)[yearIndex].Remove(4, 1);
            string month = Constant.MONTH[monthIndex].Remove(2, 1);
            string day = Constant.DAY[dayIndex].Remove(2, 1);

            return new DateTime(Int32.Parse(year), Int32.Parse(month), Int32.Parse(day));
        }

        public bool IsQualifiedToBeNormal(ConsoleKey key, string condition)
        {
            if (key == ConsoleKey.Q && string.Compare(condition, "대출 가능") != 0 && !tool.IsDeleted(condition)) return true;
            else return false;
        }

        public bool IsQualifiedToBeLost(ConsoleKey key, string condition)
        {
            if (key == ConsoleKey.W && string.Compare(condition, "분실") != 0 && !tool.IsDeleted(condition)) return true;
            else return false;
        }

        public bool IsQualifiedToBeDamaged(ConsoleKey key, string condition)
        {
            if (key == ConsoleKey.E && string.Compare(condition, "훼손") != 0 && !tool.IsDeleted(condition)) return true;
            else return false;
        }

        public bool IsQualifiedToBeSaved(ConsoleKey key, string condition)
        {
            if (key == ConsoleKey.R && string.Compare(condition, "대출 가능") == 0 && !tool.IsDeleted(condition)) return true;
            else return false;
        }

        public bool IsQualiiedToBeDeleted(ConsoleKey key, string condition)
        {
            if (key == ConsoleKey.T && !tool.IsDeleted(condition)) return true;
            else return false;
        }

        public void BookCondition(int usingMemberID, int registeredCount, BookAPIVO book, out List<float> bookID, out List<string> condition)
        {
            bookID = new List<float>();
            condition = new List<string>();

            for (int count = 0; count < registeredCount; count++)
            {
                bookID.Add(book.SerialNumber + ((float)count / 100));
                condition.Add(ConnectDatabase.SelectFromDatabase("book_condition", "book_detail", "application_number", bookID[count].ToString("n2"))[0]);
            }

            for (int count = condition.Count - 1; count >= 0; count--)
            {
                if (usingMemberID != Constant.ADMIN && !tool.IsBorrowed(condition[count]) && !tool.IsNotRented(condition[count]))
                {
                    condition.RemoveAt(count);
                    bookID.RemoveAt(count);
                }
            }
        }

        public void BookCondition(int registeredCount, BookAPIVO book, List<float> applicationNumber, List<string> bookCondition,
            out List<string> memberID, out List<string> dateBorrowed, out List<string> dateDeadlineForReturn, out List<string> numberOfRenew)
        {
            memberID = new List<string>();
            dateBorrowed = new List<string>();
            dateDeadlineForReturn = new List<string>();
            numberOfRenew = new List<string>();

            for (int count = 0; count < registeredCount; count++)
            {
                if (string.Compare(bookCondition[count], "대출중") == 0)
                {
                    memberID.Add(DetailInformationAboutBorrowedMember(Constant.MEMBER_ID, applicationNumber[count]));
                    dateBorrowed.Add(DetailInformationAboutBorrowedMember(Constant.DATE_BORROWED, applicationNumber[count]));
                    dateDeadlineForReturn.Add(DetailInformationAboutBorrowedMember(Constant.DATE_DEADLINE_FOR_RETURN, applicationNumber[count]));
                    numberOfRenew.Add(DetailInformationAboutBorrowedMember(Constant.NUMBER_OF_RENEW, applicationNumber[count]));
                }
            }
        }

        public string BookCountToRegister(int cursorLeft, int cursorTop, int registeredCount)
        {
            string count;

            while (true)
            {
                count = Information(cursorLeft, cursorTop, 3, Constant.ONLY_NUMBER, "숫자 입력(등록 취소:ESC)");

                if (IsCanceled(count)) return Constant.CANCELED_INPUT;
                else if (Int32.Parse(count) == 0) print.ErrorMessage(Constant.EXCEED_INPUT_ERROR, cursorTop + 2);
                else if (count.Length == 0) print.ErrorMessage(Constant.LENGTH_ERROR, cursorTop + 2);
                else if (Int32.Parse(count) + registeredCount > 99) print.ErrorMessage(Constant.EXCEED_INPUT_ERROR, cursorTop + 2);
                else { count = Int32.Parse(count).ToString(); break; }
            }

            return count;
        }

        public void SerialNumberAndRegisteredCount(string isbn, out int serialNumber, out int registeredCount)
        {
            int countOfTableData;

            // 등록된 책이 몇 종류인지 알아냄
            registeredCount = RegisteredCount(isbn);
            countOfTableData = ConnectDatabase.GetCountFromDatabase("book_api", Constant.BLANK, Constant.BLANK);

            // 청구기호에 쓰일 고유번호 계산(정수)
            if (registeredCount == 0) serialNumber = countOfTableData + 1;
            else serialNumber = Int32.Parse(ConnectDatabase.SelectFromDatabase("serial_number", "book_api", "isbn", isbn)[0]);
        }

        public string BookNameToSearch()
        {
            string name;
            int errorMode;

            while (true)
            {
                print.SearchCategoryAndGuideline(Constant.ADD_BOOK);

                // 정보 수정
                // - 도서명
                name = Information(19, 11, 15, Constant.ALL_CHARACTER, Constant.ADD_BOOK_CATEGORY_AND_GUILDLINE[1]);
                errorMode = tool.IsValidAnswer(Constant.ANSWER_BOOK_NAME, name);
                if (errorMode == Constant.ESCAPE_KEY_ERROR) return Constant.CANCELED_INPUT;
                else if (errorMode != Constant.VALID) { print.ErrorMessage(errorMode, 15); continue; }

                break;
            }

            return name;
        }

        public bool IsCanceled(string searchWord)
        {
            if (string.Compare(searchWord, Constant.CANCELED_INPUT) == 0) return true;
            else return false;
        }

        public string GuideForModifyingBookCondition(float bookID)
        {
            string bookCondition = ConnectDatabase.SelectFromDatabase("book_condition", "book_detail", "application_number", bookID.ToString("n2"))[0];

            if (string.Compare(bookCondition, "삭제") == 0) return "                           X";

            if (string.Compare(bookCondition, "대출 가능") == 0) return Constant.GUIDE_FOR_MODIFYING_BOOK_CONDITION[0];
            else if (string.Compare(bookCondition, "대출중") == 0) return Constant.GUIDE_FOR_MODIFYING_BOOK_CONDITION[1];
            else if (string.Compare(bookCondition, "보관도서") == 0) return Constant.GUIDE_FOR_MODIFYING_BOOK_CONDITION[3];
            else return Constant.GUIDE_FOR_MODIFYING_BOOK_CONDITION[2];
        }

        public string DetailInformationAboutBorrowedMember(int mode, float applicationNumber)
        {
            string information;
            string column = Constant.COLUMN_NAME_FOR_DETAIL_INFORMATION[mode];
            information = ConnectDatabase.SelectFromDatabase(column, "history", "book_id", applicationNumber.ToString("n2"), "date_return")[0];

            return information;
        }

        public List<BookAPIVO> RegisteredBook()
        {
            int count = ConnectDatabase.GetCountFromDatabase("book_api", Constant.BLANK, Constant.BLANK);
            List<BookAPIVO> books = new List<BookAPIVO>();
            
            for (int index = 0; index < count; index++)
            {
                books.Add(new BookAPIVO());
                books[index].Title = ConnectDatabase.SelectFromDatabase("name", "book_api", Constant.BLANK, Constant.BLANK)[index];
                books[index].Author = ConnectDatabase.SelectFromDatabase("author", "book_api", Constant.BLANK, Constant.BLANK)[index];
                books[index].Publisher = ConnectDatabase.SelectFromDatabase("publisher", "book_api", Constant.BLANK, Constant.BLANK)[index];
                books[index].Price = Int32.Parse(ConnectDatabase.SelectFromDatabase("price", "book_api", Constant.BLANK, Constant.BLANK)[index]);
                books[index].Discount = Int32.Parse(ConnectDatabase.SelectFromDatabase("discount", "book_api", Constant.BLANK, Constant.BLANK)[index]);
                books[index].Pubdate = ConnectDatabase.SelectFromDatabase("publishing_date", "book_api", Constant.BLANK, Constant.BLANK)[index];
                books[index].Count = Int32.Parse(ConnectDatabase.SelectFromDatabase("count", "book_api", Constant.BLANK, Constant.BLANK)[index]);
                books[index].Isbn = ConnectDatabase.SelectFromDatabase("isbn", "book_api", Constant.BLANK, Constant.BLANK)[index];
                books[index].SerialNumber = Int32.Parse(ConnectDatabase.SelectFromDatabase("serial_number", "book_api", Constant.BLANK, Constant.BLANK)[index]);
                books[index].Description = ConnectDatabase.SelectFromDatabase("description", "book_api", Constant.BLANK, Constant.BLANK)[index];
            }
            
            return books;
        }

        public List<BookAPIVO> MatchedBooksFromSearchWord(string nameToSearch)
        {
            List<BookAPIVO> books = new List<BookAPIVO>();
            List<string> bookTitle;
            List<string> bookDescription;

            XmlDocument bookInformation = tool.ConnectNaverAPIAndGetInformation(nameToSearch);

            XmlNodeList title = bookInformation.GetElementsByTagName("title");
            bookTitle = CleareaceKeyword(title);
            XmlNodeList author = bookInformation.GetElementsByTagName("author");
            XmlNodeList price = bookInformation.GetElementsByTagName("price");
            XmlNodeList discount = bookInformation.GetElementsByTagName("discount");
            XmlNodeList publisher = bookInformation.GetElementsByTagName("publisher");
            XmlNodeList pubdate = bookInformation.GetElementsByTagName("pubdate");
            XmlNodeList isbn = bookInformation.GetElementsByTagName("isbn");
            XmlNodeList description = bookInformation.GetElementsByTagName("description");
            bookDescription = CleareaceKeyword(description);

            for (int count = 0; count < title.Count - 1; count++)
            {
                BookAPIVO book = new BookAPIVO(Constant.ADMIN_MODE, bookTitle[count + 1], publisher[count].InnerText, pubdate[count].InnerText);
                book.SaveDetail(Constant.AUTHOR, author[count].InnerText);
                book.SavePrice(Constant.PRICE, price[count].InnerText);
                book.SavePrice(Constant.DISCOUNT, discount[count].InnerText);
                book.SaveDetail(Constant.ISBN, isbn[count].InnerText);
                book.SaveDetail(Constant.DESCRIPTION, bookDescription[count + 1]);

                books.Add(book);
            }

            return books;
        }

        public List<string> CleareaceKeyword(XmlNodeList title)
        {
            List<string> bookTitle = new List<string>();
            String modifiedTitle;

            for (int index = 0; index < title.Count; index++)
            {
                modifiedTitle = Regex.Replace(title[index].InnerText, @"(<[^>]*>|&lt;|&gt;|&#x0D;)", String.Empty);
                bookTitle.Add(modifiedTitle);
            }

            return bookTitle;
        }

        /// <summary>
        /// 사용자가 검색창에 입력한 값을 반환해주는 메소드입니다.
        /// </summary>
        /// <param name="cursorLeft">커서 설정 변수(들여쓰기)</param>
        /// <param name="cursorTop">커서 설정 변수(줄)</param>
        /// <param name="limit">검색 입력 최대 길이</param>
        /// <returns>사용자가 입력한 검색어</returns>
        public string Information(int cursorLeft, int cursorTop, int limit, int mode, string guideline)
        {
            int currentCursor = 0;
            bool isValid = false;
            StringBuilder answer = new StringBuilder();
            ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();

            // 입력 조건 출력
            print.GuidelineForSearch(guideline, cursorLeft, cursorTop);

            while (true)
            {
                currentCursor = Console.CursorLeft;
                keyInfo = Console.ReadKey();

                // 키값이 유효한지 검사
                isValid = tool.IsValid(keyInfo, mode);

                if (answer.Length == 0) print.DeleteGuideLine(cursorLeft, isValid, keyInfo);

                if (keyInfo.Key == ConsoleKey.Escape) return "@입력취소@";                                                                        // 나가기
                else if (keyInfo.Key == ConsoleKey.Backspace) answer = BackspaceInput(cursorLeft, cursorTop, answer);                             // 지우기
                else if (isValid) answer = ValidInput(currentCursor, limit, keyInfo.KeyChar, answer);                                             // 올바른 입력
                else if (keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Tab) print.InvalidInput(keyInfo, currentCursor, cursorTop); // 입력 무시
                else if (keyInfo.Key == ConsoleKey.Enter || keyInfo.Key == ConsoleKey.Tab) return answer.ToString();      // 검색 완료
                
                // 검색어 글자가 0자일 경우 가이드라인 출력
                if (answer.Length == 0) print.GuidelineForSearch(guideline, cursorLeft, cursorTop);
            }
        }

        public List<BookAPIVO> SearchBookByID(int memberID, List<string> bookID)
        {
            string conditionalExpression = " WHERE member_id =" + memberID.ToString() + " AND date_return IS NULL";
            List<BookAPIVO> books = new List<BookAPIVO>();
            List<string> name = new List<string>();
            List<string> author = new List<string>();
            string bookName, bookAuthor;

            foreach (string id in bookID)
            {
                bookName = ConnectDatabase.SelectFromDatabase("name", "book_api", "serial_number", id[0].ToString())[0];
                bookAuthor = ConnectDatabase.SelectFromDatabase("author", "book_api", "serial_number", id[0].ToString())[0];
                name.Add(print.ShortenKeyword(bookName, 38));
                author.Add(print.ShortenKeyword(bookName, 15));
            }

            for (int count = 0; count < bookID.Count; count++)
            {
                BookAPIVO book = new BookAPIVO();
                book.Title = name[count];
                book.Author = author[count];
                books.Add(book);
            }

            return books;
        }

        public void InformationAboutBorrowedBookFromMember(int usingMemberID, out List<string> bookID, out List<BookAPIVO> borrowedBook, out List<HistoryVO> histories)
        {
            bookID = BookIDFromDatabase(usingMemberID);
            borrowedBook = SearchBookByID(usingMemberID, bookID);
            histories = SearchHistoryByID(usingMemberID, bookID);

            return;
        }

        public int RegisteredCount(string isbn)
        {
            List<string> result = ConnectDatabase.SelectFromDatabase("count", "book_api", "isbn", isbn);

            if (result.Count == 0) return 0;
            else return Int32.Parse(result[0]);
        }

        public List<string> BookIDFromDatabase(int usingMemberID)
        {
            string conditionalExpression = " WHERE member_id =" + usingMemberID.ToString() + " AND date_return IS NULL";

            List<string> bookID = ConnectDatabase.SelectFromDatabase("book_id", "history", conditionalExpression);
            bookID = CorrectBookID(bookID);

            return bookID;
        }

        public List<string> CorrectBookID(List<string> bookID)
        {
            for (int index = 0; index < bookID.Count; index++)
            {
                if (bookID[index].Length == 1) bookID[index] += ".00";
            }

            return bookID;
        }

        public List<HistoryVO> SearchHistoryByID(int memberID, List<string> bookID)
        {
            string conditionalExpression = " WHERE member_id =" + memberID.ToString() + " AND date_return IS NULL";

            List<HistoryVO> histories = new List<HistoryVO>();
            List<string> dateBorrowed = ConnectDatabase.SelectFromDatabase("date_borrowed", "history", conditionalExpression);
            List<string> numberOfRenew = ConnectDatabase.SelectFromDatabase("number_of_renew", "history", conditionalExpression);
            List<string> dateDeadlineForReturn = ConnectDatabase.SelectFromDatabase("date_deadline_for_return", "history", conditionalExpression);

            for (int count = 0; count < bookID.Count; count++)
            {
                HistoryVO history = new HistoryVO(dateBorrowed[count].Remove(10), dateDeadlineForReturn[count].Remove(10), Int32.Parse(numberOfRenew[count]));
                histories.Add(history);
            }

            return histories;
        }

        public string ConditionalExpression(string bookName, string publisher, string author)
        {
            string conditionalExpression = "";

            if (bookName.Length != 0) conditionalExpression = " WHERE name REGEXP \"" + bookName + "\"";
            if (bookName.Length != 0 && publisher.Length != 0) conditionalExpression += "AND publisher REGEXP \"" + publisher + "\"";
            else if (bookName.Length == 0 && publisher.Length != 0) conditionalExpression = " WHERE publisher REGEXP \"" + publisher + "\"";
            if (conditionalExpression.Length != 0 && author.Length != 0) conditionalExpression += "AND author REGEXP \"" + author + "\"";
            else if (conditionalExpression.Length == 0 && author.Length != 0) conditionalExpression = " WHERE author REGEXP \"" + author + "\"";

            return conditionalExpression;
        }

        public List<BookAPIVO> SearchBookByCondition(string bookName, string bookPublisher, string bookAuthor)
        {
            List<BookAPIVO> searchedBook = new List<BookAPIVO>();
            string conditionalExpression = ConditionalExpression(bookName, bookPublisher, bookAuthor);
            List<string> serialNumber = ConnectDatabase.SelectFromDatabase("serial_number", "book_api", conditionalExpression);

            if (serialNumber.Count == 0) return searchedBook;

            List<string> name = ConnectDatabase.SelectFromDatabase("name", "book_api", conditionalExpression);
            List<string> publisher = ConnectDatabase.SelectFromDatabase("publisher", "book_api", conditionalExpression);
            List<string> author = ConnectDatabase.SelectFromDatabase("author", "book_api", conditionalExpression);
            List<string> pubdate = ConnectDatabase.SelectFromDatabase("publishing_date", "book_api", conditionalExpression);
            List<string> isbn = ConnectDatabase.SelectFromDatabase("isbn", "book_api", conditionalExpression);
            List<string> description = ConnectDatabase.SelectFromDatabase("description", "book_api", conditionalExpression);

            for (int count = 0; count < serialNumber.Count; count++)
            {
                BookAPIVO book = new BookAPIVO(Constant.MEMBER_MODE, name[count], publisher[count], author[count]);
                book.Pubdate = pubdate[count];
                book.SerialNumber = Int32.Parse(serialNumber[count]);
                book.Isbn = isbn[count];
                book.Description = description[count];

                searchedBook.Add(book);
            }

            return searchedBook;
        }

        public List<MemberVO> SearchMemberByCondition(string name, string address)
        {
            List<MemberVO> searchedMember = new List<MemberVO>();
            string conditionalExpression = "";
            int countOfMembers;
            string date;

            // 회원검색을 위한 sql문
            if (name.Length != 0) conditionalExpression = " WHERE name REGEXP \'" + name + "\'";
            if (string.Compare(address, "전체") != 0)
            {
                if (name.Length == 0) conditionalExpression = " WHERE address REGEXP \'" + address + "\'";
                else conditionalExpression += " AND address REGEXP \'" + address + "\'";
            }

            countOfMembers = ConnectDatabase.GetCountFromDatabase("member", conditionalExpression);

            // 회원검색
            for (int member = 0; member < countOfMembers; member++)
            {
                searchedMember.Add(new MemberVO());

                searchedMember[member].MemberID = Int32.Parse(ConnectDatabase.SelectFromDatabase("member_id", "member", conditionalExpression)[member]);
                searchedMember[member].Name = ConnectDatabase.SelectFromDatabase("name", "member", conditionalExpression)[member];
                searchedMember[member].Address = ConnectDatabase.SelectFromDatabase("address", "member", conditionalExpression)[member];
                searchedMember[member].PhoneNumber = ConnectDatabase.SelectFromDatabase("phone_number", "member", conditionalExpression)[member];
                date = ConnectDatabase.SelectFromDatabase("birthdate", "member", conditionalExpression)[member].Remove(10);
                searchedMember[member].Birthdate = StringToDateTime(date);
            }

            return searchedMember;
        }

        public string Password(int memberID)
        {
            return ConnectDatabase.SelectFromDatabase("password", "member", "member_id", memberID.ToString())[0];
        }

        /// <summary>
        /// 드롭박스에서 원하는 옵션을 선택하는 메소드입니다.
        /// </summary>
        /// <param name="mode">사용자가 선택한 검색 모드</param>
        /// <param name="cursorLeft">커서 설정 변수(들여쓰기)</param>
        /// <param name="cursorTop">커서 설정 변수(줄)</param>       
        /// <returns>사용자가 선택한 옵션</returns>
        public int DropBox(int cursorLeft, int cursorTop, int mode)
        {
            ConsoleKeyInfo keyInfo;

            int index = 0;
            string[] option = Constant.DISTRICT[0];

            // 드롭박스 선택
            if (mode == Constant.ANSWER_ADDRESS) option = Constant.DISTRICT[0];
            else if (mode >= 20) option = Constant.DISTRICT[mode - 19];
            else if (mode == Constant.ANSWER_BIRTHDATE_YEAR) option = YEAR(Constant.GENERAL_MODE);
            else if (mode == Constant.ANSWER_BIRTHDATE_MONTH) option = Constant.MONTH;
            else if (mode == Constant.ANSWER_BIRTHDATE_DAY) option = Constant.DAY;
            else if (mode == Constant.ANSWER_WHAT_TO_EDIT) option = Constant.MEMBER_EDIT_OPTION;
            else if (mode == Constant.ANSWER_ADDRESS_INCLUDE_ALL_OPTION) option = Constant.DISTRICT_INCLUDE_ALL_OPTION;
            else if (mode == Constant.ANSWER_BIRTHDATE_YEAR_INCLUDE_ALL_OPTION) option = YEAR(Constant.INCLUDE_ALL_OPTION);

            // 방향키 및 엔터, ESC키 통해 정보 선택 혹은 나가기
            while (true)
            {
                Console.SetCursorPosition(cursorLeft, cursorTop);
                Console.Write(new string(' ', Console.WindowWidth - cursorLeft));
                Console.SetCursorPosition(cursorLeft, cursorTop);
                Console.Write(option[index]);

                keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Escape:
                        return -1;
                    case ConsoleKey.Enter:
                        return index;
                    case ConsoleKey.Tab:
                        return index;
                    case ConsoleKey.UpArrow:
                        if (index == 0) index = option.Length - 1;
                        else index--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (index == option.Length - 1) index = 0;
                        else index++;
                        break;
                    default:
                        print.BlockCursorMove(cursorLeft + 4, "");
                        keyInfo = Console.ReadKey();
                        break;
                }
            }
        }
        
        public string PhoneNumber(int cursorLeft, int cursorTop)
        {
            StringBuilder phoneNumber = new StringBuilder("010-");
            StringBuilder middleNumber = new StringBuilder();
            StringBuilder endNumber = new StringBuilder();
            int currentCursor;
            bool isValid;
            ConsoleKeyInfo keyInfo;
            
            Console.SetCursorPosition(cursorLeft, cursorTop);
            Console.Write(phoneNumber);
            print.GuidelineForSearch("xxxx-xxxx", Console.CursorLeft, cursorTop);

            while (true)
            {
                currentCursor = Console.CursorLeft;

                keyInfo = Console.ReadKey();

                isValid = tool.IsValid(keyInfo, Constant.ONLY_NUMBER);

                if (middleNumber.Length == 0) print.DeleteGuideLine(cursorLeft + 4, isValid, keyInfo);

                if (keyInfo.Key == ConsoleKey.Escape) return "@입력취소@";                                                            // 나가기
                else if (keyInfo.Key == ConsoleKey.Backspace) middleNumber = BackspaceInput(cursorLeft + 4, cursorTop, middleNumber); // 지우기
                else if (isValid) middleNumber = ValidInput(currentCursor, 4, keyInfo.KeyChar, middleNumber);                         // 올바른 입력
                else print.InvalidInput(keyInfo, currentCursor, cursorTop);                                                           // 입력 무시

                if (middleNumber.Length == 0) print.GuidelineForSearch("xxxx-xxxx", cursorLeft + 4, cursorTop);
                else if (middleNumber.Length == 4) break;
            }
 
            Console.Write('-');
            phoneNumber.Append(middleNumber + "-");

            print.GuidelineForSearch("xxxx", Console.CursorLeft, cursorTop);

            while (true)
            {
                currentCursor = Console.CursorLeft;

                keyInfo = Console.ReadKey();

                isValid = tool.IsValid(keyInfo, Constant.ONLY_NUMBER);

                if (endNumber.Length == 0) print.DeleteGuideLine(currentCursor, isValid, keyInfo);

                if (keyInfo.Key == ConsoleKey.Escape) return "@입력취소@";                                                        // 나가기
                else if (keyInfo.Key == ConsoleKey.Backspace) endNumber = BackspaceInput(cursorLeft + 9, cursorTop, endNumber);    // 지우기
                else if (isValid) endNumber = ValidInput(currentCursor, 4, keyInfo.KeyChar, endNumber);                           // 올바른 입력
                else print.InvalidInput(keyInfo, currentCursor, cursorTop);                                                       // 입력 무시

                if (endNumber.Length == 0) print.GuidelineForSearch("xxxx", cursorLeft + 9, cursorTop);
                if (endNumber.Length == 4 && keyInfo.Key == ConsoleKey.Enter) break;
            }
            phoneNumber.Append(endNumber);

            Console.WriteLine();

            return phoneNumber.ToString();
        }


        /// <summary>
        /// 사용자가 검색창 입력시 백스페이스 키를 누르면 한글자 지우기를 실행한 후
        /// 그동안 입력한 검색어를 반환하는 메소드입니다.
        /// </summary>
        /// <param name="cursorLeft">커서 설정 변수(들여쓰기)</param>
        /// <param name="cursorTop">커서 설정 변수(줄)</param>
        /// <param name="answer">사용자가 입력한 검색어</param>
        /// <returns>지우기가 실행된 사용자가 입력한 검색어</returns>
        public StringBuilder BackspaceInput(int cursorLeft, int cursorTop, StringBuilder answer)
        {
            if (answer.Length > 0)
            {
                answer.Remove(answer.Length - 1, 1);
                Console.SetCursorPosition(cursorLeft, Console.CursorTop);
                Console.Write(new string(' ', Console.WindowWidth - cursorLeft));
                Console.SetCursorPosition(cursorLeft, cursorTop);
                if (answer.Length != 0) Console.Write(answer);
            }
            else if (answer.Length == 0) Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);

            return answer;
        }

        /// <summary>
        /// 사용자가 검색창 입력시 유효한 문자를 입력하면
        /// 검색어 배열에 문자를 추가하는 메소드입니다.
        /// </summary>
        /// <param name="currentCursor">커서 설정 변수(들여쓰기)</param>
        /// <param name="limit">글자 제한 수</param>
        /// <param name="userInputLetter">사용자가 입력한 문자</param>
        /// <param name="answer">사용자가 입력한 검색어</param>
        /// <returns>갱신된 사용자가 입력한 검색어</returns>
        public StringBuilder ValidInput(int currentCursor, int limit, char userInputLetter, StringBuilder answer)
        {
            if (answer.Length < limit) answer.Append(userInputLetter);
            else
            {
                Console.SetCursorPosition(currentCursor, Console.CursorTop);
                Console.Write(' ');
                Console.SetCursorPosition(currentCursor, Console.CursorTop);
            }

            return answer;
        }

        public List<string> BorrowedBookForEachMember(List<MemberVO> members)
        {
            List<string> borrowedBookForEachMember = new List<string>();
            int count;
            string conditionalExpression;
            string borrowedBook;
            string bookID;

            foreach (MemberVO member in members)
            {
                // member가 대여한 책의 수
                conditionalExpression = " WHERE member_id=" + member.MemberID + " AND date_return IS NULL";
                count = ConnectDatabase.GetCountFromDatabase("history", conditionalExpression);

                borrowedBook = "";

                // member가 대여한 책의 청구기호
                if (count == 0)
                {
                    borrowedBook = "없음";
                }
                else
                {
                    for (int book = 0; book < count; book++)
                    {
                        bookID = ConnectDatabase.SelectFromDatabase("book_id", "history", conditionalExpression)[book];
                        if (bookID.Length == 1) bookID += ".00";

                        borrowedBook += bookID;
                        if (book != count - 1) borrowedBook += ", ";
                    }
                }

                borrowedBookForEachMember.Add(borrowedBook);
            }

            return borrowedBookForEachMember;
        }
    }
}
