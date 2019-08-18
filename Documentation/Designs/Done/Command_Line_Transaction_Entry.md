# Problem Statement
Need to analyze and retrieve past transactions.

# High Level Design
Create a command line entry system for transaction data

# Considered, But Not Done
* GUI. For now, command line is a quick way to get started and create APIs that can be reused for more advanced UIs in the future.

# Detailed Design
We will add an option to the main command line menu. 

# Technical Design
Add an interface member to IDatabase, to add a transaction. We will also add a Transaction object to the core data model with the same items defined in the database. It will be the responsibility of the database implementation to take the Transaction object and translate it into a database insert.
