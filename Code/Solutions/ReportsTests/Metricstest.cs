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
        [ExpectedException(typeof(NoQuoteAvailableException))]
        public void PositionValueZero()
        {
            QuoteFeedMock quoteFeedMock = new QuoteFeedMock();

            Position position = new Position()
            {
                Symbol = "test",
                Quantity = 0,
                Class = TransactionEnums.TransactionClass.stock,
            };

            Metrics.PositionValue(position, quoteFeedMock);
        }
        /// <summary>
        /// test the position value metric with a normal positive valued position
        /// </summary>
        [TestMethod]
        public void PositionValuePositive()
        {
            IDatabase database = new SQLiteDatabase();
            database.CreateDatabase();

            QuoteFeedMock quoteFeedMock = new QuoteFeedMock();

            Quote quote = new Quote()
            {
                Date = new DateTime(2000, 1, 1),
                Price = 50.00m,
                Symbol = "test",
            };

            quoteFeedMock.AddQuote(quote);

            Position position = new Position()
            {
                Symbol = "test",
                Quantity = 10,
                Class = TransactionEnums.TransactionClass.stock,
            };

            decimal expectedPositionValue = 500m;
            decimal actualPositionValue = Metrics.PositionValue(position, quoteFeedMock);

            Assert.AreEqual(expectedPositionValue, actualPositionValue);
        }
    }
}
