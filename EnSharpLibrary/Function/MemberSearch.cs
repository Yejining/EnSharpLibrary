//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using EnSharpLibrary.Data;
//using EnSharpLibrary.IO;

//namespace EnSharpLibrary.Function
//{
//    class MemberSearch
//    {
//        Print print = new Print();
//        GetValue getValue = new GetValue();
//        LogIn logIn = new LogIn();
//        MemberManage memberManage = new MemberManage();

//        public LibraryVO Run(List<MemberVO> members, List<BookVO> books)
//        {
//            bool isFirtLoop = true;
//            LibraryVO library = new LibraryVO(members, books);
//            bool isJoin = false;

//            print.Title("회원 관리");

//            while (true)
//            {
//                if (isFirtLoop)
//                {
//                    // 타이틀 및 옵션 출력하고 커서 조절
//                    print.Title("");
//                    print.MemberSearchOption();

//                    print.SetCursorAndChoice(74, 10, '☜');

//                    isFirtLoop = false;
//                }

//                ConsoleKeyInfo keyInfo = Console.ReadKey();

//                if (keyInfo.Key == ConsoleKey.UpArrow)
//                {
//                    print.ClearOneLetter(74);
//                    if (Console.CursorTop > 8) Console.SetCursorPosition(74, Console.CursorTop - 2);
//                    Console.Write('☜');
//                }
//                else if (keyInfo.Key == ConsoleKey.DownArrow)
//                {
//                    print.ClearOneLetter(74);
//                    if (Console.CursorTop < 18) Console.SetCursorPosition(74, Console.CursorTop + 2);
//                    Console.Write('☜');
//                }
//                else
//                {
//                    if (keyInfo.Key == ConsoleKey.Enter)
//                    {
//                        switch (Console.CursorTop)
//                        {
//                            case 8:     // 전체 회원 목록
//                                library = AllMembers(books, members);
//                                break;
//                            case 10:    // 이름 검색
//                                library = Specifically(1, books, members);
//                                break;
//                            case 12:    // 학번 검색
//                                library = Specifically(2, books, members);
//                                break;
//                            case 14:    // 주소 검색
//                                library = Specifically(3, books, members);
//                                break;
//                            case 16:    // 회원 등록
//                                members = logIn.Join(members, 2);
//                                isJoin = true;
//                                break;
//                            case 18:    // 뒤로
//                                return library;
//                        }

//                        books = library.Books;
//                        if (!isJoin) members = library.Members;

//                        isJoin = true;
//                        isFirtLoop = true;
//                    }
//                    else
//                    {
//                        Console.SetCursorPosition(74, Console.CursorTop);
//                        Console.Write("☜ ");
//                        Console.SetCursorPosition(74, Console.CursorTop);
//                    }
//                }
//            }
//        }

//        public LibraryVO AllMembers(List<BookVO> books, List<MemberVO> members)
//        {
//            LibraryVO library = new LibraryVO(members, books);
//            bool isFirstLoop = true;

//            while (true)
//            {
//                if (isFirstLoop)
//                {
//                    print.Title("전체 회원 목록");
//                    print.AllMembers(books, members);

//                    print.SetCursorAndChoice(3, members.Count + 3, '☞');

//                    isFirstLoop = false;
//                }

//                ConsoleKeyInfo keyInfo = Console.ReadKey();

//                if (keyInfo.Key == ConsoleKey.UpArrow)
//                {
//                    print.ClearOneLetter(3);
//                    if (Console.CursorTop > 12) Console.SetCursorPosition(3, Console.CursorTop - 1);
//                    Console.Write('☞');
//                }
//                else if (keyInfo.Key == ConsoleKey.DownArrow)
//                {
//                    print.ClearOneLetter(3);
//                    if (Console.CursorTop < 12 + members.Count - 1) Console.SetCursorPosition(3, Console.CursorTop + 1);
//                    Console.Write('☞');
//                }
//                else if (keyInfo.Key == ConsoleKey.Escape) return library;
//                else
//                {
//                    if (keyInfo.Key == ConsoleKey.Enter)
//                    {
//                        if (Console.CursorTop >= 12 && Console.CursorTop <= 12 + members.Count - 1)
//                            library = MemberDetail(1, members[Console.CursorTop - 12], books, members);

//                        books = library.Books;
//                        members = library.Members;

//                        isFirstLoop = true;
//                    }
//                    else print.BlockCursorMove(3, "☞ ");
//                }
//            }
//        }

//        // 1 : 이름 검색
//        // 2 : 학번 검색
//        // 3 : 주소 검색
//        public LibraryVO Specifically(int mode, List<BookVO> books, List<MemberVO> members)
//        {
//            LibraryVO library = new LibraryVO(members, books);
//            StringBuilder title = new StringBuilder();
//            string keywordToSearch;
//            List<int> foundMembers = new List<int>();
//            ConsoleKeyInfo keyInfo;
//            bool isFirstLoop = false;

//            if (mode == 1)
//            {
//                title.AppendFormat("▷ 검색할 이름 : ");
//                print.Title("회원 관리 -> 이름 검색");
//            }
//            else if (mode == 2)
//            {
//                title.AppendFormat("▷ 검색할 학번 : ");
//                print.Title("회원 관리 -> 학번 검색");
//            }
//            else
//            {
//                title.AppendFormat("▷ 검색할 주소 : ");
//                print.Title("회원 관리 ->  주소 검색");
//            }
            
//            Console.SetCursorPosition(10, Console.CursorTop);
//            Console.Write(title);
//            keywordToSearch = getValue.SearchWord(5, 0);
//            title.Append(keywordToSearch);

