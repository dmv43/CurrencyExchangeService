using CurrencyExchangeService.WebApi.DTOs;
using FluentValidation;

namespace CurrencyExchangeService.WebApi.Validators;

public class ExchangeCurrencyRequestDtoValidator : AbstractValidator<ExchangeCurrencyRequestDto>
{
    public ExchangeCurrencyRequestDtoValidator()
    {
        RuleFor(x => x.FromCurrency).NotEmpty().MinimumLength(3).MaximumLength(3);
        RuleFor(x => x.ToCurrency).NotEmpty().NotEqual(x=> x.FromCurrency).MinimumLength(3).MaximumLength(3);
        RuleFor(x => x.CurrencyAmount).GreaterThanOrEqualTo(0);
    }
}
