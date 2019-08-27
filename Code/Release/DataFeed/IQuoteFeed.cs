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
        /// <param name="date">date of quote</param>
        /// <param name="ticker">security ticker</param>
        /// <returns>quote</returns>
        Quote GetQuote(DateTime date, string ticker);
    }
}
