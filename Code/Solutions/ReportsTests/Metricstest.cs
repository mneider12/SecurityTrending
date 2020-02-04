using System;
using System.Collections.Generic;
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
        /// test an empty list of positions for positions value
        /// </summary>
        [TestMethod]
        public void PositionsValueZero()
        {
            QuoteFeedMock quoteFeedMock = new QuoteFeedMock();
            List<Position> positions = new List<Position>();

            decimal expectedValue = 0m;
            decimal actualValue = Metrics.PositionsValue(positions, quoteFeedMock);

            Assert.AreEqual(expectedValue, actualValue);
        }
        [TestMethod]
        public void PositionsValueTwo()
        {
            Quote quote1 = new Quote()
            {
                Date = new DateTime(2000, 1, 1),
                Price = 10m,
                Symbol = "test1",
            };

            Quote quote2 = new Quote()
            {
                Date = new DateTime(2000, 1, 1),
                Price = 20m,
                Symbol = "test2",
            };

            QuoteFeedMock quoteFeedMock = new QuoteFeedMock();
            quoteFeedMock.AddQuote(quote1);
            quoteFeedMock.AddQuote(quote2);

            Position position1 = new Position()
            {
                Class = TransactionEnums.TransactionClass.stock,
                Quantity = 5,
                Symbol = "test1",
            };

            Position postion2 = new Position()
            {
                Class = TransactionEnums.TransactionClass.stock,
                Quantity = 10,
                Symbol = "test2",
            };

            List<Position> positions = new List<Position>()
            {
                position1, postion2,
            };

            decimal expectedValue = 250m;
            decimal actualValue = Metrics.PositionsValue(positions, quoteFeedMock);

            Assert.AreEqual(expectedValue, actualValue);
        }
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
