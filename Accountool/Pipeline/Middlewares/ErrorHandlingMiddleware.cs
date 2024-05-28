using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Accountool.Pipeline.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception)
            {
                context.Response.Redirect("/Home/Error");
            }
        }
    }
}