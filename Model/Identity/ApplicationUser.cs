using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CRM.API.enums;
using Microsoft.AspNetCore.Identity;

namespace CRM.API.Model.Identity
{
    public class ApplicationUser : IdentityUser
    {
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public short? VerificationCode { get; set; }
    public string? ImageName { get; set; }
    public bool? IsActive { get; set; }
    public string? ProfileDescription { get; set; }

    [MaxLength(128)]
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public byte RefreshTokenAttemptCount { get; set; } = 0;

    [Timestamp]
    public byte[] RowVersion { get; set; } = [];

    [NotMapped]
    public string? FullName => $"{FirstName} {LastName}";
    }
}