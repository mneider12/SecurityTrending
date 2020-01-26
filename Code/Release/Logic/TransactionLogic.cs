using Core;
using Database;
using Model;
using System;
using System.Diagnostics.Contracts;
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
            if (database != null && transaction != null)
            {
                database.NewTransaction(transaction);
                Position position = database.GetPosition(transaction.Symbol);

                if (position.Class == TransactionClass.undefined)   // position is new
                {
                    position.Class = transaction.Class;
                }

                switch (transaction.Action)
                {
                    case TransactionAction.buy:
                        position.Quantity += transaction.Quantity;
                        break;
                    case TransactionAction.sell:
                        position.Quantity -= transaction.Quantity;
                        break;
                    default:
                        throw new Exception(Resources.NotImplementedException);
                }

                if (IsTransactionValid(position, transaction, out string errorMessage))
                {
                    database.SetPosition(position);
                }
                else
                {
                    throw new LogicException(errorMessage);
                }
            } 
        }
        /// <summary>
        /// validate that a transaction is valid to apply to a position
        /// </summary>
        /// <param name="position">position being applied to</param>
        /// <param name="transaction">transaction being applied</param>
        /// <param name="errorMessage">error message if transaction is invalid</param>
        /// <returns>whether the transaction can be applied to the position</returns>
        private static bool IsTransactionValid(Position position, Transaction transaction, out string errorMessage)
        {
            errorMessage = null;
            bool hasError = false;

            if (position.Class != transaction.Class)
            {
                errorMessage = "Cannot override existing position's class";
                hasError = true;
            }
            else if (position.Quantity < 0)
            {
                errorMessage = "Position quantity must be positive";
                hasError = true;
            }

            return !hasError;
        }
    }
}
