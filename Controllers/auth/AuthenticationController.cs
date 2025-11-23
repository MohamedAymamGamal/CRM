using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CRM.API.Dtos.DtosAuthentication;
using CRM.API.Interface.authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.AuthenticationController.auth
{
    [Area("Identity")]
    [DisplayName("Authentication Controller")]
    [Route("api/authentication")]
    [ApiController]
    public class  AuthenticationController(IAuthenticationServices authenticationService) : ControllerBase
    {

        [HttpPost("login")]
        [DisplayName("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] loginRequestDto request)
        {
            var response = await authenticationService.LoginAsync(request);
            return response ? Ok(response) : BadRequest(response);
        }

        [HttpPost("register")]
        [DisplayName("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var response = await authenticationService.RegisterAsync(request);
            return response ? Ok(response) : StatusCode(500);
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