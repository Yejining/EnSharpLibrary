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

        public static string[] ENSHARP_TITLE_IN_SEARCH_MODE =
        {
            "┏                                ┓",
            "   엔 샵  도 서 | ENJOY C SHARP!",
            "┗                                ┛"
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

        public static string[] MEMBER_OPTION =
        {
            "도서 관리",
            "도서 보기",
            "정보수정",
            "로그아웃",
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

        public static string[] SEARCH_BOOK_TITLE =
        {
            "비회원 도서검색",
            "도서 보기"
        };

        public static string[] BOOK_SEARCH_CATEGORY_AND_GUIDELINE =
        {
            "도서명 | ",
            "10글자 이내 입력",
            "출판사 | ",
            "10글자 이내 입력",
            "저  자 | ",
            "10글자 이내 입력"
        };

        public static string[] SEARCHING_MENU_IN_SEARCHING_MODE =
        {
            " | 검 색 |",
            "도  서  명 | ",
            "출  판  사 | ",
            "저      자 | "
        };

        public static string[] ERROR_MESSAGE =
        {
            "검색 조건과 일치하는 도서가 없습니다!"
        };

        public static string[] SEARCHED_BOOK_GUIDELINE =
        {
            "  선택  |                    도서                   |          저자         |       출판사       |  출판년도  |  수량  |  가격  |",
            "---------------------------------------------------------------------------------------------------------------------------------"
        };

        public static string OUT = "나가기(ESC)";

        public const string NUMBER_PATTERN = "[0-9]";
        public const string ENGLISH_PATTERN = "[a-zA-Z]";
        public const string KOREAN_PATTERN = "[ㄱ-ㅎㅏ-ㅣ가-힣]";
        public const string SPECIAL_LETTER = "[`~!@#$%^&*()\\-_=+\\{\\}\\[\\]\\\\\\|:;\"\'<>,.?/]";

        // Menu Class에서의 usingMemberID
        public const int PUBLIC = 0;
        public const int ADMIN = 88888888;

        // Menu Class의 mode로 사용
        // SEARCH_BOOK_TITLE의 인덱스와 연결
        // Print.MenuOption
        // BookManage.SearchBook
        public const int NON_MEMBER_MODE = 0;
        public const int MEMBER_MODE = 1;
        public const int ADMIN_MODE = 2;

        // Menu Class의 cursorTop 정보
        public const int RELEVANT_TO_BOOK = 10;
        public const int LOG_IN_OR_CHECK_BORROWED_BOOK_OR_MANAGE_MEMBER = 12;
        public const int JOIN_IN_OR_UPDATE_USER_INFORMATION = 14;
        public const int LOG_IN_OR_LOG_OUT = 16;
        public const int CLOSE_PROGRAM = 18;

        // BookManage SearchBook에서 사용
        public const int NONE = 0;

        // Tool.EnterOrTab return값
        // BookManage.SearchBook
        public const int TAB = 1;
        public const int ENTER = 2;
        public const int ESCAPE = 3;

        // ERROR_MESSAGE의 인덱스와 연결해 사용
        public const int THERE_IS_NO_BOOK = 0;

        public const ConsoleColor FOREGROUND_COLOR = ConsoleColor.White;
    }
}
