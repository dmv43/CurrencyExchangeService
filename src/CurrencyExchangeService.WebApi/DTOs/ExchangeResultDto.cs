namespace CurrencyExchangeService.WebApi.DTOs;

public record ExchangeResultDto(decimal ConvertedRateAmount, DateTime ConversionDate, string Currency);
