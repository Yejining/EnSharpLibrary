using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EnSharpLibrary.Data;
using EnSharpLibrary.IO;

namespace EnSharpLibrary.Function
{
    class MemberManage
    {
        Print print = new Print();
        GetValue getValue = new GetValue();
        Tool tool = new Tool();

        /// <summary>
        /// 사용자의 ID에 따라 로그인 혹은 로그아웃 작업을 진행하는 메소드입니다.
        /// </summary>
        /// <param name="usingMemberID">사용자 ID</param>
        /// <returns>갱신된 사용자 ID</returns>
        public int LogInOrLogOut(int usingMemberID)
        {
            if (usingMemberID == Constant.PUBLIC) return LogIn(Constant.ADMIN_MODE);
            else return LogOut();
        }

        /// <summary>
        /// 로그인 기능을 수행하는 메소드입니다.
        /// </summary>
        /// <param name="mode">일반/관리자</param>
        /// <returns>로그인한 사용자 ID</returns>
        public int LogIn(int mode)
        {
            string memberID;
            string userInputPassword;

            print.SetWindowsizeAndPrintTitle(45, 30, Constant.LOG_IN_TITLE[mode]);

            while (true)
            {
                print.SearchCategoryAndGuideline(Constant.LOG_IN_MODE);
                if (mode == Constant.ADMIN_MODE) { Console.SetCursorPosition(0, 11); print.ClearCurrentConsoleLine(); }

                // 학번
                if (mode == Constant.MEMBER_MODE)
                {
                    memberID = getValue.Information(17, 11, 8, Constant.ONLY_NUMBER, Constant.LOGIN_SEARCH_CATEGORY_AND_GUIDELINE[1]);
                    if (string.Compare(memberID, "@입력취소@") == 0) return 0;
                    else if (memberID.Length == 0) { print.ErrorMessage(Constant.THERE_IS_NO_SEARCHWORD, 17); continue; }
                    else if (!tool.IsExist(memberID)) { print.ErrorMessage(Constant.THERE_IS_NO_SUCH_ID, 17); continue; }
                }
                else memberID = Constant.ADMIN.ToString();

                // 암호
                userInputPassword = getValue.Information(17, 13, 15, Constant.NO_KOREAN, Constant.LOGIN_SEARCH_CATEGORY_AND_GUIDELINE[3]);
                if (string.Compare(userInputPassword, "@입력취소@") == 0) return 0;
                else if (userInputPassword.Length == 0) { print.ErrorMessage(Constant.THERE_IS_NO_SEARCHWORD, 17); continue; }
                else if (!tool.IsPasswordCorrespond(memberID, userInputPassword)) { print.ErrorMessage(Constant.PASSWORD_IS_WRONG, 17); continue; }

                return Int32.Parse(memberID);
            }
        }

        /// <summary>
        /// 로그아웃 기능을 수행하는 메소드입니다.
        /// </summary>
        /// <returns>공용 사용자 ID</returns>
        public int LogOut()
        {
            return Constant.PUBLIC;
        }

