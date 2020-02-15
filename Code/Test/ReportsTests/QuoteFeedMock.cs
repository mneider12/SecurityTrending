using Core;
using Reports;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportsTests
{
    /// <summary>
    /// Mock quote feed for unit testing
    /// </summary>
    public class QuoteFeedMock: IQuoteFeed
    {
        /// <summary>
        /// create the mock
        /// </summary>
        public QuoteFeedMock()
        {
            quotes = new Dictionary<string, Quote>();
        }
        /// <summary>
        /// get a quote
        /// </summary>
        /// <param name="symbol">symbol</param>
        /// <returns>quote</returns>
        public Quote GetQuote(string symbol)
        {
            if (quotes.ContainsKey(symbol))
            {
                return quotes[symbol];
            }
            else
            {
                string message = string.Format(CultureInfo.CurrentCulture, "No quote provider for {0}", symbol);
                throw new NoQuoteAvailableException(message);
            }
        }
        /// <summary>
        /// add a quote to be returned
        /// </summary>
        /// <param name="quote">quote</param>
        public void AddQuote(Quote quote)
        {
            quotes.Add(quote.Symbol, quote);
        }
        /// <summary>
        /// keep a list of quotes to return
        /// </summary>
        private readonly Dictionary<string, Quote> quotes;
    }
}
