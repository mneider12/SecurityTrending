using Core;
using Model;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Reports
{
    /// <summary>
    /// metrics to report
    /// </summary>
    public static class Metrics
    {
        /// <summary>
        /// get the value of an account
        /// </summary>
        /// <param name="account">account</param>
        /// <param name="quoteFeed">quote feed</param>
        /// <returns>value</returns>
        public static decimal AccountValue(Account account, IQuoteFeed quoteFeed)
        {
            Contract.Requires(account?.Positions != null);

            return account.Positions.Values.Aggregate(0m, (total, position) => total + PositionValue(position, quoteFeed));
        }
        /// <summary>
        /// get the value of a single position
        /// </summary>
        /// <param name="position">position</param>
        /// <param name="quoteFeed">quote feed</param>
        /// <returns>value</returns>
        public static decimal PositionValue(Position position, IQuoteFeed quoteFeed)
        {
            Contract.Requires(position != null);
            Contract.Requires(quoteFeed != null);

            Quote lastQuote = quoteFeed.GetQuote(position.Symbol);

            return lastQuote.Price * position.Quantity;
        }
    }
}
