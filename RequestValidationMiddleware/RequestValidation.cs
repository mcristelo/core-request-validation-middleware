using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RequestValidationMiddleware
{
    public class RequestValidation
    {
        private readonly RequestDelegate _next;

        public RequestValidation(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            const string RegExInvalidCharacters = @"[\^&<>\""/]";
            Regex rx = new Regex(RegExInvalidCharacters,
     RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (context.Request.Method == "POST")
            {
                var form = await context.Request.ReadFormAsync();
                foreach (var item in form.Keys)
                {
                    if (rx.Matches(form[item]).Count > 0)
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Invalid characters");
                        return;
                    }
                }
            }
            else if (context.Request.Method == "GET")
            {
                foreach (var item in context.Request.Query.Keys)
                {
                    if (rx.Matches(context.Request.Query[item]).Count > 0)
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("Invalid characters");
                        return;
                    }
                }
            }
            await _next(context);
        }
    }
}
