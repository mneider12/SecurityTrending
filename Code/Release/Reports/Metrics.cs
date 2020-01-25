using Database;
using Model;
using System;
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
            foreach(Position position in positions)
            {
                value += PositionValue(database, position);
            }

            return value;
        }

        public static decimal PositionValue(IDatabase database, Position position)
        {
            Contract.Requires(database != null);
            Contract.Requires(position != null);

            decimal price = 0;

            return 0m;
        }
    }
}