        /// <summary>
        /// 회원가입 기능을 수행하는 메소드입니다.
        /// 사용자로부터 정보를 입력받고, 데이터베이스에 정보를 등록합니다.
        /// </summary>
        /// <param name="title">"회원가입"</param>
        /// <returns>새로 만들어진 사용자 ID</returns>
        public int JoinIn(string title)
        {
            string name;
            string userID;
            string password;
            int address1;
            int address2;
            StringBuilder address = new StringBuilder();
            string phoneNumber;
            int year, month, day;
            DateTime birthdate;

            print.SetWindowsizeAndPrintTitle(45, 30, title);

            while (true)
            {
                print.SearchCategoryAndGuideline(Constant.JOIN_IN);

                // 정보 수집
                // - 이름
                name = getValue.Information(17, 11, 5, Constant.ONLY_KOREAN, Constant.JOIN_SEARCH_CATEGORY_AND_GUIDELINE[1]);
                int errorMode = tool.IsValidAnswer(Constant.ANSWER_NAME, name);
                if (errorMode == Constant.ESCAPE_KEY_ERROR) return Constant.NONE;
                else if (errorMode != Constant.VALID) { print.ErrorMessage(errorMode, 23); continue; }

                // - 학번
                userID = getValue.Information(17, 13, 8, Constant.ONLY_NUMBER, Constant.JOIN_SEARCH_CATEGORY_AND_GUIDELINE[3]);
                errorMode = tool.IsValidAnswer(Constant.ANSWER_USER_ID, userID);
                if (errorMode == Constant.ESCAPE_KEY_ERROR) return Constant.NONE;
                else if (errorMode != Constant.VALID) { print.ErrorMessage(errorMode, 23); continue; }

                // - 암호
                password = getValue.Information(17, 15, 15, Constant.NO_KOREAN, Constant.JOIN_SEARCH_CATEGORY_AND_GUIDELINE[5]);
                errorMode = tool.IsValidAnswer(Constant.ANSWER_PASSWORD, password);
                if (errorMode == Constant.ESCAPE_KEY_ERROR) return Constant.NONE;
                else if (errorMode != Constant.VALID) { print.ErrorMessage(errorMode, 23); continue; }

                // - 주소
                address1 = getValue.DropBox(17, 17, Constant.ANSWER_ADDRESS);
                Console.SetCursorPosition(17, 17);
                Console.Write(Constant.DISTRICT[0][address1]);
                address2 = getValue.DropBox(Console.CursorLeft + 1, 17, Constant.ANSWER_ADDRESS_DEEPLY + address1);
                address.Append(Constant.DISTRICT[0][address1] + " " + Constant.DISTRICT[address1 + 1][address2]);

                // 전화번호
                phoneNumber = getValue.PhoneNumber(21, 19);
                if (string.Compare(phoneNumber, "@입력취소@") == 0) return Constant.NONE;

                // 생일
                year = getValue.DropBox(17, 21, Constant.ANSWER_BIRTHDATE_YEAR); if (year == -1) return Constant.NONE;
                month = getValue.DropBox(24, 21, Constant.ANSWER_BIRTHDATE_MONTH); if (month == -1) return Constant.NONE;
                day = getValue.DropBox(29, 21, Constant.ANSWER_BIRTHDATE_DAY); if (day == -1) return Constant.NONE;
                birthdate = getValue.Birthdate(year, month, day);

                break;
            }

            tool.RegisterMember(name, Int32.Parse(userID), password, address.ToString(), phoneNumber, birthdate);

            return Int32.Parse(userID);
        }

