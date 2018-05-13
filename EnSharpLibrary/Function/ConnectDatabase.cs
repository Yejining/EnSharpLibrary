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

using EnSharpLibrary.Data;
using EnSharpLibrary.IO;

namespace EnSharpLibrary.Function
{
    class ConnectDatabase
    {
        Tool tool = new Tool();
        GetValue getValue = new GetValue();

        public void ConnectAPI(string searchingKeyword)
        {
            string url = Constant.URL + searchingKeyword + "&target=book&display=100";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("X-Naver-Client-Id", Constant.CLIENT_ID);
            request.Headers.Add("X-Naver-Client-Secret", Constant.CLIENT_SECRET);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string status = response.StatusCode.ToString();
            if (status == "OK")
            {
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string text = reader.ReadToEnd();
                XmlDocument bookInformation = new XmlDocument();
                bookInformation.LoadXml(text);
                //Console.WriteLine(text);
                //tool.WaitUntilGetEscapeKey();
                SaveBookToDatabase(bookInformation);

            }
            else
            {
                Console.WriteLine("Error 발생=" + status);
            }
        }
        
        public void SaveBookToDatabase(XmlDocument bookInformation)
        {
            Console.SetWindowSize(155, 35);
            Console.Clear();
            Console.WriteLine(Constant.ADD_NEW_BOOK_GUIDELINE[0]);
            Console.WriteLine(Constant.ADD_NEW_BOOK_GUIDELINE[1]);
            
            XmlNodeList title = bookInformation.GetElementsByTagName("title");
            XmlNodeList author = bookInformation.GetElementsByTagName("author");
            XmlNodeList price = bookInformation.GetElementsByTagName("price");
            XmlNodeList discount = bookInformation.GetElementsByTagName("discount");
            XmlNodeList publisher = bookInformation.GetElementsByTagName("publisher");
            XmlNodeList pubdate = bookInformation.GetElementsByTagName("pubdate");
            XmlNodeList isbn = bookInformation.GetElementsByTagName("isbn");
            XmlNodeList description = bookInformation.GetElementsByTagName("description");

            List<string> bookTitle = getValue.ShortenKeyword(title, 60);
            List<string> bookAuthor = getValue.ShortenKeyword(author, 20);
            List<string> bookPublisher = getValue.ShortenKeyword(publisher, 18);

            for (int count = 0; count < title.Count - 1 ; count++)
            {
                Console.SetCursorPosition(10, Console.CursorTop);
                Console.Write(bookTitle[count + 1]);
                Console.SetCursorPosition(73, Console.CursorTop);
                Console.Write(bookAuthor[count]);
                //Console.WriteLine("가격 | " + price[count].InnerText);
                //Console.WriteLine("할인 가격 | " + discount[count].InnerText);
                Console.SetCursorPosition(96, Console.CursorTop);
                Console.Write(bookPublisher[count]);
                Console.SetCursorPosition(115, Console.CursorTop);
                Console.Write(pubdate[count].InnerText);
                Console.SetCursorPosition(126, Console.CursorTop);
                Console.WriteLine(isbn[count].InnerText);
                //Console.WriteLine("소개 | " + description[count + 1].InnerText+"\n");
            }

            tool.WaitUntilGetEscapeKey();
        }
    }
}
