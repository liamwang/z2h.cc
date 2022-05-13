namespace Server.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(
        RequestDelegate next, 
        ILogger<ErrorHandlerMiddleware> logger)
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
            var response = context.Response;
            response.ContentType = "application/json";

            var handled = false;
            if(ex is Error err)
            {
                response.StatusCode = (int)err.Code;
                handled = err.Code > 0;
            }
            else
            {
                response.StatusCode = 500;
            }

            if (!handled)
            {
                _logger.LogError(ex, "全局捕获异常");
            }

            await response.WriteAsJsonAsync(new ApiResult
            {
                Code = response.StatusCode,
                Message = ex.Message,
            });
        }
    }
}
