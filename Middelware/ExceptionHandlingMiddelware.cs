namespace ElasticSearch.Middelware;

public class ExceptionHandlingMiddelware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddelware> _logger;

    public ExceptionHandlingMiddelware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddelware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception on {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            if (context.Response.HasStarted)
                throw;

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                status = 500,
                message = "Internal Server Error"
            });
        }
    }
}