using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    /// <summary>
    /// define the necessary functions for database interaction on a SQL database.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Create the database, including table definitions
        /// </summary>
        void CreateDatabase();
        /// <summary>
        /// Add a new transaction to the database
        /// </summary>
        /// <param name="transaction">transaction to add</param>
        void NewTransaction(Transaction transaction);
        /// <summary>
        /// Add a price to the database
        /// </summary>
        /// <param name="quote">a quote with the price, symbol, and date minimally set</param>
        void SetPrice(Quote quote);
    }
}
