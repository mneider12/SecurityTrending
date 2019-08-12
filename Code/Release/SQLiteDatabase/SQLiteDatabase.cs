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
                using (SQLiteCommand command = new SQLiteCommand(CREATE_ACTIONS_SQL, connection))
                {
                    command.ExecuteNonQuery();
                }
                using (SQLiteCommand command = new SQLiteCommand(CREATE_CLASSES_SQL, connection))
                {
                    command.ExecuteNonQuery();
                }
                using (SQLiteCommand command = new SQLiteCommand(CREATE_TRANSACTIONS_SQL, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        /// <summary>
        /// SQL command to create the Actions table
        /// </summary>
        private const string CREATE_ACTIONS_SQL = "CREATE TABLE \"Actions\" (" + 
                                                    "\"ActionID\" INTEGER," +
                                                    "\"Name\" TEXT NOT NULL," +
                                                    "PRIMARY KEY(\"ActionID\")" +
                                                    ");";
        /// <summary>
        /// SQL command to create the Classes table
        /// </summary>
        private const string CREATE_CLASSES_SQL = "CREATE TABLE \"Classes\" (" +
                                                    "\"ClassID\" INTEGER," +
                                                    "\"Name\" TEXT NOT NULL," +
                                                    "PRIMARY KEY(\"ClassID\")" +
                                                    ");";
        /// <summary>
        /// SQL command to create the transactions table
        /// </summary>
        private const string CREATE_TRANSACTIONS_SQL = "CREATE TABLE \"Transactions\" (" +
	                                                    "\"TransactionID\"	INTEGER," +
	                                                    "\"Date\"	TEXT NOT NULL," +
	                                                    "\"ActionID\"	INTEGER NOT NULL," +
	                                                    "\"ClassID\"	INTEGER NOT NULL," +
	                                                    "\"Ticker\"	TEXT," +
	                                                    "\"Amount\"	NUMERIC NOT NULL," +
	                                                    "FOREIGN KEY(\"ActionID\") REFERENCES \"Actions\"(\"ActionID\")," +
	                                                    "PRIMARY KEY(\"TransactionID\")" +
                                                        ");";
    }
}
