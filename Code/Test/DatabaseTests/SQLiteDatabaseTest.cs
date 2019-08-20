using System;
using System.Data;
using System.Data.SQLite;
using Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=database.sqlite"))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("select name from sqlite_master order by name;", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        CheckNextTableName(reader, "Actions");
                        CheckNextTableName(reader, "Classes");
                        CheckNextTableName(reader, "Transactions");
                    }
                }
            }
        }
        /// <summary>
        /// Check that the next table name in a reader matches the expected table name
        /// </summary>
        /// <param name="reader">reader with table name in rows</param>
        /// <param name="expectedTableName">expected table name</param>
        private void CheckNextTableName(SQLiteDataReader reader, string expectedTableName)
        {
            reader.Read();
            Assert.AreEqual(expectedTableName, reader["name"]);
        }
    }
}
