namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
  readonly ILogger<CustomExceptionHandler> _logger = logger
    ?? throw new ArgumentNullException(nameof(logger));
  public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
  {
    _logger.LogError("Erro MessagE: {exceptionMessage}, Time of iccurrence {time}", exception.Message, DateTime.UtcNow);

    (string Detail, string Title, int StatusCode) = exception switch
    {
      InternalServerException =>
      (
      exception.Message,
      exception.GetType().Name,
      httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
      ),
      ValidationException =>
      (
        exception.Message,
        exception.GetType().Name,
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
      ),
      BadRequestException =>
      (
        exception.Message,
        exception.GetType().Name,
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
      ),
      NotFoundException =>
      (
        exception.Message,
        exception.GetType().Name,
        httpContext.Response.StatusCode = StatusCodes.Status404NotFound
      ),
      _ =>
      (
        exception.Message,
        exception.GetType().Name,
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
      )
    };

    var problemDetails = new ProblemDetails
    {
      Title = Title,
      Detail = Detail,
      Status = StatusCode,
      Instance = httpContext.Request.Path
    };

    problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

    if (exception is ValidationException validationException)
      problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);

    await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

    return true;
  }
}
