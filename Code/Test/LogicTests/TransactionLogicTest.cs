using System.IO;
using Core;
using Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using static Core.TransactionEnums;
using static Logic.TransactionLogic;

namespace LogicTests
{
    /// <summary>
    /// transaction logic tests
    /// </summary>
    [TestClass]
    public class TransactionLogicTest
    {
        /// <summary>
        /// test committing new buy transactions
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
            };

            Transaction secondPurchase = new Transaction()
            {
                TransactionID = 2,
                Symbol = "TEST",
                Class = TransactionClass.stock,
                Action = TransactionAction.buy,
                Quantity = 150,
            };

            Position expectedPosition = new Position()
            {
                Symbol = "TEST",
                Class = TransactionClass.stock,
                Quantity = 250,
            };

            CommitTransaction(database, firstPurchase);
            CommitTransaction(database, secondPurchase);

            VerifyPosition(database, expectedPosition);
        }
        /// <summary>
        /// test committing a sell transaction
        /// </summary>
        [TestMethod]
        public void CommitTransaction_Sell_HasEnoughQuantity()
        {
            IDatabase database = new SQLiteDatabase();
            database.CreateDatabase();

            Transaction purchase = new Transaction()
            {
                TransactionID = 1,
                Symbol = "TEST",
                Class = TransactionClass.stock,
                Action = TransactionAction.buy,
                Quantity = 100,
                Amount = 100,
            };

            Transaction sale = new Transaction()
            {
                TransactionID = 2,
                Symbol = "TEST",
                Class = TransactionClass.stock,
                Action = TransactionAction.sell,
                Quantity = 25,
                Amount = 100,
            };

            Position expectedPosition = new Position()
            {
                Symbol = "TEST",
                Class = TransactionClass.stock,
                Quantity = 75,
            };

            CommitTransaction(database, purchase);
            CommitTransaction(database, sale);

            VerifyPosition(database, expectedPosition);
        }
        /// <summary>
        /// test that attempting to sell more quantity than available results in an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void CommitTransaction_Sell_NotEnoughQuantity()
        {
            IDatabase database = new SQLiteDatabase();
            database.CreateDatabase();

            Transaction purchase = new Transaction()
            {
                TransactionID = 1,
                Symbol = "TEST",
                Class = TransactionClass.stock,
                Action = TransactionAction.buy,
                Quantity = 100,
                Amount = 100,
            };

            Transaction sale = new Transaction()
            {
                TransactionID = 2,
                Symbol = "TEST",
                Class = TransactionClass.stock,
                Action = TransactionAction.sell,
                Quantity = 125,
                Amount = 100,
            };

            CommitTransaction(database, purchase);
            CommitTransaction(database, sale);
        }
        /// <summary>
        /// test that attempting to apply a transaction with a class different than a symbol's position throws an exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void CommitTransaction_Buy_ChangeClassException()
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
                Class = TransactionClass.bond,
                Action = TransactionAction.buy,
                Quantity = 150,
                Amount = 200,
            };

            CommitTransaction(database, firstPurchase);
            CommitTransaction(database, secondPurchase);
        }
        /// <summary>
        /// delete the database after the test runs
        /// </summary>
        [TestCleanup]
        public void DeleteDatabase()
        {
            File.Delete("database.sqlite");
        }
        #region private helper methods

        /// <summary>
        /// verify that an expected position matches the same position in the database
        /// </summary>
        /// <param name="database">database</param>
        /// <param name="expectedPosition">expected position</param>
        private void VerifyPosition(IDatabase database, Position expectedPosition)
        {
            Position actualPosition = database.GetPosition(expectedPosition.Symbol);
            Assert.AreEqual(expectedPosition, actualPosition);
        }

        #endregion
    }
}
