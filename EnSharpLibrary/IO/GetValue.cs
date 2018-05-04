using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

using EnSharpLibrary.Data;
using EnSharpLibrary.Function;

namespace EnSharpLibrary.IO
{
    class GetValue
    {
        Print print = new Print();
        Tool tool = new Tool();

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

        //public int NumberOfSameBook(List<BookVO> books, string name, string author, string publisher, int year)
        //{
        //    for (int order = 0; order < books.Count; order++)
        //    {
        //        if (books[order].Name == name && books[order].Author == author &&
        //            books[order].Publisher == publisher && books[order].PublishingYear == year) return (int)Math.Floor(books[order].NumberOfThis);
        //    }

        //    return -1;
        //}

        //public int FirstBooksCount(List<BookVO> books)
        //{
        //    int count = 0;

        //    for (int i = 0; i < books.Count; i++)
        //        if (books[i].OrderOfBooks == 0) count++;

        //    return count;
        //}

        //public int DetailBooksCount(List<BookVO> books, int numberOfBook)
        //{
        //    int count = 0;

        //    for (int i = 0; i < books.Count; i++)
        //        if ((int)Math.Floor(books[i].NumberOfThis) == numberOfBook) count++;

        //    return count;
        //}

        // 1 : 전체 도서 리스트
        // 2 : 대출 도서 리스트
        //public List<float> BookList(List<BookVO> books, int type, int usingMemberNumber)
        //{
        //    List<float> bookList = new List<float>();
        //    bookList.Clear();
            
        //    for (int order = 0; order < books.Count; order++)
        //    {
        //        if (books[order].OrderOfBooks == 0 && type == 1)
        //            bookList.Add((int)Math.Floor(books[order].NumberOfThis));
        //        if (books[order].NumberOfMember == usingMemberNumber && type == 2)
        //            bookList.Add(books[order].NumberOfThis);
        //    }

        //    return bookList;
        //}

        // 0 : (도서검색어)영어, 한글, 숫자 등 문자
        // 1 : (로그인 학번 검색) 숫자
        // 2 : (로그인 암호 검색) 암호
        //public string SearchWord(int limit, int searchType)
        //{
        //    ConsoleKeyInfo keyInfo;
        //    StringBuilder answer = new StringBuilder();
        //    int leftCursor = Console.CursorLeft;
        //    int currentCursor;
        //    int full = 0;

        //    while (true)
        //    {
        //        currentCursor = Console.CursorLeft;
        //        keyInfo = Console.ReadKey();

        //        if (keyInfo.Key == ConsoleKey.Escape) return "@입력취소@";
        //        else if (keyInfo.Key == ConsoleKey.Enter)
        //        {
        //            if (IsValidAnswer(answer.ToString())) return answer.ToString();
        //            else
        //            {
        //                Console.SetCursorPosition(leftCursor, Console.CursorTop);
        //                Console.Write(new string(' ', Console.WindowWidth - leftCursor - 1));
        //                Console.SetCursorPosition(leftCursor, Console.CursorTop);
        //                answer.Clear();
        //            }
        //        }
        //        else if (keyInfo.Key == ConsoleKey.Backspace)
        //        {
        //            if (answer.Length > 0)
        //            {
        //                answer.Remove(answer.Length - 1, 1);
        //                Console.SetCursorPosition(leftCursor, Console.CursorTop);
        //                if (answer.Length == 0)
        //                {
        //                    Console.Write(' ');
        //                    Console.SetCursorPosition(leftCursor, Console.CursorTop);
        //                }
        //                else print.ClearSearchBar(leftCursor, answer.ToString(), searchType);
        //            }
        //            else if (answer.Length == 0) Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
        //        }
        //        else if (IsNotAvailableKey(keyInfo)) Console.SetCursorPosition(currentCursor, Console.CursorTop);
        //        else if (searchType == 1 && IsNotNumber(keyInfo)) Console.SetCursorPosition(currentCursor, Console.CursorTop);
        //        else
        //        {
        //            full = 0;
        //            if (answer.Length < limit) answer.Append(keyInfo.KeyChar);
        //            else
        //            {
        //                if (full == 0)
        //                {
        //                    answer.Remove(answer.Length - 1, 1);
        //                    full = 1;
        //                }
        //                Console.SetCursorPosition(leftCursor, Console.CursorTop);
        //                print.ClearSearchBar(leftCursor, answer.ToString(), searchType);
        //            }
        //            if (searchType == 2) { print.ClearOneLetter(Console.CursorLeft - 1); Console.Write('*'); }
        //        }
        //    }
        //}

