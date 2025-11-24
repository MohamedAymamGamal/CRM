using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.API.Dtos.DtosAuthentication
{
    public class ApplicationUserVerificationDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        public string FullName { get; set; } = "CRM User";

        public string? Code { get; set; }
    }
}