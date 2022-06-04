using CurrencyExchangeService.WebApi.DTOs;
using MediatR;

namespace CurrencyExchangeService.WebApi.Application.Commands;

public sealed class ExchangeCurrencyCommand : IRequest<ExchangeResultDto?>
{
    public ExchangeCurrencyCommand(ExchangeCurrencyRequestDto exchangeCurrencyRequestDto)
    {
        FromCurrency = exchangeCurrencyRequestDto.FromCurrency;
        ToCurrency = exchangeCurrencyRequestDto.ToCurrency;
        CurrencyAmount = exchangeCurrencyRequestDto.CurrencyAmount;
        EffectiveDate = exchangeCurrencyRequestDto.EffectiveDate;
    }

    public string FromCurrency { get; }
    public string ToCurrency { get; }
    public decimal CurrencyAmount { get; }
    public DateTime? EffectiveDate { get; }
}
