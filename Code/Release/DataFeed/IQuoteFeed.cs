using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataFeed
{
    /// <summary>
    /// Interface for any datafeed capable of daily quotes
    /// </summary>
    public interface IQuoteFeed
    {
        /// <summary>
        /// get a quote for a ticker and date
        /// </summary>
        /// <param name="ticker">security ticker</param>
        /// <param name="date">date of quote</param>
        /// <returns>quote</returns>
        Quote GetQuote(string ticker, DateTime date);
    }
}
