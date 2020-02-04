using Core;
using Database;
using Model;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

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
    }
}
