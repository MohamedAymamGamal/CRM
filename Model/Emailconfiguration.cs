using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.API.Model
{
    public class Emailconfiguration
    {
        public required string FromName { get; set; }
        public required string FromEmail { get; set; }
        public required string SmtpServer { get; set; }
        public int Port { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }

    }
}