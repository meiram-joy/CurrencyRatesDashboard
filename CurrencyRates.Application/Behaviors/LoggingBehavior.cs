using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CurrencyRates.Application.Behaviors;

public class LoggingBehavior<TRequest,TResponse>  : IPipelineBehavior<TRequest, TResponse> where TResponse : IResult
{
    private readonly ILogger<LoggingBehavior<TRequest,TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Request {@RequestName}, {@DateTimeUtc}", typeof(TRequest).Name, DateTime.UtcNow);
        
        var result = await next();
        
        if (result.IsFailure)
        {
            _logger.LogError("Request failure {@RequestName},{@DateTimeUtc} ", typeof(TRequest).Name,DateTime.UtcNow);
        }
        _logger.LogInformation("Finished Request {@RequestName}", typeof(TRequest).Name);
        
        return result;
    }
}