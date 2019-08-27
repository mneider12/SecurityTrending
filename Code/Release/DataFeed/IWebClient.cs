using System;
using System.Collections.Generic;
using System.Text;

namespace DataFeed
{
    /// <summary>
    /// interface for WebClient members that we need to access, so that we can replace calls for unit tests
    /// </summary>
    public interface IWebClient: IDisposable
    {
        /// <summary>
        /// download a response from a URI
        /// </summary>
        /// <param name="address">web address to download from</param>
        /// <returns>response</returns>
        string DownloadString(string address);
    }
}
