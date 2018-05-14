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

        public string GuideForModifyingBookCondition(string bookCondition)
        {
            if (string.Compare(bookCondition, "삭제") == 0) return "                          X";

            if (string.Compare(bookCondition, "대출 가능") == 0) return Constant.GUIDE_FOR_MODIFYING_BOOK_CONDITION[0];
            else if (string.Compare(bookCondition, "대출중") == 0) return Constant.GUIDE_FOR_MODIFYING_BOOK_CONDITION[1];
            else if (string.Compare(bookCondition, "보관도서") == 0) return Constant.GUIDE_FOR_MODIFYING_BOOK_CONDITION[3];
            else return Constant.GUIDE_FOR_MODIFYING_BOOK_CONDITION[2];
        }

        public string DetailInformationAboutBorrowedMember(int mode, float applicationNumber)
        {
            string information;
            string column = Constant.COLUMN_NAME_FOR_DETAIL_INFORMATION[mode];
            information = ConnectDatabase.SelectFromDatabase(column, "history", "book_id", applicationNumber.ToString(), "date_return")[0];

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

        public List<BookAPIVO> OrganizedFoundBook(XmlDocument bookInformation)
        {
            List<BookAPIVO> books = new List<BookAPIVO>();
            List<string> bookTitle;
            List<string> bookDescription;

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
                BookAPIVO book = new BookAPIVO(bookTitle[count + 1], publisher[count].InnerText, pubdate[count].InnerText);
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

        public List<BookVO> SearchBookByID(int mode, int ID)
        {
            List<BookVO> searchedBook = new List<BookVO>();
            StringBuilder sql = new StringBuilder();

            if (mode == Constant.BOOK_ID) sql.Append("SELECT * FROM book WHERE FLOOR(book_id)=" + ID + ";");
            else sql.Append("SELECT * FROM book WHERE FLOOR(borrowed_member_id)=" + ID + ";");

            string nameForVO;
            string authorForVO;
            string publisherForVO;
            int publishingYearForVO;
            float bookIDForVO;
            string bookConditionForVO;
            int borrowedMemberIDForVO;
            int priceForVO;

            String databaseConnect;
            MySqlConnection connect;

            databaseConnect = "Server=Localhost;Database=ensharp_library;Uid=root;Pwd=0000";
            connect = new MySqlConnection(databaseConnect);

            connect.Open();

            MySqlCommand command = new MySqlCommand(sql.ToString(), connect);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                nameForVO = reader["name"].ToString();
                authorForVO = reader["author"].ToString();
                publisherForVO = reader["publisher"].ToString();
                publishingYearForVO = Int32.Parse(reader["publishing_year"].ToString());
                bookIDForVO = float.Parse(reader["book_id"].ToString());
                bookConditionForVO = reader["book_condition"].ToString();
                borrowedMemberIDForVO = Int32.Parse(reader["borrowed_member_id"].ToString());
                priceForVO = Int32.Parse(reader["price"].ToString());

                BookVO book = new BookVO(nameForVO, authorForVO, publisherForVO, publishingYearForVO);
                book.AppendInformation(bookIDForVO, bookConditionForVO, borrowedMemberIDForVO, priceForVO);

                searchedBook.Add(book);
            }

            reader.Close();

            return searchedBook;
        }

        public List<BookVO> SearchBookByCondition(string bookName, string publisher, string author)
        {
            string nameForVO;
            string authorForVO;
            string publisherForVO;
            int publishingYearForVO;
            float bookIDForVO;
            string bookConditionForVO;
            int borrowedMemberIDForVO;
            int priceForVO;
            int numberOfBooksForVO = 0;

            List<BookVO> searchedBook = new List<BookVO>();
            StringBuilder sql = new StringBuilder("SELECT * FROM book ");

            String databaseConnect;
            MySqlConnection connect;

            databaseConnect = "Server=Localhost;Database=ensharp_library;Uid=root;Pwd=0000";
            connect = new MySqlConnection(databaseConnect);

            connect.Open();

            if (bookName.Length != 0) sql.Append("WHERE name REGEXP \'" + bookName + "\'");
            if (publisher.Length != 0 && bookName.Length != 0) sql.Append("AND publisher REGEXP \'" + publisher + "\'");
            else if (publisher.Length != 0) sql.Append("WHERE publisher REGEXP \'" + publisher + "\'");
            if (author.Length != 0 && (bookName.Length != 0 || publisher.Length != 0)) sql.Append("AND publisher REGEXP \'" + publisher + "\'");
            else if (author.Length != 0) sql.Append("WHERE author REGEXP \'" + author + "\'");
            sql.Append(";");

            MySqlCommand command = new MySqlCommand(sql.ToString(), connect);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                nameForVO = reader["name"].ToString();
                authorForVO = reader["author"].ToString();
                publisherForVO = reader["publisher"].ToString();
                publishingYearForVO = Int32.Parse(reader["publishing_year"].ToString());
                bookIDForVO = float.Parse(reader["book_id"].ToString());
                bookConditionForVO = reader["book_condition"].ToString();
                borrowedMemberIDForVO = Int32.Parse(reader["borrowed_member_id"].ToString());
                priceForVO = Int32.Parse(reader["price"].ToString());

                BookVO book = new BookVO(nameForVO, authorForVO, publisherForVO, publishingYearForVO);
                book.AppendInformation(bookIDForVO, bookConditionForVO, borrowedMemberIDForVO, priceForVO);

                if (book.BookID - Math.Floor(book.BookID) == 0) searchedBook.Add(book);
            }

            reader.Close();

            foreach (BookVO book in searchedBook)
            {
                sql.Clear();
                sql.Append("SELECT count(*) FROM book WHERE FLOOR(book_id)=");
                sql.Append(Math.Floor(book.BookID));
                sql.Append(";");

                command = new MySqlCommand(sql.ToString(), connect);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    numberOfBooksForVO = Int32.Parse(reader["count(*)"].ToString());
                    book.NumberOfBooks = numberOfBooksForVO;
                }

                reader.Close();
            }
            
            connect.Close();

            return searchedBook;
        }

        public List<MemberVO> SearchMemberByCondition(string name, int age, string address)
        {
            List<MemberVO> searchedMember = new List<MemberVO>();
            StringBuilder sql = new StringBuilder("SELECT * FROM member ");
            int memberID;
            string memberName;
            string memberAddress;
            string phoneNumber;
            string password;
            int accumulatedOverdueNumber;
            int overdueNumber;
            string birthdate;
            string[] date;
            int year, month, day;

            if (name.Length != 0) sql.Append("WHERE name REGEXP \'" + name + "\'");
            if (age != 0)
            {
                if (name.Length == 0) sql.Append("WHERE birthdate=" + age + 1989);
                else sql.Append(" AND birthdate=" + age + 1989);
            }
            if (string.Compare(address, "전체") != 0)
            {
                if (name.Length == 0 || age == 0) sql.Append("WHERE address REGEXP \'" + address + "\'");
                else sql.Append(" AND address REGEXP \'" + address + "\'");
            }
            sql.Append(";");

            String databaseConnect;
            MySqlConnection connect;

            databaseConnect = "Server=Localhost;Database=ensharp_library;Uid=root;Pwd=0000";
            connect = new MySqlConnection(databaseConnect);

            connect.Open();

            MySqlCommand command = new MySqlCommand(sql.ToString(), connect);
            MySqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                memberID = Int32.Parse(reader["member_id"].ToString());
                memberName = reader["name"].ToString();
                memberAddress = reader["address"].ToString();
                phoneNumber = reader["phone_number"].ToString();
                password = reader["password"].ToString();
                accumulatedOverdueNumber = Int32.Parse(reader["accumulated_overdue_number"].ToString());
                overdueNumber = Int32.Parse(reader["overdue_number"].ToString());
                birthdate = reader["birthdate"].ToString().Remove(10, 12);
                date = birthdate.Split('-');
                year = Int32.Parse(date[0]);
                month = Int32.Parse(date[1]);
                day = Int32.Parse(date[2]);

                MemberVO member = new MemberVO(memberID, memberName, password);
                member.AppendInformation(memberAddress, phoneNumber, new DateTime(year, month, day));
                member.AppendInformation(accumulatedOverdueNumber, overdueNumber);

                searchedMember.Add(member); 
            }
            
            reader.Close();
            connect.Close();

            return searchedMember;
        }

        public string Password(int memberID)
        {
            string password = "";

            StringBuilder sql = new StringBuilder("SELECT password FROM member WHERE member_id=" + memberID + ";");

            String databaseConnect;
            MySqlConnection connect;

            databaseConnect = "Server=Localhost;Database=ensharp_library;Uid=root;Pwd=0000";
            connect = new MySqlConnection(databaseConnect);

            connect.Open();

            MySqlCommand command = new MySqlCommand(sql.ToString(), connect);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) password = reader["password"].ToString();

            reader.Close();
            connect.Close();

            return password;
        }

        public List<HistoryVO> BookHistory(int memberID)
        {
            List<HistoryVO> histories = new List<HistoryVO>();
            StringBuilder sql = new StringBuilder("SELECT * FROM history WHERE member_id=" + memberID + " AND date_return IS NULL;");

            String databaseConnect;
            MySqlConnection connect;

            databaseConnect = "Server=Localhost;Database=ensharp_library;Uid=root;Pwd=0000";
            connect = new MySqlConnection(databaseConnect);

            connect.Open();

            MySqlCommand command = new MySqlCommand(sql.ToString(), connect);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string date1 = reader["date_borrowed"].ToString().Remove(10, 12);
                string[] date2 = date1.Split('-');
                string date3 = reader["date_deadline_for_return"].ToString().Remove(10, 12);
                string[] date4 = date3.Split('-');
                int numberOfRenew = Int32.Parse(reader["number_of_renew"].ToString());

                HistoryVO history = new HistoryVO(new DateTime(Int32.Parse(date2[0]), Int32.Parse(date2[1]), Int32.Parse(date2[2])),
                    new DateTime(Int32.Parse(date4[0]), Int32.Parse(date4[1]), Int32.Parse(date4[2])), numberOfRenew);

                histories.Add(history);
            }

            reader.Close();
            connect.Close();

            return histories;
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
            StringBuilder sql = new StringBuilder();
            StringBuilder IDInOnePlace = new StringBuilder();
            int count = 0;

            String databaseConnect;
            MySqlConnection connect;

            databaseConnect = "Server=Localhost;Database=ensharp_library;Uid=root;Pwd=0000";
            connect = new MySqlConnection(databaseConnect);

            connect.Open();

            foreach (MemberVO member in members)
            {
                sql.Clear();
                sql.Append("SELECT count(*) FROM history WHERE member_id=" + member.MemberID + " AND date_return IS NULL;");

                MySqlCommand command = new MySqlCommand(sql.ToString(), connect);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) count = Int32.Parse(reader["count(*)"].ToString());
                reader.Close();

                IDInOnePlace.Clear();
                if (count == 0) { IDInOnePlace.Append("없음"); borrowedBookForEachMember.Add(IDInOnePlace.ToString()); }
                else
                {
                    sql.Clear();
                    sql.Append("SELECT * FROM history WHERE member_id=" + member.MemberID + " AND date_return IS NULL;");
                    command = new MySqlCommand(sql.ToString(), connect);
                    reader = command.ExecuteReader();

                    while (reader.Read()) IDInOnePlace.Append(reader["book_id"].ToString() + " ");

                    borrowedBookForEachMember.Add(IDInOnePlace.ToString());

                    reader.Close();
                }
            }

            connect.Close();

            return borrowedBookForEachMember;
        }

        public float BookID(string name, string author, string publisher, int publishingYear)
        {
            StringBuilder sql = new StringBuilder("SELECT count(*) FROM book WHERE name=\'" + name + "\' AND author=\'" + author + "\' AND publisher=\'" + publisher + "\' AND publishing_year=" + publishingYear + ";");
            float count = 0;
            float bookCount = 0;
                       
            String databaseConnect;
            MySqlConnection connect;

            databaseConnect = "Server=Localhost;Database=ensharp_library;Uid=root;Pwd=0000";
            connect = new MySqlConnection(databaseConnect);

            connect.Open();

            MySqlCommand command = new MySqlCommand(sql.ToString(), connect);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) count = float.Parse(reader["count(*)"].ToString());
            reader.Close();

            sql.Clear();
            sql.Append("SELECT count(*) FROM book;");

            command = new MySqlCommand(sql.ToString(), connect);
            reader = command.ExecuteReader();

            while (reader.Read()) bookCount = float.Parse(reader["count(*)"].ToString());
            reader.Close();
            connect.Close();

            float ID = bookCount + 1 + (count / 100);

            return ID;
        }
    }
}
