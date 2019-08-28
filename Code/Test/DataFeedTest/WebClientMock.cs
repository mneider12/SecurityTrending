﻿using DataFeed;
using System.Collections.Generic;

namespace DataFeedTest
{
    /// <summary>
    /// mock class for WebClient
    /// </summary>
    public class WebClientMock : IWebClient
    {
        /// <summary>
        /// don't need to dispose anything in mock
        /// </summary>
        public void Dispose() { }
        /// <summary>
        /// return from a dictionary of set responses
        /// </summary>
        /// <param name="address">address, should have an entry in Responses</param>
        /// <returns></returns>
        public string DownloadString(string address)
        {
            return Responses[address];
        }
        /// <summary>
        /// set responses that should be returned by the mock
        /// </summary>
        public Dictionary<string, string> Responses { private get; set; }
    }
}