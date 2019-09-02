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
        /// get the latest quote for a symbol
        /// </summary>
        /// <param name="symbol">security symbol</param>
        /// <returns>quote</returns>
        Quote GetQuote(string symbol);
    }
}
