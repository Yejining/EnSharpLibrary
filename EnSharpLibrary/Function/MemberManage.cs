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

        public int LogInOrLogOut(int usingMemberID)
        {
            if (usingMemberID == Constant.PUBLIC) return LogIn(Constant.ADMIN_MODE);
            else return LogOut();
        }

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

        public int LogOut()
        {
            return Constant.PUBLIC;
        }

        public int JoinIn()
        {
            int usingMemberID = 0;

            string name;
            string userID;
            string password;
            int address1;
            int address2;
            StringBuilder address = new StringBuilder();
            string phoneNumber;
            int year, month, day;
            DateTime birthdate;

            print.SetWindowsizeAndPrintTitle(45, 30, "회원가입");

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

        public void ChangeUserInformation(int usingMemberID)
        {

        }

        public void ManageMember()
        {

        }

        //public List<MemberVO> MemberEdit(int usingMemberNumber, List<MemberVO> members)
        //{
        //    bool isFirstLoop = true;
        //    MemberVO user = new MemberVO();

        //    foreach (MemberVO member in members)
        //        if (member.IdentificationNumber == usingMemberNumber) user = member;

        //    while (true)
        //    {
        //        if (isFirstLoop)
        //        {
        //            // 타이틀 및 옵션 출력하고 커서 조절
        //            print.Title("정보수정  ");
        //            print.MemberEditOption();

        //            print.SetCursorAndChoice(74, 10, '☜');

        //            isFirstLoop = false;
        //        }

        //        ConsoleKeyInfo keyInfo = Console.ReadKey();

        //        if (keyInfo.Key == ConsoleKey.UpArrow)
        //        {
        //            print.ClearOneLetter(74);
        //            if (Console.CursorTop > 11) Console.SetCursorPosition(74, Console.CursorTop - 2);
        //            Console.Write('☜');
        //        }
        //        else if (keyInfo.Key == ConsoleKey.DownArrow)
        //        {
        //            print.ClearOneLetter(74);
        //            if (Console.CursorTop < 19) Console.SetCursorPosition(74, Console.CursorTop + 2);
        //            Console.Write('☜');
        //        }
        //        else
        //        {
        //            if (keyInfo.Key == ConsoleKey.Enter)
        //            {
        //                switch (Console.CursorTop)
        //                {
        //                    case 11:     // 내 정보
        //                        print.MemberInformation(user, 1);
        //                        break;
        //                    case 13:    // 암호 변경
        //                        members = MemberPasswordEdit(usingMemberNumber, members);
        //                        break;
        //                    case 15:    // 주소 변경
        //                        members = MemberInformationEdit(1, usingMemberNumber, members);
        //                        break;
        //                    case 17:    // 전화번호 변경
        //                        members = MemberInformationEdit(2, usingMemberNumber, members);
        //                        break;
        //                    case 19:    // 뒤로
        //                        return members;
        //                }

        //                isFirstLoop = true;
        //            }
        //            else print.BlockCursorMove(74, "☜ ");
        //        }
        //    }
        //}

        //public List<MemberVO> MemberPasswordEdit(int usingMemberNumber, List<MemberVO> members)
        //{
        //    string userPassword = "";

        //    foreach (MemberVO member in members)
        //        if (member.IdentificationNumber == usingMemberNumber)
        //        {
        //            userPassword = member.Password;
        //            break;
        //        }

        //    string newPassword;
        //    int cursorTop = 10;

        //    print.Title("암호변경");

        //    Console.SetCursorPosition(30, cursorTop);
        //    Console.Write("▷ 현재 암호 입력 : ");
        //    newPassword = getValue.SearchWord(17, 2);

        //    if (string.Compare(newPassword, "@입력취소@") == 0) return members;

        //    while (string.Compare(userPassword, newPassword) != 0)
        //    {
        //        Console.SetCursorPosition(0, Console.CursorTop + 1);
        //        print.Announce("암호가 일치하지 않습니다!");
        //        Console.SetCursorPosition(30, cursorTop);
        //        print.ClearCurrentConsoleLine();
        //        Console.SetCursorPosition(30, cursorTop);
        //        Console.Write("▷ 현재 암호 입력 : ");
        //        newPassword = getValue.SearchWord(17, 2);
        //    }

        //    Console.SetCursorPosition(30, cursorTop + 2);
        //    Console.Write("▷ 변경할 암호 입력(8자 이상 15자 이하) : ");
        //    newPassword = getValue.SearchWord(17, 2);
        //    if (string.Compare(newPassword, "@입력취소@") == 0) return members;
        //    while (getValue.NotValidPassword(newPassword) != 0)
        //    {
        //        Console.SetCursorPosition(0, Console.CursorTop + 1);
        //        if (getValue.NotValidPassword(newPassword) == 1) print.Announce("길이에 맞는 암호를 입력하세요!");
        //        else if (getValue.NotValidPassword(newPassword) == 2) print.Announce("3자리 연속으로 문자 혹은 숫자가  사용되면 안 됩니다!");
        //        Console.SetCursorPosition(30, cursorTop + 2);
        //        print.ClearCurrentConsoleLine();
        //        Console.SetCursorPosition(30, cursorTop + 2);
        //        Console.Write("▷ 변경할 암호 입력(8자 이상 15자 이하) : ");
        //        newPassword = getValue.SearchWord(17, 2);
        //    }

        //    foreach (MemberVO member in members)
        //        if (member.IdentificationNumber == usingMemberNumber)
        //        {
        //            member.Password = newPassword;
        //            break;
        //        }

        //    return members;
        //}

        //// 1 : 주소 변경
        //// 2 : 전화번호 변경
        //public List<MemberVO> MemberInformationEdit(int mode, int usingMemberNumber, List<MemberVO> members)
        //{
        //    int cursorTop = 10;
        //    StringBuilder newPhone = new StringBuilder();
        //    string newPhoneNumber;
        //    string newAddress;

        //    MemberVO user = new MemberVO();
        //    foreach (MemberVO member in members)
        //        if (member.IdentificationNumber == usingMemberNumber)
        //        {
        //            user = member;
        //            break;
        //        }

        //    if (mode == 1)
        //    {
        //        print.Title("주소 변경");
        //        Console.SetCursorPosition(20, cursorTop);
        //        Console.Write("▷ 현재 주소 : {0}", user.Address);
        //        Console.SetCursorPosition(20, cursorTop + 2);
        //        Console.Write("▷ 변경할 주소 입력(○○도 ○○시 ○○동 형식) : ");
        //        newAddress = getValue.SearchWord(20, 0);

        //        foreach (MemberVO member in members)
        //            if (member.IdentificationNumber == usingMemberNumber)
        //            {
        //                member.Address = newAddress;
        //                break;
        //            }
        //    }
        //    else
        //    {
        //        print.Title("전화번호 변경");
        //        Console.SetCursorPosition(30, cursorTop);
        //        Console.Write("▷ 현재 전화번호 : {0}", user.PhoneNumber);
        //        Console.SetCursorPosition(30, cursorTop + 2);
        //        Console.Write("▷ 변경할 전화번호 입력('-'없이 입력) : ");
        //        newPhoneNumber = getValue.SearchWord(12, 1);

        //        if (string.Compare(newPhoneNumber, "@입력취소@") == 0) return members;

        //        while (getValue.NotValidPhoneNumber(newPhoneNumber, members))
        //        {
        //            Console.SetCursorPosition(0, Console.CursorTop + 1);
        //            print.Announce("유효하지 않은 전화번호입니다!");
        //            Console.SetCursorPosition(30, cursorTop + 2);
        //            print.ClearCurrentConsoleLine();
        //            Console.SetCursorPosition(30, cursorTop + 2);
        //            Console.Write("▷ 변경할 전화번호 입력('-'없이 입력) : ");
        //            newPhoneNumber = getValue.SearchWord(17, 1);
        //        }

        //        StringBuilder phoneNumber = new StringBuilder();
        //        phoneNumber.Append(newPhoneNumber);
        //        phoneNumber.Insert(3, '-');
        //        phoneNumber.Insert(8, '-');

        //        foreach (MemberVO member in members)
        //            if (member.IdentificationNumber == usingMemberNumber)
        //            {
        //                member.PhoneNumber = phoneNumber.ToString();
        //                break;
        //            }
        //    }
        //    return members;
        //}

        //public AdminVO AdminEdit(AdminVO admin)
        //{
        //    string newPassword;
        //    int cursorTop = 10;

        //    print.Title("관리자 암호 수정");

        //    Console.SetCursorPosition(30, cursorTop);
        //    Console.Write("▷ 현재 암호 입력 : ");
        //    newPassword = getValue.SearchWord(17, 2);

        //    if (string.Compare(newPassword, "@입력취소@") == 0) return admin;

        //    while (admin.Password != newPassword)
        //    {
        //        print.Announce("암호가 일치하지 않습니다!");
        //        Console.SetCursorPosition(30, cursorTop);
        //        print.ClearCurrentConsoleLine();
        //        Console.SetCursorPosition(30, cursorTop);
        //        Console.Write("▷ 현재 암호 입력 : ");
        //        newPassword = getValue.SearchWord(17, 2);
        //    }

        //    Console.SetCursorPosition(30, cursorTop + 2);
        //    Console.Write("▷ 변경할 암호 입력(8자 이상 15자 이하) : ");
        //    newPassword = getValue.SearchWord(17, 2);
        //    if (string.Compare(newPassword, "@입력취소@") == 0) return admin;
        //    while (getValue.NotValidPassword(newPassword) != 0)
        //    {
        //        Console.SetCursorPosition(0, Console.CursorTop + 1);
        //        if (getValue.NotValidPassword(newPassword) == 1) print.Announce("길이에 맞는 암호를 입력하세요!");
        //        else if (getValue.NotValidPassword(newPassword) == 2) print.Announce("3자리 연속으로 문자 혹은 숫자가  사용되면 안 됩니다!");
        //        else if (getValue.NotValidPassword(newPassword) == 3) print.Announce("암호는 영어와 숫자만 가능합니다!");
        //        Console.SetCursorPosition(30, cursorTop + 2);
        //        print.ClearCurrentConsoleLine();
        //        Console.SetCursorPosition(30, cursorTop + 2);
        //        Console.Write("▷ 변경할 암호 입력(8자 이상 15자 이하) : ");
        //        newPassword = getValue.SearchWord(17, 2);
        //    }

        //    admin.Password = newPassword;

        //    return admin;
        //}

        //public LibraryVO RemoveMember(int memberNumber, List<MemberVO> members, List<BookVO> books)
        //{
        //    LibraryVO library = new LibraryVO(members, books);
        //    int userIndex = 0;

        //    for (int index = 0; index < members.Count; index++)
        //        if (members[index].IdentificationNumber == memberNumber)
        //        {
        //            userIndex = index;
        //            break;
        //        }

        //    if (members[userIndex].BorrowedBook.Count != 0)
        //    {
        //        for (int index = 0; index < members[userIndex].BorrowedBook.Count; index++)
        //        {
        //            for (int j = 0; j < books.Count; j++)
        //            {
        //                if (members[userIndex].BorrowedBook[index] == books[j].NumberOfThis)
        //                {
        //                    books[j].SetNonRentalMode();
        //                    books[j].BookCondition = 2;
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    members.RemoveAt(userIndex);
        //    library.Books = books;

        //    return library;
        //}
    }
}