        /// <summary>
        /// 사용자의 정보를 수정하는 메소드입니다.
        /// </summary>
        /// <param name="usingMemberID">사용자 ID</param>
        public void ChangeUserInformation(int usingMemberID)
        {
            int informationToEdit;
            int currentCursor;
            StringBuilder sql = new StringBuilder();

            // 배경 출력
            if (usingMemberID == Constant.ADMIN) print.SetWindowsizeAndPrintTitle(45, 30, "암호수정");
            else print.SetWindowsizeAndPrintTitle(45, 30, "정보수정");

            if (usingMemberID != Constant.ADMIN)
            {
                Console.SetCursorPosition(10, 12);
                Console.Write("수정할 정보 | ");
                informationToEdit = getValue.DropBox(Console.CursorLeft, 12, Constant.ANSWER_WHAT_TO_EDIT);
            }
            else informationToEdit = Constant.EDIT_PASSWORD;

            Console.SetCursorPosition(10, 14);
            if (informationToEdit == Constant.EDIT_PASSWORD) Console.Write("현재 ");
            Console.Write(Constant.MEMBER_EDIT_OPTION[informationToEdit] + " | ");

            currentCursor = Console.CursorLeft;

            while (true)
            {
                Console.SetCursorPosition(currentCursor, 14); print.ClearSearchBar(currentCursor, "", 1);
                for (int clear = 1; clear < 5; clear++) { print.ClearCurrentConsoleLine(); Console.SetCursorPosition(0, 14 + clear); }
                Console.SetCursorPosition(0, 14);
                sql.Clear();

                if (informationToEdit == Constant.EDIT_ADDRESS)                                         // 주소 수정
                {
                    int address1 = getValue.DropBox(currentCursor, 14, Constant.ANSWER_ADDRESS);
                    if (address1 == -1) return;
                    Console.SetCursorPosition(currentCursor, 14);
                    Console.Write(Constant.DISTRICT[0][address1]);
                    int address2 = getValue.DropBox(Console.CursorLeft + 1, 14, Constant.ANSWER_ADDRESS_DEEPLY + address1);
                    if (address2 == -1) return;
                    StringBuilder address = new StringBuilder(Constant.DISTRICT[0][address1] + " " + Constant.DISTRICT[address1 + 1][address2]);
                    sql.Append("UPDATE member SET address=\'" + address + "\' WHERE member_id=" + usingMemberID + ";");
                }
                else if (informationToEdit == Constant.EDIT_PHONE_NUMBER)                               // 전화번호 수정
                {
                    string phoneNumber = getValue.PhoneNumber(currentCursor, 14);
                    if (string.Compare(phoneNumber, "@입력취소@") == 0) return;
                    sql.Append("UPDATE member SET phone_number=\'" + phoneNumber + "\' WHERE member_id=" + usingMemberID + ";");
                }
                else if (informationToEdit == Constant.EDIT_PASSWORD)                                   // 암호 수정
                {
                    print.GuidelineForSearch("입력", currentCursor, 14);
                    string password = getValue.Information(currentCursor, 14, 15, Constant.NO_KOREAN, Constant.LOGIN_SEARCH_CATEGORY_AND_GUIDELINE[3]);
                    if (string.Compare(password, "@입력취소@") == 0) return;
                    else if (password.Length == 0) { print.ErrorMessage(Constant.THERE_IS_NO_SEARCHWORD, 16); continue; }
                    else if (!tool.IsPasswordCorrespond(usingMemberID.ToString(), password)) { print.ErrorMessage(Constant.PASSWORD_IS_WRONG, 16); continue; }

                    Console.SetCursorPosition(10, 16);
                    Console.Write("새 암호 입력 | ");
                    print.GuidelineForSearch("입력", Console.CursorLeft, 16);
                    string newPassword = getValue.Information(Console.CursorLeft, 16, 15, Constant.NO_KOREAN, Constant.LOGIN_SEARCH_CATEGORY_AND_GUIDELINE[3]);
                    if (string.Compare(newPassword, "@입력취소@") == 0) return;
                    else if (newPassword.Length == 0) { print.ErrorMessage(Constant.THERE_IS_NO_SEARCHWORD, 18); continue; }
                    else if (newPassword.Length < 8) { print.ErrorMessage(Constant.WRONG_LENGTH_ERROR, 18); continue; }
                    sql.Append("UPDATE member SET password=\'" + newPassword + "\' WHERE member_id=" + usingMemberID + ";");
                }
                tool.MakeQuerry(sql.ToString());

                break;
            }

            print.Announce("변경이 완료되었습니다!", Console.CursorTop + 2);
        }

