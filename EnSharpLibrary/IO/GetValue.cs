using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EnSharpLibrary.Data;
using EnSharpLibrary.Function;

namespace EnSharpLibrary.IO
{
    class GetValue
    {
        Print print = new Print();

        public int NumberOfSameBook(List<BookVO> books, string name, string author, string publisher, int year)
        {
            for (int order = 0; order < books.Count; order++)
            {
                if (books[order].Name == name && books[order].Author == author &&
                    books[order].Publisher == publisher && books[order].PublishingYear == year) return (int)Math.Floor(books[order].NumberOfThis);
            }

            return -1;
        }

        public int FirstBooksCount(List<BookVO> books)
        {
            int count = 0;

            for (int i = 0; i < books.Count; i++)
                if (books[i].OrderOfBooks == 0) count++;

            return count;
        }

        public int DetailBooksCount(List<BookVO> books, int numberOfBook)
        {
            int count = 0;

            for (int i = 0; i < books.Count; i++)
                if ((int)Math.Floor(books[i].NumberOfThis) == numberOfBook) count++;

            return count;
        }

        public List<int> BookList(List<BookVO> books)
        {
            List<int> bookList = new List<int>();
            bookList.Clear();

            for (int order = 0; order < books.Count; order++)
            {
                if (books[order].OrderOfBooks == 0)
                    bookList.Add((int)Math.Floor(books[order].NumberOfThis));
            }

            return bookList;
        }

        // 0 : (도서검색어)영어, 한글, 숫자 등 문자
        // 1 : (로그인 학번 검색) 숫자
        // 2 : (로그인 암호 검색) 암호
        public string SearchWord(int limit, int searchType)
        {
            ConsoleKeyInfo keyInfo;
            StringBuilder answer = new StringBuilder();
            int leftCursor = Console.CursorLeft;
            int currentCursor;
            int full = 0;

            while (true)
            {
                currentCursor = Console.CursorLeft;
                keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.Escape) return "@입력취소@";
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (IsValidAnswer(answer.ToString())) return answer.ToString();
                    else
                    {
                        Console.SetCursorPosition(leftCursor, Console.CursorTop);
                        Console.Write(new string(' ', Console.WindowWidth - leftCursor - 1));
                        Console.SetCursorPosition(leftCursor, Console.CursorTop);
                        answer.Clear();
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (answer.Length > 0)
                    {
                        answer.Remove(answer.Length - 1, 1);
                        Console.SetCursorPosition(leftCursor, Console.CursorTop);
                        if (answer.Length == 0)
                        {
                            Console.Write(' ');
                            Console.SetCursorPosition(leftCursor, Console.CursorTop);
                        }
                        else print.ClearSearchBar(leftCursor, answer.ToString());
                    }
                    else if (answer.Length == 0) Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                }
                else if (IsNotAvailableKey(keyInfo)) Console.SetCursorPosition(currentCursor, Console.CursorTop);
                else if (searchType == 1 && !IsNotNumber(keyInfo)) Console.SetCursorPosition(currentCursor, Console.CursorTop);
                else
                {
                    full = 0;
                    if (answer.Length < limit) answer.Append(keyInfo.KeyChar);
                    else
                    {
                        if (full == 0)
                        {
                            answer.Remove(answer.Length - 1, 1);
                            full = 1;
                        }
                        Console.SetCursorPosition(leftCursor, Console.CursorTop);
                        print.ClearSearchBar(leftCursor, answer.ToString());
                    }
                    if (searchType == 2) { print.ClearOneLetter(Console.CursorLeft - 1); Console.Write('*'); }
                }
            }
        }

