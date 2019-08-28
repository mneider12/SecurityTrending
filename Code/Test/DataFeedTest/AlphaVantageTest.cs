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
                    "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=TEST&apikey=key",
                        @"{ ""Meta Data"": { },
                            ""Time Series (Daily)"": { 
                                ""2000-01-01"": {
                                    ""4. close"": ""100.0000"",
                                },
                                ""1999-12-31"": {
                                    ""4. close"": ""100.5000"",
                                }
                            }
                        }"
                },
            };

            using (WebClientMock webClient = new WebClientMock() { Responses = responses })
            {
                IQuoteFeed quoteFeed = new AlphaVantage()
                {
                    APIKey = "key",
                    WebClient = webClient,
                };

                DateTime date = new DateTime(1999, 12, 31);
                string ticker = "TEST";
                Quote quote = quoteFeed.GetQuote(date, ticker);

                Assert.AreEqual(100.5m, quote.Close);
            }
        }
    }
}
