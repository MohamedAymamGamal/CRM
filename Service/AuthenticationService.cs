using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using CRM.API.Dtos.DtosAuthentication;
using CRM.API.Helpers;
using CRM.API.Interface;
using CRM.API.Interface.authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace CRM.API.Service
{

    public class AuthenticationService(

        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        JsonLocalizationService loc,
        IApplicatiomEmailSender applicationEmailSender,
        ItokenService tokenService 
    ) : IAuthenticationServices
    {
         private readonly ItokenService _tokenService = tokenService;
        public Task<bool> ChangePasswordAsync(RegisterRequestDto request)
        {
            throw new NotImplementedException();
        }
        private short GenerateVerificationCode()
        {
            Random random = new Random();
            return (short)random.Next(1000, 9999);
        }

        private async Task SendEmailConfirmationCodeAsync(ConfirmEmailInputDto request)
        {

            var emailContent = TemplateHelper.LoadTemplate("confirm-email.html", new Dictionary<string, string>
            {
                { "FullName", request.FullName },
          { "Code", request.Code }
            });

            MailMessage mail = new();
            mail.To.Add(request.Email);
            mail.Subject = "CRM | Email Confirmation";

            var alternateView = AlternateView.CreateAlternateViewFromString(
                emailContent,
                null,
                MediaTypeNames.Text.Html
            );

            mail.AlternateViews.Add(alternateView);

            await applicationEmailSender.SendEmailAsync(mail);
        }

        public async Task<UserResponceDto<bool>> ConfirmEmailAsync(ConfirmEmailInputDto request)
        {
            ArgumentNullException.ThrowIfNull(request.Email);
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new UserResponceDto<bool>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    Data = false
                };
            }

            if (user.EmailConfirmed)
            {
                return new UserResponceDto<bool>
                {
                    IsSuccess = false,
                    Message = "Email already confirmed",
                    Data = false
                };
            }

            user.VerificationCode = GenerateVerificationCode();
            user.VerificationCodeExpiry = DateTime.UtcNow.AddMinutes(5);
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return new UserResponceDto<bool>
                {
                    IsSuccess = false,
                    Message = "Failed to save verification code"
                };
            }

            // Send email with verification code
            request.Code = user.VerificationCode.ToString();
            request.FullName = $"{user.FirstName} {user.LastName}";
            await SendEmailConfirmationCodeAsync(request);
            return new UserResponceDto<bool>
            {
                IsSuccess = true,
                Message = "Verification code sent successfully"
            };
        }

        public async Task<UserResponceDto<bool>> ConfirmEmailVerifyCodeAsync(ConfirmEmailInputDto request)
        {
            ArgumentNullException.ThrowIfNull(request.Email);
            ArgumentNullException.ThrowIfNull(request.Code);

            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new UserResponceDto<bool>
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            // 1. Check expiry
            if (user.VerificationCodeExpiry == null || user.VerificationCodeExpiry < DateTime.UtcNow)
            {
                return new UserResponceDto<bool>
                {
                    IsSuccess = false,
                    Message = "Verification code expired, please request a new one"
                };
            }

            bool isCodeValid = user.VerificationCode != null &&
                               user.VerificationCode.ToString() == request.Code;

            if (!isCodeValid)
            {
                return new UserResponceDto<bool>
                {
                    IsSuccess = false,
                    Message = "Invalid confirmation code"
                };
            }

            // 3. Mark email confirmed
            user.EmailConfirmed = true;
            user.IsActive = true;

            // Clear code after success
            user.VerificationCode = null;
            user.VerificationCodeExpiry = null;

            var result = await userManager.UpdateAsync(user);

            return new UserResponceDto<bool>
            {
                IsSuccess = result.Succeeded,
                Message = result.Succeeded ? "Email confirmed successfully" : "Failed to confirm email"
            };
        }

        public Task<bool> ForgotPasswordAsync(RegisterRequestDto request)
        {
            throw new NotImplementedException();
        }

        public async Task<UserResponceDto<bool>> LoginAsync(loginRequestDto request)
        {
            var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);
            var user = await userManager.FindByEmailAsync(request.Email);
            if (result.Succeeded)
            {
                var token  = _tokenService.CreateToken(user);
                return new UserResponceDto<bool>
                {
                    Token = token,
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