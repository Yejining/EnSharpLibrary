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

namespace EnSharpLibrary.Function
{
    class ConnectDatabase
    {
        public void ConnectAPI(string searchingKeyword, int displayCount)
        {
            string url = Constant.URL + searchingKeyword + "&target=book&display=" + displayCount;
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
                Console.WriteLine(text);
                SaveBookToDatabase(bookInformation);
                
            }
            else
            {
                Console.WriteLine("Error 발생=" + status);
            }
        }
        
        public void SaveBookToDatabase(XmlDocument bookInformation)
        {
            XmlNodeList authors = bookInformation.GetElementsByTagName("discount");
            foreach(XmlNode author in authors) 
            Console.WriteLine("author = " + author.InnerText);

            
        }
    }
}
