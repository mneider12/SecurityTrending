using Core;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataEntry
{
    public interface ICashTransactionLoader
    {
        List<Transaction> LoadTransactions(Account account, FileInfo fileInfo);
    }
}
