using Core;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
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
            if (transaction != null)
            {
                using (SQLiteConnection connection = OpenConnection())
                {
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = "insert into Transactions values (@transactionID, @date, @action, @class, @symbol, @amount, @quantity);";
                        
                        command.Parameters.Add("@transactionID", DbType.Int32).Value = transaction.TransactionID;
                        command.Parameters.Add("@date", DbType.String).Value = transaction.Date.ToString(CultureInfo.InvariantCulture);
                        command.Parameters.Add("@action", DbType.Int32).Value = transaction.Action;
                        command.Parameters.Add("@class", DbType.Int32).Value = transaction.Class;
                        command.Parameters.Add("@symbol", DbType.String).Value = transaction.Symbol;
                        command.Parameters.Add("@amount", DbType.Currency).Value = transaction.Amount;
                        command.Parameters.Add("@quantity", DbType.Double).Value = transaction.Quantity;

                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        /// <summary>
        /// get 
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public Quote GetLastQuote(string symbol)
        {
            Quote quote = new Quote
            {
                Symbol = symbol,
            };

            using (SQLiteConnection connection = OpenConnection())
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.Parameters.Add("@symbol", DbType.String).Value = symbol;
                    command.CommandText = GET_PRICE_SQL;

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            quote.Date = DateTime.Parse((string)reader["DateTime"], CultureInfo.InvariantCulture);
                            quote.Price = (decimal)reader["Price"];
                        }
                    }
                }
            }

                return quote;
        }
        /// <summary>
        /// deprecated. Call SaveQuote instead
        /// </summary>
        /// <param name="quote"></param>
        public void SetPrice(Quote quote)
        {
            SaveQuote(quote);
        }
        /// <summary>
        /// Insert a price into the database
        /// </summary>
        /// <param name="quote">quote with at least symbol, date, and price</param>
        public void SaveQuote(Quote quote)
        {
            if (quote != null)
            {
                using (SQLiteConnection connection = OpenConnection())
                {
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        using (SQLiteCommand command = new SQLiteCommand(connection))
                        {
                            command.Parameters.Add("@date", DbType.String).Value = quote.Date.ToString(CultureInfo.InvariantCulture);
                            command.Parameters.Add("@price", DbType.Decimal).Value = quote.Price;
                            command.Parameters.Add("@symbol", DbType.String).Value = quote.Symbol;
                            command.CommandText = "update LastPrice set DateTime=@date, Price=@price where Symbol=@symbol";

                            if (command.ExecuteNonQuery() == 0)
                            {
                                command.CommandText = "insert into LastPrice values (@symbol, @date, @price);";
                                command.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                    connection.Close();
                }
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

            using (SQLiteConnection connection = OpenConnection())
            {
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.Parameters.Add("@symbol", DbType.String).Value = symbol;
                    command.CommandText = "select ClassID, Quantity from Positions where Symbol=@symbol;";
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
        /// get the positions in the portfolio
        /// </summary>
        /// <returns>list of positions</returns>
        public List<Position> GetPositions()
        {
            List<Position> positions = new List<Position>();

            using (SQLiteConnection connection = OpenConnection())
            {
                using (SQLiteCommand command = new SQLiteCommand(GET_POSITIONS_SQL, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            string symbol = (string)reader["Symbol"];
                            long classID = (long)reader["ClassID"];
                            decimal quantity = (decimal)reader["Quantity"];

                            Position position = new Position()
                            {
                                Symbol = symbol,
                                Class = (TransactionClass)classID,
                                Quantity = (double)quantity,
                            };

                            positions.Add(position);
                        }
                    }
                }
            }

            return positions;
        }
        /// <summary>
        /// set one position into the database
        /// </summary>
        /// <param name="position">position</param>
        public void SetPosition(Position position)
        {
            if (position != null)
            {
                using (SQLiteConnection connection = OpenConnection())
                {
                    using (SQLiteTransaction transaction = connection.BeginTransaction())
                    {
                        using (SQLiteCommand command = new SQLiteCommand(connection))
                        {
                            command.Parameters.Add("@quantity", DbType.Decimal).Value = position.Quantity;
                            command.Parameters.Add("@symbol", DbType.String).Value = position.Symbol;
                            command.CommandText = "update Positions set Quantity=@quantity where Symbol=@symbol;";
                            if (command.ExecuteNonQuery() == 0)
                            {
                                command.Parameters.Add("@class", DbType.Int32).Value = position.Class;
                                command.CommandText = "insert into Positions values (@symbol, @class, @quantity);";
                                command.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                    connection.Close();
                }
            }
        }
        #endregion
        /// <summary>
        /// Get an open database connection. Caller is responsible for disposing the connection.
        /// </summary>
        /// <returns>connection</returns>
        private static SQLiteConnection OpenConnection()
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
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "create table Actions (" +
                    "ActionID integer," +
                    "Name text not null," +
                    "primary key(ActionID)" +
                    ");";

                command.ExecuteNonQuery();
            }

            (int, string)[] values = new (int, string)[]
            {
                (1, "buy"),
                (2, "sell"),
            };

            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "insert into Actions (ActionID, Name) values (@actionID, @name)";
                foreach ((int, string) value in values)
                {
                    command.Parameters.Add("@actionID", DbType.Int32).Value = value.Item1;
                    command.Parameters.Add("@name", DbType.String).Value = value.Item2;

                    command.ExecuteNonQuery();
                }
            }
                
        }
        /// <summary>
        /// Create the Classes table
        /// </summary>
        /// <param name="connection">database connection</param>
        private void CreateClassesTable(SQLiteConnection connection)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "create table Classes (" +
                    "ClassID integer," +
                    "Name text not null," +
                    "primary key(ClassID)" +
                    ");";

                command.ExecuteNonQuery();
            }

            (int, string)[] values = new (int, string)[]
            {
                (1, "cash"),
                (2, "stock"),
            };

            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = "insert into Classes (ClassID, Name) values (@classID, @name)";
                foreach ((int, string) value in values)
                {
                    command.Parameters.Add("@classID", DbType.Int32).Value = value.Item1;
                    command.Parameters.Add("@name", DbType.String).Value = value.Item2;

                    command.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// Execute a non query sequel statement
        /// </summary>
        /// <param name="connection">database connection</param>
        /// <param name="sql">SQL to execute</param>
        private static void ExecuteNonQuery(string sql, SQLiteConnection connection)
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
        /// get SQL statement to get the last price for a symbol and its timestamp stored in the database
        /// </summary>
        /// <param name="symbol">symbol</param>
        /// <returns>SQL statement</returns>
        private string GetPriceSql(string symbol)
        {
            return string.Format("select DateTime, Price from LastPrice where Symbol='{0}'", symbol);
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
        /// SQL query to select the sysmbols for all positions
        /// </summary>
        private const string GET_SYMBOLS_SQL = @"
select Symbol
from Positions;";
        /// <summary>
        /// SQL query to get all positions
        /// </summary>
        private const string GET_POSITIONS_SQL = @"
select Symbol, ClassID, Quantity
from Positions;";
        /// <summary>
        /// SQL query to get the last price for a symbol. @symbol needs to be replaced by a command parameter.
        /// </summary>
        private const string GET_PRICE_SQL = @"
select DateTime, Price
from LastPrice
where Symbol=@symbol;";
    }
}
