using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthTools.Authorization
{
    public class HasPermissionFilter : IAuthorizationFilter
    {
        private readonly string[] _requiredPermissions;

        public HasPermissionFilter(string[] requiredPermissions)
        {
            _requiredPermissions = requiredPermissions;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? false)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userPermissions = user.Claims
                .Where(c => c.Type == "permission")
                .Select(c => c.Value);

            if (!_requiredPermissions.All(p => userPermissions.Contains(p)))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
