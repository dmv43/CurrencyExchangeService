using CurrencyExchangeService.Client.DTOs;
using Refit;
namespace CurrencyExchangeService.Client;

public interface IBankOfCanadaValetApiClient
{
    [Get("/observations/{seriesName}")]
    Task<SeriesObservationDto> GetSeriesObservationAsync(string seriesName,[AliasAs("start_date")] string? startDate = null, [AliasAs("end_date")]string? endDate = null, [AliasAs("recent")] int? amountOfRecentRecords = null, CancellationToken cancellationToken = default);
}
