using System.Net;
using System.Net.Http.Json;
using CurrencyExchangeService.WebApi.DTOs;
using CurrencyExchangeService.WebApi.IntegrationTests.Fixtures;
using Xunit;

namespace CurrencyExchangeService.WebApi.IntegrationTests;

public class CurrencyControllerTests : IClassFixture<ServiceFixture>
{
    private readonly ServiceFixture _fixture;

    public CurrencyControllerTests(ServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task ValidInputDataWithoutCurrentDateReturnsOk()
    {
        using HttpClient client = _fixture.CreateClient();
        var dto = new ExchangeCurrencyRequestDto("CAD", "USD", 100);
        using HttpResponseMessage  response = await client.PostAsJsonAsync("/currency/exchange", dto);

        Assert.True(response.IsSuccessStatusCode);
        Assert.True(response.StatusCode == HttpStatusCode.OK);
    }

    [Theory]
    [MemberData(nameof(BadRequestTestCases))]
    public async Task InValidInputDataWithoutCurrentDateReturnsBadRequest(string fromCurrency, string toCurrency, decimal currencyAmount)
    {
        using HttpClient client = _fixture.CreateClient();
        var dto = new ExchangeCurrencyRequestDto(fromCurrency, toCurrency, currencyAmount);
        using HttpResponseMessage  response = await client.PostAsJsonAsync("/currency/exchange", dto);

        Assert.True(!response.IsSuccessStatusCode);
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
    }

    [Theory]
    [MemberData(nameof(RequestedSupportForCountryCodesTestCases))]
    public async Task ValidInputDataWithoutCurrentDateReturnsOkForRequestedCountryCodes(string fromCurrency, string toCurrency, decimal currencyAmount)
    {
        using HttpClient client = _fixture.CreateClient();
        var dto = new ExchangeCurrencyRequestDto(fromCurrency, toCurrency, currencyAmount);
        using HttpResponseMessage  response = await client.PostAsJsonAsync("/currency/exchange", dto);

        Assert.True(response.IsSuccessStatusCode);
        Assert.True(response.StatusCode == HttpStatusCode.OK);
    }

    [Theory]
    [MemberData(nameof(DateTestCases))]
    public async Task InputDataWithDateReturnsExpectedResult(string fromCurrency, string toCurrency, decimal currencyAmount, DateTime? date, bool isSuccess)
    {
        using HttpClient client = _fixture.CreateClient();
        var dto = new ExchangeCurrencyRequestDto(fromCurrency, toCurrency, currencyAmount, date);
        using HttpResponseMessage  response = await client.PostAsJsonAsync("/currency/exchange", dto);

        Assert.True(response.IsSuccessStatusCode == isSuccess);
    }
    public static IEnumerable<object[]> BadRequestTestCases
    {
        get
        {
            yield return new object[] { "CA", "CAD", 100m };
            yield return new object[] { "CAD", "CAD", 100m };
            yield return new object[] { "CAD", "USD", -10m};
            yield return new object[] { "CAD", "USDA", 100m };
        }
    }

    public static IEnumerable<object[]> RequestedSupportForCountryCodesTestCases
    {
        get
        {
            yield return new object[] { "CAD", "USD", 100.100m };
            yield return new object[] { "CAD", "EUR", 100.100m };
            yield return new object[] { "CAD", "JPY", 100.100m };
            yield return new object[] { "CAD", "GBP", 100.100m };
            yield return new object[] { "CAD", "AUD", 100.100m };
            yield return new object[] { "CAD", "CHF", 100.100m };
            yield return new object[] { "CAD", "CNY", 100.100m };
            yield return new object[] { "CAD", "HKD", 100.100m };
            yield return new object[] { "CAD", "MXN", 100.100m };
            yield return new object[] { "USD", "CAD", 1m };
            yield return new object[] { "USD", "CAD", 0.35m };
            yield return new object[] { "USD", "CAD", 0m };
        }
    }

    public static IEnumerable<object[]> DateTestCases
    {
        get
        {
            yield return new object[] { "CAD", "USD", 100.100m, DateTime.Now.AddDays(1),false };
            yield return new object[] { "CAD", "USD", 100.100m, new DateTime(2022,6,3),true };
        }
    }
}
