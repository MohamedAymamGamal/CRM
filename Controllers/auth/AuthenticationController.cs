using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using api.Helpers;
using CRM.API.Dtos.DtosAuthentication;
using CRM.API.Interface.authentication;
using CRM.API.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> LoginAsync([FromBody] loginRequestDto request)
        {
            var response = await authenticationService.LoginAsync(request);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPost("register")]
        [DisplayName("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var (success, errors) = await authenticationService.RegisterAsync(request);
            if (!success)
                return BadRequest(ApiResponse.Error(
                    errors, 
                    loc.localize("Register.Login failed")

            ));

            return Ok(ApiResponse.Success(null, "Registered successfully"));
        }

        // [HttpPost("forgot-password")]
        // [DisplayName("Forgot Password")]
        // public async Task<IActionResult> ForgotPassword([FromBody] ApplicationUserRegisterInputModel request)
        // {
        //     var response = await authenticationService.ForgotPasswordAsync(request);
        //     return response ? Ok(response) : StatusCode(500);
        // }

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