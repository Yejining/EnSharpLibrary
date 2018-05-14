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
            "도서 대출",
            "연장 및 반납",
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

        public static string[] SEARCHING_BOOK_MENU_IN_SEARCHING_MODE =
        {
            " | 검 색 |",
            "도  서  명 | ",
            "출  판  사 | ",
            "저      자 | "
        };

        public static string[] SEARCHING_BOOK_MENU_IN_ADDING_MODE =
        {
            " | 검 색 |",
            "도  서  명 | ",
        };

        public static string[] SEARCHING_MEMBER_MENU_IN_SEARCHING_MODE =
        {
            " | 검 색 |",
            "이      름 | ",
            "출생  년도 | ",
            "주      소 | "
        };

        public static string[] LOG_IN_TITLE =
        {
            "",
            "로그인",
            "관리자 로그인"
        };

        public static string[] LOGIN_SEARCH_CATEGORY_AND_GUIDELINE =
        {
            "학번 | ",
            "8자리 숫자 입력",
            "암호 | ",
            "입력"
        };

        public static string[] JOIN_SEARCH_CATEGORY_AND_GUIDELINE =
        {
            "이름 | ",
            "3-5자 한글 입력",
            "학번 | ",
            "8자 숫자 입력",
            "암호 | ",
            "입력",
            "주소 | ",
            "선택",
            "전화번호 | ",
            "010-xxxx-xxxx",
            "생일 | ",
            "선택"
        };

        public static string[] ADD_BOOK_CATEGORY_AND_GUILDLINE =
        {
            "도서명 | ",
            "15자 이내 입력"
        };

        public static string[] ERROR_MESSAGE =
        {
            "검색 조건과 일치하는 도서가 없습니다!",
            "등록되지 않은 학번입니다!",
            "잘못된 암호 입력입니다!",
            "값을 입력하세요!",
            "올바르지 않은 입력 길이입니다!",
            "90년부터 현재 년도의 학번만 가입 가능합니다!",
            "이미 등록된 학번입니다!",
            "8자 이상 15자 이하로 입력하세요!",
            "같은 도서 등록은 1~99권까지만 가능합니다!"
        };

        public static string[] SEARCHED_BOOK_GUIDELINE =
        {
            "  선택  |                    도서                   |          저자         |       출판사       |  출판년도  |  수량  |  가격  |",
            "---------------------------------------------------------------------------------------------------------------------------------"
        };

        public static string[] SEARCHED_BOOK_DETAILED_GUIDLINE =
        {
            "  선택  |                  도서                 |       저자      |     출판사     | 출판년도 |  도서상태  | 청구기호  ",
            "-----------------------------------------------------------------------------------------------------------------------"
        };

        public static string[] MANAGE_BORROWED_BOOK_GUIDELINE =
        {
            " 연장(Q)반납(W) |                  도서                 |       저자      |     대출일     | 연장횟수 |  반납예정  | 청구기호  ",
            "-------------------------------------------------------------------------------------------------------------------------------"
        };

        public static string[] SEARCHED_MEMBER_GUIDELINE =
        {
            "  선택  |    이름    |   학번   |              주소               |    전화번호    |        대출도서 번호        ",
            "-----------------------------------------------------------------------------------------------------------------"
        };

        public static string[] ADD_NEW_BOOK_GUIDELINE =
        {
            "  선택  |                            도서명                            |         저자         |      출판사      |  출판일  |            ISBN            ",
            "---------------------------------------------------------------------------------------------------------------------------------------------------------"
        };

        public static string[] MANAGE_REGISTERED_BOOK_GUIDLINE =
        {
            "                          선택                          |   청구기호   |   도서상태   |   대출인 학번   |     대출일     |   반납 에정일   | 연장 횟수",
            "---------------------------------------------------------------------------------------------------------------------------------------------------------"
        };

        public static string[][] DISTRICT =
        {
            new string[] {"강원도", "경기도", "경상남도", "경상북도", "광주광역시", "대구광역시", "대전광역시", "부산광역시", "서울특별시", "세종특별자치시", "울산광역시", "인천광역시", "제주특별자치도", "충청남도", "충청북도" },
            new string[] {"춘천시", "원주시", "강릉시", "태백시", "삼척시", "동해시", "속초시", "고성군", "양양군", "철원군", "화천군", "인제군", "양구군", "영월군", "홍천군", "정선군", "평창군", "횡성군" },
            new string[] { "수원시", "고양시", "용인시", "성남시", "부천시", "안산시", "남양주시", "화성시", "안양시", "평택시", "의정부시", "파주시", "시흥시", "김포시", "광명시", "광주시", "군포시", "오산시", "이천시", "양주시", "구리시", "안성시", "하남시", "의왕시", "포천시", "여주시", "양평군", "동두천시", "과천시", "가평군", "연천군" },
            new string[] { "창원시", "김해시", "진주시", "거제시", "통영시", "사천시", "밀양시", "양산시", "고성군", "거창군", "남해군", "산청군", "의령군", "창녕군", "하동군", "함안군", "함양군", "합천군" },
            new string[] { "포항시", "경주시", "울진군", "영덕군", "울릉군", "구미시", "경산시", "김천시", "영천시", "칠곡군", "성주군", "고령군", "청도군", "군위군", "안동시", "영주시", "의성군", "예천군", "봉화군", "청송군", "영양군", "상주시", "문경시"},
            new string[] { "남구", "동구", "북구", "서구", "광산구"},
            new string[] { "중구", "동구", "서구", "남구", "북구", "수성구", "달서구", "달성군"},
            new string[] { "중구", "동구", "서구", "대덕구", "유성구"},
            new string[] { "강서구", "금정구", "남구", "동구", "동래구", "부산진구", "북구", "사상구", "사하구", "서구", "수영구", "연제구", "영도구", "중구", "해운대구", "기장군"},
            new string[] { "종로구", "중구", "용산구", "도봉구", "노원구", "강북구", "성북구", "중랑구", "동대문구", "성동구", "광진구", "은평구", "마포구", "서대문구", "강서구", "관악구", "구로구", "금천구", "동작구", "양천구", "영등포구", "서초구", "강남구", "송파구", "강동구" },
            new string[] { "조치원읍", "금남면", "보람동", "연기면", "도담동", "고운동", "아름동", "종촌동", "한솔동", "연동면", "연서면", "소정면", "전동면", "전의면", "금남면", "장군면", "한솔동", "도담동", "아름동", "종촌동", "부강면" },
            new string[] { "울주군", "북구", "중구", "남구", "동구" },
            new string[] { "중구", "동구", "남구", "연수구", "남동구", "부평구", "계양구", "서구", "강화군", "옹진군" },
            new string[] { "제주시", "서귀포시" },
            new string[] { "천안시", "아산시", "보령시", "공주시", "서산시", "논산시", "계롱시", "당진시", "금산군", "부여군", "서천군", "청양군", "홍성군", "예산군", "태안군" },
            new string[] { "청주시", "충주시", "제천시", "단양군", "괴산군", "보은군", "옥천군", "영동군", "음성군", "진천군", "증평군" }
        };

        public static string[] DISTRICT_INCLUDE_ALL_OPTION =
        {
            "전체", "강원도", "경기도", "경상남도", "경상북도", "광주광역시", "대구광역시", "대전광역시", "부산광역시", "서울특별시", "세종특별자치시", "울산광역시", "인천광역시", "제주특별자치도", "충청남도", "충청북도"
        };

        public static string[] MONTH =
        {
            " 1월", " 2월", " 3월", " 4월", " 5월", " 6월", " 7월", " 8월", " 9월", "10월", "11월", "12월"
        };

        public static string[] DAY =
        {
            " 1일", " 2일", " 3일", " 4일", " 5일", " 6일", " 7일", " 8일", " 9일", "10일",
            "11일", "12일", "13일", "14일", "15일", "16일", "17일", "18일", "19일", "20일",
            "21일", "22일", "23일", "24일", "25일", "26일", "27일", "28일", "29일", "30일", "31일"
        };

        public static string[] MEMBER_EDIT_OPTION =
        {
            "주소",
            "전화번호",
            "암호"
        };

        public static string[] MANAGE_MEMBER_OPTION =
        {
            "회원 등록",
            "회원 검색 및 관리"
        };

        public static string[] MANAGE_BOOK_OPTION =
        {
            "도서 검색 및 등록",
            "등록된 도서 관리"
        };

        public static string[] MEMBER_SEARCH_CATEGORY_AND_GUIDELINE =
        {
            "회원 이름 | ",
            "5글자 이내 한글 입력",
            "회원 생년 | ",
            "선택",
            "회원 주소 | ",
            "선택"
        };

        public static string[] COLUMN_NAME_FOR_DETAIL_INFORMATION =
        {
            "",
            "member_id",
            "date_borrowed",
            "date_deadline_for_return",
            "number_of_renew"
        };

        public static string[] GUIDE_FOR_MODIFYING_BOOK_CONDITION =
        {
            "▷분실(W)훼손(E)보관도서(R)삭제(T)◁",
            "▷대출가능(Q)분실(W)훼손(E)삭제(T)◁",
            "▷대출가능(Q)훼손(E)삭제(T)◁",
            "▷대출가능(Q)분실(W)훼손(E)삭제(T)◁"
        };

        public static string URL = "https://openapi.naver.com/v1/search/book_adv.xml?d_titl=";
        public static string CLIENT_ID = "X1ZDuE2lngjCh3S3EV8x";
        public static string CLIENT_SECRET = "9hOYQ7WQkY";

        public static string SEARCH_MEMBER_TITLE = "등록된 회원 관리";

        public static string ADD_BOOK_COLUMNS = "(name, author, publisher, price, discount, publishing_date, count, isbn, description, serial_number)";
        public static string INSERT_NEW_APPLICATION_NUMBER = "(application_number, book_condition)";

        public static string OUT = "나가기(ESC)";

        public const string NUMBER_PATTERN = "[0-9]";
        public const string ENGLISH_PATTERN = "[a-zA-Z]";
        public const string KOREAN_PATTERN = "[ㄱ-ㅎㅏ-ㅣ가-힣]";
        public const string SPECIAL_LETTER = "[`~!@#$%^&*()\\-_=+\\{\\}\\[\\]\\\\\\|:;\"\'<>,.?/]";

        public const string BLANK = "";

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
        public const int MANAGE_MEMBER_MODE = 3;
        public const int MANAGE_BOOK_MODE = 4;

        // Print Class의 SearchCategoryAndGuideline에서 사용
        public const int BOOK_SEARCH_MODE = 0;
        public const int LOG_IN_MODE = 1;
        public const int JOIN_IN = 2;
        public const int MEMBER_SEARCH_MODE = 3;
        public const int MANAGE_BORROWED_BOOK = 4;
        public const int ADD_BOOK = 5;

        // Menu Class의 cursorTop 정보
        public const int RELEVANT_TO_BOOK = 10;
        public const int LOG_IN_OR_CHECK_BORROWED_BOOK_OR_MANAGE_MEMBER = 12;
        public const int JOIN_IN_OR_UPDATE_USER_INFORMATION = 14;
        public const int LOG_IN_OR_LOG_OUT = 16;
        public const int CLOSE_PROGRAM = 18;

        public const int APPEND_MEMBER = 12;
        public const int MANAGE_REGISTERED_MEMBER = 14;
        public const int APPEND_BOOK = 12;
        public const int MANAGE_REGISTERED_BOOK = 14;

        // BookManage SearchBook에서 사용
        // Menu에서 사용
        public const int NONE = 0;

        // Tool.EnterOrTab return값
        // BookManage.SearchBook
        public const int TAB = 1;
        public const int ENTER = 2;
        public const int ESCAPE = 3;

        // ERROR_MESSAGE의 인덱스와 연결해 사용
        public const int THERE_IS_NO_BOOK = 0;
        public const int THERE_IS_NO_SUCH_ID = 1;
        public const int PASSWORD_IS_WRONG = 2;
        public const int THERE_IS_NO_SEARCHWORD = 3;

        // GetValue.Information 에서 입력받는 문자 조건
        public const int ONLY_NUMBER = 0;
        public const int ALL_CHARACTER = 1;
        public const int NO_KOREAN = 2;
        public const int ONLY_KOREAN = 3;

        // MemberManage.SignIn에서 사용
        public const int ANSWER_NAME = 0;
        public const int ANSWER_USER_ID = 1;
        public const int ANSWER_PASSWORD = 2;
        public const int ANSWER_ADDRESS = 3;
        public const int ANSWER_ADDRESS_DEEPLY = 20;
        public const int ANSWER_ADDRESS_INCLUDE_ALL_OPTION = 9;
        public const int ANSWER_PHONE_NUMBER = 4;
        public const int ANSWER_BIRTHDATE_YEAR = 5;
        public const int ANSWER_BIRTHDATE_YEAR_INCLUDE_ALL_OPTION = 11;
        public const int ANSWER_BIRTHDATE_MONTH = 6;
        public const int ANSWER_BIRTHDATE_DAY = 7;
        public const int ANSWER_WHAT_TO_EDIT = 8;
        public const int ANSWER_BOOK_NAME = 10;

        public const int GENERAL_MODE = 0;
        public const int INCLUDE_ALL_OPTION = 1;

        // Tools.IsValidAnswer에서 사용
        // ERROR_MESSAGE와 연동
        public const int VALID = 0;
        public const int ESCAPE_KEY_ERROR = 1;
        public const int LENGTH_ERROR = 4;
        public const int YEAR_ERROR = 5;
        public const int OVERRIDE_ERROR = 6;
        public const int WRONG_LENGTH_ERROR = 7;
        public const int EXCEED_INPUT_ERROR = 8;

        // MemberMabage.ChangeUserInformation에서 사용
        public const int EDIT_ADDRESS = 0;
        public const int EDIT_PHONE_NUMBER = 1;
        public const int EDIT_PASSWORD = 2;

        public const int BORROW = 0;
        public const int EXTEND = 1;
        public const int RETURN = 2;
        public const int FAIL = 3;
        public const int SUCCESS = 4;

        public const int BOOK_ID = 0;
        public const int MEMBER_ID = 1;

        public const int DATE_BORROWED = 2;
        public const int DATE_DEADLINE_FOR_RETURN = 3;
        public const int NUMBER_OF_RENEW = 4;

        public const int PRICE = 0;
        public const int DISCOUNT = 1;
        public const int AUTHOR = 2;
        public const int ISBN = 3;
        public const int DESCRIPTION = 4;

        public const ConsoleColor FOREGROUND_COLOR = ConsoleColor.White;
    }
}
