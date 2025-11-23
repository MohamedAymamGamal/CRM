using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.API.Dtos.DtosAuthentication;
using CRM.API.Interface.authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace CRM.API.Service
{

    public class AuthenticationService(

        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        JsonLocalizationService loc

    ) : IAuthenticationServices
    {
        public Task<bool> ChangePasswordAsync(RegisterRequestDto request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ForgotPasswordAsync(RegisterRequestDto request)
        {
            throw new NotImplementedException();
        }

        public async Task<UserResponceDto<bool>> LoginAsync(loginRequestDto request)
        {
            var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);
            if (result.Succeeded)
            {
                return new UserResponceDto<bool>
                {
                    IsSuccess = true,
                    Data = true,
                    Message = loc.localize("Register.Login_successful")
                };
            }
            else
            {
                return new UserResponceDto<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = loc.localize("Register.login_failed")
                };
            }
        }

        public Task<bool> RefreshTokenAsync(RegisterRequestDto request)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool Success, List<string> Errors)> RegisterAsync(RegisterRequestDto request)
        {
            ArgumentNullException.ThrowIfNull(request.Email);
            ArgumentNullException.ThrowIfNull(request.Password);

            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                RegistrationDate = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors
                    .Select(e => $"{e.Code}: {e.Description}")
                    .ToList();

                return (false, errors);
            }

            return (true, new List<string>());
        }

        public Task<bool> ResetPasswordAsync(RegisterRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}