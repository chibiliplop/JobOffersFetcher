# JobOffersFetcher

JobOffersFetcher is a console application designed to fetch job offers from the FranceTravail API, process the data, and provide various statistics.

## Configuration

The application configuration is managed through the `appsettings.json` file. Key configurations include:

- **ConnectionStrings**: Database connection string. Default value will create a SQLite database in the project directory no need to change for testing purpose.
- **FranceTravailApi**: API base URLs and credentials. You will need to obtain a client ID and secret from FranceTravail to access the API. Ensure the API have access to "o2dsoffre api_offresdemploiv2" scope.

Example `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "JobOfferConnection": "Data Source=JobOffersFetcher.db;"
  },
  "FranceTravailApi": {
    "AuthBaseUrl": "https://entreprise.francetravail.fr",
    "ApiBaseUrl": "https://api.francetravail.io",
    "ClientId": "your_client_id",
    "ClientSecret": "your_client_secret"
  }
}
```

## Installation

1. **Clone the repository**:
    ```sh
    git clone https://github.com/yourusername/JobOffersFetcher.git
    cd JobOffersFetcher
    ```
   
2. **Build the project**:
   The Application rely on cli commands "dotnet run --project JobOffersFetcher.Console" will interfere with parameters. To avoid that it is recommended to publish the project
    ```sh
    dotnet publish -c Release
    ```



## Usage

1. **Run the application**:
   Go to the project publish directory and run the application:
    ```sh
    .\JobOffersFetcher.Console.exe -h
    ```

2. **Execute commands**:
   The application uses CLI commands. For example, to fetch job offers for Paris,Bordeaux and Rennes and store them in the database, run:
    ```sh
    .\JobOffersFetcher.Console.exe fetch -c 33063 35238 -d 75
    ```
   To display statistics for the stored job offers, run:
    ```sh
    .\JobOffersFetcher.Console.exe stats
    ```
    For more information on available commands, run:
     ```sh 
     .\JobOffersFetcher.Console.exe --help
   ```
   For more information on specific commands, run:
     ```sh 
     .\JobOffersFetcher.Console.exe {commandName} --help
   ```
   
## Known issues

- The FranceTravail API has limitation to 3000 item per request. The application will fetch the first 3000 items for each commune/departement.
- The FranceTravail API has limitation with number of concurrent request. The application is configured to limit concurrent request to 2. You can change the value if you still have any issue in the appsettings.json file by adding "MaxConcurrentRequests" : 1 in FranceTravailApi section.
- The Application do not delete the old data from the database every fetch is incremental only new offre is added. You can delete the database file to start fresh.
