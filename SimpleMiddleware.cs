using SimpleApp.Exceptions;

namespace SimpleApp
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SimpleMiddleware
    {
        private readonly RequestDelegate _next;

        public SimpleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            try 
            {
                await  _next.Invoke(httpContext);
            }
            catch (MyException ex)
            {
                httpContext.Response.StatusCode = ex.Code;

                await httpContext.Response.WriteAsJsonAsync(new
                {
                    ex.Code,
                    ex.Message
                });
            }
            catch(Exception ex)
            {
                httpContext.Response.StatusCode = 500;

                await httpContext.Response.WriteAsJsonAsync(new
                {
                    Code = 500,
                    ex.Message
                });
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SimpleMiddlewareExtensions
    {
        public static IApplicationBuilder UseSimpleMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SimpleMiddleware>();
        }
    }
}
