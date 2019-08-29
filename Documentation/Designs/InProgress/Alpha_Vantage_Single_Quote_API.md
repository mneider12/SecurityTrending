# Problem Statement
Need to be able to get a current stock price to calculate current position values

# Background Research
Alpha Vantage is a free web API to get stock prices.  
APIs are documented [here](https://www.alphavantage.co/documentation/).
The Global Quote API returns the latest price information for a security.

# High Level Design
We will call the global quote Alpha Vantage API to get the latest stock price information.  
For now, we will just provide latest quote access from the command line.

# Considered, But Not Done
Grab time series data - this could eventually be useful for trending, but is not needed for the baseline performance calculation, which relies on transaction prices, not historical quotes.

# Detailed Design

# Technical Design
New project DataFeed.csproj for data feeds