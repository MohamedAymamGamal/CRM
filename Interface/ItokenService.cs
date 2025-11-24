using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.API.Interface
{
    public interface ItokenService
    {
        string CreateToken(ApplicationUser  user);

    }
}