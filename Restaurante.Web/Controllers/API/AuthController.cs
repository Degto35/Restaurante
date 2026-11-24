using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.Core;
using Restaurante.Web.DTOs.Account;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Controllers.Api
{
    [Route("api/[controller]")]
    public class AuthController : ApiController
    {
        private readonly IAccountService _accountService;

        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            Response<UserTokenDTO> response = await _accountService.LoginApiAsync(dto);

            return ControllerBasicValidation(response,
                                             statusCode: response.IsSuccess ? null : StatusCodes.Status401Unauthorized);
        }
    }
}