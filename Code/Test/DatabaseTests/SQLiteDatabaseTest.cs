using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using Core;
using Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using static Core.TransactionEnums;

namespace DatabaseTests
{
    /// <summary>
    /// Unit tests for SQLiteDatabase.cs
    /// </summary>
    [TestClass]
    public class SQLiteDatabaseTest
    {
        /// <summary>
        /// test the CreateDatabase method
        /// </summary>
        [TestMethod]
        public void CreateDatabaseTest()
        {
            SQLiteDatabase database = new SQLiteDatabase();
            database.CreateDatabase();

            using SQLiteConnection connection = new SQLiteConnection("Data Source=database.sqlite");
            connection.Open();

            CheckTableNames(connection);

            CheckActionsTable(connection);
            CheckClassesTable(connection);
            CheckLastPriceTable(connection);
            CheckPositionsTable(connection);
            CheckTransactionsTable(connection);
        }
        /// <summary>
        /// test the NewTransaction method for adding a single transaction
        /// </summary>
        [TestMethod]
        public void NewTransactionTest()
        {
            SQLiteDatabase database = new SQLiteDatabase();
            database.CreateDatabase();

            Transaction transaction = new Transaction()
            {
                TransactionID = 1,
                Date = new DateTime(2000, 1, 1),
                Action = TransactionAction.buy,
                Class = TransactionClass.stock,
                Symbol = "AMZN",
                Amount = 1.00M,
                Quantity = 100,
            };

            database.NewTransaction(transaction);

            using SQLiteConnection connection = new SQLiteConnection("Data Source=database.sqlite");
            connection.Open();
            using SQLiteCommand command = new SQLiteCommand("select * from Transactions where TransactionID = 1;", connection);
            using SQLiteDataReader reader = command.ExecuteReader();
            reader.Read();

            Assert.AreEqual(1L, reader["TransactionID"]);
            Assert.AreEqual(new DateTime(2000, 1, 1).ToString(CultureInfo.InvariantCulture), reader["Date"]);
            Assert.AreEqual((long)TransactionAction.buy, reader["ActionID"]);
            Assert.AreEqual((long)TransactionClass.stock, reader["ClassID"]);
            Assert.AreEqual("AMZN", reader["Symbol"]);
            Assert.AreEqual(1.00M, reader["Amount"]);

        }
        /// <summary>
        /// test the GetPosition function for a single position
        /// </summary>
        [TestMethod]
        public void GetPositionTest()
        {
            SQLiteDatabase database = new SQLiteDatabase();
            database.CreateDatabase();

            Position expectedPosition = new Position()
            {
                Symbol = "TEST",
                Class = TransactionClass.stock,
                Quantity = 100.55,
            };
            database.SetPosition(expectedPosition);

            Position actualPosition = database.GetPosition(expectedPosition.Symbol);

            Assert.AreEqual(expectedPosition.Symbol, actualPosition.Symbol);
            Assert.AreEqual(expectedPosition.Class, actualPosition.Class);
            Assert.AreEqual(expectedPosition.Quantity, actualPosition.Quantity);
        }
        /// <summary>
        /// test the GetPositions function
        /// </summary>
        [TestMethod]
        public void GetPositionsTest()
        {
            SQLiteDatabase database = new SQLiteDatabase();
            database.CreateDatabase();

            Position position1 = new Position()
            {
                Symbol = "ONE",
                Quantity = 100,
                Class = TransactionClass.stock,
            };

            Position position2 = new Position()
            {
                Symbol = "TWO",
                Quantity = 150,
                Class = TransactionClass.bond,
            };

            database.SetPosition(position1);
            database.SetPosition(position2);

            List<Position> expectedPositions = new List<Position>()
            {
                position1, position2
            };

            List<Position> actualPositions = database.GetPositions();

            CollectionAssert.AreEqual(expectedPositions, actualPositions);

        }
        /// <summary>
        /// test GetPrice
        /// </summary>
        [TestMethod]
        public void GetPriceTest()
        {
            SQLiteDatabase database = new SQLiteDatabase();
            database.CreateDatabase();

            Quote expectedQuote = new Quote()
            {
                Symbol = "TEST",
                Date = new DateTime(2000, 1, 1),
                Price = 100.00m,
            };

            database.SaveQuote(expectedQuote);

            Quote actualQuote = database.GetLastQuote(expectedQuote.Symbol);

            Assert.AreEqual(expectedQuote.Price, actualQuote.Price);
        }
        /// <summary>
        /// delete the database after the test runs
        /// </summary>
        [TestCleanup]
        public void DeleteDatabase()
        {
            File.Delete("database.sqlite");
        }
        /// <summary>
        /// Check the table names in the database
        /// </summary>
        /// <param name="connection">database connection</param>
        private void CheckTableNames(SQLiteConnection connection)
        {
            using (SQLiteCommand command = new SQLiteCommand("select name from sqlite_master order by name;", connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    CheckNextTableName(reader, "Actions");
                    CheckNextTableName(reader, "Classes");
                    CheckNextTableName(reader, "LastPrice");
                    CheckNextTableName(reader, "Positions");
                    CheckNextTableName(reader, "Transactions");
                }
            }
        }
        /// <summary>
        /// Check that the next table name in a reader matches the expected table name
        /// </summary>
        /// <param name="reader">reader with table name in rows</param>
        /// <param name="expectedTableName">expected table name</param>
        private static void CheckNextTableName(SQLiteDataReader reader, string expectedTableName)
        {
            reader.Read();
            Assert.AreEqual(expectedTableName, reader["name"]);
        }
        /// <summary>
        /// Check the Actions table metadata
        /// </summary>
        /// <param name="connection">database connection</param>
        private void CheckActionsTable(SQLiteConnection connection)
        {
            using (SQLiteCommand command = new SQLiteCommand("pragma table_info(\"Actions\");", connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    CheckNextColumn(reader, 0, "ActionID", "integer", 0, DBNull.Value, 1);
                    CheckNextColumn(reader, 1, "Name", "text", 1, DBNull.Value, 0);
                }
            }

            using (SQLiteCommand command = new SQLiteCommand("select * from Actions;", connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    reader.Read();

                    Assert.AreEqual(1L, reader["ActionID"]);
                    Assert.AreEqual("buy", reader["Name"]);

                    reader.Read();

                    Assert.AreEqual(2L, reader["ActionID"]);
                    Assert.AreEqual("sell", reader["Name"]);
                }
            }
        }
        /// <summary>
        /// Check the Classes table metadata
        /// </summary>
        /// <param name="connection">database connection</param>
        private void CheckClassesTable(SQLiteConnection connection)
        {
            CheckClassesTableInfo(connection);
            CheckClassTableContent(connection);
        }
        /// <summary>
        /// check the content of the class table
        /// </summary>
        /// <param name="connection">database connection</param>
        private static void CheckClassTableContent(SQLiteConnection connection)
        {
            using SQLiteCommand command = new SQLiteCommand("select * from Classes;", connection);
            using SQLiteDataReader reader = command.ExecuteReader();

            reader.Read();

            Assert.AreEqual(1L, reader["ClassID"]);
            Assert.AreEqual("cash", reader["Name"]);

            reader.Read();

            Assert.AreEqual(2L, reader["ClassID"]);
            Assert.AreEqual("stock", reader["Name"]);
        }
        /// <summary>
        /// check the metadata for the Class table
        /// </summary>
        /// <param name="connection">database connection</param>
        private void CheckClassesTableInfo(SQLiteConnection connection)
        {
            using SQLiteCommand command = new SQLiteCommand("pragma table_info(\"Classes\");", connection);
            using SQLiteDataReader reader = command.ExecuteReader();

            CheckNextColumn(reader, 0, "ClassID", "integer", 0, DBNull.Value, 1);
            CheckNextColumn(reader, 1, "Name", "text", 1, DBNull.Value, 0);
        }
        /// <summary>
        /// Check the metadata of the Transactions table
        /// </summary>
        /// <param name="connection">database connection</param>
        private void CheckTransactionsTable(SQLiteConnection connection)
        {
            using (SQLiteCommand command = new SQLiteCommand("pragma table_info(Transactions);", connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    CheckNextColumn(reader, 0, "TransactionID", "integer", 0, DBNull.Value, 1);
                    CheckNextColumn(reader, 1, "Date", "text", 1, DBNull.Value, 0);
                    CheckNextColumn(reader, 2, "ActionID", "integer", 1, DBNull.Value, 0);
                    CheckNextColumn(reader, 3, "ClassID", "integer", 1, DBNull.Value, 0);
                    CheckNextColumn(reader, 4, "Symbol", "text", 0, DBNull.Value, 0);
                    CheckNextColumn(reader, 5, "Amount", "numeric", 1, DBNull.Value, 0);
                    CheckNextColumn(reader, 6, "Quantity", "numeric", 1, DBNull.Value, 0);
                }
            }
        }
        /// <summary>
        /// check the metadata of the last price table
        /// </summary>
        /// <param name="connection">database connection</param>
        private void CheckLastPriceTable(SQLiteConnection connection)
        {
            using (SQLiteCommand command = new SQLiteCommand(@"pragma table_info(LastPrice);", connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    CheckNextColumn(reader, 0, "Symbol", "text", 0, DBNull.Value, 1);
                    CheckNextColumn(reader, 1, "DateTime", "text", 1, DBNull.Value, 0);
                    CheckNextColumn(reader, 2, "Price", "numeric", 1, DBNull.Value, 0);
                }
            }
        }
        /// <summary>
        /// check the metadata of the positions table
        /// </summary>
        /// <param name="connection">database connection</param>
        private void CheckPositionsTable(SQLiteConnection connection)
        {
            using (SQLiteCommand command = new SQLiteCommand(@"pragma table_info(Positions);", connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    CheckNextColumn(reader, 0, "Symbol", "text", 0, DBNull.Value, 1);
                    CheckNextColumn(reader, 1, "ClassID", "integer", 1, DBNull.Value, 0);
                    CheckNextColumn(reader, 2, "Quantity", "numeric", 1, "0", 0);
                }
            }
        }
        /// <summary>
        /// Check the metadata of one column in a table 
        /// </summary>
        /// <param name="reader">reader where the next row is the column to check</param>
        /// <param name="cid">expected cid</param>
        /// <param name="name">expected name</param>
        /// <param name="type">expected type</param>
        /// <param name="notnull">expected value of notnull</param>
        /// <param name="dflt_value">expected default value</param>
        /// <param name="pk">expected value of primary key</param>
        private void CheckNextColumn(SQLiteDataReader reader, long cid, string name, string type, long notnull, object dflt_value, long pk)
        {
            reader.Read();

            Assert.AreEqual(cid, reader["cid"]);
            Assert.AreEqual(name, reader["name"]);
            Assert.AreEqual(type, reader["type"]);
            Assert.AreEqual(notnull, reader["notnull"]);
            Assert.AreEqual(dflt_value, reader["dflt_value"]);
            Assert.AreEqual(pk, reader["pk"]);
        }
    }
}
