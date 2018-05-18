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

        public static void Log(int usingMemberID, string content)
        {
            string member;
            string name;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\log.txt";

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

            string columns = "(time, member, content)";
            string values = "(NOW(), \"" + member + "\", \"" + content + "\")";
            
            InsertIntoDatabase("log", columns, values);

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

        public static void ConnectToMySQL()
        {
            String databaseConnect;
            
            databaseConnect = "Server=Localhost;Database=ensharp_library;Uid=root;Pwd=0000";
            connect = new MySqlConnection(databaseConnect);

            connect.Open();
        }

        public static void CloseConnectMySQL()
        {
            connect.Close();
        }

        public static int GetCountFromDatabase(string tableName, string searchCategory, string searchKey)
        {
            string sql = "SELECT count(*) FROM " + tableName;
            if (searchCategory == Constant.BLANK) sql = sql + ";";
            else sql = sql + " WHERE " + searchCategory + "=\"" + searchKey + "\";";

            int count = 0;

            command = new MySqlCommand(sql.ToString(), connect);
            reader = command.ExecuteReader();

            while (reader.Read()) count = Int32.Parse(reader["count(*)"].ToString());

            reader.Close();

            return count;
        }

        public static int GetCountFromDatabase(string tableName, string conditionalExpression)
        {
            string sql = "SELECT count(*) FROM " + tableName + conditionalExpression + ";";
            int count = 0;

            command = new MySqlCommand(sql.ToString(), connect);
            reader = command.ExecuteReader();

            while (reader.Read()) count = Int32.Parse(reader["count(*)"].ToString());

            reader.Close();

            return count;
        }

        public static List<string> SelectFromDatabase(string column, string tableName, string searchCategory, string searchKey)
        {
            List<string> data = new List<string>();
            string sql = "SELECT " + column + " FROM " + tableName;
            if (searchCategory == Constant.BLANK) sql += ";";
            else sql += " WHERE " + searchCategory + "=\"" + searchKey + "\";";
            
            command = new MySqlCommand(sql.ToString(), connect);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (string.Compare(column, "*") != 0) data.Add(reader[column].ToString());
                else data.Add(reader.ToString());
            }

            reader.Close();

            return data;
        }

        public static List<string> SelectFromDatabase(string column, string tableName, string searchCategory1, string searchKey, string searchCategory2)
        {
            List<string> data = new List<string>();
            string sql = "SELECT " + column + " FROM " + tableName + " WHERE " + searchCategory1 + "=\"" + searchKey + "\" AND " + searchCategory2 + " IS NULL;";

            command = new MySqlCommand(sql.ToString(), connect);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (string.Compare(column, "*") != 0) data.Add(reader[column].ToString());
                else data.Add(reader.ToString());
            }

            reader.Close();

            return data;
        }
        
        public static void DeleteFromDatabase(string tableName, string conditionalExpression)
        {
            string sql = "DELETE FROM " + tableName + conditionalExpression + ";";

            MakeCommand(sql);
        }

        public static List<string> SelectFromDatabase(string column, string tableName, string conditionalExpression)
        {
            List<string> data = new List<string>();
            string sql = "SELECT " + column + " FROM " + tableName + conditionalExpression + ";";

            command = new MySqlCommand(sql.ToString(), connect);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (string.Compare(column, "*") != 0) data.Add(reader[column].ToString());
                else data.Add(reader.ToString());
            }

            reader.Close();

            return data;
        }

        public static void InsertIntoDatabase(string table, string columns, string values)
        {
            string sql = "INSERT INTO " + table + " " + columns + " VALUES " + values + ";";

            command = new MySqlCommand(sql.ToString(), connect);
            reader = command.ExecuteReader();
            reader.Close();
        }

        public static void UpdateToDatabase(string table, string column, string data, string category, string key)
        {
            StringBuilder sql = new StringBuilder("UPDATE " + table + " SET " + column + "=\"" + data);
            if (category == Constant.BLANK) sql.Append("\";");
            else sql.Append("\" WHERE " + category + "=\"" + key + "\";");

            command = new MySqlCommand(sql.ToString(), connect);
            reader = command.ExecuteReader();
            reader.Close();
        }

        public static void UpdateToDatabase(string table, string column, string data, string category1, string key, string category2)
        {
            string sql = "UPDATE " + table + " SET " + column + "=" + data + " WHERE ";
            sql += category1 + "=\"" + key + "\" AND " + category2 + " IS NULL;";

            command = new MySqlCommand(sql.ToString(), connect);
            reader = command.ExecuteReader();
            reader.Close();
        }

        public static void BorrowBook(int memberID, string applicationNumber)
        {
            string sql1 = "UPDATE book_detail SET book_condition=\"대출중\" WHERE application_number = \"" + applicationNumber + "\";";
            string sql2 = "INSERT INTO history (member_id, book_id, date_borrowed, date_deadline_for_return, number_of_renew) ";
            sql2 += "VALUES (" + memberID + ", \"" + applicationNumber + "\", NOW(), DATE_ADD(NOW(), INTERVAL 6 DAY), 0);";

            MakeCommand(sql1);
            MakeCommand(sql2);
        }

        public static void MakeCommand(string sql)
        {
            command = new MySqlCommand(sql.ToString(), connect);
            reader = command.ExecuteReader();
            reader.Close();
        }

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

        public static void Return(string bookID)
        {
            UpdateToDatabase("history", "date_return", "NOW()", "book_id", bookID, "date_return");
            UpdateToDatabase("book_detail", "book_condition", "대출 가능", "application_number", bookID);
        }

        
    }
}
