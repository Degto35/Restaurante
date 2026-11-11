using Microsoft.AspNetCore.Authorization;

namespace Restaurante.Web.Services
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }

    public class PermissionAuthorizationHandler
        : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (context.User.IsInRole("Administrador"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var tienePermiso = context.User.Claims.Any(
                x => x.Type == "Permission" &&
                     x.Value == requirement.Permission);

            if (tienePermiso)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
