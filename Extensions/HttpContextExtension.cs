using Microsoft.AspNetCore.Http;
using System;

namespace RefactorThis.Extensions
{
    public static class HttpContextExtension
    {
        public static string GetBearerToken(this HttpContext context)
        {
            return context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
        }
    }
}
