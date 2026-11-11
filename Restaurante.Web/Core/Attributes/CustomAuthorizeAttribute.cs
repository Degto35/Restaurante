using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Core.Attributes
{

    public class CustomAuthorizeAttribute : TypeFilterAttribute
    {
        public CustomAuthorizeAttribute(string permissionCode) : base(typeof(CustomAuthorizeFilter))
        {
            Arguments = [permissionCode];
        }
    }

    public class CustomAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly string _permissionCode;
        private readonly IAccountService _accountService;

        public CustomAuthorizeFilter(string permissionCode, IAccountService accountService)
        {
            _permissionCode = permissionCode;
            _accountService = accountService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            
            if (!_accountService.AccountUserIsAuthenticated())
            {
                context.Result = new ChallengeResult();
                return;
            }

           
            if (!await _accountService.AccountUserIsAuthorizedAsync(_permissionCode))
            {
                context.Result = new ForbidResult();
            }

        
        }
    }
}