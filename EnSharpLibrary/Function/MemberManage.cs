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

        public List<MemberVO> MemberEdit(int usingMemberNumber, List<MemberVO> members)
        {
            bool isFirstLoop = true;
            MemberVO user = new MemberVO();

            foreach (MemberVO member in members)
                if (member.IdentificationNumber == usingMemberNumber) user = member;

            while (true)
            {
                if (isFirstLoop)
                {
                    // 타이틀 및 옵션 출력하고 커서 조절
                    print.Title("정보수정  ");
                    print.MemberEditOption();

                    print.SetCursorAndChoice(74, 10, '☜');

                    isFirstLoop = false;
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    print.ClearOneLetter(74);
                    if (Console.CursorTop > 11) Console.SetCursorPosition(74, Console.CursorTop - 2);
                    Console.Write('☜');
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    print.ClearOneLetter(74);
                    if (Console.CursorTop < 19) Console.SetCursorPosition(74, Console.CursorTop + 2);
                    Console.Write('☜');
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        switch (Console.CursorTop)
                        {
                            case 11:     // 내 정보
                                print.MemberInformation(user, 1);
                                break;
                            case 13:    // 암호 변경
                                members = MemberPasswordEdit(usingMemberNumber, members);
                                break;
                            case 15:    // 주소 변경
                                members = MemberInformationEdit(1, usingMemberNumber, members);
                                break;
                            case 17:    // 전화번호 변경
                                members = MemberInformationEdit(2, usingMemberNumber, members);
                                break;
                            case 19:    // 뒤로
                                return members;
                        }

                        isFirstLoop = true;
                    }
                    else print.BlockCursorMove(74, "☜ ");
                }
            }
        }

        public List<MemberVO>  MemberPasswordEdit(int usingMemberNumber, List<MemberVO> members)
        {
            string userPassword = "";
            
            foreach (MemberVO member in members)
                if (member.IdentificationNumber == usingMemberNumber)
                {
                    userPassword = member.Password;
                    break;
                }

            string newPassword;
            int cursorTop = 10;

            print.Title("암호변경");

            Console.SetCursorPosition(30, cursorTop);
            Console.Write("▷ 현재 암호 입력 : ");
            newPassword = getValue.SearchWord(17, 2);

            if (string.Compare(newPassword, "@입력취소@") == 0) return members;

            while (string.Compare(userPassword, newPassword) != 0)
            {
                Console.SetCursorPosition(0, Console.CursorTop + 1);
                print.Announce("암호가 일치하지 않습니다!");
                Console.SetCursorPosition(30, cursorTop);
                print.ClearCurrentConsoleLine();
                Console.SetCursorPosition(30, cursorTop);
                Console.Write("▷ 현재 암호 입력 : ");
                newPassword = getValue.SearchWord(17, 2);
            }

            Console.SetCursorPosition(30, cursorTop + 2);
            Console.Write("▷ 변경할 암호 입력(8자 이상 15자 이하) : ");
            newPassword = getValue.SearchWord(17, 2);
            if (string.Compare(newPassword, "@입력취소@") == 0) return members;
            while (getValue.NotValidPassword(newPassword) != 0)
            {
                Console.SetCursorPosition(0, Console.CursorTop + 1);
                if (getValue.NotValidPassword(newPassword) == 1) print.Announce("길이에 맞는 암호를 입력하세요!");
                else if (getValue.NotValidPassword(newPassword) == 2) print.Announce("3자리 연속으로 문자 혹은 숫자가  사용되면 안 됩니다!");
                Console.SetCursorPosition(30, cursorTop + 2);
                print.ClearCurrentConsoleLine();
                Console.SetCursorPosition(30, cursorTop + 2);
                Console.Write("▷ 변경할 암호 입력(8자 이상 15자 이하) : ");
                newPassword = getValue.SearchWord(17, 2);
            }

            foreach (MemberVO member in members)
                if (member.IdentificationNumber == usingMemberNumber)
                {
                    member.Password = newPassword;
                    break;
                }

            return members;
        }

        // 1 : 주소 변경
        // 2 : 전화번호 변경
        public List<MemberVO> MemberInformationEdit(int mode, int usingMemberNumber, List<MemberVO> members)
        {
            int cursorTop = 10;
            StringBuilder newPhone = new StringBuilder();
            string newPhoneNumber;
            string newAddress;

            MemberVO user = new MemberVO();
            foreach (MemberVO member in members) 
                if (member.IdentificationNumber == usingMemberNumber)
                {
                    user = member;
                    break;
                }

            if (mode == 1)
            {
                print.Title("주소 변경");
                Console.SetCursorPosition(20, cursorTop);
                Console.Write("▷ 현재 주소 : {0}", user.Address);
                Console.SetCursorPosition(20, cursorTop + 2);
                Console.Write("▷ 변경할 주소 입력(○○도 ○○시 ○○동 형식) : ");
                newAddress = getValue.SearchWord(20, 0);

                foreach (MemberVO member in members)
                    if (member.IdentificationNumber == usingMemberNumber)
                    {
                        member.Address = newAddress;
                        break;
                    }
            }
            else
            {
                print.Title("전화번호 변경");
                Console.SetCursorPosition(30, cursorTop);
                Console.Write("▷ 현재 전화번호 : {0}", user.PhoneNumber);
                Console.SetCursorPosition(30, cursorTop + 2);
                Console.Write("▷ 변경할 전화번호 입력('-'없이 입력) : ");
                newPhoneNumber = getValue.SearchWord(12, 1);

                if (string.Compare(newPhoneNumber, "@입력취소@") == 0) return members;

                while (getValue.NotValidPhoneNumber(newPhoneNumber, members))
                {
                    Console.SetCursorPosition(0, Console.CursorTop + 1);
                    print.Announce("유효하지 않은 전화번호입니다!");
                    Console.SetCursorPosition(30, cursorTop + 2);
                    print.ClearCurrentConsoleLine();
                    Console.SetCursorPosition(30, cursorTop + 2);
                    Console.Write("▷ 변경할 전화번호 입력('-'없이 입력) : ");
                    newPhoneNumber = getValue.SearchWord(17, 1);
                }

                StringBuilder phoneNumber = new StringBuilder();
                phoneNumber.Append(newPhoneNumber);
                phoneNumber.Insert(3, '-');
                phoneNumber.Insert(8, '-');

                foreach (MemberVO member in members)
                    if (member.IdentificationNumber == usingMemberNumber)
                    {
                        member.PhoneNumber = phoneNumber.ToString();
                        break;
                    }
            } 
            return members;
        }

        public AdminVO AdminEdit(AdminVO admin)
        {
            string newPassword;
            int cursorTop = 10;

            print.Title("관리자 암호 수정");

            Console.SetCursorPosition(30, cursorTop);
            Console.Write("▷ 현재 암호 입력 : ");
            newPassword = getValue.SearchWord(17, 2);

            if (string.Compare(newPassword, "@입력취소@") == 0) return admin;

            while (admin.Password != newPassword)
            {
                print.Announce("암호가 일치하지 않습니다!");
                Console.SetCursorPosition(30, cursorTop);
                print.ClearCurrentConsoleLine();
                Console.SetCursorPosition(30, cursorTop);
                Console.Write("▷ 현재 암호 입력 : ");
                newPassword = getValue.SearchWord(17, 2);
            }

            Console.SetCursorPosition(30, cursorTop + 2);
            Console.Write("▷ 변경할 암호 입력(8자 이상 15자 이하) : ");
            newPassword = getValue.SearchWord(17, 2);
            if (string.Compare(newPassword, "@입력취소@") == 0) return admin;
            while (getValue.NotValidPassword(newPassword) != 0)
            {
                Console.SetCursorPosition(0, Console.CursorTop + 1);
                if (getValue.NotValidPassword(newPassword) == 1) print.Announce("길이에 맞는 암호를 입력하세요!");
                else if (getValue.NotValidPassword(newPassword) == 2) print.Announce("3자리 연속으로 문자 혹은 숫자가  사용되면 안 됩니다!");
                Console.SetCursorPosition(30, cursorTop + 2);
                print.ClearCurrentConsoleLine();
                Console.SetCursorPosition(30, cursorTop + 2);
                Console.Write("▷ 변경할 암호 입력(8자 이상 15자 이하) : ");
                newPassword = getValue.SearchWord(17, 2);
            }

            admin.Password = newPassword;

            return admin;
        }

        public LibraryVO RemoveMember(int memberNumber, List<MemberVO> members, List<BookVO> books)
        {
            LibraryVO library = new LibraryVO(members, books);
            int userIndex = 0;

            for (int index = 0; index < members.Count; index++)
                if (members[index].IdentificationNumber == memberNumber)
                {
                    userIndex = index;
                    break;
                }

            if (members[userIndex].BorrowedBook.Count != 0)
            {
                for (int index = 0; index < members[userIndex].BorrowedBook.Count; index++)
                {
                    for (int j = 0; j < books.Count; j++)
                    {
                        if (members[userIndex].BorrowedBook[index] == books[j].NumberOfThis)
                        {
                            books[j].SetNonRentalMode();
                            books[j].BookCondition = 2;
                            break;
                        }
                    }    
                }
            }
            
            members.RemoveAt(userIndex);
            library.Books = books;

            return library;
        }
    }
}
