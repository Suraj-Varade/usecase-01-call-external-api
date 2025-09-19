using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Diagnostics;
using Polly;
using Microsoft.Extensions.Http;
using usecase_01_call_external_api.Abstractions;
using usecase_01_call_external_api.Clients;
using usecase_01_call_external_api.Middlewares;

namespace usecase_01_call_external_api.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //services.AddScoped<RequestLoggingMiddleware>();
        services.AddScoped<RequestLoggingMiddleware>();
        
        services.AddHttpClient<IExternalApiClient, ExternalApiClient>(client =>
        {
            client.BaseAddress = new Uri($"https://jsonplaceholder.typicode.com/");
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        })
        .AddPolicyHandler((sp, request) =>
        {
            var logger = sp.GetRequiredService<ILogger<RequestLoggingMiddleware>>();
            return Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .OrResult(r => r.StatusCode == HttpStatusCode.NotFound) //added this for retry test.
                .WaitAndRetryAsync(
                    3,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (outcome, timeSpan, retryCount, context) =>
                    {
                        if (outcome.Exception != null)
                        {
                            logger.LogWarning(
                                outcome.Exception,
                                "Retry {RetryAttempt} after {Delay}s due to exception: {Message}",
                                retryCount,
                                timeSpan.TotalSeconds,
                                outcome.Exception.Message);
                        }
                        else
                        {
                            logger.LogWarning(
                                "Retry {RetryAttempt} after {Delay}s due to status code: {StatusCode}",
                                retryCount,
                                timeSpan.TotalSeconds,
                                outcome.Result.StatusCode);
                        }
                    });
        });
        return services;
    }
}