using System.Net;
using System.Reflection;
using CurrencyExchangeService.Client;
using CurrencyExchangeService.Client.Configurations;
using CurrencyExchangeService.WebApi.Validators;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Refit;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.Configure<BankOfCanadaValetClientOptions>(builder.Configuration.GetSection(BankOfCanadaValetClientOptions.SectionName));
builder.Services.AddMvcCore().AddFluentValidation(v => v.RegisterValidatorsFromAssemblyContaining<ExchangeCurrencyRequestDtoValidator>());
builder.Services.AddRefitClient<IBankOfCanadaValetApiClient>()
    .ConfigureHttpClient(
        (serviceProvider, client) =>
        {
            BankOfCanadaValetClientOptions options = serviceProvider.GetRequiredService<IOptions<BankOfCanadaValetClientOptions>>().Value;
            client.BaseAddress = GetClientUri(options.ApiEndpoint);
        })
    .AddPolicyHandler(GetClientCircuitBreakerPolicy());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();

static Uri? GetClientUri(string url)
{
    if (string.IsNullOrWhiteSpace(url) || !Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
    {
        throw new ArgumentException("Value must be a valid absolute Uri.", nameof(url));
    }

    return uri;
}

static IAsyncPolicy<HttpResponseMessage> GetClientCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(m => m.StatusCode == HttpStatusCode.TooManyRequests)
        .CircuitBreakerAsync(
            3,
            TimeSpan.FromSeconds(30));
}

public partial class Program
{
    // Expose the Program class for use with WebApplicationFactory<T>
}