        public bool IsNotAvailableKey(ConsoleKeyInfo key)
        {
            List<int> nonAvailableKey = new List<int>();

            nonAvailableKey.Add(9);
            nonAvailableKey.Add(12);
            nonAvailableKey.Add(19);
            nonAvailableKey.Add(33);
            nonAvailableKey.Add(229);
            nonAvailableKey.Add(131);

            for (int valid = 35; valid <= 38; valid++) nonAvailableKey.Add(valid);
            for (int valid = 127; valid <= 135; valid++) nonAvailableKey.Add(valid);
            for (int valid = 166; valid <= 183; valid++) nonAvailableKey.Add(valid);
            for (int valid = 246; valid <= 253; valid++) nonAvailableKey.Add(valid);

            foreach (int valid in nonAvailableKey) if (key.KeyChar == valid) return true;

            return false;
        }

        public bool IsNotNumber(ConsoleKeyInfo key)
        {
            List<int> numbers = new List<int>();

            for (int number = 48; number <= 57; number++) numbers.Add(number);

            bool isNotAvailable = IsNotAvailableKey(key);
            
            if (isNotAvailable) return true;
            else
            {
                foreach (int number in numbers) if (key.KeyChar == number) return false;
                return true;
            }
        }

        //public bool IsValidAnswer(string answer)
        //{
        //    bool isAllSpaceBar = true;

        //    if (answer.Length == 0)
        //    {
        //        print.Announce("입력값이 없습니다!");
        //        return false;
        //    }

        //    foreach (char letter in answer) if (letter != 32) isAllSpaceBar = false;
        //    if (isAllSpaceBar)
        //    {
        //        print.Announce("올바르지 않은 입력값입니다!");
        //        return false;
        //    }

        //    return true;
        //}

        //public bool IsAvailableStudentNumber(List<MemberVO> members, string studentNumber)
        //{
        //    if (string.Compare(studentNumber, "@입력취소@") == 0) return false;
        //    foreach (MemberVO member in members) if (member.IdentificationNumber == Int32.Parse(studentNumber)) return true;
        //    return false;
        //}

        //public bool IsCorrectPassword(List<MemberVO> members, int studentNumber, string password)
        //{
        //    foreach (MemberVO member in members)
        //        if (member.IdentificationNumber == studentNumber && string.Compare(member.Password, password) == 0) return true;
        //    return false;
        //}

        // 2 : 도서명 검색
        // 3 : 출판사 검색
        // 4 : 저자 검색
        //public List<int> FoundBooks(List<BookVO> books, string searchWord, int searchMode)
        //{
        //    List<int> indexesOfSearchResult = new List<int>();
        //    indexesOfSearchResult.Clear();

        //    switch (searchMode)
        //    {
        //        case 2:
        //            for (int i = 0; i < books.Count; i++)
        //                if (System.Text.RegularExpressions.Regex.IsMatch(books[i].Name, searchWord, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        //                {
        //                    bool thereIs = false;
        //                    foreach (int number in indexesOfSearchResult) if ((int)Math.Floor(books[i].NumberOfThis) == number) thereIs = true;

        //                    if (!thereIs) indexesOfSearchResult.Add((int)Math.Floor(books[i].NumberOfThis));
        //                }
        //            break;
        //        case 3:
        //            for (int i = 0; i < books.Count; i++)
        //                if (System.Text.RegularExpressions.Regex.IsMatch(books[i].Publisher, searchWord, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        //                {
        //                    bool thereIs = false;
        //                    foreach (int number in indexesOfSearchResult) if ((int)Math.Floor(books[i].NumberOfThis) == number) thereIs = true;

