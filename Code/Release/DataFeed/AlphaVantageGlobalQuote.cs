using System.Runtime.Serialization;

namespace DataFeed
{
    /// <summary>
    /// global quote from Alpha Vantage
    /// global quote is the quote for the last trading day for a ticker
    /// </summary>
    [DataContract]
    public class AlphaVantageGlobalQuote
    {
        /// symbol / ticker
        /// </summary>
        [DataMember(Name = "01. symbol")]
        public string Symbol { get; set; }
        /// <summary>
        /// open price
        /// </summary>
        [DataMember(Name = "02. open")]
        public decimal Open { get; set; }
        /// <summary>
        /// high price
        /// </summary>
        [DataMember(Name = "03. high")]
        public decimal High { get; set; }
        /// <summary>
        /// low price
        /// </summary>
        [DataMember(Name = "04. low")]
        public decimal Low { get; set; }
        /// <summary>
        /// last price
        /// </summary>
        [DataMember(Name = "05. price")]
        public decimal Price { get; set; }
        /// <summary>
        /// shares traded
        /// </summary>
        [DataMember(Name = "06. volume")]
        public int Volume { get; set; }
        /// <summary>
        /// last day of trading
        /// </summary>
        [DataMember(Name = "07. latest trading day")]
        public string LatestTradingDay { get; set; }
        /// <summary>
        /// prior trading day close
        /// </summary>
        [DataMember(Name = "08. previous close")]
        public decimal PreviousClose { get; set; }
        /// <summary>
        /// change from previous close
        /// </summary>
        [DataMember(Name = "09. change")]
        public decimal Change { get; set; }
        /// <summary>
        /// change percent from previous close
        /// </summary>
        [DataMember(Name = "10. change percent")]
        public string ChangePercent { get; set; }
    }
}
