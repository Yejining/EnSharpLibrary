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

        public void Member(List<MemberVO> members)
        {
            string studentNumber = "";
            string password;

            print.Title("회원 로그인    ");

            Console.SetCursorPosition(45, Console.CursorTop);
            Console.Write("▷ 학번 : ");
            studentNumber = getValue.SearchWord(10, 1);

            while (!getValue.IsAvailableStudentNumber(members, studentNumber))
            {
                if (string.Compare(studentNumber, "@입력취소@") == 0) { Menu menu = new Menu(); menu.Start(1); return; }

                Console.SetCursorPosition(0, Console.CursorTop + 1);
                print.Announce("등록되지 않은 학번입니다!");
                Console.SetCursorPosition(55, Console.CursorTop - 1);
                Console.Write(new string(' ', studentNumber.Length));
                Console.SetCursorPosition(55, Console.CursorTop);
                studentNumber = getValue.SearchWord(10, 1);
            }
            
            //Console.
        }

        public void Admin(AdminVO admin)
        {
            print.Title("관리자 로그인     ");
        }

        public void Join(List<MemberVO> members)
        {
            print.Title("회원가입   ");
        }
    }
}
