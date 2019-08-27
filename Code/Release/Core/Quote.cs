using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    /// <summary>
    /// historical data for a security
    /// </summary>
    public class Quote
    {
        /// <summary>
        /// closing price
        /// </summary>
        public decimal Close { get; set; }
        /// <summary>
        /// date of quote
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// security ticker
        /// </summary>
        public string Ticker { get; set; }
    }
}
