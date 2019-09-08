namespace Core
{
    public class TransactionEnums
    {
        /// <summary>
        /// actions describe the transaction interaction with the cash account.
        /// </summary>
        public enum TransactionAction
        {
            /// <summary>
            /// buy results in a purchased asset and spent cash
            /// </summary>
            buy = 1,
            /// <summary>
            /// sell results in a sold asset and recieved cash
            /// </summary>
            sell = 2,
            /// <summary>
            /// deposit from another account
            /// </summary>
            deposit = 3,
            /// <summary>
            /// withdrawal to another account
            /// </summary>
            withdrawal = 4,
            /// <summary>
            /// dividend received
            /// </summary>
            dividend = 5,
            /// <summary>
            /// interest received
            /// </summary>
            interest = 6,
        }

        /// <summary>
        /// classes describe additional type information about a transaction
        /// </summary>
        public enum TransactionClass
        {
            /// <summary>
            /// both sides of a transaction are cash (i.e. a transfer)
            /// </summary>
            cash = 1,
            /// <summary>
            /// transaction is related to a stock
            /// </summary>
            stock = 2,
            /// <summary>
            /// transaction is related to a bond
            /// </summary>
            bond = 3,
        }
    }
}
