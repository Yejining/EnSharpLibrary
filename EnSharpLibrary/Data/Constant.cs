using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnSharpLibrary.Data
{
    class Constant
    {
        public static string[] ENSHARP_TITLE =
        {
            "┏                     ┓",
            "En# Library",
            "┗                     ┛"
        };

        public static string[] LINE_FOR_OPTION =
        {
            "┏                     ┓",
            "┣                     ┫",
            "┗                     ┛"
        };

        public static string[] NON_MEMBER_OPTION = 
        {
            "비회원 도서검색",
            "로그인",
            "회원가입",
            "관리자 로그인",
            "종료"
        };

        public static string[] ADMIN_OPTION =
        {
            "도서 관리",
            "회원 관리",
            "암호 수정",
            "로그아웃",
            "종료",
        };

        public static string[] MEMBER_OPTION =
        {
            "도서 관리",
            "도서 보기",
            "정보수정",
            "로그아웃",
            "종료"
        };

        // Menu Class에서의 usingMemberID
        public const int PUBLIC = 0;
        public const int ADMIN = 88888888;

        // Menu Class의 mode로 사용,
        // Print.MenuOption
        public const int NON_MEMBER_MODE = 0;
        public const int ADMIN_MODE = 1;
        public const int MEMBER_MODE = 2;

        // Menu Class의 cursorTop 정보
        public const int RELEVANT_TO_BOOK = 8;
        public const int LOG_IN_OR_CHECK_BORROWED_BOOK_OR_MANAGE_MEMBER = 10;
        public const int JOIN_IN_OR_UPDATE_USER_INFORMATION = 12;
        public const int LOG_IN_OR_LOG_OUT = 14;
        public const int CLOSE_PROGRAM = 16;

    }
}
