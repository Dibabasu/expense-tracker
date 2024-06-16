using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace expensetracker.api.Middleware
{
    public class ETagMiddleware
    {
        private readonly RequestDelegate _next;

        public ETagMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                if (context.Items.TryGetValue("ETag", out var etag))
                {
                    context.Response.Headers["ETag"] = etag.ToString();
                }
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}