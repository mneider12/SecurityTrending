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
        /// get the latest quote for a ticker
        /// </summary>
        /// <param name="ticker">security ticker</param>
        /// <returns>quote</returns>
        Quote GetQuote(string ticker);
    }
}