        /// <summary>
        /// 관리자가 회원을 관리하는 메소드입니다.
        /// </summary>
        public void ManageMember()
        {
            bool isFirstLoop = true;
            
            while (true)
            {
                if (isFirstLoop)
                {
                    // 메뉴 출력
                    print.SetWindowsizeAndPrintTitle(45, 30, "회원 관리");
                    print.MenuOption(Constant.MANAGE_MEMBER_MODE, Console.CursorTop + 2);

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

        /// <summary>
        /// 관리자가 회원 관리시 다음으로 수행할 메소드를 고르는 메소드입니다.
        /// </summary>
        /// <param name="cursorTop">현재 커서 위치</param>
        /// <returns></returns>
        public bool GoNextFunction(int cursorTop)
        {
            switch (Console.CursorTop)
            {
                case Constant.APPEND_MEMBER: JoinIn("회원 등록"); return true;
                case Constant.MANAGE_REGISTERED_MEMBER: SearchMember(); return true;
            }

            return true;
        }

        /// <summary>
        /// 관리자가 회원 정보를 검색하는 메소드입니다.
        /// </summary>
        public void SearchMember()
        {
            List<MemberVO> searchedMember;
            string name;
            int age;

            print.SearchCategoryAndGuideline(Constant.MEMBER_SEARCH_MODE);

            // 정보 수집
            // -이름
            name = getValue.Information(22, 11, 10, Constant.ONLY_KOREAN, Constant.MEMBER_SEARCH_CATEGORY_AND_GUIDELINE[1]);
            if (string.Compare(name, "@입력취소@") == 0) return;
            age = getValue.DropBox(22, 13, Constant.ANSWER_BIRTHDATE_YEAR_INCLUDE_ALL_OPTION);
            if (age == -1) return;

            // - 주소
            int currentCursor = 22;
            int address1 = getValue.DropBox(currentCursor, 15, Constant.ANSWER_ADDRESS_INCLUDE_ALL_OPTION);
            if (address1 == -1) return;
            Console.SetCursorPosition(currentCursor, 15);
            Console.Write(Constant.DISTRICT_INCLUDE_ALL_OPTION[address1]);
            StringBuilder address = new StringBuilder(Constant.DISTRICT_INCLUDE_ALL_OPTION[address1]);
            if (address1 != 0)
            {
                int address2 = getValue.DropBox(Console.CursorLeft + 1, 15, Constant.ANSWER_ADDRESS_DEEPLY + address1 - 1);
                if (address2 == -1) return;
                address.Append(" " + Constant.DISTRICT[address1][address2]);
                if (string.Compare(address.ToString(), "@입력취소@") == 0) return;
            }

            // 검색
            searchedMember = getValue.SearchMemberByCondition(name, age, address.ToString());
            List<string> borrowedBookForEachMember = getValue.BorrowedBookForEachMember(searchedMember);

            // 열람
            CheckMemberAndDelete(searchedMember, borrowedBookForEachMember, name, age.ToString(), address.ToString());
        }

        /// <summary>
        /// 관리자가 검색한 회원 정보를 열람하는 메소드입니다.
        /// </summary>
        /// <param name="searchedMember">검색된 회원 정보</param>
        /// <param name="name">관리자가 검색한 회원 이름</param>
        /// <param name="age">관리자가 검색한 회원 출생년도</param>
        /// <param name="address">관리자가 검색한 회원 주소</param>
        public void CheckMemberAndDelete(List<MemberVO> searchedMember, List<string> borrowedBookForEachMember, string name, string age, string address)
        {
            bool isFirstLoop = true;
            int cursorTop = 13;

            print.SearchedMember(searchedMember, borrowedBookForEachMember, name, age, address);
            if (searchedMember.Count == 0) return;

            // 방향키 및 엔터, ESC키를 이용해 기능 수행
            while (true)
            {
                if (isFirstLoop)
                {
                    Console.SetCursorPosition(4, cursorTop);
                    Console.Write('▷');
                    print.Members(searchedMember, borrowedBookForEachMember, cursorTop);
                    Console.SetCursorPosition(4, cursorTop);
                    isFirstLoop = false;
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow) tool.UpArrow(4, cursorTop, searchedMember.Count, 1, '▷');          // 위로 커서 옮김
                else if (keyInfo.Key == ConsoleKey.DownArrow) tool.DownArrow(4, cursorTop, searchedMember.Count, 1, '▷'); // 밑으로 커서 옮김
                else if (keyInfo.Key == ConsoleKey.Escape) { print.BlockCursorMove(4, "▷"); return; }                     // 나가기
                else if (keyInfo.Key == ConsoleKey.Enter)                                                                  // 해당 회원 선택
                {
                    // db에서 삭제

                    // serachedMember에서 삭제
                    searchedMember.RemoveAt(Console.CursorTop - cursorTop);

                    // 이전 검색 결과 지움
                    Console.SetCursorPosition(0, cursorTop);
                    for (int clear = 0; clear <= searchedMember.Count + 1; clear++) { print.ClearCurrentConsoleLine(); Console.SetCursorPosition(0, cursorTop + clear); }
                    isFirstLoop = true;
                }
                else print.BlockCursorMove(4, "▷");                                                                       // 입력 무시 
            }
        }    
    }
}

