# Automatic Fund Revoking
This project is an innovative financial application for revoking the bought funds automatically (24/7)


**This project uses:**
- elastic search for logging
- redis service for caching
- sql server database
- this app has some jobs which call Rayan/pasargad bank apis
- #saga pattern to run a financial transaction over several APIs from two companies
