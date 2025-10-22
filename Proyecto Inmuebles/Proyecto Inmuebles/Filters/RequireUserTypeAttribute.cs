using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Inmuebles.Models;
using Microsoft.AspNetCore.Authentication;

namespace Proyecto_Inmuebles.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class RequireUserTypeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly int _requiredUserType;

        public RequireUserTypeAttribute(int requiredUserType)
        {
            _requiredUserType = requiredUserType;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var http = context.HttpContext;

            if (http.User?.Identity?.IsAuthenticated != true)
            {
                context.Result = new RedirectToActionResult(
                    "Unauthorized", "Error",
                    new { returnUrl = http.Request.Path.Value });
                return;
            }

            var userType = http.Session.GetInt32(SessionKeys.UserType);

            if (userType is null || userType.Value != _requiredUserType)
            {
                http.Session.Clear();
                await http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                context.Result = new RedirectToActionResult(
                    "Unauthorized", "Error",
                    new { returnUrl = http.Request.Path.Value });
                return;
            }

        }
    }
}
