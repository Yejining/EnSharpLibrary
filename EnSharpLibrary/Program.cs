using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EnSharpLibrary.Data;
using EnSharpLibrary.Function;

namespace EnSharpLibrary
{
    class Program
    {
        // 김예진, 2018-05-04
        // 관리자 암호 0000
        // 저장한 정보는 엑셀에 정리해 올렸습니다
        // 사용자 간편 로그인 ID: 16011000 PW:970106

        /// <summary>
        /// 메뉴클래스를 불러와 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Menu menu = new Menu();

            ConnectDatabase.ConnectToMySQL();
            menu.RunLibraryProgram(Constant.NON_MEMBER_MODE);

            ConnectDatabase.CloseConnectMySQL();
        }
    }
}
