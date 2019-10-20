using static Core.TransactionEnums;

namespace Model
{
    /// <summary>
    /// represents an interest in a single security
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Symbol representing the position
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// Number of shares of the security
        /// </summary>
        public double Shares { get; set; }
        /// <summary>
        /// class of the position
        /// </summary>
        public TransactionClass Class { get; set; }
    }
}
