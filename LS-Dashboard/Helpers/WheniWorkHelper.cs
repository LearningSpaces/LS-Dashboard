using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Apis;
using System.Net;
using System.IO;
using LS_Dashboard.Models;
using System.Text;
using Newtonsoft.Json.Linq;

namespace LS_Dashboard.Helpers
{
    public static class WhenIWorkHelper
    {
        const string URL = @"https://api.wheniwork.com/";
        const string Username = "verbick@ou.edu";
        const string Password = "whenipassword";
        const string APIKEY = "23588245629b429c427b1dad455c72581260b56c";
        private static List<WhenIWorkUserModel> Users = new List<WhenIWorkUserModel>();
        private static WhenIWorkLoginModel LoginInfo = null;

        public static WhenIWorkUserModel GetUser(int id)
        {
            if (Users.Count == 0)
            {
                GetAllUsers();
            }

            return Users.SingleOrDefault(u => u.ID == id);
        }

        public static List<WhenIWorkUserModel> GetUsers(List<int> ids)
        {
            var results = new List<WhenIWorkUserModel>();
            foreach (var id in ids)
            {
                results.Add(GetUser(id));
            }

            return results;
        }

        private static void GetAllUsers()
        {
            var request = WebRequest.CreateHttp(URL + "2/users");
            var credentials = GetLoginInfo();
            request.Headers.Add("W-Token:" + credentials.Token);

            var response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string Json = reader.ReadToEnd();

            reader.Close();
            dataStream.Close();

            JObject results = JObject.Parse(Json);
            Users = results["users"].ToObject<List<WhenIWorkUserModel>>();
        }

        private static WhenIWorkLoginModel GetLoginInfo()
        {
            if (LoginInfo != null)
            {
                return LoginInfo;
            }

            var request = WebRequest.CreateHttp(URL + "2/login");

            var data = @"{""username"":""" + Username + @""", ""password"":""" + Password + @"""}";
            request.Headers.Add("W-Key:" + APIKEY);
            request.Method = "Post";
            var dataBytes = Encoding.ASCII.GetBytes(data);
            request.ContentLength = dataBytes.Length;
            request.ContentType = "application/json";
            var stream = request.GetRequestStream();
            stream.Write(dataBytes, 0, dataBytes.Length);
            stream.Close();

            var response = request.GetResponse();

            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            string Json = reader.ReadToEnd();

            JObject results = JObject.Parse(Json);

            var token = (string) results["login"]["token"];

            var id = (int)results["accounts"][0]["id"];

            LoginInfo = new WhenIWorkLoginModel()
            {
                Token = token,
                AccountId = id
            };

            return LoginInfo;
        }

        public static List<ShiftModel> GetShifts(int shift)
        {
            string StartTime = GetStartTime(shift);
            string EndTime = GetEndTime(shift);
            var request = WebRequest.CreateHttp(URL + "2/shifts?include_objects[]=users&start=" + StartTime + "&end=" + EndTime);
            var credentials = GetLoginInfo();
            request.Headers.Add("W-Token:" + credentials.Token);

            var response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string Json = reader.ReadToEnd();

            reader.Close();
            dataStream.Close();

            JObject results = JObject.Parse(Json);
            var shifts = results["shifts"].ToObject<List<ShiftModel>>();

            return shifts;
        }

        public static List<ShiftModel> GetShifts(DateTime Time)
        {
            return GetShifts(GetShiftFromTime(Time));
        }

        private static string GetStartTime(int shift)
        {
            switch (shift)
            {
                case 0:
                    return DateTime.Parse("7:30").ToString("yyyy-MM-dd HH:mm:ss");
                case 1:
                    return DateTime.Parse("10:00").ToString("yyyy-MM-dd HH:mm:ss");
                case 2:
                    return DateTime.Parse("12:00").ToString("yyyy-MM-dd HH:mm:ss");
                case 3:
                    return DateTime.Parse("14:00").ToString("yyyy-MM-dd HH:mm:ss");
                case 4:
                    return DateTime.Parse("16:00").ToString("yyyy-MM-dd HH:mm:ss");
                case 5:
                    return DateTime.Parse("18:00").ToString("yyyy-MM-dd HH:mm:ss");
                default:
                    return null; 


            }
        }

        private static string GetEndTime(int shift)
        {
            switch (shift)
            {
                case 0:
                    return DateTime.Parse("10:30").ToString("yyyy-MM-dd HH:mm:ss");
                case 1:
                    return DateTime.Parse("12:30").ToString("yyyy-MM-dd HH:mm:ss");
                case 2:
                    return DateTime.Parse("14:30").ToString("yyyy-MM-dd HH:mm:ss");
                case 3:
                    return DateTime.Parse("16:30").ToString("yyyy-MM-dd HH:mm:ss");
                case 4:
                    return DateTime.Parse("18:30").ToString("yyyy-MM-dd HH:mm:ss");
                case 5:
                    return DateTime.Parse("20:30").ToString("yyyy-MM-dd HH:mm:ss");
                default:
                    return null;
            }

           
        }

        public static int GetShiftFromTime(DateTime Time)
        {
            double TimeMinutes = (Time - DateTime.Today).TotalMinutes;

            if (TimeMinutes >= 7.5 * 60 && TimeMinutes < 10.5 * 60 - 1)
                return 0;
            else if (TimeMinutes >= 10.5 * 60 && TimeMinutes < 12.5 * 60 - 1)
                return 1;
            else if (TimeMinutes >= 12.5 * 60 && TimeMinutes < 14.5 * 60 - 1)
                return 2;
            else if (TimeMinutes >= 14.5 * 60 && TimeMinutes < 16.5 * 60 - 1)
                return 3;
            else if (TimeMinutes >= 16.5 * 60 && TimeMinutes < 18.5 * 60 - 1)
                return 4;
            else if (TimeMinutes >= 18.5 * 60 && TimeMinutes < 20.5 * 60 - 1)
                return 5;
            else
                return -1;
        }
    }
}