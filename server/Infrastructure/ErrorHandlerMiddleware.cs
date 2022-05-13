namespace Server;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
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
            response.StatusCode = (ex is Error err) ? (int)err.Code : 500;

            await response.WriteAsJsonAsync(new ApiResult
            {
                Message = ex.Message,
                Code = response.StatusCode
            });
        }
    }
}
