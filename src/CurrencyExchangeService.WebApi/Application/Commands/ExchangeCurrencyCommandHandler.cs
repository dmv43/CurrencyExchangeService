using CurrencyExchangeService.Client;
using CurrencyExchangeService.Client.DTOs;
using CurrencyExchangeService.Contracts.Models;
using CurrencyExchangeService.WebApi.DTOs;
using CurrencyExchangeService.WebApi.Mappers;
using MediatR;

namespace CurrencyExchangeService.WebApi.Application.Commands;

public class ExchangeCurrencyCommandHandler : IRequestHandler<ExchangeCurrencyCommand, ExchangeResultDto?>
{
    private readonly IBankOfCanadaValetApiClient _apiClient;

    public ExchangeCurrencyCommandHandler(IBankOfCanadaValetApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<ExchangeResultDto?> Handle(ExchangeCurrencyCommand request, CancellationToken cancellationToken)
    {
        string seriesName = GetSeriesName(request.FromCurrency, request.ToCurrency);
        string? effectiveDateString = request.EffectiveDate?.ToString("yyyy-MM-dd");
        SeriesObservationDto seriesObservationDto;

        try
        {
            seriesObservationDto = request.EffectiveDate is null
                ? await _apiClient.GetSeriesObservationAsync(
                    seriesName,
                    amountOfRecentRecords: 1,
                    cancellationToken: cancellationToken)
                : await _apiClient.GetSeriesObservationAsync(
                    seriesName,
                    effectiveDateString,
                    effectiveDateString,
                    cancellationToken: cancellationToken);
        }
        catch (Refit.ApiException)
        {
            return null;
        }

        ExchangeRateResult? exchangeRateResult = seriesObservationDto.ExchangeRateResult(seriesName);

        if (exchangeRateResult is null)
        {
            return null;
        }

        decimal convertedAmount = request.CurrencyAmount * exchangeRateResult.ExchangeRate;
        return new ExchangeResultDto(decimal.Round(convertedAmount, 4), exchangeRateResult.ExchangeDate, request.ToCurrency);
    }

    private static string GetSeriesName(string fromCurrency, string toCurrency)
    {
        return $"FX{fromCurrency.ToUpperInvariant()}{toCurrency.ToUpperInvariant()}";
    }
}
