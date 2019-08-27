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
        /// set the API key
        /// </summary>
        /// <param name="key"></param>
        void SetAPIKey(string key);

    }
}
