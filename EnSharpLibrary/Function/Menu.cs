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
            bool isFirtLoop = true;

            if (mode == Constant.NON_MEMBER_MODE) usingMemberID = Constant.PUBLIC;
            
            while (true)
            {
                if (isFirtLoop)
                {
                    // 메뉴 출력
                    print.SetWindowsizeAndPrintTitle(120, 30, "");
                    print.MenuOption(mode, Console.CursorTop + 2);

                    // 기능 선택
                    print.SetCursorAndChoice(74, 10, '◁');
                    
                    isFirtLoop = false;
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                // 기능 선택
                switch(keyInfo.Key)
                {
                    case ConsoleKey.UpArrow: tool.UpArrow(74, 10, 5, 2, '◁'); break;
                    case ConsoleKey.DownArrow: tool.DownArrow(74, 10, 5, 2, '◁'); break;
                    case ConsoleKey.Enter: GoNextFunction(Console.CursorTop); break;
                    default: print.BlockCursorMove(74, "◁"); break;
                }
            }
        }

        /// <summary>
        /// 사용자가 다음 기능을 선택하면 해당 메소드를 실행시키는 메소드입니다.
        /// </summary>
        /// <param name="cursorTop">사용자가 선택한 기능</param>
        public void GoNextFunction(int cursorTop)
        {
            switch (Console.CursorTop)                                           // 비회원,          회원,          관리자
            {
                case Constant.RELEVANT_TO_BOOK:                                  // 비회원 도서검색, 도서보기.      도서관리
                    if (usingMemberID != Constant.ADMIN) bookManage.SearchBook(usingMemberID);
                    else bookManage.ManageBook();
                    break;
                case Constant.LOG_IN_OR_CHECK_BORROWED_BOOK_OR_MANAGE_MEMBER:    // 로그인,          대출도서 보기, 회원관리
                    if (usingMemberID == Constant.PUBLIC) usingMemberID = memberManage.LogIn();
                    else if (usingMemberID != Constant.ADMIN) bookManage.ManageBorrowedBook(usingMemberID);
                    else memberManage.ManageMember();
                    break;
                case Constant.JOIN_IN_OR_UPDATE_USER_INFORMATION:                // 회원가입,        정보수정,      암호수정
                    if (usingMemberID == Constant.PUBLIC) usingMemberID = memberManage.JoinIn();
                    else memberManage.ChangeUserInformation(usingMemberID);
                    break;
                case Constant.LOG_IN_OR_LOG_OUT:                                 // 관리자 로그인,   로그아웃,      로그아웃
                    usingMemberID = memberManage.LogInOrLogOut(usingMemberID);
                    break;
                case Constant.CLOSE_PROGRAM:                                     // 종료
                    return;
            }
        }
    }
}
