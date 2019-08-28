# Problem Statment
Program needs a place to store persistent settings. The first setting we need is to save the API key for Alpha Vantage.

# Background Research
## Alpha Vantage
[Alpha Vantage](https://www.alphavantage.co/) appears to be a free API for accessing security data such as stock quotes. The API is supposed to be used no more than 5 times per minute, or 500 times per day. [FAQ](https://www.alphavantage.co/support/#api-key). There are options to upgrade to a premium account if needed.

Alpha Vantage requires a free API key in order to access it. In order to keep the key secure, it cannot be included in source code.
## Serialization
There are two main types of serialization - binary and XML. Binary is more compact, XML results in a human and machine readable file. I don't anticipate a large raw number of settings, or accessing them frequently, so the benefits of a human readable file outwiegh the costs.

[How To: Write Object Data to an XML File (C#)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/serialization/how-to-write-object-data-to-an-xml-file)

# High Level Design
We will use an XML serializer to save the Alpha Vantage API Key to a local config file the does not get synced to SVN. The setup will be accessed from the command line program for now.

# Considered, But Not Done
* include the key directly in code - that's just asking to be abused
* encrypt the API key - not sensitive enough to worry about, and we'd still have a problem of storing the encryption key somewhere.
