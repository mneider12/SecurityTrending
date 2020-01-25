using System;
using System.Collections.Generic;
using System.Globalization;
using Core;
using Database;
using DataFeed;
using static Core.TransactionEnums;

namespace CommandLine
{
    /// <summary>
    /// Entry point for the command line application
    /// </summary>
    public static class CommandLineMain
    {
        /// <summary>
        /// entry point for the command line application.
        /// creates necessary dependencies and runs the main menu
        /// </summary>
        public static void Main()
        {
            // create dependencies here, so they can easily be switched out later
            IDatabase database = new SQLiteDatabase();
            using (IWebClient webClient = new WebClient())
            {
                IAPIKeyQuoteFeed quoteFeed = new AlphaVantage()
                {
                    WebClient = webClient,
                };

                RunMenu(database, quoteFeed);
            }
        }
        /// <summary>
        /// run the main menu
        /// </summary>
        /// <param name="database">database</param>
        /// <param name="quoteFeed">quote feed</param>
        private static void RunMenu(IDatabase database, IAPIKeyQuoteFeed quoteFeed)
        {
            Choice choice;
            do
            {
                do
                {
                    ShowMenu();
                    string input = Console.ReadLine();
                    choice = GetChoiceFromInput(input);
                } while (choice == Choice.Invalid);
                
                RunChoice(choice, database, quoteFeed);

            } while (choice != Choice.Quit);
        }
        /// <summary>
        /// Show the main menu
        /// </summary>
        private static void ShowMenu()
        {
            Console.WriteLine(Resources.SelectAnOptionPrompt);

            WriteChoice(Choice.Create, Resources.CreateDatabaseOption);
            WriteChoice(Choice.NewTransaction, Resources.NewTransactionOption);
            WriteChoice(Choice.SetAPIKey, Resources.SetAPIKeyOption);
            WriteChoice(Choice.GetQuote, Resources.GetQuoteOption);
            WriteChoice(Choice.SetPrice, Resources.SetPriceOption);
            WriteChoice(Choice.UpdatePrices, Resources.UpdatePricesOption);
            WriteChoice(Choice.Quit, Resources.QuitOption);
        }
        /// <summary>
        /// write out one choice in a menu
        /// </summary>
        /// <param name="choice">choice available to user</param>
        /// <param name="text">text to display</param>
        private static void WriteChoice(Choice choice, string text)
        {
            string display = string.Format(CultureInfo.CurrentCulture, "\t[{0}] {1}", (int)choice, text);
            Console.WriteLine(display);
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
        /// <param name="database">database</param>

        private static void RunChoice(Choice choice, IDatabase database, IAPIKeyQuoteFeed quoteFeed)
        {
            switch (choice)
            {
                case Choice.Create:
                    database.CreateDatabase();
                    break;
                case Choice.NewTransaction:
                    NewTransaction(database);
                    break;
                case Choice.SetAPIKey:
                    SetAPIKey(quoteFeed);
                    break;
                case Choice.GetQuote:
                    GetQuote(quoteFeed);
                    break;
                case Choice.SetPrice:
                    SetPrice(database);
                    break;
                case Choice.UpdatePrices:
                    UpdatePrices(database, quoteFeed);
                    break;
            }
        }
        /// <summary>
        /// Add a new transaction to the database
        /// </summary>
        /// <param name="database">database implementation</param>
        private static void NewTransaction(IDatabase database)
        {
            Transaction transaction = new Transaction();
            string input;

            Console.WriteLine("Transaction ID:");
            input = Console.ReadLine();
            if (int.TryParse(input, out int transactionID))
            {
                transaction.TransactionID = transactionID;
            }

            Console.WriteLine("Date:");
            input = Console.ReadLine();
            if (DateTime.TryParse(input, out DateTime date))
            {
                transaction.Date = date;
            }

            Console.WriteLine("Action:");
            input = Console.ReadLine();
            if (Enum.TryParse(input, out TransactionAction action) && Enum.IsDefined(typeof(TransactionAction), action))
            {
                transaction.Action = action;
            }

            Console.WriteLine("Class:");
            input = Console.ReadLine();
            if (Enum.TryParse(input, out TransactionClass transactionClass) && Enum.IsDefined(typeof(TransactionClass), transactionClass))
            {
                transaction.Class = transactionClass;
            }

            Console.WriteLine("Ticker:");
            input = Console.ReadLine();
            if (input.Length > 0)
            {
                transaction.Symbol = input;
            }

            Console.WriteLine("Amount:");
            input = Console.ReadLine();
            if (decimal.TryParse(input, out decimal amount))
            {
                transaction.Amount = amount;
            }

            database.NewTransaction(transaction);
        }
        /// <summary>
        /// set the API key for the data feed
        /// </summary>
        /// <param name="quoteFeed"></param>
        private static void SetAPIKey(IAPIKeyQuoteFeed quoteFeed)
        {
            Console.WriteLine("APIKey:");
            quoteFeed.APIKey = Console.ReadLine();
        }
        /// <summary>
        /// Get the latest quote for a ticker
        /// </summary>
        /// <param name="quoteFeed"></param>
        private static void GetQuote(IAPIKeyQuoteFeed quoteFeed)
        {
            Console.WriteLine("Ticker:");
            string ticker = Console.ReadLine();

            Quote quote = quoteFeed.GetQuote(ticker);
            Console.WriteLine(quote.Price);

        }
        /// <summary>
        /// Set the price for a security
        /// </summary>
        /// <param name="database">database connection</param>
        private static void SetPrice(IDatabase database)
        {
            Console.WriteLine("Symbol:");
            string symbol = Console.ReadLine();

            Console.WriteLine("Date:");
            string input = Console.ReadLine();
            DateTime.TryParse(input, out DateTime date);

            Console.WriteLine("Price:");
            input = Console.ReadLine();
            decimal.TryParse(input, out decimal price);

            Quote quote = new Quote()
            {
                Symbol = symbol,
                Date = date,
                Price = price,
            };

            database.SaveQuote(quote);
        }
        /// <summary>
        /// update the prices in the database
        /// </summary>
        /// <param name="database">database</param>
        /// <param name="quoteFeed">quote feed</param>
        private static void UpdatePrices(IDatabase database, IQuoteFeed quoteFeed)
        {
            List<string> symbols = database.GetSymbols();

            foreach (string symbol in symbols)
            {
                Quote quote = quoteFeed.GetQuote(symbol);
                database.SaveQuote(quote);
            }
        }
        /// <summary>
        /// Represents a choice at the main menu
        /// </summary>
        private enum Choice
        {
            Create = 1, // manually set to 1 to set the start of the menu options to 1
            NewTransaction,
            SetAPIKey,
            GetQuote,
            SetPrice,
            UpdatePrices,
            Quit,
            Invalid,
        }
    }
}
