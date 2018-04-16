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
                                print.MemberInformation(user);
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
            print.Title("암호변경");

            return members;
        }

        // 1 : 주소 변경
        // 2 : 전화번호 변경
        public List<MemberVO> MemberInformationEdit(int mode, int usingMemberNumber, List<MemberVO> members)
        {
            if (mode == 1) print.Title("주소 변경");
            else print.Title("전화번호 변경");

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
    }
}
