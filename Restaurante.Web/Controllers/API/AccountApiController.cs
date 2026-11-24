using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Web.Core;
using Restaurante.Web.DTOs.Account;
using Restaurante.Web.Services.Abstractions;

namespace Restaurante.Web.Controllers.Api
{
    [Route("api/account")]
    public class AccountApiController : ApiController
    {
        private readonly IAccountService _accountService;

        public AccountApiController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            Response<ProfileDTO> response = await _accountService.GetProfileAsync();
            return ControllerBasicValidation(response);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileDTO dto)
        {
            Response response = await _accountService.UpdateProfileAsync(dto);
            return ControllerBasicValidation(response);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            Response response = await _accountService.ChangePasswordAsync(dto);
            return ControllerBasicValidation(response);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            Response response = await _accountService.SendPasswordResetEmailAsync(dto, token =>
                $"{baseUrl}/Account/ResetPassword?email={Uri.EscapeDataString(dto.Email)}&token={token}");

            return ControllerBasicValidation(response);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            Response response = await _accountService.ResetPasswordAsync(dto);
            return ControllerBasicValidation(response);
        }
    }
}