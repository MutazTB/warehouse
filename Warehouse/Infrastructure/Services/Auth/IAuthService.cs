using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Auth
{
    public interface IAuthService
    {
        string GenerateToken(string email, string role);
    }
}
