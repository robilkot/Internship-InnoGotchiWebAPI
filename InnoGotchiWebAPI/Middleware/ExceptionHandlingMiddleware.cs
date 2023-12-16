using InnoGotchiWebAPI.Exceptions;

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
            catch (InnoGotchiPetNotFoundException ex)
            {
                context.Response.StatusCode = (int)ex.StatusCode!;
                await context.Response.WriteAsync($"I am a wonderful middleware and I couldn't find a pet: {ex.Message}.");
            }
            catch (InnoGotchiException ex)
            {
                if(ex.StatusCode != null)
                {
                    context.Response.StatusCode = (int)ex.StatusCode;
                }
                await context.Response.WriteAsync($"I am a wonderful middleware and I caught this specific exception: {ex.Message}.");
            }
            catch (Exception ex)
            {
                await context.Response.WriteAsync($"I am a wonderful middleware and I caught this exception: {ex.Message}");
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
