using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Core;
using Model;

namespace DataEntry
{
    public class CSVTransactionLoader: ICashTransactionLoader
    {
        public List<Transaction> LoadTransactions(Account account, FileInfo fileInfo)
        {
            List<Transaction> transactions = new List<Transaction>();

            using (StreamReader reader = fileInfo.OpenText())
            {
                int lineCount = 0;
                string inputLine;
                while ((inputLine = reader.ReadLine()) != null)
                {
                    lineCount++;
                    string[] inputPieces = inputLine.Split(',');
                    if (inputPieces.Length != 2)
                    {
                        string message = string.Format(CultureInfo.CurrentCulture, "Invalid file format on line {0}", lineCount);
                        throw new FileLoaderException(message);
                    }

                    if (!DateTime.TryParse(inputPieces[0], out DateTime dateTime))
                    {
                        string message = string.Format(CultureInfo.CurrentCulture, "Invalid date format on line {0}", lineCount);
                        throw new FileLoaderException(message);
                    }

                    if (!decimal.TryParse(inputPieces[1], out decimal amount))
                    {
                        string message = string.Format(CultureInfo.CurrentCulture, "Invalid amount format on line {0}", lineCount);
                        throw new FileLoaderException(message);
                    }

                    TransactionEnums.TransactionAction action = amount > 0 ? TransactionEnums.TransactionAction.deposit : TransactionEnums.TransactionAction.withdrawal;

                    transactions.Add(new Transaction()
                    {
                        Account = account,
                        Action = action,
                        Amount = amount,
                        Class = TransactionEnums.TransactionClass.cash,
                        Date = dateTime,
                        Quantity = 0m,
                        Symbol = "",
                    });
                }
            }

            return transactions;
        }
    }
}
