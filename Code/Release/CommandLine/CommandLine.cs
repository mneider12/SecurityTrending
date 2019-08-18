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
            Choice choice;
            do
            {
                ShowMenu();
                string input = Console.ReadLine();
                choice = GetChoiceFromInput(input);
            } while (choice == Choice.Invalid);

            RunChoice(choice, database);
        }
        /// <summary>
        /// Show the main menu
        /// </summary>
        private static void ShowMenu()
        {
            Console.WriteLine("Select an option");
            Console.WriteLine("\t[1] Create database");
            Console.WriteLine("\t[2] New transaction");
            Console.WriteLine(string.Format("\t[{0}] Quit", (int) Choice.Quit));
        }
        /// <summary>
        /// Convert user input into a Choice. Blank inputs are treated as quits.
        /// </summary>
        /// <param name="input">user input</param>
        /// <returns>choice made by user</returns>
        private static Choice GetChoiceFromInput(string input)
        {
            Choice choice;

            if (string.IsNullOrEmpty(input))
            {
                choice = Choice.Quit;
            }
            else if (!int.TryParse(input, out int numericInput))
            {
                choice = Choice.Invalid;
 
            }
            else if (Enum.IsDefined(typeof(Choice), numericInput))
            {
                choice = (Choice)numericInput;
            }
            else
            {
                choice = Choice.Invalid;
            }

            return choice;
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
                case Choice.NewTransaction:
                    NewTransaction();
                    break;
            }
        }
        private static void NewTransaction()
        {
            throw new Exception("Not Implemented");
        }
        /// <summary>
        /// Represents a choice at the main menu
        /// </summary>
        private enum Choice
        {
            Create = 1,
            NewTransaction = 2,
            Quit,
            Invalid,
        }
    }
}
