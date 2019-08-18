using System;
using static Core.TransactionEnums;

namespace Core
{
    /// <summary>
    /// Represents a single interaction with the cash position of a portfolio
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Unique transaction ID
        /// </summary>
        public int TransactionID { get; set; }
        /// <summary>
        /// date transaction occured
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// interaction of transaction with the cash account
        /// </summary>
        public TransactionAction Action { get; set; }
        /// <summary>
        /// additional type information about the transaction
        /// </summary>
        public TransactionClass Class { get; set; }
        /// <summary>
        /// ticker associated with the transaction
        /// </summary>
        public string Ticker { get; set; }
        /// <summary>
        /// amount of the transaction
        /// </summary>
        public decimal Amount { get; set; }
    }
}
