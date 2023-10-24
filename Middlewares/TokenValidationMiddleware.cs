using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RefactorThis.Extensions;
using RefactorThis.Services;
using System;
using System.Threading.Tasks;

namespace RefactorThis.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public TokenValidationMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.GetBearerToken();

            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = 401; // Unauthorized
                return;
            }
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var loginService = scope.ServiceProvider.GetRequiredService<ILoginService>();
                var user = await loginService.AuthenticateWithToken(Guid.Parse(token));
                if (user == null)
                {
                    context.Response.StatusCode = 401;
                    return;
                }
                var isVaild = loginService.IsVaildToken(user);
                if (!isVaild)
                {
                    context.Response.StatusCode = 401; // Unauthorized
                }
                await _next(context);
            }
        }
    }
}
