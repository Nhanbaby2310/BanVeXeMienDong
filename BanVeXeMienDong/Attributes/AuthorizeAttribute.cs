using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BanVeXeMienDong.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _allowedRoles;

        public AuthorizeAttribute(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Session.GetString("user");

            // Nếu chưa đăng nhập
            if (string.IsNullOrEmpty(user))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // Nếu có yêu cầu role cụ thể
            if (_allowedRoles.Length > 0)
            {
                var role = context.HttpContext.Session.GetString("role");

                if (string.IsNullOrEmpty(role) || !_allowedRoles.Contains(role))
                {
                    // Không có quyền
                    context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                }
            }
        }
    }
}
