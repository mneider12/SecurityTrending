using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reports
{
    /// <summary>
    /// Provide access to quotes
    /// </summary>
    public interface IQuoteFeed
    {
        /// <summary>
        /// lookup a quote
        /// </summary>
        /// <param name="Symbol">symbol to get rice for</param>
        /// <returns>quote</returns>
        Quote GetQuote(string Symbol);
    }
}
