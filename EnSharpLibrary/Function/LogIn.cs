using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EnSharpLibrary.IO;
using EnSharpLibrary.Data;

namespace EnSharpLibrary.Function
{
    class LogIn
    {
        Print print = new Print();
        GetValue getValue = new GetValue();

        public int LogInOrLogOut(int usingMemberNumber)
        {
            return usingMemberNumber;
        }

        public int Member(AdminVO admin, List<MemberVO> members, List<BookVO>books)
        {
            string studentNumber = "";
            string password;
            string memberName = "";

            print.Title("회원 로그인    ");

            Console.SetCursorPosition(45, Console.CursorTop);
            Console.Write("▷ 학번 : ");
            studentNumber = getValue.SearchWord(10, 1);

            while (!getValue.IsAvailableStudentNumber(members, studentNumber))
            {
                if (string.Compare(studentNumber, "@입력취소@") == 0) return -1;

                Console.SetCursorPosition(0, Console.CursorTop + 1);
                print.Announce("등록되지 않은 학번입니다!");
                Console.SetCursorPosition(55, Console.CursorTop - 1);
                Console.Write(new string(' ', studentNumber.Length));
                Console.SetCursorPosition(55, Console.CursorTop);
                studentNumber = getValue.SearchWord(10, 1);
            }

            Console.SetCursorPosition(45, Console.CursorTop + 2);
            Console.Write("▷ 암호 : ");
            password = getValue.SearchWord(18, 2);

            while (!getValue.IsCorrectPassword(members, Int32.Parse(studentNumber), password))
            {
                if (string.Compare(password, "@입력취소@") == 0) return -1;

                Console.SetCursorPosition(0, Console.CursorTop + 1);
                print.Announce("암호가 틀렸습니다!");
                Console.SetCursorPosition(55, Console.CursorTop - 1);
                Console.Write(new string(' ', password.Length));
                Console.SetCursorPosition(55, Console.CursorTop);
                password = getValue.SearchWord(18, 2);
            }

            foreach (MemberVO member in members)
                if (member.IdentificationNumber == Int32.Parse(studentNumber) && string.Compare(member.Password, password) == 0)
                    memberName = member.Name;

            StringBuilder welcome = new StringBuilder();
            welcome.AppendFormat("{0}님 환영합니다!", memberName);

            Console.SetCursorPosition(0, Console.CursorTop + 1);
            print.Announce(welcome.ToString());

            return Int32.Parse(studentNumber);
        }

        public int Admin(AdminVO admin)
        {
            string password;

            print.Title("관리자 로그인     ");

            Console.SetCursorPosition(45, Console.CursorTop);
            Console.Write("▷ 암호 : ");
            password = getValue.SearchWord(18, 2);

            while (string.Compare(password, admin.Password) != 0)
            {
                if (string.Compare(password, "@입력취소@") == 0) { return 1; }

                Console.SetCursorPosition(0, Console.CursorTop + 1);
                print.Announce("암호가 틀립니다!");
                Console.SetCursorPosition(55, Console.CursorTop - 1);
                Console.Write(new string(' ', password.Length));
                Console.SetCursorPosition(55, Console.CursorTop);
                password = getValue.SearchWord(18, 2);
            }

            Console.SetCursorPosition(0, Console.CursorTop + 1);
            print.Announce("관리자님 환영합니다!");

            return 3;
        }

