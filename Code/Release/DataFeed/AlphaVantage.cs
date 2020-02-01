using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
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
        /// create an instance of AlphaVantage
        /// </summary>
        /// <param name="webClient">client to use for web access</param>
        public AlphaVantage(IWebClient webClient)
        {
            this.webClient = webClient;
            LoadAPIKey();
        }
        /// <summary>
        /// get a quote for a ticker and date
        /// </summary>
        /// <param name="ticker">security ticker</param>
        /// <returns>quote</returns>
        public Quote GetQuote(string ticker)
        {
            string requesturl = string.Format(CultureInfo.InvariantCulture, "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={0}&apikey={1}",ticker, APIKey);
            string jsonResponse = webClient.DownloadString(requesturl);
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AlphaVantageGlobalQuoteResponse));
            AlphaVantageGlobalQuoteResponse response;
            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(jsonResponse)))
            {
                response = (AlphaVantageGlobalQuoteResponse)serializer.ReadObject(stream);
            }

            DateTime date = DateTime.Parse(response.GlobalQuote.LatestTradingDay, CultureInfo.InvariantCulture);

            return new Quote()
            {
                Price = response.GlobalQuote.Price,
                Date = date,
                Symbol = response.GlobalQuote.Symbol,
            };
        }
        public void SetAPIKey(string APIKey)
        {
            this.APIKey = APIKey;
            SaveAPIKey();
        }
        /// <summary>
        /// load the API key from disk
        /// </summary>
        private void LoadAPIKey()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(string));
            using (FileStream stream = new FileStream(config_file, FileMode.Open))
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    APIKey = (string)serializer.Deserialize(reader);
                }
            }
        }
        /// <summary>
        /// save the API Key to disk
        /// </summary>
        /// <param name="APIKey">API key</param>
        private  void SaveAPIKey()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(string));
            using (TextWriter writer = new StreamWriter(config_file))
            {
                serializer.Serialize(writer, APIKey);
            }
        }
        /// <summary>
        /// web client to use for web access
        /// </summary>
        private readonly IWebClient webClient;
        /// <summary>
        /// APIKey property backing
        /// </summary>
        private string APIKey;
        /// <summary>
        /// config file name
        /// </summary>
        private const string config_file = "Alpha_Vantage_Config.ser";
    }
}
