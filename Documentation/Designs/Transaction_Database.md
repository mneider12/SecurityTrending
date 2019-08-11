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

