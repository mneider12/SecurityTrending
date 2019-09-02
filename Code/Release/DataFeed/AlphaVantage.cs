using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
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
        /// <returns>quote</returns>
        public Quote GetQuote(string ticker)
        {
            string requesturl = string.Format("https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={0}&apikey={1}",ticker, APIKey);
            string jsonResponse = WebClient.DownloadString(requesturl);
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AlphaVantageGlobalQuoteResponse));
            AlphaVantageGlobalQuoteResponse response;
            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(jsonResponse)))
            {
                response = (AlphaVantageGlobalQuoteResponse)serializer.ReadObject(stream);
            }

            DateTime.TryParse(response.GlobalQuote.LatestTradingDay, out DateTime date);

            return new Quote()
            {
                Price = response.GlobalQuote.Price,
                Date = date,
                Symbol = response.GlobalQuote.Symbol,
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
