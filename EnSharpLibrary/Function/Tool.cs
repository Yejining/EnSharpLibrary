﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

using EnSharpLibrary.Data;
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

        /// <summary>
        /// 입력받은 키가 유호한지 검사하는 메소드입니다.
        /// </summary>
        /// <param name="keyInfo">입력받은 키</param>
        /// <returns>입력받은 키의 유효성</returns>
        public bool IsValid(ConsoleKeyInfo keyInfo, int mode)
        {
            // 엔터, 탭
            if (keyInfo.Key == ConsoleKey.Enter) return false;
            if (keyInfo.Key == ConsoleKey.Tab) return false;

            // 숫자
            if (System.Text.RegularExpressions.Regex.IsMatch(keyInfo.KeyChar.ToString(), Constant.NUMBER_PATTERN)) return true;
            if (mode == Constant.ONLY_NUMBER) return false;

            // 한글, 영어
            if (System.Text.RegularExpressions.Regex.IsMatch(keyInfo.KeyChar.ToString(), Constant.ENGLISH_PATTERN)) return true;
            if (System.Text.RegularExpressions.Regex.IsMatch(keyInfo.KeyChar.ToString(), Constant.KOREAN_PATTERN))
            {
                if (mode == Constant.NO_KOREAN) return false;
                else return true;
            }

            // 특수기호
            if (System.Text.RegularExpressions.Regex.IsMatch(keyInfo.KeyChar.ToString(), Constant.SPECIAL_LETTER)) return true;

            return false;
        }

        public bool IsExist(string userInputMemberID)
        {
            int isExist = 0;

            StringBuilder sql = new StringBuilder("SELECT count(*) FROM member WHERE member_id=" + userInputMemberID + ";");

            String databaseConnect;
            MySqlConnection connect;

            databaseConnect = "Server=Localhost;Database=ensharp_library;Uid=root;Pwd=0000";
            connect = new MySqlConnection(databaseConnect);

            connect.Open();

            MySqlCommand command = new MySqlCommand(sql.ToString(), connect);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) isExist = Int32.Parse(reader["count(*)"].ToString());

            reader.Close();
            connect.Close();

            if (isExist > 0) return true;
            else return false;
        }

        public bool IsPasswordCorrespond(string userInputMemberID, string userInputPassword)
        {
            string password = "";

            StringBuilder sql = new StringBuilder("SELECT password FROM member WHERE member_id=" + userInputMemberID + ";");

            String databaseConnect;
            MySqlConnection connect;

            databaseConnect = "Server=Localhost;Database=ensharp_library;Uid=root;Pwd=0000";
            connect = new MySqlConnection(databaseConnect);

            connect.Open();

            MySqlCommand command = new MySqlCommand(sql.ToString(), connect);
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) password = reader["password"].ToString();

            reader.Close();
            connect.Close();

            if (string.Compare(password, userInputPassword) == 0) return true;
            else return false;
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