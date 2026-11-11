using Microsoft.AspNetCore.Authorization;

namespace Restaurante.Web.Extensions
{
    public class PermissionAttribute : AuthorizeAttribute
    {
        public PermissionAttribute(string permission)
        {
            Policy = permission;
        }
    }
}
