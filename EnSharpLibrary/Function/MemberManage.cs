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
    }
}