        // 1 : 회원가입
        // 2 : 회원등록
        public List<MemberVO> Join(List<MemberVO> members, int type)
        {
            string content1 = "▷ 이름 : ";
            string content2 = "▷ 학번 : ";
            string content3 = "▷ 암호(8자 이상 15자 이하) : ";
            string content4 = "▷ 주소(○○도 ○○시 ○○동 형식) : ";
            string content5 = "▷ 전화번호('-' 없이 입력) : ";
            string name;
            string number;
            string password;
            string address;
            string phoneNumber;
            int cursor = 0;

            if (type == 1) print.Title("회원가입   ");
            else print.Title("회원등록   ");

            // 이름
            Console.SetCursorPosition(40, Console.CursorTop);
            Console.Write(content1);
            name = getValue.SearchWord(5, 0);
            if (string.Compare(name, "@입력취소@") == 0) return members;
            
            // 학번
            Console.SetCursorPosition(40, Console.CursorTop + 2);
            cursor = Console.CursorTop;
            Console.Write(content2);
            number = getValue.SearchWord(10, 1);
            if (string.Compare(number, "@입력취소@") == 0) return members;

            while(getValue.NotValidNumber(number, members) != 0)
            {
                Console.SetCursorPosition(0, Console.CursorTop + 1);
                if (getValue.NotValidNumber(number, members) == 1) print.Announce("길이에 맞는 학번을 입력하세요!");
                else if (getValue.NotValidNumber(number, members) == 2) print.Announce("이미 등록된 학번입니다!");
                else if (getValue.NotValidNumber(number, members) == 3) print.Announce("90학번부터만 가입이 가능합니다!");
                Console.SetCursorPosition(40, cursor);
                print.ClearCurrentConsoleLine();
                Console.SetCursorPosition(40, cursor);
                Console.Write(content2);
                number = getValue.SearchWord(10, 1);
                if (string.Compare(number, "@입력취소@") == 0) return members;
            }

            // 암호
            if (type == 1)
            {
                Console.SetCursorPosition(40, Console.CursorTop + 2);
                cursor = Console.CursorTop;
                Console.Write(content3);
                password = getValue.SearchWord(18, 2);
                if (string.Compare(number, "@입력취소@") == 0) return members;

                while (getValue.NotValidPassword(password) != 0)
                {
                    Console.SetCursorPosition(0, Console.CursorTop + 1);
                    if (getValue.NotValidPassword(password) == 1) print.Announce("길이에 맞는 암호를 입력하세요!");
                    else if (getValue.NotValidPassword(password) == 2) print.Announce("3자리 연속으로 문자 혹은 숫자가  사용되면 안 됩니다!");
                    else if (getValue.NotValidPassword(password) == 3) print.Announce("암호는 영어와 숫자만 가능합니다!");
                    Console.SetCursorPosition(40, cursor);
                    print.ClearCurrentConsoleLine();
                    Console.SetCursorPosition(40, cursor);
                    Console.Write(content3);
                    password = getValue.SearchWord(17, 2);
                    if (string.Compare(number, "@입력취소@") == 0) return members;
                }
            }
            else password = number;

            // 주소
            Console.SetCursorPosition(40, Console.CursorTop + 2);
            Console.Write(content4);
            address = getValue.SearchWord(20, 0);
            if (string.Compare(address, "@입력취소@") == 0) return members;

            // 전화번호
            Console.SetCursorPosition(40, Console.CursorTop + 2);
            cursor = Console.CursorTop;
            Console.Write(content5);
            phoneNumber = getValue.SearchWord(15, 1);
            if (string.Compare(phoneNumber, "@입력취소@") == 0) return members;

            while(getValue.NotValidPhoneNumber(phoneNumber, members))
            {
                Console.SetCursorPosition(0, Console.CursorTop + 1);

                if (getValue.NotValidPhoneNumberSpecifically(phoneNumber, members) == 1)
                    print.Announce("전화번호의 길이가 유효하지 않습니다!");
                else if (getValue.NotValidPhoneNumberSpecifically(phoneNumber, members) == 2)
                    print.Announce("이미 등록된 번호입니다!");
                else print.Announce("유효하지 않은 전화번호입니다!");
                
                Console.SetCursorPosition(40, cursor);
                print.ClearCurrentConsoleLine();
                Console.SetCursorPosition(40, cursor);
                Console.Write(content5);
                phoneNumber = getValue.SearchWord(15, 1);
                if (string.Compare(phoneNumber, "@입력취소@") == 0) return members;
            }

            StringBuilder phone = new StringBuilder(phoneNumber);
            phone.Insert(3, '-');
            phone.Insert(8, '-');

            MemberVO newMember = new MemberVO();
            newMember.SetMember(Int32.Parse(number), name, password);
            newMember.SetMember(address, phone.ToString());

            members.Add(newMember);

            Console.SetCursorPosition(40, Console.CursorTop + 2);
            if (type == 1) Console.Write("{0}님 회원가입을 축하합니다!");
            else Console.Write("{0}님의 등록이 완료되었습니다!");

            return members;
        }
    }
}
