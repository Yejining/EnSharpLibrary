using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnSharpLibrary.Data;
using EnSharpLibrary.IO;

namespace EnSharpLibrary.Function
{
    class LogManage
    {
        Print print = new Print();

        public void LogMenu()
        {
            bool isFirstLoop = true;
            
            //while (true)
            //{
            //    if (isFirstLoop)
            //    {
            //        // 메뉴 출력
            //        print.SetWindowsizeAndPrintTitle(45, 30, "로그 관리");
            //        print.MenuOption(, Console.CursorTop + 2);

            //        // 기능 선택
            //        print.SetCursorAndChoice(38, 10, "◁");

            //        isFirstLoop = false;
            //    }

            //    ConsoleKeyInfo keyInfo = Console.ReadKey();

            //    // 기능 선택
            //    switch (keyInfo.Key)
            //    {
            //        case ConsoleKey.UpArrow: tool.UpArrow(38, 10, optionCount, 2, '◁'); break;
            //        case ConsoleKey.DownArrow: tool.DownArrow(38, 10, optionCount, 2, '◁'); break;
            //        case ConsoleKey.Enter:
            //            isFirstLoop = GoNextFunction(Console.CursorTop);
            //            if (Console.CursorTop == Constant.CLOSE_PROGRAM) return; break;
            //        default: print.BlockCursorMove(38, "◁"); break;
            //    }
            //}
        }
    }
}
