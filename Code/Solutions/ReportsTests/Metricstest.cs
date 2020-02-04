using System;
using Core;
using Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Reports;

namespace ReportsTests
{
    /// <summary>
    /// test the Metrics Class
    /// </summary>
    [TestClass]
    public class MetricsTest
    {
        /// <summary>
        /// test the zero state for position value
        /// </summary>
        [TestMethod]
        public void PositionValueZero()
        {
            IDatabase database = new SQLiteDatabase();
            database.CreateDatabase();

            Position position = new Position()
            {
                Symbol = "test",
                Quantity = 0,
                Class = Core.TransactionEnums.TransactionClass.stock,
            };

            decimal expectedPositionValue = 0m;
            decimal actualPositionValue = Metrics.PositionValue(database, position);

            Assert.AreEqual(expectedPositionValue, actualPositionValue);
        }
        /// <summary>
        /// test the position value metric with a normal positive valued position
        /// </summary>
        [TestMethod]
        public void PositionValuePositive()
        {
            IDatabase database = new SQLiteDatabase();
            database.CreateDatabase();

            Quote lastQuote = new Quote()
            {
                Date = new DateTime(2000, 1, 1),
                Price = 50.00m,
                Symbol = "test",
            };

            database.SaveQuote(lastQuote);

            Position position = new Position()
            {
                Symbol = "test",
                Quantity = 10,
                Class = TransactionEnums.TransactionClass.stock,
            };

            decimal expectedPositionValue = 500m;
            decimal actualPositionValue = Metrics.PositionValue(database, position);

            Assert.AreEqual(expectedPositionValue, actualPositionValue);
        }
    }
}
