using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using RefactorThis.Models;
using System;

namespace RefactorThis.Extensions
{
    public static class HttpRequestExtension
    {
        public static UrlActionContext BuildUrlActionContext<T>(this HttpRequest request, string controllerName, string actionName, T t)
        {
            return new UrlActionContext()
            {
                Controller = controllerName,
                Action = actionName,
                Host = request?.Host.ToString(),
                Protocol = Uri.UriSchemeHttps,
                Values = t
            };
        }
    }
}
