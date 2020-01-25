using Core;
using Model;
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
        /// Add a quote to the database
        /// </summary>
        /// <param name="quote">a quote with the price, symbol, and date minimally set</param>
        void SaveQuote(Quote quote);
        /// <summary>
        /// deprecated. Use SaveQuote instead
        /// </summary>
        /// <param name="quote"></param>
        [Obsolete("Use SaveQuote instead")]
        void SetPrice(Quote quote);
        /// <summary>
        /// get the latest quote stored in the database
        /// </summary>
        /// <param name="symbol">symbol</param>
        /// <returns>latest price stored in the database</returns>
        Quote GetLastQuote(string symbol);
        /// <summary>
        /// get a list of the symbols relevant to a portfolio
        /// </summary>
        /// <returns>list of symbols</returns>
        List<string> GetSymbols();
        /// <summary>
        /// get one position
        /// </summary>
        /// <param name="symbol">symbol for position to retrieve</param>
        /// <returns>position</returns>
        Position GetPosition(string symbol);
        /// <summary>
        /// get the positions in the portfolio
        /// </summary>
        /// <returns>list of positions</returns>
        List<Position> GetPositions();
        /// <summary>
        /// set a position into the database
        /// </summary>
        /// <param name="postion">position</param>
        void SetPosition(Position postion);
    }
}
