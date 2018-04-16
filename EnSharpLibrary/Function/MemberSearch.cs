using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EnSharpLibrary.Data;
using EnSharpLibrary.IO;

namespace EnSharpLibrary.Function
{
    class MemberSearch
    {
        Print print = new Print();
        GetValue getValue = new GetValue();

        public LibraryVO Run(List<MemberVO> members, List<BookVO> books)
        {
            bool isFirtLoop = true;
            LibraryVO library = new LibraryVO(members, books);

            print.Title("회원 관리");

            while (true)
            {
                if (isFirtLoop)
                {
                    // 타이틀 및 옵션 출력하고 커서 조절
                    print.Title("");
                    print.MemberSearchOption();

                    print.SetCursorAndChoice(74, 10, '☜');

                    isFirtLoop = false;
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    print.ClearOneLetter(74);
                    if (Console.CursorTop > 8) Console.SetCursorPosition(74, Console.CursorTop - 2);
                    Console.Write('☜');
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    print.ClearOneLetter(74);
                    if (Console.CursorTop < 22) Console.SetCursorPosition(74, Console.CursorTop + 2);
                    Console.Write('☜');
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        switch (Console.CursorTop)
                        {
                            case 8:     // 전체 회원 목록
                                break;
                            case 10:    // 이름 검색
                                break;
                            case 12:    // 학번 검색
                                break;
                            case 14:    // 주소 검색
                                break;
                            case 16:    // 대출자 보기
                                break;
                            case 18:    // 연체자 보기
                                break;
                            case 20:    // 회원 등록
                                break;
                            case 22:    // 뒤로
                                return library;
                        }

                        isFirtLoop = true;
                    }
                    else
                    {
                        Console.SetCursorPosition(74, Console.CursorTop);
                        Console.Write("☜ ");
                        Console.SetCursorPosition(74, Console.CursorTop);
                    }
                }
            }
        }
    }
}
