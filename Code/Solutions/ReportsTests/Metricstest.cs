using System;
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
    }
}
