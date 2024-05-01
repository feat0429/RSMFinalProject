using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Extensions;
using System.Diagnostics;

namespace RSMFinalProject.API.ExceptionHandler
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            string? traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

            LogException(exception, traceId);

            var (statusCode, title) = MapException(exception);

            if (exception is ValidationException)
            {
                var validationException = (ValidationException)exception;

                var errorsDictionary = validationException.Errors
                    .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                    .ToDictionary(f => f.Key, f => f.ToArray());

                await Results.ValidationProblem(
                    title: title,
                    statusCode: statusCode,
                    errors: errorsDictionary,
                    extensions: new Dictionary<string, object?>
                    {
                        {nameof(traceId), traceId},
                    }
                ).ExecuteAsync(httpContext);

                return true;
            }


            await Results.Problem(
                title: title,
                statusCode: statusCode,
                extensions: new Dictionary<string, object?>
                {
                    {nameof(traceId), traceId},
                }
            ).ExecuteAsync(httpContext);

            return true;
        }

        private (int StatusCode, string Title) MapException(Exception exception)
        {
            return exception switch
            {
                ValidationException => (StatusCodes.Status400BadRequest, "One or more validation errors occurred."),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error.")
            };
        }

        private void LogException(Exception exception, string traceId)
        {

            (LogLevel logLevel, LoggingEvents logEvent) = exception switch
            {
                ValidationException => (LogLevel.Information, LoggingEvents.VALIDATION_FAILURE),
                _ => (LogLevel.Error, LoggingEvents.UNSPECIFIED)
            };

            var loggerMessage = LoggerMessage.Define<string, string, string>(
                 logLevel,
                 eventId: new EventId((int)logEvent, logEvent.GetDisplayName()),
                 formatString: "  TraceID: {TraceID}\n\tUTCTime: {Time}\n\t{Message}\n"
             );

            loggerMessage(
                _logger,
                traceId,
                DateTime.UtcNow.ToString(),
                exception.Message,
                exception
            );
        }
    }
}
