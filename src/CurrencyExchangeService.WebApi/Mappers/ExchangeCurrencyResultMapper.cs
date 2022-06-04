using System.Text.Json;
using CurrencyExchangeService.Client.DTOs;
using CurrencyExchangeService.Contracts.Models;

namespace CurrencyExchangeService.WebApi.Mappers;

public static class ExchangeCurrencyResultMapper
{
    public static ExchangeRateResult? ExchangeRateResult(this SeriesObservationDto seriesObservationDto, string seriesName)
    {
        if (!seriesObservationDto.Observations.Any())
        {
            return null;
        }

        JsonElement rootElement = seriesObservationDto.Observations.First().RootElement;
        bool canGetDateTime = rootElement.GetProperty("d").TryGetDateTime(out DateTime reportedDate);
        string? exchangeRateString = rootElement.GetProperty(seriesName).GetProperty("v").GetString();
        bool canGetExchangeRate = decimal.TryParse(exchangeRateString, out decimal exchangeRate);

        if (canGetDateTime && canGetExchangeRate)
        {
            return new ExchangeRateResult(reportedDate, exchangeRate);
        }

        return null;
    }
}
