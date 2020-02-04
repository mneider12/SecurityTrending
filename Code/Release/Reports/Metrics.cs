using Core;
using Database;
using Model;
using System.Collections.Generic;
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
        /// get the value of a list of positions
        /// </summary>
        /// <param name="positions">list of positions</param>
        /// <param name="quoteFeed">quote feed</param>
        /// <returns>value</returns>
        public static decimal PositionsValue(List<Position> positions, IQuoteFeed quoteFeed)
        {
            Contract.Requires(quoteFeed != null);

            return positions.Aggregate(0m, ( total, position) => total + PositionValue(position, quoteFeed));
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
