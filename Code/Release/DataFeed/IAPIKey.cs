using System;
using System.Collections.Generic;
using System.Text;

namespace DataFeed
{
    /// <summary>
    /// data feed with a single API key for security
    /// </summary>
    public interface IAPIKey
    {
        /// <summary>
        /// API Key
        /// </summary>
        /// <param name="APIKey">API key</param>
        void SetAPIKey(string APIKey);
    }
}
