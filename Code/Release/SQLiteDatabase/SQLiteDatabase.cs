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
    }
}
