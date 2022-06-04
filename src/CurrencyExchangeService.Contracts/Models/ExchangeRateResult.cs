namespace CurrencyExchangeService.Contracts.Models;

public class ExchangeRateResult
{
    public ExchangeRateResult(DateTime date, decimal exchangeRate)
    {
        ExchangeRate = exchangeRate;
        ExchangeDate = date;
    }

    public decimal ExchangeRate { get; }
    public DateTime ExchangeDate { get; }
}
