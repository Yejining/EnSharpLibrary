using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using MySql.Data.MySqlClient;

using EnSharpLibrary.Data;
using EnSharpLibrary.IO;

namespace EnSharpLibrary.Function
{
    class ConnectDatabase
    {
        static MySqlConnection connect;
        static MySqlCommand command;
        static MySqlDataReader reader;

        /// <summary>
        /// MySQL에 연결합니다.
        /// </summary>
        public static void ConnectToMySQL()
        {
            String databaseConnect;

            databaseConnect = "Server=Localhost;Database=ensharp_library;Uid=root;Pwd=0000";
            connect = new MySqlConnection(databaseConnect);

            connect.Open();
        }

        /// <summary>
        /// MySQL과의 연결을 종료합니다.
        /// </summary>
        public static void CloseConnectMySQL()
        {
            connect.Close();
        }

        /// <summary>
        /// sql 쿼리문을 실행시킵니다.
        /// </summary>
        /// <param name="sql"></param>
        public static void MakeCommand(string sql)
        {
            command = new MySqlCommand(sql.ToString(), connect);
            reader = command.ExecuteReader();
            reader.Close();
        }

        /// <summary>
        /// sql 쿼리문을 실행 후 데이터베이스에서 읽은 값을 반환해주는 메소드입니다.
        /// </summary>
        /// <param name="sql">쿼리문</param>
        /// <param name="searchCategory">컬럼</param>
        /// <returns></returns>
        public static List<string> MakeCommand(string sql, string column)
        {
            List<string> result = new List<string>();

            command = new MySqlCommand(sql.ToString(), connect);
            reader = command.ExecuteReader();

            while (reader.Read()) result.Add(reader[column].ToString());

            reader.Close();

            return result;
        }

        /// <summary>
        /// 테이블 안에서 일정 조건을 만족하는 데이터의 갯수를 반환하는 메소드입니다.
        /// </summary>
        /// <param name="tableName">테이블명</param>
        /// <param name="searchCategory">검색 조건문 조건</param>
        /// <param name="searchKey">검색 조건문 내용</param>
        /// <returns>데이터 갯수</returns>
        public static int GetCountFromDatabase(string tableName, string searchCategory, string searchKey)
        {
            string sql = "SELECT count(*) FROM " + tableName;
            if (searchCategory == Constant.BLANK) sql = sql + ";";
            else sql = sql + " WHERE " + searchCategory + "=\"" + searchKey + "\";";

            return Int32.Parse(MakeCommand(sql, "count(*)")[0]);
        }

        /// <summary>
        /// 테이블 안에서 일정 조건을 만족하는 데이터의 갯수를 반환하는 메소드입니다.
        /// </summary>
        /// <param name="tableName">테이블명</param>
        /// <param name="conditionalExpression">검색 조건</param>
        /// <returns>데이터 갯수</returns>
        public static int GetCountFromDatabase(string tableName, string conditionalExpression)
        {
            string sql = "SELECT count(*) FROM " + tableName + conditionalExpression + ";";

            return Int32.Parse(MakeCommand(sql, "count(*)")[0]);
        }

        /// <summary>
        /// 데이터베이스에서 일정 조건을 만족하는 column값을 선택해 가져오는 메소드입니다.
        /// </summary>
        /// <param name="column">컬럼명</param>
        /// <param name="tableName">테이블명</param>
        /// <param name="searchCategory">검색 조건문 조건</param>
        /// <param name="searchKey">검색 조건문 내용</param>
        /// <returns>테이블에서 선택한 컬럼 내용</returns>
        public static List<string> SelectFromDatabase(string column, string tableName, string searchCategory, string searchKey)
        {
            List<string> data = new List<string>();
            string sql = "SELECT " + column + " FROM " + tableName;
            if (searchCategory == Constant.BLANK) sql += ";";
            else sql += " WHERE " + searchCategory + "=\"" + searchKey + "\";";

            return MakeCommand(sql, column);
        }

