using System;
using System.Collections.Generic;
using System.Text;

namespace DataFeed
{
    /// <summary>
    /// combined interface for a quote feed with a single API key
    /// </summary>
    public interface IAPIKeyQuoteFeed : IAPIKey, IQuoteFeed { }
}
