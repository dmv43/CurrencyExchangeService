using System.Text.Json;
using System.Text.Json.Serialization;

namespace CurrencyExchangeService.Client.DTOs;

public class SeriesObservationDto
{
    public SeriesObservationDto(List<JsonDocument> observations)
    {
        Observations = observations;
    }

    [JsonPropertyName("observations")]
    public List<JsonDocument> Observations { get; }
}
