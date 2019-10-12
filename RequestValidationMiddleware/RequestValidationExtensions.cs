using Microsoft.AspNetCore.Builder;

namespace RequestValidationMiddleware
{
    public static class RequestValidationExtensions
    {
        public static IApplicationBuilder UseRequestValidation(
           this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestValidation>();
        }
    }
}
