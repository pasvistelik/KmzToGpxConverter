using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace KmzToGpxConverter
{
    class Converter
    {
        private Converter(string filepath, string filename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath + "\\" + filename);

            XmlNodeList elemList = doc.GetElementsByTagName("coordinates");
            string coordsStr = elemList[0].InnerText;

            StringBuilder builder = new StringBuilder();

            Regex regex = new Regex("[0-9]+.[0-9]+,[0-9]+.[0-9]+,[0-9]+");
            foreach (Match m in regex.Matches(coordsStr))
            {
                string s = m.ToString();
                if (s == null) continue;
                string[] tmp = s.Split(',');

                string lat = tmp[1];
                string lon = tmp[0];
                string height = tmp[2];

                string pointXml = "<trkpt lat=\""+lat+"\" lon=\""+lon+"\"></trkpt>";
                builder.Append(pointXml);
            }


            XmlDocument template = new XmlDocument();
            doc.Load("template.gpx");
            doc.GetElementsByTagName("trkseg")[0].InnerXml = builder.ToString();

            DirectoryInfo outFilePath = Directory.CreateDirectory(filepath);
            string outFilename = filepath + "\\" + filename + ".gpx";
            doc.Save(outFilename);

            GC.Collect();
        }
        public static void Convert(string filepath, string filename)
        {
            Converter converter = new Converter(filepath, filename);
        }
    }
}
