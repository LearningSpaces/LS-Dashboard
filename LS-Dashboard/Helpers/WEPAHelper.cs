using HtmlAgilityPack;
using LS_Dashboard.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace LS_Dashboard.Helpers
{
    public static class WEPAHelper
    {
        public static List<WEPAModel> GetWEPAStatus(byte filter)
        {
            HttpWebRequest request = WebRequest.CreateHttp("http://cs.wepanow.com/000ou148.html");
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string html = reader.ReadToEnd();
                    var results = ParseResponse(html);
                    return results.Where(k => ((byte) k.Filter & filter) != 0).ToList();
                }

            }
            catch (Exception e)
            {
                //Something bad happened. Log Results
                //Logger.Med(e.ToString());
            }
            return new List<WEPAModel>();
        }

        private static List<WEPAModel> ParseResponse(string response)
        {
            var doc = new HtmlDocument();
            var result = new List<WEPAModel>();
            WEPAModel.Filters filter;
            doc.LoadHtml(response);
            var rows = doc.DocumentNode.Descendants("tr").Where(r => r.ParentNode.ParentNode.Attributes.Contains("class") &&
                r.ParentNode.ParentNode.Attributes["class"].Value.Contains("ps-table") &&
                r.Attributes.Contains("class") &&
                !r.Attributes["class"].Value.Contains("small-row"));
            foreach (var row in rows)
            {
                filter = 0;
                if (row.Attributes.Contains("class"))
                {
                    if (row.Attributes["class"].Value.Contains("green"))
                    {
                        filter = WEPAModel.Filters.GREEN;
                    }
                    else if (row.Attributes["class"].Value.Contains("yellow"))
                    {
                        filter = WEPAModel.Filters.YELLOW;
                    }
                    else if (row.Attributes["class"].Value.Contains("red"))
                    {
                        filter = WEPAModel.Filters.RED;
                    }
                }
                result.Add(new WEPAModel()
                {
                    PSNumber = row.ChildNodes.ElementAtOrDefault(1).InnerText,
                    Description = row.ChildNodes.ElementAtOrDefault(2).InnerText,
                    Message = row.ChildNodes.ElementAtOrDefault(3).InnerText,
                    PrinterText = row.ChildNodes.ElementAtOrDefault(4).InnerText,
                    Toner = new WEPAModel.PrintColors()
                    {
                        Black = Byte.Parse(row.ChildNodes.ElementAtOrDefault(5).InnerText),
                        Cyan = Byte.Parse(row.ChildNodes.ElementAtOrDefault(6).InnerText),
                        Magenta = Byte.Parse(row.ChildNodes.ElementAtOrDefault(7).InnerText),
                        Yellow = Byte.Parse(row.ChildNodes.ElementAtOrDefault(8).InnerText)
                    },
                    Drum = new WEPAModel.PrintColors()
                    {
                        Black = Byte.Parse(row.ChildNodes.ElementAtOrDefault(9).InnerText),
                        Cyan = Byte.Parse(row.ChildNodes.ElementAtOrDefault(10).InnerText),
                        Magenta = Byte.Parse(row.ChildNodes.ElementAtOrDefault(11).InnerText),
                        Yellow = Byte.Parse(row.ChildNodes.ElementAtOrDefault(12).InnerText)
                    },
                    Belt = Byte.Parse(row.ChildNodes.ElementAtOrDefault(13).InnerText),
                    Fuser = Byte.Parse(row.ChildNodes.ElementAtOrDefault(14).InnerText),
                    Filter = filter
                });
            }
            return result;
        }
    }
}