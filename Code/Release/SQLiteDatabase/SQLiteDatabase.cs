using System.Data.SQLite;

namespace Database
{
    /// <summary>
    /// SQLite database implementation
    /// </summary>
    public class SQLiteDatabase : IDatabase
    {
        /// <summary>
        /// Create the database, including table definitions
        /// </summary>
        public void CreateDatabase()
        {
            SQLiteConnection.CreateFile("database.sqlite");
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=database.sqlite"))
            {
                connection.Open();

                CreateActionsTable(connection);
                CreateClassesTable(connection);
                using (SQLiteCommand command = new SQLiteCommand(CREATE_TRANSACTIONS_SQL, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        /// <summary>
        /// Create the Actions table
        /// </summary>
        /// <param name="connection">database connection</param>
        public void CreateActionsTable(SQLiteConnection connection)
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
        public void CreateClassesTable(SQLiteConnection connection)
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
        public void CreateCategoryNameTable(SQLiteConnection connection, string tableName, string singularOfTableName, (int, string)[] values)
        {
            string createSql = string.Format("create table \"{0}\" (" +
                                                    "\"{1}ID\" integer," +
                                                    "\"Name\" text not null," +
                                                    "primary key(\"{1}ID\")" +
                                                    ");", tableName, singularOfTableName);
            ExecuteNonQuery(connection, createSql);

            foreach ((int, string) value in values)
            {
                string insertSql = string.Format("insert into {0} ({1}ID, Name) values ({2}, '{3}')", tableName, singularOfTableName, value.Item1, value.Item2);
                ExecuteNonQuery(connection, insertSql);
            }
        }
        /// <summary>
        /// Execute a non query sequel statement
        /// </summary>
        /// <param name="connection">database connection</param>
        /// <param name="sql">SQL query to execute</param>
        private void ExecuteNonQuery(SQLiteConnection connection, string sql)
        {
            using (SQLiteCommand command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// SQL command to create the transactions table
        /// </summary>
        private const string CREATE_TRANSACTIONS_SQL = "create table \"Transactions\" (" +
	                                                    "\"TransactionID\"	integer," +
	                                                    "\"Date\"	text not null," +
	                                                    "\"ActionID\"	integer not null," +
	                                                    "\"ClassID\"	integer not null," +
	                                                    "\"Ticker\"	text," +
	                                                    "\"Amount\"	numeric not null," +
	                                                    "foreign key(\"ActionID\") references \"Actions\"(\"ActionID\")," +
	                                                    "primary key(\"TransactionID\")" +
                                                        ");";
    }
}
