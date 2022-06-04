# CurrencyExchangeService
### Service Converts between currencies.

To start the service simply run CurrencyExchangeService.WebApi
It has one endpoint at the address ```currency/exchange```.
#### Example POST request body.
```
{
  "fromCurrency": "CAD",
  "toCurrency": "USD",
  "currencyAmount": 100.35,
  "effectiveDate": "2022-06-03"
}
```
Effective date can be omitted, which would mean date would be current if applicable or most recent date for which there is conversion rate.
