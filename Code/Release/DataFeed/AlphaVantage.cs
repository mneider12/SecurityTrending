using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using Core;

namespace DataFeed
{
    /// <summary>
    /// access to the Alpha Vantage APIs
    /// </summary>
    public class AlphaVantage : IAPIKeyQuoteFeed
    {
        /// <summary>
        /// get a quote for a ticker and date
        /// </summary>
        /// <param name="ticker">security ticker</param>
        /// <param name="date">date of quote</param>
        /// <returns>quote</returns>
        public Quote GetQuote(string ticker, DateTime date)
        {
            string requesturl = string.Format("https://www.alphavantage.co/qurey?function=TIME_SERIES_DAILY&symbol={0}&apikey={1}",ticker, APIKey);
            string jsonResponse;

            using (WebClient webClient = new WebClient())
            {
                jsonResponse = webClient.DownloadString(requesturl);
            }

            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        private void LoadAPIKey()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(string));
            using (TextReader reader = new StreamReader(config_file))
            {
                APIKey = (string) serializer.Deserialize(reader);
            }
        }
        /// <summary>
        /// save the API Key to the database
        /// </summary>
        private void SaveAPIKey()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(string));
            using (TextWriter writer = new StreamWriter(config_file))
            {
                serializer.Serialize(writer, APIKey);
            }
        }
        /// <summary>
        /// API Key
        /// </summary>
        public string APIKey
        {
            private get
            {
                if (apiKey == null)
                {
                    LoadAPIKey();
                }

                return apiKey;
            }
            set
            {
                apiKey = value;
                SaveAPIKey();
            }
        }
        /// <summary>
        /// APIKey property backing
        /// </summary>
        private string apiKey;
        /// <summary>
        /// config file name
        /// </summary>
        private const string config_file = "Alpha_Vantage_Config.ser";
    }
}
