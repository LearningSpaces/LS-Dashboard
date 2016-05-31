using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Apis;
using System.Net;
using System.IO;
using LS_Dashboard.Models;

namespace LS_Dashboard.Helpers
{
    public static class GoogleDriveHelper
    {
        public static List<ItemModel> GetItemCheck()
        {

            List<List<string>> itemList = new List<List<string>>();

            string URL = @"https://docs.google.com/spreadsheets/d/1QMZTBl2K40P4ULrUUWovP5f6qCCqfc5-hSyCqI1bNJc/pub?gid=2054336677&single=true&output=csv";

            HttpWebRequest request = WebRequest.CreateHttp(URL);

            request.Credentials = CredentialCache.DefaultCredentials;

            WebResponse response = request.GetResponse();

            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            string line;

            while ((line = reader.ReadLine()) != null)
            {
                string[] temp = line.Split(',');
                List<string> sublist = new List<string>();

                for (int i = 0; i < temp.Length; i++)
                {
                    sublist.Add(temp[i]);
                }

                itemList.Add(sublist);
            }
              

            reader.Close();
            response.Close();

            var results = new List<ItemModel>();
            foreach (var row in itemList.Skip(1))
            {
                results.Add(new ItemModel()
                {
                    Name = row[0],
                    ItemStatus = row[3],
                    CheckoutInfo = new ItemModel.CheckoutData()
                    {
                        Name = row[1]
                    }
                });
            }

            return results;
        }

        public static List<TechCheckSector> GetTechCheck()
        {

            List<List<string>> techList = new List<List<string>>();

            string URL = @"https://docs.google.com/spreadsheets/d/1HCHTXuvch1iBDLUDeHaYIdEOEj_8HVZSFsv6mw_C220/pub?gid=2047447980&single=true&output=csv";

            HttpWebRequest request = WebRequest.CreateHttp(URL);

            request.Credentials = CredentialCache.DefaultCredentials;

            WebResponse response = request.GetResponse();

            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            string line;

            while ((line = reader.ReadLine()) != null)
            {
                string[] temp = line.Split(',');
                List<string> sublist = new List<string>();

                for (int i = 0; i < temp.Length; i++)
                {
                    sublist.Add(temp[i]);
                }

                techList.Add(sublist);
            }


            reader.Close();
            response.Close();

            TechCheckSector Sector;
            var results = new List<TechCheckSector>();
            techList[0].RemoveAll(cell => cell == "");
            for (var i = 0; i < techList[0].Count; i++)
            {
                if(techList[0][i] == "")
                {
                    continue;
                }
                Sector = new TechCheckSector()
                {
                    Name = techList[0][i],
                    Labs = new List<TechCheckModel>()
                };

                foreach (var row in techList.Skip(1))
                {
                    if (techList[0][i] == "" || row[i] == "")
                    {
                        continue;
                    }
                    try
                    {
                        Sector.Labs.Add(new TechCheckModel()
                        {
                            Name = row[i],
                            LocationStatus = Convert.ToBoolean(row[i + techList[0].Count])
                        });
                    }
                    catch(FormatException fe)
                    {
                        Sector.Labs.Add(new TechCheckModel()
                        {
                            Name = row[i],
                            LocationStatus = false
                        });
                    }

                }

                results.Add(Sector);
            }

            return results;

        }

        public static List<CartModel> GetVehicleCheck()
        {

            List<List<string>> vehicleList = new List<List<string>>();

            string URL = @"https://docs.google.com/spreadsheets/d/1VBipdd7l2mvqUeXEw02krMh8L_F0dUZCNXLCUubuj6o/pub?gid=1908307120&single=true&output=csv";

            HttpWebRequest request = WebRequest.CreateHttp(URL);

            request.Credentials = CredentialCache.DefaultCredentials;

            WebResponse response = request.GetResponse();

            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            string line;

            while ((line = reader.ReadLine()) != null)
            {
                string[] temp = line.Split(',');
                List<string> sublist = new List<string>();

                for (int i = 0; i < temp.Length; i++)
                {
                    sublist.Add(temp[i]);
                }

                vehicleList.Add(sublist);
            }


            reader.Close();
            response.Close();

            var results = new List<CartModel>();
            foreach(var row in vehicleList)
            {
                results.Add(new CartModel()
                {
                    Name = row[0],
                    CartStatus = row[4],
                    CheckoutInfo = new CartModel.CheckoutData()
                    {
                        Name = row[1],
                        Number = row[2],
                        Section = row[3]
                    }
                });
            }
            return results;
        }

    }
}