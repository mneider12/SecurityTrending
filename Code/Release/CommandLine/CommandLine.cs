using Database;

namespace CommandLine
{
    /// <summary>
    /// Entry point for the command line application
    /// </summary>
    class CommandLine
    {
        /// <summary>
        /// entry point for the command line application.
        /// creates the application database
        /// </summary>
        static void Main()
        {
            IDatabase database = new SQLiteDatabase();
            database.CreateDatabase();
        }
    }
}
