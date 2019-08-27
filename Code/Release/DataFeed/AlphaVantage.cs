using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using Core;
using Newtonsoft.Json.Linq;

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
        public Quote GetQuote(DateTime date, string ticker)
        {
            string requesturl = string.Format("https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={0}&apikey={1}",ticker, APIKey);
            string jsonResponse = WebClient.DownloadString(requesturl);
            JObject jObject = JObject.Parse(jsonResponse);
            Dictionary<string, Dictionary<string, string>> timeSeries = jObject["Time Series (Daily)"].ToObject<Dictionary<string, Dictionary<string, string>>>();
            decimal.TryParse(timeSeries[date.ToString("yyyy-MM-dd")]["4. close"], out decimal close);

            return new Quote()
            {
                Close = close,
            };
        }
        /// <summary>
        /// load the API key from disk
        /// </summary>
        private void LoadAPIKey()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(string));
            using (TextReader reader = new StreamReader(config_file))
            {
                apiKey = (string) serializer.Deserialize(reader);
            }
        }
        /// <summary>
        /// save the API Key to disk
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
        /// web client to use for web access
        /// </summary>
        public IWebClient WebClient { private get; set; }
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
