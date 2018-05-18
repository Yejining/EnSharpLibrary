using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.IO;
using System.Xml;
using MySql.Data.MySqlClient;

using EnSharpLibrary.Data;
using EnSharpLibrary.IO;

namespace EnSharpLibrary.Function
{
    class Tool
    {
        Print print = new Print();

        public XmlDocument ConnectNaverAPIAndGetInformation(string searchingKeyword)
        {
            string url = Constant.URL + searchingKeyword + "&target=book&display=100";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("X-Naver-Client-Id", Constant.CLIENT_ID);
            request.Headers.Add("X-Naver-Client-Secret", Constant.CLIENT_SECRET);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string text = reader.ReadToEnd();
            XmlDocument bookInformation = new XmlDocument();
            bookInformation.LoadXml(text);
            return bookInformation;
        }

        /// <summary>
        /// 방향키를 이용하는 기능을 수행할 경우, 위 방향키를 누를 때 호출되는 메소드입니다.
        /// </summary>
        /// <param name="cursorLocation">들여쓰기를 위한 커서 정보</param>
        /// <param name="startingLine">첫 선택지가 위치한 줄</param>
        /// <param name="interval">줄간격</param>
        /// <param name="pointer">포인터</param>
        public void UpArrow(int cursorLocation, int startingLine, int countOfOption, int interval, string pointer)
        {
            print.ClearOneLetter(cursorLocation);
            if (Console.CursorTop > startingLine) Console.SetCursorPosition(cursorLocation, Console.CursorTop - interval);
            else if (Console.CursorTop == startingLine) Console.SetCursorPosition(cursorLocation, startingLine + (interval * (countOfOption - 1)));
            Console.Write(pointer);
        }

        public void UpArrow(int cursorLocation, int startingLine, int countOfOption, int interval, int left)
        {
            int length = left - cursorLocation;
            Console.SetCursorPosition(cursorLocation, Console.CursorTop);
            Console.Write(new string(' ', length));
            if (Console.CursorTop > startingLine) Console.SetCursorPosition(cursorLocation, Console.CursorTop - interval);
            else if (Console.CursorTop == startingLine) Console.SetCursorPosition(cursorLocation, startingLine + (interval * (countOfOption - 1)));
        }

        /// <summary>
        /// 방향키를 이용하는 기능을 수행할 경우, 아래 방향키를 누를 때 호출되는 메소드입니다.
        /// </summary>
        /// <param name="cursorLocation">들여쓰기를 위한 커서 정보</param>
        /// <param name="startingLine">첫 선택지가 위치한 줄</param>
        /// <param name="countOfOption">선택지 개수</param>
        /// <param name="interval">줄간격</param>
        /// <param name="pointer">포인터</param>
        public void DownArrow(int cursorLocation, int startingLine, int countOfOption, int interval, string pointer)
        {
            print.ClearOneLetter(cursorLocation);
            if (Console.CursorTop < startingLine + (interval * (countOfOption - 1))) Console.SetCursorPosition(cursorLocation, Console.CursorTop + interval);
            else if (Console.CursorTop == startingLine + (interval * (countOfOption - 1))) Console.SetCursorPosition(cursorLocation, startingLine);
            Console.Write(pointer);
        }

        public void DownArrow(int cursorLocation, int startingLine, int countOfOption, int interval, int left)
        {
            int length = left - cursorLocation;
            Console.SetCursorPosition(cursorLocation, Console.CursorTop);
            Console.Write(new string(' ', length));
            if (Console.CursorTop < startingLine + (interval * (countOfOption - 1))) Console.SetCursorPosition(cursorLocation, Console.CursorTop + interval);
            else if (Console.CursorTop == startingLine + (interval * (countOfOption - 1))) Console.SetCursorPosition(cursorLocation, startingLine);
        }

        /// <summary>
        /// 사용자로부터 엔터 혹은 탭키를 받을 때까지 기다리는 메소드입니다.
        /// </summary>
        /// <returns></returns>
        public int EnterOrTab()
        {
            ConsoleKeyInfo keyInfo;

            while (true)
            {
                keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.Enter) return Constant.ENTER;
                else if (keyInfo.Key == ConsoleKey.Tab) return Constant.TAB;
                else if (keyInfo.Key == ConsoleKey.Escape) return Constant.ESCAPE;
            }
        }

        /// <summary>
        /// ESC키를 받을 때까지 키를 입력받는 메소드입니다.
        /// </summary>
        public void WaitUntilGetEscapeKey()
        {
            ConsoleKeyInfo keyInfo;

            while (true)
            {
                keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.Escape) return;
                else if (Console.CursorLeft == 0) print.BlockCursorMove(Console.CursorLeft, " ");
                else print.BlockCursorMove(Console.CursorLeft - 1, " ");
            }
        }
    }
}