        //                    if (!thereIs) indexesOfSearchResult.Add((int)Math.Floor(books[i].NumberOfThis));
        //                }
        //            break;
        //        case 4:
        //            for (int i = 0; i < books.Count; i++)
        //                if (System.Text.RegularExpressions.Regex.IsMatch(books[i].Author, searchWord, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        //                {
        //                    bool thereIs = false;
        //                    foreach (int number in indexesOfSearchResult) if ((int)Math.Floor(books[i].NumberOfThis) == number) thereIs = true;

        //                    if (!thereIs) indexesOfSearchResult.Add((int)Math.Floor(books[i].NumberOfThis));
        //                }
        //            break;
        //    }

        //    return indexesOfSearchResult;
        //}

        // 1 : 이름 검색
        // 2 : 학번 검색
        // 3 : 주소 검색
        //public List<int> FoundMembers(List<MemberVO> members, string searchWord, int mode)
        //{
        //    List<int> indexesOfSearchResult = new List<int>();
        //    indexesOfSearchResult.Clear();

        //    switch (mode)
        //    {
        //        case 1:
        //            for (int i = 0; i < members.Count; i++)
        //                if (System.Text.RegularExpressions.Regex.IsMatch(members[i].Name, searchWord, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        //                    indexesOfSearchResult.Add(members[i].IdentificationNumber);
        //            break;
        //        case 2:
        //            for (int i = 0; i < members.Count; i++)
        //                if (System.Text.RegularExpressions.Regex.IsMatch(members[i].IdentificationNumber.ToString(), searchWord, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        //                    indexesOfSearchResult.Add(members[i].IdentificationNumber);
        //            break;
        //        case 3:
        //            for (int i = 0; i < members.Count; i++)
        //                if (System.Text.RegularExpressions.Regex.IsMatch(members[i].Address, searchWord, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        //                    indexesOfSearchResult.Add(members[i].IdentificationNumber);
        //            break;
        //    }

        //    return indexesOfSearchResult;
        //}

        //public List<int> IndexOfBooks(List<BookVO> books, int numberOfBook)
        //{
        //    List<int> indexOfBooks = new List<int>();

        //    for (int order = 0; order < books.Count; order++)
        //        if ((int)Math.Floor(books[order].NumberOfThis) == numberOfBook) indexOfBooks.Add(order);

        //    return indexOfBooks;
        //}

        //public bool DidIOverdue(List<float> borrowedBooks, List<BookVO> books)
        //{
        //    TimeSpan date;

        //    foreach (BookVO book in books)
        //        foreach (float number in borrowedBooks)
        //            if (book.NumberOfThis == number)
        //            {
        //                date = DateTime.Now - book.ExpectedToReturn;
        //                if (date.Days > 0) return true;
        //            }

        //    return false;
        //}

        //public int OverdueDate(BookVO book)
        //{
        //    TimeSpan date;

        //    date = DateTime.Now - book.ExpectedToReturn;

        //    return date.Days;
        //}

        //public List<float> OverdueBooks(List<float> borrowedBooks, List<BookVO> books)
        //{
        //    List<float> overdueBooks = new List<float>();

        //    TimeSpan date;

        //    foreach (BookVO book in books)
        //        foreach (float number in borrowedBooks)
        //            if (book.NumberOfThis == number)
        //            {
        //                date = DateTime.Now - book.ExpectedToReturn;
        //                if (date.Days > 0) overdueBooks.Add(book.NumberOfThis);
        //            }

        //    return overdueBooks;
        //}

