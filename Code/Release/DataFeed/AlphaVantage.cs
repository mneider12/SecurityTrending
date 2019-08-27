using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
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
            string APIKey = GetAPIKey();
            string requesturl = string.Format("https://www.alphavantage.co/qurey?function=TIME_SERIES_DAILY&symbol={0}&apikey={1}",ticker, APIKey);
            string jsonResponse;

            using (WebClient webClient = new WebClient())
            {
                jsonResponse = webClient.DownloadString(requesturl);
            }

            return null;
        }

        public void SetAPIKey(string key)
        {

        }

        private string GetAPIKey()
        {
            return "";
        }
    }
}
