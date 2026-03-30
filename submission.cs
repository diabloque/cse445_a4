using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;



/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{


    public class Submission
    {
        public static string xmlURL = "https://raw.githubusercontent.com/diabloque/cse445_a4/main/NationalParks.xml";
        public static string xmlErrorURL = "https://raw.githubusercontent.com/diabloque/cse445_a4/main/NationalParksErrors.xml";
        public static string xsdURL = "https://raw.githubusercontent.com/diabloque/cse445_a4/main/NationalParks.xsd";
        private static string errorText = "";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);


            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);


            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            string output = "No errors are found";
            errorText = "";

            try
            {
                string xsdData = DownloadContent(xsdUrl);
                string xmlData = DownloadContent(xmlUrl);

                XmlSchemaSet schemas = new XmlSchemaSet();
                XmlReader schemaReader = XmlReader.Create(new StringReader(xsdData));
                schemas.Add(null, schemaReader);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas = schemas;
                settings.ValidationEventHandler += ValidationCallBack;

                XmlReader reader = XmlReader.Create(new StringReader(xmlData), settings);
                using (reader)
                {
                    while (reader.Read())
                    {
                    }
                }
            }
            catch (Exception e)
            {
                errorText = errorText + e.Message;
            }

            if (errorText.Length > 0)
            {
                output = errorText;
            }

            return output;
        }

        // Q2.2
        public static string Xml2Json(string xmlUrl)
        {
            string xml = DownloadContent(xmlUrl);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string jsonData = JsonConvert.SerializeXmlNode(doc);
            // The returned jsonText needs to be de-serializable by Newtonsoft.Json package. (JsonConvert.DeserializeXmlNode(jsonText))
            return jsonData;
        }

        
        private static string DownloadContent(string url)
        {
            using (System.Net.WebClient web = new System.Net.WebClient())
            {
                return web.DownloadString(url);
            }
        }

        private static void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            errorText = errorText + e.Message + "\n";
        }
    }

}
