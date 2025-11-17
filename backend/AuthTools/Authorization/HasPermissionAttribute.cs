using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthTools.Authorization
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class HasPermissionAttribute : Attribute, IFilterFactory
    {
        public string[] Permissions { get; }

        public HasPermissionAttribute(params string[] permissions)
        {
            Permissions = permissions;
        }

        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new HasPermissionFilter(Permissions);
        }
    }
}
