using InnoGotchiWebAPI.Exceptions;
using Serilog;

namespace InnoGotchiWebAPI.Middleware
{
    public class ExceptionHandlingMiddleware 
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
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
                if(ex is InnoGotchiDBException dbEx)
                {
                    context.Response.StatusCode = (int)dbEx.StatusCode!;
                    Log.Error("InnoGotchiDBException caught: {msg}", ex.Message);
                    await context.Response.WriteAsync($"Some problem from DB layer: {ex.Message}.");
                }
                else if(ex is InnoGotchiLogicException logicEx)
                {
                    context.Response.StatusCode = (int)logicEx.StatusCode!;
                    Log.Error("InnoGotchiLogicException caught: {msg}", ex.Message);
                    await context.Response.WriteAsync($"Some problem from logic layer: {ex.Message}.");
                } else
                {
                    Log.Error("Exception caught: {msg}", ex.Message);
                    await context.Response.WriteAsync($"Some problem not from program-specific layer: {ex.Message}");
                }
            }
        }
    }
    public static class ExceptionHandlingMiddlewareExtension
    {
        public static IApplicationBuilder UseInnoGotchiExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
