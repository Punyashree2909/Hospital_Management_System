using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HospitalManagementSystem.Filters
{
    public class AuthFilter : ActionFilterAttribute
    {
        private readonly string[] _roles;

        public AuthFilter(params string[] roles)
        {
            _roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var role = httpContext.Session.GetString("UserRole");

            if (role == null || (_roles.Length > 0 && !_roles.Contains(role)))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
        }
    }
}
