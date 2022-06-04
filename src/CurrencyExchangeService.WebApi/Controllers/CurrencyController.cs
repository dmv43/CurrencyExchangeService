using CurrencyExchangeService.WebApi.Application.Commands;
using CurrencyExchangeService.WebApi.DTOs;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchangeService.WebApi.Controllers;

[Route("currency")]
public class CurrencyController : ControllerBase
{
    private readonly IMediator _mediator;
    public CurrencyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Exchanges Currencies by accepting two ISO 4217 currency codes, sum of money to exchange and an optional date of exchange.
    /// </summary>
    /// <returns>ExchangeResultDto</returns>
    [HttpPost]
    [Route("exchange")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExchangeResultDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ActionResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ActionResult))]
    public async Task<ActionResult<ExchangeResultDto>> Post(
        [FromBody] ExchangeCurrencyRequestDto? exchangeCurrencyRequestDto,
        [FromServices] IValidator<ExchangeCurrencyRequestDto> validator)
    {
        if (exchangeCurrencyRequestDto is null)
        {
            return BadRequest();
        }

        ValidationResult? validationResult = await validator.ValidateAsync(exchangeCurrencyRequestDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        ExchangeResultDto? result = await _mediator.Send(new ExchangeCurrencyCommand(exchangeCurrencyRequestDto));
        if (result is null)
        {
            return NotFound();
        }

        return result;
    }
}