        public bool YesOrNoAnswer(int leftCursor, int topCursor)
        {
            string chocie1 = "예";
            string choice2 = "아니오";
            bool isFirstLoop = true;

            while (true)
            {
                if (isFirstLoop)
                {
                    Console.SetCursorPosition(leftCursor + 2, topCursor);
                    Console.Write(chocie1);
                    Console.SetCursorPosition(leftCursor + 2, topCursor + 2);
                    Console.Write(choice2);

                    print.SetCursorAndChoice(leftCursor + 10, 2, '☜');

                    isFirstLoop = false;
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    print.ClearOneLetter(leftCursor + 10);
                    if (Console.CursorTop > topCursor) Console.SetCursorPosition(leftCursor + 10, Console.CursorTop - 2);
                    Console.Write('☜');
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    print.ClearOneLetter(leftCursor + 10);
                    if (Console.CursorTop < topCursor + 2) Console.SetCursorPosition(leftCursor + 10, Console.CursorTop + 2);
                    Console.Write('☜');
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        if (Console.CursorTop == topCursor)
                        {
                            return true;
                        }
                        else if (Console.CursorTop == topCursor + 2)
                        {
                            return false;
                        }
                        isFirstLoop = true;
                    }
                    else
                    {
                        Console.SetCursorPosition(leftCursor + 10, Console.CursorTop);
                        Console.Write("☜ ");
                        Console.SetCursorPosition(leftCursor + 10, Console.CursorTop);
                    }
                }
            }
        }

        public bool DidIBorrowedSameBook(int numberOfBook, List<float> borrowedBook)
        {
            foreach(float book in borrowedBook)
            {
                if (numberOfBook == (int)Math.Floor(book)) return true;
            }

            return false;
        }

        //public int GetIndex(float numberOfBook, List<BookVO> books)
        //{
        //    for (int index = 0; index < books.Count; index++)
        //        if (numberOfBook == books[index].NumberOfThis)
        //            return index;

        //    return -1;
        //}

        public int NotValidPassword(string newPassword)
        {
            if (newPassword.Length < 8 || newPassword.Length > 15) return 1;

            for (int letter = 0; letter < newPassword.Length; letter++)
            {
                if ((newPassword[letter] >= 'a' && newPassword[letter] <= 'z') ||
                    (newPassword[letter] >= 'A' && newPassword[letter] <= 'Z') ||
                    (newPassword[letter] >= '0' && newPassword[letter] <= '9'))
                    continue;
                else return 3;
            }

            for (int i = 0; i < newPassword.Length - 2; i++)
            {
                if (newPassword[i + 2] - newPassword[i + 1] == 1 && newPassword[i + 1] - newPassword[i] == 1)
                    return 2;
            }

            return 0;
        }

        //public int NotValidNumber(string number, List<MemberVO> members)
        //{
        //    StringBuilder twoLetters = new StringBuilder(number);
        //    int year;
        //    int nextyear = DateTime.Now.AddYears(1).Year - 2000;

        //    if (number.Length != 8) return 1;
        //    foreach (MemberVO member in members)
        //        if (string.Compare(number, member.IdentificationNumber.ToString()) == 0) return 2;

        //    twoLetters.Remove(2, 6);
        //    year = Int32.Parse(twoLetters.ToString());

        //    if (year >= nextyear && year <= 89) return 3;

        //    return 0;
        //}

        public bool NotValidPhoneNumber(string newPhoneNumber, List<MemberVO> members)
        {
            if (newPhoneNumber.Length != 11) return true;
            int middleNumber;

            StringBuilder phoneNumber = new StringBuilder();
            phoneNumber.Append(newPhoneNumber);
            phoneNumber.Insert(3, '-');
            phoneNumber.Insert(8, '-'); 
            
            foreach (MemberVO member in members)
                if (string.Compare(member.PhoneNumber, phoneNumber.ToString()) == 0) return true;

            phoneNumber.Remove(3, 10);
            if (string.Compare(phoneNumber.ToString(), "010") != 0) return true;

            StringBuilder middle = new StringBuilder();
            middle.Append(newPhoneNumber);
            middle.Remove(5, 6);
            middle.Remove(0, 3);
            middleNumber = Int32.Parse(middle.ToString());

            if (middleNumber >= 0 && middleNumber <= 19 || middleNumber >= 59 && middleNumber <= 61) return true;

            return false;
        }

        public int NotValidPhoneNumberSpecifically(string phoneNumber, List<MemberVO> members)
        {
            StringBuilder newPhoneNumber = new StringBuilder(phoneNumber);
            newPhoneNumber.Insert(3, '-');
            newPhoneNumber.Insert(8, '-');

            if (phoneNumber.Length != 11) return 1;
            foreach (MemberVO member in members)
            {
                if (member.PhoneNumber == newPhoneNumber.ToString()) return 2;
            }
            return 0;
        }
    }
}
