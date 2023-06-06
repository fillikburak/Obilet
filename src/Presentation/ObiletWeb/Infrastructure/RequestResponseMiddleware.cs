using System.Text;

namespace ObiletWeb.Infrastructure;

public class RequestResponseMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<RequestResponseMiddleware> logger;

    public RequestResponseMiddleware(RequestDelegate next, ILogger<RequestResponseMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        LogRequest(context);

        var originalBodyStream = context.Response.Body;
        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            await next(context);

            LogResponse(context);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private void LogRequest(HttpContext context)
    {
        var request = context.Request;
        var logMessage = new StringBuilder();
        logMessage.AppendLine("Incoming Request:");
        logMessage.AppendLine($"Method: {request.Method}");
        logMessage.AppendLine($"Path: {request.Path}");
        logMessage.AppendLine($"QueryString: {request.QueryString}");
        logMessage.AppendLine($"Content-Type: {request.ContentType}");
        logMessage.AppendLine($"Content-Length: {request.ContentLength}");

        logger.LogInformation(logMessage.ToString());
    }

    private void LogResponse(HttpContext context)
    {
        var response = context.Response;
        response.Body.Seek(0, SeekOrigin.Begin);
        var responseContent = new StreamReader(response.Body).ReadToEnd();

        var logMessage = new StringBuilder();
        logMessage.AppendLine("Outgoing Response:");
        logMessage.AppendLine($"StatusCode: {response.StatusCode}");
        logMessage.AppendLine($"Content-Type: {response.ContentType}");
        logMessage.AppendLine($"Content-Length: {response.ContentLength}");
        logMessage.AppendLine($"Content: {responseContent}");

        logger.LogInformation(logMessage.ToString());

        response.Body.Seek(0, SeekOrigin.Begin);
    }
}
