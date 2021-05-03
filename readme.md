# MyBank Web API

This is the MyBank Web API which partners with the client app to provide a simple banking application. Basic logging to the console window is provided.

A very simple SQL table named `transaction` stores a record of all transactions on the account. This simple approach enabled me  focus my time on the API and client app. In a production application there would be tables to store user details, the accounts associated with those users and transactions would be assigned to a specific account.

Inline SQL has been used to speed development together with Dapper providing a very light weight data access layer. Inline SQL is unsuitable for a production environment due to potential injection threats. 

## Installation

Download or clone the repo.

In the MyBank.API\Scripts is a sql script `CreateTransactionTable.sql` which should be run to create the transaction table you require for the app to work. 

Within the API the connection string can be found and altered in appsettings.json, it assumes a SQL server will be running locally with a database named `test`

```json
"ConnectionStrings": {
        "DefaultConnection": "Data Source=(local); Initial Catalog=test;MultipleActiveResultSets=False;Integrated Security=True;"
    },
```

The API used to obtain FX rates and the base currency of the application are configurable 

```json
   "MyBankSettings": {
        "RatesUrl": "https://api.ratesapi.io/api/latest",
        "BaseCCY":  "GBP"
    },
```

## Usage

The API is configured to run on port 19664, this can be changed by altering the port setting in appsettings.Development.json

Open a command prompt in the MyBank.API folder. Run the following command

```
dotnet run MyBank.API.csproj
```

## Tests

xUnit tests are divided into BankingServiceTests and FXTests within the MyBank.Tests project


## Contributing
Pull requests are welcome. Please contact me to suggest changes.

## License
[MIT](https://choosealicense.com/licenses/mit/)