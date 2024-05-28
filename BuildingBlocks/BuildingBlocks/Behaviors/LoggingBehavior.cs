using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
  : IPipelineBehavior<TwoFactorRequest, TResponse>
  where TRequest : notnull, IRequest<TResponse>
  where TResponse : notnull
{
  readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger
    ?? throw new ArgumentNullException(nameof(logger));
  public async Task<TResponse> Handle(TwoFactorRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    _logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={RequestData}",
                           typeof(TRequest).Name,
                           typeof(TResponse).Name,
                           request);
    var timer = new Stopwatch();
    timer.Start();
    var response = await next();
    timer.Stop();
    var timeTaken = timer.Elapsed;
    if (timeTaken.Seconds > 3)
      _logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken}", typeof(TRequest).Name, timeTaken.Seconds);
  }
}