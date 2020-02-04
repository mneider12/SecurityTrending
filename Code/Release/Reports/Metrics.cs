using Core;
using Database;
using Model;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Reports
{
    public static class Metrics
    {
        public static decimal PortfolioValue(IDatabase database)
        {
            Contract.Requires(database != null);

            decimal value = 0m;

            List<Position> positions = database.GetPositions();
            foreach (Position position in positions)
            {
                value += PositionValue(database, position);
            }

            return value;
        }
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
        /// calculate the value of a single position
        /// </summary>
        /// <param name="database"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static decimal PositionValue(IDatabase database, Position position)
        {
            Contract.Requires(database != null);
            Contract.Requires(position != null);

            Quote lastQuote = database.GetLastQuote(position.Symbol);

            return lastQuote.Price * position.Quantity;
        }
        public static decimal PositionValue(Position position, IQuoteFeed quoteFeed)
        {
            Contract.Requires(position != null);
            Contract.Requires(quoteFeed != null);

            Quote lastQuote = quoteFeed.GetQuote(position.Symbol);

            return lastQuote.Price * position.Quantity;
        }
    }
}
