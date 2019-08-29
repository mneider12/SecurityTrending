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
        #region public properties
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
        #endregion
        #region Object overrides
        /// <summary>
        /// determine if two quotes are equal
        /// </summary>
        /// <param name="obj">other quote to compare to</param>
        /// <returns>whether the quotes are equal</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Quote);
        }
        /// <summary>
        /// get the hash code for the quote
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Close.GetHashCode() + Date.GetHashCode() + Ticker.GetHashCode();
        }
        #endregion
        #region private methods
        /// <summary>
        /// determine if two quotes are equal
        /// </summary>
        /// <param name="other">quote to compare to</param>
        /// <returns>whether the quotes are equal</returns>
        private bool Equals(Quote other)
        {
            return other != null && Close == other.Close && Date.Equals(other.Date) && Ticker == other.Ticker;
        }
        #endregion
    }
}
