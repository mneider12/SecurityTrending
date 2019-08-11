# Problem Statement:
Need a long term storage solution to allow a user to persist data across sessions. The data to be entered and stored need to be sufficient to calculate return on investment for individual transactions as an annualized rate. 

# Background Research:
The formula for calculating a single transaction's performance is:

ROI = (Current Value of Investment - Cost of Investment) / Cost of Investment

https://www.investopedia.com/terms/r/returnoninvestment.asp

ROI is interesting, but is biased by length of time and inflows and outflows of cash. In order to compare performance over time across portfolios, we need to adjust for time.

TWR = [(1 + HP1) * (1 + HP2) * ... * (1 + HPN)] - 1
where:
TWR = Time weighted return
n = Number of sub-periods
HP = (End Value - Initial Value + Cash Flow) / (Initial Value + Cash Flow)
HPN = Return for sub-period n

https://www.investopedia.com/terms/t/time-weightedror.asp

## SQLite:
SQLite is simple and free to use. It allows us to use familiar SQL syntax for database access.
https://www.sqlite.org/index.html

### Datatypes:
https://www.sqlite.org/datatype3.html
NULL, INTEGER, REAL, TEXT, BLOB
DATETIMEs can be stored as TEXT, INTEGER, or REAL. Advantages / disadvantages here: https://stackoverflow.com/questions/17227110/how-do-datetime-values-work-in-sqlite

### Conventions:
https://www.xaprb.com/blog/2008/10/26/the-power-of-a-good-sql-naming-convention/
https://stackoverflow.com/questions/7662/database-table-and-column-naming-conventions
Top takeaways:
Pascal case for table and column names. ID columns should be named by the table + ID. Column names with the same data as in another table should be named the same.

# High Level Design:
We will store transaction level data that can be used to build the time weighted periods as needed.

# Considered, but Not Done:
Storing period performance data. We could, but that immediately introduces a second source of truth to keep in sync. The raw data gives more flexibility to calculate other data of interest.

# Detailed Design:
There will be a new database to hold application data. At this point, the plan is to use a single database, with multiple tables as needed. There will be a new Transactions table. The fields will be as follows:



# Technical Design:
We will use SQLite as the backend.
