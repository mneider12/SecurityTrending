Problem Statement:
Need a long term storage solution to allow a user to persist data across sessions. The data to be entered and stored need to be sufficient to calculate return on investment for individual transactions as an annualized rate. 

Background Research:
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

SQLite Datatypes:
https://www.sqlite.org/datatype3.html
NULL, INTEGER, REAL, TEXT, BLOB
DATETIMEs can be stored as TEXT, INTEGER, or REAL. Advantages / disadvantages here: https://stackoverflow.com/questions/17227110/how-do-datetime-values-work-in-sqlite

High Level Design:
We will store transaction level data that can be used to build the time weighted periods as needed.

Considered, but Not Done:
Storing period performance data. We could, but that immediately introduces a second source of truth to keep in sync. The raw data gives more flexibility to calculate other data of interest.

Detailed Design:
There will be a new database to hold application data. At this point, the plan is to use a single database, with multiple tables as needed. There will be a new Transactions table. 

Technical Design:
We will use SQLite as the backend. It is simple to use and allows familiar SQL accesses.

https://www.sqlite.org/index.html