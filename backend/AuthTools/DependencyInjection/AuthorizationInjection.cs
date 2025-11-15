using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthTools.DependencyInjection
{
    public static class AuthorizationInjection
    {
        public static void AddAuthorizationInjection(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            services.AddAuthorization(options =>
            {
                //options.AddPolicy(nameof(AccessPolicy.Admin), policy =>
                //    policy.Requirements.Add(new AccessRequirement(AccessPolicy.Admin)));

                //options.AddPolicy(nameof(AccessPolicy.Moderator), policy =>
                //    policy.Requirements.Add(new AccessRequirement(AccessPolicy.Moderator)));

                //options.AddPolicy(nameof(AccessPolicy.SameUser), policy =>
                //    policy.Requirements.Add(new AccessRequirement(AccessPolicy.SameUser)));
            });
        }
    }
}