        /// <summary>
        /// 데이터베이스에서 일정 조건을 만족하는 column값을 선택해 가져오는 메소드입니다.
        /// </summary>
        /// <param name="column">컬럼명</param>
        /// <param name="tableName">테이블명</param>
        /// <param name="searchCategory1">검색 조건문 조건</param>
        /// <param name="searchKey">검색 조건문 내용</param>
        /// <param name="searchCategory2">NULL이어야 할 컬럼</param>
        /// <returns>테이블에서 선택한 컬럼 내용</returns>
        public static List<string> SelectFromDatabase(string column, string tableName, string searchCategory1, string searchKey, string searchCategory2)
        {
            List<string> data = new List<string>();
            string sql = "SELECT " + column + " FROM " + tableName + " WHERE " + searchCategory1 + "=\"" + searchKey + "\" AND " + searchCategory2 + " IS NULL;";

            return MakeCommand(sql, column);
        }

        /// <summary>
        /// 데이터베이스에서 일정 조건을 만족하는 column값을 선택해 가져오는 메소드입니다.
        /// </summary>
        /// <param name="column">컬럼명</param>
        /// <param name="tableName">테이블명</param>
        /// <param name="conditionalExpression">조건식</param>
        /// <returns>테이블에서 선택한 컬럼 내용</returns>
        public static List<string> SelectFromDatabase(string column, string tableName, string conditionalExpression)
        {
            List<string> data = new List<string>();
            string sql = "SELECT " + column + " FROM " + tableName + conditionalExpression + ";";

            return MakeCommand(sql, column);
        }

        /// <summary>
        /// 테이블에서 조건문을 만족시키는 데이터를 삭제하는 메소드입니다.
        /// </summary>
        /// <param name="tableName">테이블명</param>
        /// <param name="conditionalExpression">조건문</param>
        public static void DeleteFromDatabase(string tableName, string conditionalExpression)
        {
            string sql = "DELETE FROM " + tableName + conditionalExpression + ";";

            MakeCommand(sql);
        }

        /// <summary>
        /// 테이블에 데이터를 삽입하는 메소드입니다.
        /// </summary>
        /// <param name="table">테이블명</param>
        /// <param name="columns">삽입할 데이터 변수명</param>
        /// <param name="values">삽입할 데이터 내용</param>
        public static void InsertIntoDatabase(string table, string columns, string values)
        {
            string sql = "INSERT INTO " + table + " " + columns + " VALUES " + values + ";";

            MakeCommand(sql);
        }

        /// <summary>
        /// 테이블 내에서 조건을 만족하는 데이터를 업데이트하는 메소드입니다.
        /// </summary>
        /// <param name="table">테이블명</param>
        /// <param name="column">컬럼명</param>
        /// <param name="data">업데이트할 정보</param>
        /// <param name="category">검색할 칼럼명</param>
        /// <param name="key">검색할 칼럼의 데이터</param>
        public static void UpdateToDatabase(string table, string column, string data, string category, string key)
        {
            StringBuilder sql = new StringBuilder("UPDATE " + table + " SET " + column + "=\"" + data);
            if (category == Constant.BLANK) sql.Append("\";");
            else sql.Append("\" WHERE " + category + "=\"" + key + "\";");

            MakeCommand(sql.ToString());
        }

        /// <summary>
        /// 테이블 내에서 조건을 만족하는 데이터를 업데이트하는 메소드입니다.
        /// </summary>
        /// <param name="table">테이블명</param>
        /// <param name="column">컬럼명</param>
        /// <param name="data">업데이트할 데이터</param>
        /// <param name="category1">검색할 컬럼명</param>
        /// <param name="key">category1의 검색 조건</param>
        /// <param name="category2">NULL로 검색할 컬럼명</param>
        public static void UpdateToDatabase(string table, string column, string data, string category1, string key, string category2)
        {
            string sql = "UPDATE " + table + " SET " + column + "=" + data + " WHERE ";
            sql += category1 + "=\"" + key + "\" AND " + category2 + " IS NULL;";

            MakeCommand(sql);
        }

