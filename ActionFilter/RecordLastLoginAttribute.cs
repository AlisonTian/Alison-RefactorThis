using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using RefactorThis.Extensions;
using RefactorThis.Services;
using System;
using System.Threading.Tasks;

namespace RefactorThis.ActionFilter
{
    public class RecordLastLoginAttribute : ActionFilterAttribute
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public RecordLastLoginAttribute(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            HttpContext httpContext = context.HttpContext;
            var token = httpContext.GetBearerToken();
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var loginService = scope.ServiceProvider.GetRequiredService<ILoginService>();
                var user = await loginService.AuthenticateWithToken(Guid.Parse(token));
                if (user != null)
                {
                    user.LastLoggedIn = DateTime.UtcNow;
                    await loginService.UpdateAsync(user);
                }
            }
            await next();
        }
    }
}
