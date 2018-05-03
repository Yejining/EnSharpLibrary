using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

using EnSharpLibrary.Data;

namespace EnSharpLibrary.Function
{
    class DatabaseManager
    {
        public LibraryVO LoadDataFromDatabase()
        {
            LibraryVO library = new LibraryVO();

            string id;
            string password;
            string name;

            String databaseConnect;
            MySqlConnection connect;

            databaseConnect = "Server=Localhost;Database=ensharp_libarary;Uid=ensharpstudy;Pwd=0000";
            connect = new MySqlConnection(databaseConnect);

            connect.Open(); // open MySQL      

            //sql = "select * from member;";
            
            //reader.Close();
            connect.Close();

            return library;
        }
    }
}
