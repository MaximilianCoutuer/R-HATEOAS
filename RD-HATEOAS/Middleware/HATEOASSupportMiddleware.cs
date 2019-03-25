using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RDHATEOAS.Middleware
{
    public class HATEOASSupportMiddleware
    {
        private readonly RequestDelegate _next;

        public HATEOASSupportMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // HACK
            var originResponseBody = context.Response.Body;
            var newResponseBody = new MemoryStream();
            context.Response.Body = newResponseBody;

            await _next(context);

            newResponseBody.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(newResponseBody).ReadToEnd();

            context.Response.Body = originResponseBody;
            // TODO: set content length
            await context.Response.WriteAsync("lol");
            //context.Response.StatusCode = 418;

            // of: context.Response.OnStarting();
        }
    }
}
