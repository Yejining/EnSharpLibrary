using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EnSharpLibrary.Function;

namespace EnSharpLibrary
{
    class Program
    {
        /// <summary>
        /// 메뉴클래스를 불러와 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            menu.Start(0);
        }
    }
}
