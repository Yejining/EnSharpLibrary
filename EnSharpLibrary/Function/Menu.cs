﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EnSharpLibrary.IO;
using EnSharpLibrary.Data;

namespace EnSharpLibrary.Function
{
    class Menu
    {
        Print print = new Print();
        Tool tool = new Tool();

        BookManage bookManage = new BookManage();
        MemberManage memberManage = new MemberManage();

        private int usingMemberID;

        /// <summary>
        /// 도서관리 프로그램을 시작합니다.
        /// </summary>
        /// <param name="mode">프로그램 사용자</param>
        public void RunLibraryProgram(int mode)
        {
            bool isFirstLoop = true;

            if (mode == Constant.NON_MEMBER_MODE) usingMemberID = Constant.PUBLIC;

            while (true)
            {
                if (isFirstLoop)
                {
                    // 모드 설정
                    if (usingMemberID == Constant.ADMIN) mode = Constant.ADMIN_MODE;
                    else if (usingMemberID != Constant.PUBLIC) mode = Constant.MEMBER_MODE;
                    else mode = Constant.NON_MEMBER_MODE;

                    // 메뉴 출력
                    print.SetWindowsizeAndPrintTitle(45, 30, "");
                    print.MenuOption(mode, Console.CursorTop + 2);

                    // 기능 선택
                    print.SetCursorAndChoice(38, 10, "◁");

                    isFirstLoop = false;
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                // 기능 선택
                switch(keyInfo.Key)
                {
                    case ConsoleKey.UpArrow: tool.UpArrow(38, 10, 5, 2, '◁'); break;
                    case ConsoleKey.DownArrow: tool.DownArrow(38, 10, 5, 2, '◁'); break;
                    case ConsoleKey.Enter: isFirstLoop = GoNextFunction(Console.CursorTop);
                        if (Console.CursorTop == Constant.CLOSE_PROGRAM) return; break;
                    default: print.BlockCursorMove(38, "◁"); break;
                }
            }
        }

        /// <summary>
        /// 사용자가 다음 기능을 선택하면 해당 메소드를 실행시키는 메소드입니다.
        /// </summary>
        /// <param name="cursorTop">사용자가 선택한 기능</param>
        public bool GoNextFunction(int cursorTop)
        {
            switch (Console.CursorTop)                                           // 비회원,          회원,          관리자
            {
                case Constant.RELEVANT_TO_BOOK:                                  // 비회원 도서검색, 도서대출.      도서관리
                    if (usingMemberID != Constant.ADMIN) bookManage.SearchBook(usingMemberID);
                    else bookManage.ManageBook();
                    break;
                case Constant.LOG_IN_OR_CHECK_BORROWED_BOOK_OR_MANAGE_MEMBER:    // 로그인,          연장 및 반납,  회원관리
                    if (usingMemberID == Constant.PUBLIC) usingMemberID = memberManage.LogIn(Constant.MEMBER_MODE);
                    else if (usingMemberID != Constant.ADMIN) bookManage.ManageBorrowedBook(usingMemberID);
                    else memberManage.ManageMember();
                    break;
                case Constant.JOIN_IN_OR_UPDATE_USER_INFORMATION:                // 회원가입,        정보수정,      암호수정
                    if (usingMemberID == Constant.PUBLIC) usingMemberID = memberManage.JoinIn("회원가입");
                    else memberManage.ChangeUserInformation(usingMemberID);
                    break;
                case Constant.LOG_IN_OR_LOG_OUT:                                 // 관리자 로그인,   로그아웃,      로그아웃
                    usingMemberID = memberManage.LogInOrLogOut(usingMemberID);
                    break;
                case Constant.CLOSE_PROGRAM:                                     // 종료
                    return true;
            }

            return true;
        }
    }
}
