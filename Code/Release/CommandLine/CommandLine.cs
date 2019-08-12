using System;
using Database;

namespace CommandLine
{
    /// <summary>
    /// Entry point for the command line application
    /// </summary>
    public class CommandLine
    {
        /// <summary>
        /// entry point for the command line application.
        /// creates the application database
        /// </summary>
        public static void Main()
        {
            // create dependencies here, so they can easily be switched out later
            IDatabase database = new SQLiteDatabase();

            RunMenu(database);
        }

        private static void RunMenu(IDatabase database)
        {
            string input;
            Choice choice;
            do
            {
                ShowMenu();
                input = Console.ReadLine();
            } while (!ValidateChoice(input, out choice));

            RunChoice(choice, database);
        }

        private static void ShowMenu()
        {
            Console.WriteLine("Select an option");
            Console.WriteLine("\t[1] Create database");
        }

        private static bool ValidateChoice(string input, out Choice choice)
        {
            if (string.IsNullOrEmpty(input))
            {
                choice = Choice.Quit;
                return true;
            }

            if (Enum.TryParse(input, out choice))
            {
                return Enum.IsDefined(typeof(Choice), choice);
            }

            return false;
        }

        private static void RunChoice(Choice choice, IDatabase database)
        {
            switch (choice)
            {
                case Choice.Create:
                    database.CreateDatabase();
                    break;
            }
        }

        private enum Choice
        {
            Create = 1,
            Quit = 999,
        }
    }
}
