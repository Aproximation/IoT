using System;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace MirrorApp
{
    public static class Weather
    {
        private const string serviceUrl = "http://weather.yahooapis.com/forecastrss?w=523920&u=c"; //523920 is Warsaw WOEID
        public static string IconUrl { get; private set; }
        public static string WindSpeed{ get; private set; }
        public static string  CityName{ get; private set; }
        public static string WeatherText{ get; private set; }
        public static string Temperature{ get; private set; }

        public static async Task<string> UpdateData() {
            try
            {
                var xmlDoc = await GetData();

                FillData(xmlDoc);
                return "ok";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        
        public static void FillData(XmlDocument weatherXml)
        {
            var channelNode = weatherXml.DocumentElement["channel"];
            CityName = channelNode["yweather:location"].GetAttribute("city");
            WindSpeed = channelNode["yweather:wind"].GetAttribute("speed");
            Temperature = channelNode["item"]["yweather:condition"].GetAttribute("temp");
            var cdata = channelNode["item"]["description"].InnerText;
            var splitted = cdata.Split('\"');
            IconUrl = splitted[1];
            WeatherText = channelNode["item"]["yweather:condition"].GetAttribute("text");
        }

        private static async Task<XmlDocument> GetData()
        {
            HttpWebRequest request = WebRequest.Create(serviceUrl) as HttpWebRequest;
            WebResponse response = await request.GetResponseAsync();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(response.GetResponseStream());
            return (xmlDoc);
        }
    }
}
