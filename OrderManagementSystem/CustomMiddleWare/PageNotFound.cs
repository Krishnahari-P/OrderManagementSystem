using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace OrderManagementSystem.CustomMiddleWare
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class PageNotFound
    {
        private readonly RequestDelegate _next;

        public PageNotFound(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await _next(httpContext);
            if (httpContext.Response.StatusCode == 404 && !httpContext.Response.HasStarted)
            {
                httpContext.Response.Redirect("/Error/Error404");
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UsePageNotFound(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PageNotFound>();
        }
    }
}
