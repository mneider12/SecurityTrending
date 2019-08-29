using System.Runtime.Serialization;

namespace DataFeed
{
    /// <summary>
    /// a quote response from Alpah Vantage for the global quote function
    /// </summary>
    [DataContract]
    public class AlphaVantageGlobalQuoteResponse
    {
        /// <summary>
        /// The global quote data
        /// </summary>
        [DataMember(Name = "Global Quote")]
        public AlphaVantageGlobalQuote GlobalQuote { get; set; }
    }
}
