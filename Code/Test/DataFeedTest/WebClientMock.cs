using DataFeed;
using System;
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
        public void Dispose() 
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// part of the dispose pattern. Finalizer can call this with disposing as false
        /// </summary>
        /// <param name="disposing">whether called from dispose (true) or finalizer (false)</param>
        protected virtual void Dispose(bool disposing) { }
        /// <summary>
        /// destructor
        /// </summary>
        ~WebClientMock()
        {
            Dispose(false);
        }
        /// <summary>
        /// return from a dictionary of set responses
        /// </summary>
        /// <param name="address">address, should have an entry in Responses</param>
        /// <returns></returns>
        public string DownloadString(string address)
        {
            return responses[address];
        }
        /// <summary>
        /// set responses that should be returned by the mock
        /// </summary>
        public void SetResponses(Dictionary<string, string> responses)
        {
            this.responses = responses;
        }

        private Dictionary<string, string> responses;
    }
}
