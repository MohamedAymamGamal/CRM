using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.API.Dtos.DtosAuthentication;

namespace CRM.API.Interface.authentication
{
    public interface IAuthenticationServices
    {
        Task<UserResponceDto<bool>> LoginAsync(loginRequestDto request);
        Task<(bool Success, List<string> Errors)> RegisterAsync(RegisterRequestDto request);
        Task<bool> ForgotPasswordAsync(RegisterRequestDto request);
        Task<bool> ResetPasswordAsync(RegisterRequestDto request);
        Task<bool> ChangePasswordAsync(RegisterRequestDto request);
        Task<bool> RefreshTokenAsync(RegisterRequestDto request);

        Task<UserResponceDto<bool>> ConfirmEmailAsync( ConfirmEmailInputDto request);
        Task<UserResponceDto<bool>> ConfirmEmailVerifyCodeAsync(ConfirmEmailInputDto request);

        
    }
}