namespace CurrencyExchangeService.WebApi.DTOs;

public sealed record ExchangeCurrencyRequestDto(string FromCurrency, string ToCurrency, decimal CurrencyAmount, DateTime? EffectiveDate = null);

