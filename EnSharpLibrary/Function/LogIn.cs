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
            if (type == 1) print.Title("회원가입   ");
            else print.Title("회원등록   ");

            return members;
        }
    }
}
