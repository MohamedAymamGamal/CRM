using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.API.Dtos.DtosAuthentication
{
    public class UserResponceDto<T>
    {
        public bool IsSuccess { get; set; } = true;
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}