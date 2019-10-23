using Core;
using Database;
using Model;
using System;
using System.Diagnostics;
using static Core.TransactionEnums;

namespace Logic
{
    /// <summary>
    /// Logic for handling transaction data integrity
    /// </summary>
    public static class TransactionLogic
    {
        /// <summary>
        /// Commit a new transaction and update or create the related position
        /// </summary>
        /// <param name="database">database to commit to</param>
        /// <param name="transaction">transaction to commit</param>
        public static void CommitTransaction(IDatabase database, Transaction transaction)
        {
            database.NewTransaction(transaction);
            Position position = database.GetPosition(transaction.Symbol);

            if (position.Class == TransactionClass.undefined)   // position is new
            {
                position.Class = transaction.Class;
            }

            Debug.Assert(position.Class == transaction.Class, "Cannot override existing position class");

            switch (transaction.Action)
            {
                case TransactionAction.buy:
                    position.Shares += transaction.Quantity;
                    break;
                default:
                    throw new Exception("Not implemented");
            }
            

            database.SetPosition(position);
        }
    }
}
