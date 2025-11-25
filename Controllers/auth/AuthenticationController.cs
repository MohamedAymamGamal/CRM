using System;
using System.ComponentModel;
using CRM.API.Dtos.DtosAuthentication;
using CRM.API.Interface.authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CRM.API.AuthenticationController.auth
{
    [Area("Identity")]
    [DisplayName("Authentication Controller")]
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController(IAuthenticationServices authenticationService, JsonLocalizationService loc) : ControllerBase
    {

        [HttpPost("login")]
        [DisplayName("Login")]
        [EnableRateLimiting("AuthPolicy")]
        public async Task<IActionResult> LoginAsync([FromBody] loginRequestDto request)
        {
            var response = await authenticationService.LoginAsync(request);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPost("register")]
        [DisplayName("Register")]
        [EnableRateLimiting("AuthPolicy")]

        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var (success, errors) = await authenticationService.RegisterAsync(request);
            if (!success)
                return BadRequest(ApiResponse.Error(
                    errors,
                    loc.localize("Register.Login_failed")
                ));

            return Ok(ApiResponse.Success(
                null,
                loc.localize("Register.Login_successful")
            ));


        }
        [HttpPost("confirm-email")]
        [DisplayName("Confirm Email")]
        [EnableRateLimiting("AuthPolicy")]

        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailInputDto request)
        {
            var response = await authenticationService.ConfirmEmailAsync(request);
            return response.IsSuccess ? Ok() : BadRequest(response);
        }

        [HttpPost("confirm-email-verify-code")]
        [DisplayName("Confirm Email Verify Code")]
        [EnableRateLimiting("AuthPolicy")]

        public async Task<IActionResult> ConfirmEmailVerifyCode([FromBody] ConfirmEmailInputDto request)
        {
            var response = await authenticationService.ConfirmEmailVerifyCodeAsync(request);
            return response.IsSuccess ? Ok() : BadRequest(response);
        }


        // [HttpPost("reset-password")]
        // [DisplayName("Reset Password")]
        // public async Task<IActionResult> ResetPassword([FromBody] ApplicationUserRegisterInputModel request)
        // {
        //     var response = await authenticationService.ResetPasswordAsync(request);
        //     return response ? Ok(response) : StatusCode(500);
        // }

        // [HttpPost("change-password")]
        // [DisplayName("Change Password")]
        // public async Task<IActionResult> ChangePassword([FromBody] ApplicationUserRegisterInputModel request)
        // {
        //     var response = await authenticationService.ChangePasswordAsync(request);
        //     return response ? Ok(response) : StatusCode(500);
        // }

        // [HttpPost("refresh-token")]
        // [DisplayName("Refresh Token")]
        // public async Task<IActionResult> RefreshToken([FromBody] ApplicationUserRegisterInputModel request)
        // {
        //     var response = await authenticationService.RefreshTokenAsync(request);
        //     return response ? Ok(response) : StatusCode(500);
        // }
    }
}