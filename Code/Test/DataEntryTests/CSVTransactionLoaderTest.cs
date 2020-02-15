using System;
using System.Collections.Generic;
using System.IO;
using Core;
using DataEntry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace DataEntryTests
{
    [TestClass]
    public class CSVTransactionLoaderTest
    {
        [TestMethod]
        public void LoadTransactionsTest()
        {
            FileInfo fileInfo = new FileInfo("CashTransactionsTest.txt");
            Account account = new Account();
            ICashTransactionLoader loader = new CSVTransactionLoader();

            List<Transaction> expectedTransactions = new List<Transaction>()
            {
                new Transaction()
                {
                    Account = account,
                    Action = TransactionEnums.TransactionAction.deposit,
                    Amount = 10000,
                    Class = TransactionEnums.TransactionClass.cash,
                    Date = new DateTime(2000, 1, 1),
                    Quantity = 0m,
                    Symbol = "",
                    TransactionID = 0,
                },

                new Transaction()
                {
                    Account = account,
                    Action = TransactionEnums.TransactionAction.deposit,
                    Amount = 5000,
                    Class = TransactionEnums.TransactionClass.cash,
                    Date = new DateTime(2000, 2, 1),
                    Quantity = 0m,
                    Symbol = "",
                    TransactionID = 0,
                },

                new Transaction()
                {
                    Account = account,
                    Action = TransactionEnums.TransactionAction.withdrawal,
                    Amount = -7000,
                    Class = TransactionEnums.TransactionClass.cash,
                    Date = new DateTime(2000, 3, 1),
                    Quantity = 0m,
                    Symbol = "",
                    TransactionID = 0,
                }
            };

            List<Transaction> actualTransactions = loader.LoadTransactions(account, fileInfo);

            for (int i = 0; i < 3; i++)
            {
                Assert.IsTrue(AreTransactionsEquivalent(expectedTransactions[i], actualTransactions[i]));
            }
            
        }

        private bool AreTransactionsEquivalent(Transaction transaction1, Transaction transaction2)
        {
            if (transaction1.Amount == transaction2.Amount && transaction1.Account == transaction1.Account &&
                transaction1.Action == transaction2.Action && transaction1.Class == transaction2.Class &&
                transaction1.Date == transaction2.Date && transaction1.Quantity == transaction2.Quantity &&
                transaction1.Symbol == transaction2.Symbol && transaction1.TransactionID == transaction2.TransactionID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
