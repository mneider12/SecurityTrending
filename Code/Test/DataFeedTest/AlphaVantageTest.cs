using System;
using System.Collections.Generic;
using Core;
using DataFeed;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataFeedTest
{
    /// <summary>
    /// test the AlphaVantage class
    /// </summary>
    [TestClass]
    public class AlphaVantageTest
    {
        /// <summary>
        /// test the get quote API
        /// </summary>
        [TestMethod]
        public void GetQuoteTest()
        {
            Dictionary<string, string> responses = new Dictionary<string, string>()
            {
                {   
                    "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=MSFT&apikey=key",
                        @"{
                            ""Global Quote"": {
                                ""01. symbol"": ""MSFT"",
                                ""02. open"": ""134.8800"",
                                ""03. high"": ""135.7600"",
                                ""04. low"": ""133.5500"",
                                ""05. price"": ""135.5600"",
                                ""06. volume"": ""16321151"",
                                ""07. latest trading day"": ""2019-08-28"",
                                ""08. previous close"": ""135.7400"",
                                ""09. change"": ""-0.1800"",
                                ""10. change percent"": ""-0.1326%""
                            }
                        }"
                },
            };

            string ticker = "MSFT";

            Quote expectedQuote = new Quote()
            {
                Close = 135.5600m,
                Date = new DateTime(2019, 8, 28),
                Ticker = ticker,
            };

            IQuoteFeed quoteFeed;

            using (WebClientMock webClient = new WebClientMock() { Responses = responses })
            {
                quoteFeed = new AlphaVantage()
                {
                    APIKey = "key",
                    WebClient = webClient,
                };
            }

            Quote quote = quoteFeed.GetQuote(ticker);

            Assert.AreEqual(expectedQuote, quote);
        }
    }
}
