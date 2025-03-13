## Crypto Portfolio Calculator Web Application

This is a web application for calculating cryptocurrency portfolio value using real-time data from Coinlore's API(https://www.coinlore.com/cryptocurrency-data-api).
- 📂 **Upload a file** containing their cryptocurrency holdings.  
- 📉 **Calculate portfolio value**, including initial and current values.  
- 🔄 **Track price changes** every 5 minutes (configurable).  
- 📝 **Log all performed operations** to a file.  

## Prerequisites

Before running the solution, make sure you have the following installed:

- [.NET 8](https://dotnet.microsoft.com/download/dotnet)
- A code editor such as [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

## Clone the Repository

Start by cloning the repository to your local machine:

```bash
git clone https://github.com/pdimov1/crypto-portfolio-calculator.git
```

## Run the Application

- Build and run CryptoPortfolioCalculator.API project
- Get the API localhost URL and paste it in web app appsettings.json -> ApiEndpoint within CryptoPortfolioCalculator project
- Build and run CryptoPortfolioCalculator project

## File Upload Format
The supported file extensions are txt and csv delimited by a | ( pipe ) symbol.
The uploaded file should contain cryptocurrency holdings in the following format:

```bash
X|COIN|Y
X|COIN|Y
```

Format Explanation
X → Number of coins owned (XXX.XXXX format).
COIN → Cryptocurrency symbol (BTC, ETH, USDT, etc.).
Y → Initial buy price per coin (Y.YYYY format).

Example File Contents
```bash
10|ETH|123.14
12.12454|BTC|24012.43
```
