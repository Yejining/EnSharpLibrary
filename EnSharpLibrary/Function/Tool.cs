using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnSharpLibrary.IO;

namespace EnSharpLibrary.Function
{
    class Tool
    {
        Print print = new Print();

        /// <summary>
        /// 방향키를 이용하는 기능을 수행할 경우, 위 방향키를 누를 때 호출되는 메소드입니다.
        /// </summary>
        /// <param name="cursorLocation">들여쓰기를 위한 커서 정보</param>
        /// <param name="startingLine">첫 선택지가 위치한 줄</param>
        /// <param name="interval">줄간격</param>
        /// <param name="pointer">포인터</param>
        public void UpArrow(int cursorLocation, int startingLine, int countOfOption, int interval, char pointer)
        {
            print.ClearOneLetter(cursorLocation);
            if (Console.CursorTop > startingLine) Console.SetCursorPosition(cursorLocation, Console.CursorTop - interval);
            else if (Console.CursorTop == startingLine) Console.SetCursorPosition(cursorLocation, startingLine + (interval * (countOfOption - 1)));
            Console.Write(pointer);
        }

        /// <summary>
        /// 방향키를 이용하는 기능을 수행할 경우, 아래 방향키를 누를 때 호출되는 메소드입니다.
        /// </summary>
        /// <param name="cursorLocation">들여쓰기를 위한 커서 정보</param>
        /// <param name="startingLine">첫 선택지가 위치한 줄</param>
        /// <param name="countOfOption">선택지 개수</param>
        /// <param name="interval">줄간격</param>
        /// <param name="pointer">포인터</param>
        public void DownArrow(int cursorLocation, int startingLine, int countOfOption, int interval, char pointer)
        {
            print.ClearOneLetter(cursorLocation);
            if (Console.CursorTop < startingLine + (interval * (countOfOption - 1))) Console.SetCursorPosition(cursorLocation, Console.CursorTop + interval);
            else if (Console.CursorTop == startingLine + (interval * (countOfOption - 1))) Console.SetCursorPosition(cursorLocation, startingLine);
            Console.Write(pointer);
        }
    }
}
