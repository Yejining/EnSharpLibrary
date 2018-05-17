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
                    if (getValue.IsCanceled(memberID)) return 0;
                    else if (memberID.Length == 0) { print.ErrorMessage(Constant.THERE_IS_NO_SEARCHWORD, 17); continue; }
                    else if (!tool.IsExist(memberID)) { print.ErrorMessage(Constant.THERE_IS_NO_SUCH_ID, 17); continue; }
                }
                else memberID = Constant.ADMIN.ToString();

                // 암호
                userInputPassword = getValue.Information(17, 13, 15, Constant.NO_KOREAN, Constant.LOGIN_SEARCH_CATEGORY_AND_GUIDELINE[3]);
                if (getValue.IsCanceled(userInputPassword)) return 0;
                else if (userInputPassword.Length == 0) { print.ErrorMessage(Constant.THERE_IS_NO_SEARCHWORD, 17); continue; }
                else if (!getValue.IsPasswordCorrespond(memberID, userInputPassword)) { print.ErrorMessage(Constant.PASSWORD_IS_WRONG, 17); continue; }

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
            string address;
            string phoneNumber;
            DateTime birthdate;

            print.SetWindowsizeAndPrintTitle(45, 30, title);
            print.SearchCategoryAndGuideline(Constant.JOIN_IN);

            // 정보 수집
            getValue.UserInformationFromUser(out name, out userID, out password, out address, out phoneNumber, out birthdate);
            if (getValue.IsCanceled(name) || getValue.IsCanceled(userID) || getValue.IsCanceled(password)) return Constant.PUBLIC;
            else if (getValue.IsCanceled(address) || getValue.IsCanceled(phoneNumber) || birthdate == new DateTime(1980, 1, 1)) return Constant.PUBLIC;

            // 회원 등록
            RegisterMember(name, Int32.Parse(userID), password, address.ToString(), phoneNumber, birthdate);

            return Int32.Parse(userID);
        }

        public void RegisterMember(string name, int userID, string password, string address, string phoneNumber, DateTime birthdate)
        {
            string table = "member";
            string columns = "(member_id, name, address, phone_number, password, accumulated_overdue_number, overdue_number, birthdate)";
            string values = "(" + userID + ",\'" + name + "\',\'" + address + "\',\'" + phoneNumber + "\',\'" + password + "\',0,0,\'" + birthdate.ToShortDateString() + "\');";

            ConnectDatabase.InsertIntoDatabase(table, columns, values);            
        }

        /// <summary>
        /// 사용자의 정보를 수정하는 메소드입니다.
        /// </summary>
        /// <param name="usingMemberID">사용자 ID</param>
        public void ChangeUserInformation(int usingMemberID)
        {
            int informationToEdit;
            int cursorLeft, cursorTop;
            string name, address, phoneNumber;
            DateTime birthDate;
            bool isChanged = true;

            // 배경 출력
            getValue.UserInformationFromDatabase(usingMemberID, out name, out address, out phoneNumber, out birthDate);
            print.ChangeUserInformationTitle(usingMemberID, name, address, phoneNumber, birthDate);

            // 사용자로부터 수정할 정보 입력받음
            informationToEdit = getValue.GetDataTypeFromUser(usingMemberID, out cursorLeft, out cursorTop);

            // 사용자 정보 수정
            switch (informationToEdit)
            {
                case (Constant.EDIT_ADDRESS):
                    isChanged = EditAddress(usingMemberID, cursorLeft, cursorTop);
                    break;
                case (Constant.EDIT_PHONE_NUMBER):
                    isChanged = EditPhoneNumber(usingMemberID, cursorLeft, cursorTop);
                    break;
                case (Constant.EDIT_PASSWORD):
                    isChanged = EditPassword(usingMemberID, cursorLeft, cursorTop);
                    break;
                default:
                    return;
            }

            if (!isChanged) return;

            print.PrintSentence("변경이 완료되었습니다!(나가기:ESC)", Console.CursorTop + 2, Constant.FOREGROUND_COLOR);
            tool.WaitUntilGetEscapeKey();
        }

        public bool EditAddress(int usingMemberID, int cursorLeft, int cursorTop)
        {
            int address1, address2;
            string address;

            // 도단위 정보 입력
            address1 = getValue.DropBox(cursorLeft, cursorTop, Constant.ANSWER_ADDRESS);
            if (address1 == -1) return false;
            print.SetCursorAndWrite(cursorLeft, cursorTop, Constant.DISTRICT[0][address1]);

            // 시/군/구단위 정보 입력
            address2 = getValue.DropBox(Console.CursorLeft + 1, cursorTop, Constant.ANSWER_ADDRESS_DEEPLY + address1);
            if (address2 == -1) return false;
            address = Constant.DISTRICT[0][address1] + " " + Constant.DISTRICT[address1 + 1][address2];
            print.SetCursorAndWrite(cursorLeft, cursorTop, address);

            // DB에 변경할 주소 저장
            ConnectDatabase.UpdateToDatabase("member", "address", address, "member_id", usingMemberID.ToString());

            return true;
        }

        public bool EditPhoneNumber(int usingMemberID, int cursorLeft, int cursorTop)
        {
            string phoneNumber;

            // 수정할 전화번호 입력
            phoneNumber = getValue.PhoneNumber(cursorLeft, cursorTop);
            if (getValue.IsCanceled(phoneNumber)) return false;

            // DB에 변경할 전화번호 저장
            ConnectDatabase.UpdateToDatabase("member", "phone_number", phoneNumber, "member_id", usingMemberID.ToString());

            return true;
        }

        public bool EditPassword(int usingMemberID, int cursorLeft, int cursorTop)
        {
            string password;
            string newPassword;

            // 사용자 암호와 일치여부 검사
            while (true)
            {
                password = getValue.Information(cursorLeft, cursorTop, 15, Constant.NO_KOREAN, Constant.LOGIN_SEARCH_CATEGORY_AND_GUIDELINE[3]);
                if (getValue.IsCanceled(password)) return false;
                else if (password.Length == 0) { print.ErrorMessage(Constant.THERE_IS_NO_SEARCHWORD, cursorTop + 2); continue; }
                else if (!getValue.IsPasswordCorrespond(usingMemberID.ToString(), password)) { print.ErrorMessage(Constant.PASSWORD_IS_WRONG, cursorTop + 2); continue; }
                break;
            }

            cursorTop += 2;

            // 변경할 암호
            while (true)
            {
                print.SetCursorAndWrite(5, cursorTop, "새 암호 입력 | ");
                newPassword = getValue.Information(Console.CursorLeft, cursorTop, 15, Constant.NO_KOREAN, "8-15자 입력");
                if (getValue.IsCanceled(newPassword)) return false;
                else if (newPassword.Length == 0) { print.ErrorMessage(Constant.THERE_IS_NO_SEARCHWORD, cursorTop + 2); continue; }
                else if (newPassword.Length < 8) { print.ErrorMessage(Constant.WRONG_LENGTH_ERROR, cursorTop + 2); continue; }
                break;
            }

            // DB에 수정할 암호 저장
            ConnectDatabase.UpdateToDatabase("member", "password", newPassword, "member_id", usingMemberID.ToString());

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
            //searchedMember = getValue.SearchMemberByCondition(name, age, address.ToString());
            //List<string> borrowedBookForEachMember = getValue.BorrowedBookForEachMember(searchedMember);

            // 열람
            //CheckMemberAndDelete(searchedMember, borrowedBookForEachMember, name, age.ToString(), address.ToString());

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

