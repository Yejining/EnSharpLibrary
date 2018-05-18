using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using EnSharpLibrary.Data;
using EnSharpLibrary.IO;

namespace EnSharpLibrary.Function
{
    class LogManage
    {
        GetValue getValue = new GetValue();
        Print print = new Print();
        Tool tool = new Tool();

        public void ReadLog()
        {
            List<string> time;
            List<string> member;
            List<string> content;

            print.SetWindowsizeAndPrintTitle(135, 35, "로그 열람");
            print.PrintSentences(Constant.LOG_GUIDE, Console.CursorTop + 2);

            getValue.Log(out time, out member, out content);

            print.Log(time, member, content);

            ConnectDatabase.Log(Constant.ADMIN, "로그 열람");
            tool.WaitUntilGetEscapeKey();
        }

        public void DeleteLogFile()
        {
            string file =  Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\log.txt";

            File.Delete(file);
        }
    }
}
