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
        SignInManager<ApplicationUser> signInManager
    
    
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

        public async Task<bool> LoginAsync(loginRequestDto request)
        {
           var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);
           return result.Succeeded;
        }

        public Task<bool> RefreshTokenAsync(RegisterRequestDto request)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RegisterAsync(RegisterRequestDto request)
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

            return result.Succeeded ? true : throw new Exception("Unable to create user, Errors: " + result.Errors);
        }

        public Task<bool> ResetPasswordAsync(RegisterRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}