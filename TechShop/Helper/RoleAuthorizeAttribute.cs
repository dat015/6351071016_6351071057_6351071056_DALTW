using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;

namespace TechShop.Helper
{
    public class RoleAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public RoleAuthorizeAttribute(string roles) 
        {
            _roles = roles.Split(',').Select(r => r.Trim()).ToArray();
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userRole = context.HttpContext.Session.GetString(ClaimTypes.Role);

            // Kiểm tra nếu người dùng chưa đăng nhập
            if (string.IsNullOrEmpty(userRole))
            {
                if (!context.HttpContext.Request.Path.Value.Contains("Login") &&
                    !context.HttpContext.Request.Path.Value.Contains("Register") &&
                    !context.HttpContext.Request.Path.Value.Contains("Home"))
                {
                    context.Result = new ForbidResult();
                }
            }
            else if (!_roles.Contains(userRole))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