        public bool IsNotAvailableKey(ConsoleKeyInfo key)
        {
            List<int> nonAvailableKey = new List<int>();

            nonAvailableKey.Add(9);
            nonAvailableKey.Add(12);
            nonAvailableKey.Add(19);
            nonAvailableKey.Add(33);
            nonAvailableKey.Add(118);
            nonAvailableKey.Add(229);
            nonAvailableKey.Add(131);

            for (int valid = 35; valid <= 38; valid++) nonAvailableKey.Add(valid);
            for (int valid = 127; valid <= 135; valid++) nonAvailableKey.Add(valid);
            for (int valid = 166; valid <= 183; valid++) nonAvailableKey.Add(valid);
            for (int valid = 246; valid <= 253; valid++) nonAvailableKey.Add(valid);

            foreach (int valid in nonAvailableKey) if (key.KeyChar == valid) return true;

            return false;
        }

        public bool IsNotNumber(ConsoleKeyInfo key)
        {
            List<int> numbers = new List<int>();

            for (int number = 48; number < -57; number++) numbers.Add(number);

            bool isNotAvailable = IsNotAvailableKey(key);
            
            if (isNotAvailable) return true;
            else
            {
                foreach (int number in numbers) if (key.KeyChar == number) return false;
                return true;
            }
        }

        public bool IsValidAnswer(string answer)
        {
            bool isAllSpaceBar = true;

            if (answer.Length == 0)
            {
                print.Announce("입력값이 없습니다!");
                return false;
            }

            foreach (char letter in answer) if (letter != 32) isAllSpaceBar = false;
            if (isAllSpaceBar)
            {
                print.Announce("올바르지 않은 입력값입니다!");
                return false;
            }

            return true;
        }

        public bool IsAvailableStudentNumber(List<MemberVO> members, string studentNumber)
        {
            if (string.Compare(studentNumber, "@입력취소@") == 0) return false;
            foreach (MemberVO member in members) if (member.IdentificationNumber == Int32.Parse(studentNumber)) return true;
            return false;
        }

        public bool IsCorrectPassword(List<MemberVO> members, int studentNumber, string password)
        {
            foreach (MemberVO member in members)
                if (member.IdentificationNumber == studentNumber && string.Compare(member.Password, password) == 0) return true;
            return false;
        }

        // 1 : 도서명 검색
        // 2 : 출판사 검색
        // 3 : 저자 검색
        public List<int> FoundBooks(List<BookVO> books, string searchWord, int searchMode)
        {
            List<int> indexesOfSearchResult = new List<int>();
            indexesOfSearchResult.Clear();

            switch (searchMode)
            {
                case 1:
                    for (int i = 0; i < books.Count; i++)
                        if (System.Text.RegularExpressions.Regex.IsMatch(books[i].Name, searchWord, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                        {
                            bool thereIs = false;
                            foreach (int number in indexesOfSearchResult) if ((int)Math.Floor(books[i].NumberOfThis) == number) thereIs = true;

                            if (!thereIs) indexesOfSearchResult.Add((int)Math.Floor(books[i].NumberOfThis));
                        }
                    break;
                case 2:
                    for (int i = 0; i < books.Count; i++)
                        if (System.Text.RegularExpressions.Regex.IsMatch(books[i].Publisher, searchWord, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                        {
                            bool thereIs = false;
                            foreach (int number in indexesOfSearchResult) if ((int)Math.Floor(books[i].NumberOfThis) == number) thereIs = true;

                            if (!thereIs) indexesOfSearchResult.Add((int)Math.Floor(books[i].NumberOfThis));
                        }
                    break;
                case 3:
                    for (int i = 0; i < books.Count; i++)
                        if (System.Text.RegularExpressions.Regex.IsMatch(books[i].Author, searchWord, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                        {
                            bool thereIs = false;
                            foreach (int number in indexesOfSearchResult) if ((int)Math.Floor(books[i].NumberOfThis) == number) thereIs = true;

                            if (!thereIs) indexesOfSearchResult.Add((int)Math.Floor(books[i].NumberOfThis));
                        }
                    break;
            }

            return indexesOfSearchResult;
        }
    }
}
