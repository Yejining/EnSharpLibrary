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
        Tool tool = new Tool();
        static MySqlConnection connect;
        static MySqlCommand command;
        static MySqlDataReader reader;

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

        public static XmlDocument BookSearchResult(string searchingKeyword)
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
    }
}
