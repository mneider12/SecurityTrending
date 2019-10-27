using Core;
using Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using static Core.TransactionEnums;

namespace Database
{
    /// <summary>
    /// SQLite database implementation
    /// </summary>
    public class SQLiteDatabase : IDatabase
    {
        #region IDatabase
        /// <summary>
        /// Create the database, including table definitions
        /// </summary>
        public void CreateDatabase()
        {
            SQLiteConnection.CreateFile("database.sqlite");
            using (SQLiteConnection connection = OpenConnection())
            {
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    CreateActionsTable(connection);
                    CreateClassesTable(connection);
                    ExecuteNonQuery(CREATE_TRANSACTIONS_SQL, connection);
                    ExecuteNonQuery(CREATE_POSITIONS_SQL, connection);
                    ExecuteNonQuery(CREATE_LAST_PRICE_SQL, connection);

                    transaction.Commit();
                }
                connection.Close();
            }
        }
        /// <summary>
        /// Add a new transaction to the database
        /// </summary>
        /// <param name="transaction">transaction to add</param>
        public void NewTransaction(Transaction transaction)
        {
            string sql = GetInsertTransactionSql(transaction);
            ExecuteNonQuery(sql);
        }
        /// <summary>
        /// Insert a price into the database
        /// </summary>
        /// <param name="quote">quote with at least symbol, date, and price</param>
        public void SetPrice(Quote quote)
        {
            using (SQLiteConnection connection = OpenConnection())
            {
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = GetUpdatePriceSql(quote);
                        if (command.ExecuteNonQuery() == 0)
                        {
                            command.CommandText = GetInsertPriceSql(quote);
                            command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                connection.Close();
            }
        }
        /// <summary>
        /// get a list of symbols relevant to a portfolio
        /// </summary>
        /// <returns>list of symbols</returns>
        public List<string> GetSymbols()
        {
            List<string> symbols = new List<string>();

            using (SQLiteConnection connection = OpenConnection())
            {
                using (SQLiteCommand command = new SQLiteCommand(GET_SYMBOLS_SQL, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            symbols.Add(reader.GetString(0));
                        }
                    }
                }
                connection.Close();
            }

            return symbols;
        }
        /// <summary>
        /// get a position from the database
        /// </summary>
        /// <param name="symbol">symbol for the position</param>
        /// <returns>position</returns>
        public Position GetPosition(string symbol)
        {
            Position position = new Position()
            {
                Symbol = symbol,
            };
            string sql = GetPositionSql(symbol);

            using (SQLiteConnection connection = OpenConnection())
            {
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            long classID = (long)reader["ClassID"];
                            decimal quantity = (decimal)reader["Quantity"];

                            position.Class = (TransactionClass)classID;
                            position.Quantity = (double) quantity;
                        }
                    }
                }
            }

            return position;
        }
        /// <summary>
        /// set one position into the database
        /// </summary>
        /// <param name="position">position</param>
        public void SetPosition(Position position)
        {
            using (SQLiteConnection connection = OpenConnection())
            {
                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = GetUpdatePositionSql(position);
                        if (command.ExecuteNonQuery() == 0)
                        {
                            command.CommandText = GetInsertPositionSql(position);
                            command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                connection.Close();
            }
        }
        #endregion
        /// <summary>
        /// Get an open database connection. Caller is responsible for disposing the connection.
        /// </summary>
        /// <returns>connection</returns>
        private SQLiteConnection OpenConnection()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=database.sqlite");
            connection.Open();
            return connection;
        }
        /// <summary>
        /// Create the Actions table
        /// </summary>
        /// <param name="connection">database connection</param>
        private void CreateActionsTable(SQLiteConnection connection)
        {
            (int, string)[] values = new (int, string)[]
            {
                (1, "buy"),
                (2, "sell"),
            };
            CreateCategoryNameTable(connection, "Actions", "Action", values);
        }
        /// <summary>
        /// Create the Classes table
        /// </summary>
        /// <param name="connection">database connection</param>
        private void CreateClassesTable(SQLiteConnection connection)
        {
            (int, string)[] values = new (int, string)[]
            {
                (1, "cash"),
                (2, "stock"),
            };
            CreateCategoryNameTable(connection, "Classes", "Class", values);
        }
        /// <summary>
        /// create a basic category ID / name table
        /// </summary>
        /// <param name="connection">database connection</param>
        /// <param name="tableName">table name</param>
        /// <param name="singularOfTableName">singular of the table name</param>
        /// <param name="values">values to insert into the table</param>
        private void CreateCategoryNameTable(SQLiteConnection connection, string tableName, string singularOfTableName, (int, string)[] values)
        {
            string createSql = string.Format("create table \"{0}\" (" +
                                                    "\"{1}ID\" integer," +
                                                    "\"Name\" text not null," +
                                                    "primary key(\"{1}ID\")" +
                                                    ");", tableName, singularOfTableName);
            ExecuteNonQuery(createSql, connection);

            foreach ((int, string) value in values)
            {
                string insertSql = string.Format("insert into {0} ({1}ID, Name) values ({2}, '{3}')", tableName, singularOfTableName, value.Item1, value.Item2);
                ExecuteNonQuery(insertSql, connection);
            }
        }
        /// <summary>
        /// Execute a non query sequel statement on a new database connection
        /// </summary>
        /// <param name="sql">SQL to execute</param>
        private void ExecuteNonQuery(string sql)
        {
            using (SQLiteConnection connection = OpenConnection())
            {
                ExecuteNonQuery(sql, connection);
            }
        }
        /// <summary>
        /// Execute a non query sequel statement
        /// </summary>
        /// <param name="connection">database connection</param>
        /// <param name="sql">SQL to execute</param>
        private void ExecuteNonQuery(string sql, SQLiteConnection connection)
        {
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// get the SQL command to insert a transaction into the database
        /// </summary>
        /// <param name="transaction">transaction to insert</param>
        /// <returns>SQL</returns>
        private string GetInsertTransactionSql(Transaction transaction)
        {
            return string.Format("insert into \"Transactions\" values ({0}, '{1}', {2}, {3}, '{4}', {5}, {6});",
                transaction.TransactionID, transaction.Date.ToString(), (int)transaction.Action, (int)transaction.Class, transaction.Symbol,
                transaction.Amount, transaction.Quantity);
        }
        /// <summary>
        /// get the SQL command to insert a price into the database
        /// </summary>
        /// <param name="quote">quote with the price</param>
        /// <returns>SQL</returns>
        private string GetInsertPriceSql(Quote quote)
        {
            return string.Format("insert into LastPrice values ('{0}', '{1}', {2});", quote.Symbol, quote.Date.ToString(), quote.Price);
        }
        /// <summary>
        /// get the SQL command to update a price in the database
        /// </summary>
        /// <param name="quote">quote with the price</param>
        /// <returns>SQL</returns>
        private string GetUpdatePriceSql(Quote quote)
        {
            return string.Format("update LastPrice set DateTime='{0}', Price={1} where Symbol='{2}'", quote.Date.ToString(), quote.Price, quote.Symbol);
        }
        /// <summary>
        /// get SQL statement to get a position from the database
        /// </summary>
        /// <param name="symbol">symbol</param>
        /// <returns>SQL statement</returns>
        private string GetPositionSql(string symbol)
        {
            return string.Format("select ClassID, Quantity from Positions where Symbol='{0}'", symbol);
        }
        /// <summary>
        /// get SQL statement to update a position in the database
        /// </summary>
        /// <param name="position">position</param>
        /// <returns>SQL statement</returns>
        private string GetUpdatePositionSql(Position position)
        {
            return string.Format("update Positions set Quantity='{0}' where Symbol='{1}';", position.Quantity, position.Symbol);
        }
        /// <summary>
        /// get SQL statement to insert a position into the database
        /// </summary>
        /// <param name="position">position</param>
        /// <returns>SQL statement</returns>
        private string GetInsertPositionSql(Position position)
        {
            return string.Format("insert into Positions values ('{0}', '{1}', '{2}');", position.Symbol, (int)position.Class, position.Quantity);
        }
        /// <summary>
        /// SQL command to create the transactions table
        /// </summary>
        private const string CREATE_TRANSACTIONS_SQL = @"
create table Transactions (
TransactionID	integer, 
Date text not null, 
ActionID integer not null, 
ClassID	integer not null, 
Symbol text, 
Amount numeric not null,
Quantity numeric not null,
foreign key(ActionID) references Actions(ActionID), 
primary key(TransactionID)
);";
        /// <summary>
        /// SQL command to create the positions table
        /// </summary>
        private const string CREATE_POSITIONS_SQL = @"
create table Positions (
Symbol text,
ClassID integer not null,
Quantity numeric not null default 0,
primary key(Symbol),
foreign key(ClassID) references Classes(ClassID)
);";
        /// <summary>
        /// SQL command to create the last price table
        /// </summary>
        private const string CREATE_LAST_PRICE_SQL = @"
create table LastPrice (
Symbol text,
DateTime text not null,
Price numeric not null,
primary key(Symbol)
);";
        /// <summary>
        /// SQL query to select the symbols in the last price table
        /// </summary>
        private const string GET_SYMBOLS_SQL = @"
select Symbol
from Positions;";
    }
}
