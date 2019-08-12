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
        /// creates necessary dependencies and runs the main menu
        /// </summary>
        public static void Main()
        {
            // create dependencies here, so they can easily be switched out later
            IDatabase database = new SQLiteDatabase();

            RunMenu(database);
        }
        /// <summary>
        /// run the main menu
        /// </summary>
        /// <param name="database"></param>
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
        /// <summary>
        /// Show the main menu
        /// </summary>
        private static void ShowMenu()
        {
            Console.WriteLine("Select an option");
            Console.WriteLine("\t[1] Create database");
            Console.WriteLine("\t[2] Quit");
        }
        /// <summary>
        /// validate the user made a valid choice. Blank inputs are treated as quits.
        /// </summary>
        /// <param name="input">user input</param>
        /// <param name="choice">choice made by user, if it corresponded to a valid choice</param>
        /// <returns>true if the user made a valid selection, including blank</returns>
        private static bool ValidateChoice(string input, out Choice choice)
        {
            if (string.IsNullOrEmpty(input) || input.Equals("2")) 
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
        /// <summary>
        /// handle a choice made at the main menu
        /// </summary>
        /// <param name="choice">choice that was made</param>
        /// <param name="database">database implementation</param>

        private static void RunChoice(Choice choice, IDatabase database)
        {
            switch (choice)
            {
                case Choice.Create:
                    database.CreateDatabase();
                    break;
            }
        }
        /// <summary>
        /// Represents a choice at the main menu
        /// </summary>
        private enum Choice
        {
            Create = 1,
            Quit = 999,
        }
    }
}
