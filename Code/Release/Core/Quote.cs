using System;

namespace Core
{
    /// <summary>
    /// historical data for a security
    /// </summary>
    public class Quote
    {
        #region public properties
        /// <summary>
        /// price
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// date of quote
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// security symbol
        /// </summary>
        public string Symbol { get; set; }
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
            return Price.GetHashCode() + Date.GetHashCode() + Symbol.GetHashCode();
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
            return other != null && Price == other.Price && Date.Equals(other.Date) && Symbol == other.Symbol;
        }
        #endregion
    }
}
