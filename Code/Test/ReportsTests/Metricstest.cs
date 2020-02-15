using System;
using System.Collections.Generic;
using Core;
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
        /// test an empty list of positions for account value
        /// </summary>
        [TestMethod]
        public void AccountValueZero()
        {
            QuoteFeedMock quoteFeedMock = new QuoteFeedMock();
            Account account = new Account();

            decimal expectedValue = 0m;
            decimal actualValue = Metrics.AccountValue(account, quoteFeedMock);

            Assert.AreEqual(expectedValue, actualValue);
        }
        /// <summary>
        /// test account value for a single position
        /// </summary>
        [TestMethod]
        public void AccountValueOne()
        {
            Quote quote1 = new Quote()
            {
                Date = new DateTime(2000, 1, 1),
                Price = 10m,
                Symbol = "test1",
            };

            QuoteFeedMock quoteFeedMock = new QuoteFeedMock();
            quoteFeedMock.AddQuote(quote1);

            Position position1 = new Position()
            {
                Class = TransactionEnums.TransactionClass.stock,
                Quantity = 5,
                Symbol = "test1",
            };

            Account account = new Account();
            account.Positions.Add(position1.Symbol, position1);

            decimal expectedValue = 50m;
            decimal actualValue = Metrics.AccountValue(account, quoteFeedMock);

            Assert.AreEqual(expectedValue, actualValue);
        }
        /// <summary>
        /// test a list of two positions
        /// </summary>
        [TestMethod]
        public void AccountValueTwo()
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

            Account account = new Account();
            account.Positions.Add(position1.Symbol, position1);
            account.Positions.Add(postion2.Symbol, postion2);

            decimal expectedValue = 250m;
            decimal actualValue = Metrics.AccountValue(account, quoteFeedMock);

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
