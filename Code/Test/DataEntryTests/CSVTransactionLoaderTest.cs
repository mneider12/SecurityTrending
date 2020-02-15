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

            List<Transaction> transactions = loader.LoadTransactions(account, fileInfo);


        }
    }
}