        /// <summary>
        /// 도서를 데이터베이스에 등록하는 메소드입니다.
        /// </summary>
        /// <param name="book">책 정보</param>
        /// <param name="registeredCount">등록된 책 수량</param>
        /// <param name="count">등록할 책 수</param>
        /// <param name="serialNumber">청구기호(정수)</param>
        public static void RegisterBookToDatabase(BookAPIVO book, int registeredCount, string count, int serialNumber)
        {
            int countToUpdate = registeredCount + Int32.Parse(count);

            // sql문 작성
            string values1 = "(\"" + book.Title + "\",\"" + book.Author + "\",\"" + book.Publisher + "\"," + book.Price + "," + book.Discount + ",\"";
            string values2 = book.Pubdate + "\"," + count + ",\"" + book.Isbn + "\",\"" + book.Description + "\"," + serialNumber + ")";

            // 데이터베이스에 저장_1(book_api)
            if (registeredCount == 0) InsertIntoDatabase("book_api", Constant.ADD_BOOK_COLUMNS, values1 + values2);
            else UpdateToDatabase("book_api", "count", countToUpdate.ToString(), "isbn", book.Isbn);

            float applicationNumber;
            string value;

            // 데이터베이스에 저장_2(book_detail)
            for (int create = 0; create < Int32.Parse(count); create++)
            {
                applicationNumber = serialNumber + ((float)(create + registeredCount) / 100);
                value = "(" + applicationNumber.ToString("n2") + ",\"대출 가능\")"; 
                InsertIntoDatabase("book_detail", Constant.INSERT_NEW_APPLICATION_NUMBER, value);
            }
        }

        /// <summary>
        /// 책을 대여하는 메소드입니다.
        /// </summary>
        /// <param name="memberID">회원의 학번</param>
        /// <param name="applicationNumber">대여할 도서의 청구기호</param>
        public static void BorrowBook(int memberID, string applicationNumber)
        {
            string sql1 = "UPDATE book_detail SET book_condition=\"대출중\" WHERE application_number = \"" + applicationNumber + "\";";
            string sql2 = "INSERT INTO history (member_id, book_id, date_borrowed, date_deadline_for_return, number_of_renew) ";
            sql2 += "VALUES (" + memberID + ", \"" + applicationNumber + "\", NOW(), DATE_ADD(NOW(), INTERVAL 6 DAY), 0);";

            MakeCommand(sql1);
            MakeCommand(sql2);
        }

        /// <summary>
        /// 책의 반납기한을 연장하는 메소드입니다.
        /// </summary>
        /// <param name="bookID">연장할 책의 청구기호</param>
        public static void Extend(string bookID)
        {
            string sql = " WHERE book_id=" + bookID + " AND date_return IS NULL";
            int renew = Int32.Parse(SelectFromDatabase("number_of_renew", "history", sql)[0]) + 1;
            string deadline = SelectFromDatabase("date_deadline_for_return", "history", sql)[0].Remove(10);

            string sql1 = "UPDATE history SET date_deadline_for_return=DATE_ADD(\"" + deadline + "\", INTERVAL 6 DAY) WHERE book_id=" + bookID + " AND date_return IS NULL;";
            string sql2 = "UPDATE history SET number_of_renew=" + renew + " WHERE book_id=" + bookID + " AND date_return IS NULL;";

            MakeCommand(sql1);
            MakeCommand(sql2);
        }

        /// <summary>
        /// 도서를 반납하는 메소드입니다.
        /// </summary>
        /// <param name="bookID">반납할 도서의 청구기호</param>
        public static void Return(string bookID)
        {
            UpdateToDatabase("history", "date_return", "NOW()", "book_id", bookID, "date_return");
            UpdateToDatabase("book_detail", "book_condition", "대출 가능", "application_number", bookID);
        }

        /// <summary>
        /// 각 기능이 끝날 때마다 로그기록을 남기는 메소드입니다.
        /// </summary>
        /// <param name="usingMemberID">사용자 학번</param>
        /// <param name="content">기록 내용</param>
        public static void Log(int usingMemberID, string content)
        {
            string member;
            string name;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\log.txt";

            // 사용자 학번에 따라 member의 이름을 달리 정함
            if (usingMemberID == Constant.ADMIN)
            {
                member = "관리자";
            }
            else if (usingMemberID == Constant.PUBLIC)
            {
                member = "User";
            }
            else
            {
                name = SelectFromDatabase("name", "member", "member_id", usingMemberID.ToString())[0];
                member = "\'학번:" + usingMemberID + " 이름:" + name + "\'";
            }

            // 데이터베이스에 기록 저장
            InsertIntoDatabase("log", "(time, member, content)", "(NOW(), \"" + member + "\", \"" + content + "\")");

            // 파일에 기록 저장
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("[" + DateTime.Now + "] " + member + " " + content);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine("[" + DateTime.Now + "] " + member + " " + content);
                }
            }
        }
    }
}
