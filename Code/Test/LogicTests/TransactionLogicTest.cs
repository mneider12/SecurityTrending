using System;
using System.IO;
using Core;
using Database;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using static Core.TransactionEnums;

namespace LogicTests
{
    /// <summary>
    /// transaction logic tests
    /// </summary>
    [TestClass]
    public class TransactionLogicTest
    {
        /// <summary>
        /// test committing a new transaction
        /// </summary>
        [TestMethod]
        public void CommitTransaction_Buy()
        {
            IDatabase database = new SQLiteDatabase();
            database.CreateDatabase();

            Transaction firstPurchase = new Transaction()
            {
                TransactionID = 1,
                Symbol = "TEST",
                Class = TransactionClass.stock,
                Action = TransactionAction.buy,
                Quantity = 100,
                Amount = 100,
            };

            Transaction secondPurchase = new Transaction()
            {
                TransactionID = 2,
                Symbol = "TEST",
                Class = TransactionClass.stock,
                Action = TransactionAction.buy,
                Quantity = 150,
                Amount = 200,
            };

            Position expectedPosition = new Position()
            {
                Symbol = "TEST",
                Class = TransactionClass.stock,
                Shares = 250,
            };

            TransactionLogic.CommitTransaction(database, firstPurchase);
            TransactionLogic.CommitTransaction(database, secondPurchase);

            Position actualPosition = database.GetPosition("TEST");

            Assert.AreEqual(expectedPosition.Symbol, actualPosition.Symbol);
            Assert.AreEqual(expectedPosition.Class, actualPosition.Class);
            Assert.AreEqual(expectedPosition.Shares, actualPosition.Shares);
        }
        /// <summary>
        /// delete the database after the test runs
        /// </summary>
        [TestCleanup]
        public void DeleteDatabase()
        {
            File.Delete("database.sqlite");
        }
    }
}