//            if (string.Compare(keywordToSearch, "@입력취소@") == 0) return library;

//            Console.SetCursorPosition(0, Console.CursorTop + 2);

//            foundMembers = getValue.FoundMembers(members, keywordToSearch, mode);
//            print.MemberSearchResult(members, books, foundMembers);

//            while (foundMembers.Count == 0)
//            {
//                keyInfo = Console.ReadKey();
//                if (keyInfo.Key == ConsoleKey.Escape) return library;
//            }

//            print.SetCursorAndChoice(3, foundMembers.Count + 3, '☞');

//            while (true)
//            {
//                if (isFirstLoop)
//                {
//                    if (mode == 1) print.Title("회원 관리 -> 이름 검색");
//                    else if (mode == 2) print.Title("회원 관리 -> 학번 검색");
//                    else print.Title("회원 관리 ->  주소 검색");

//                    Console.SetCursorPosition(10, Console.CursorTop);
//                    Console.Write(title);

//                    foundMembers = getValue.FoundMembers(members, keywordToSearch, mode);

//                    while (foundMembers.Count == 0)
//                    {
//                        keyInfo = Console.ReadKey();
//                        if (keyInfo.Key == ConsoleKey.Escape) return library;
//                    }

//                    Console.SetCursorPosition(0, Console.CursorTop + 2);
//                    print.MemberSearchResult(members, books, foundMembers);

//                    isFirstLoop = false;
//                }

//                keyInfo = Console.ReadKey();

//                if (keyInfo.Key == ConsoleKey.UpArrow)
//                {
//                    print.ClearOneLetter(3);
//                    if (Console.CursorTop > 15) Console.SetCursorPosition(3, Console.CursorTop - 1);
//                    Console.Write('☞');
//                }
//                else if (keyInfo.Key == ConsoleKey.DownArrow)
//                {
//                    print.ClearOneLetter(3);
//                    if (Console.CursorTop < 15 + foundMembers.Count - 1) Console.SetCursorPosition(3, Console.CursorTop + 1);
//                    Console.Write('☞');
//                }
//                else if (keyInfo.Key == ConsoleKey.Escape) return library;
//                else
//                {
//                    if (keyInfo.Key == ConsoleKey.Enter)
//                    {
//                        if (Console.CursorTop >= 15 && Console.CursorTop <= 15 + foundMembers.Count - 1)
//                        {
//                            library = MemberDetail(mode, members[Console.CursorTop - 15], books, members);
//                        }

//                        books = library.Books;
//                        members = library.Members;

//                        isFirstLoop = true;
//                    }
//                    else print.BlockCursorMove(3, "☞ ");
//                }
//            }
//        }

//        // 1 : 전체 회원 -> 회원 상세
//        // 2 : 이름 검색 -> 회원 상세
//        // 3 : 학번 검색 -> 회원 상세
//        // 4 : 주소 검색 -> 회원 상세
//        public LibraryVO MemberDetail(int mode, MemberVO member, List<BookVO> books, List<MemberVO> members)
//        {
//            LibraryVO library = new LibraryVO(members, books);
//            int cursorTop;
//            bool isFirstLoop = true;

//            print.Title(member.Name);
//            print.MemberInformation(member, 2);

//            cursorTop = Console.CursorTop + 2;
            
//            print.MemberManageOption(45, cursorTop);

//            Console.SetCursorPosition(65, cursorTop);

//            while (true)
//            {
//                if (isFirstLoop)
//                {
//                    print.Title(member.Name);
//                    print.MemberInformation(member, 2);

//                    cursorTop = Console.CursorTop + 2;

//                    print.MemberManageOption(45, cursorTop);

//                    Console.SetCursorPosition(65, cursorTop);
//                    print.SetCursorAndChoice(65, -2, '☜');

//                    isFirstLoop = false;
//                }

//                ConsoleKeyInfo keyInfo = Console.ReadKey();

//                if (keyInfo.Key == ConsoleKey.UpArrow)
//                {
//                    print.ClearOneLetter(65);
//                    if (Console.CursorTop > 20) Console.SetCursorPosition(65, Console.CursorTop - 2);
//                    Console.Write('☜');
//                }
//                else if (keyInfo.Key == ConsoleKey.DownArrow)
//                {
//                    print.ClearOneLetter(65);
//                    if (Console.CursorTop < 26) Console.SetCursorPosition(65, Console.CursorTop + 2);
//                    Console.Write('☜');
//                }
//                else
//                {
//                    if (keyInfo.Key == ConsoleKey.Enter)
//                    {
//                        switch (Console.CursorTop)
//                        {
//                            case 20:    // 회원 삭제
//                                library = memberManage.RemoveMember(member.IdentificationNumber, members, books);
//                                return library;
//                            case 22:    // 주소 변경
//                                members = memberManage.MemberInformationEdit(1, member.IdentificationNumber, members);
//                                break;
//                            case 24:    // 전화번호 변경
//                                members = memberManage.MemberInformationEdit(2, member.IdentificationNumber, members);
//                                break;
//                            case 26:    // 뒤로
//                                return library;
//                        }

//                        library.Members = members;
//                        library.Books = books;

//                        isFirstLoop = true;
//                    }
//                    else
//                    {
//                        Console.SetCursorPosition(74, Console.CursorTop);
//                        Console.Write("☜ ");
//                        Console.SetCursorPosition(74, Console.CursorTop);
//                    }
//                }
//            }
//        }
//    }
//}
