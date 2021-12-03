using sarapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sarapi.Services
{
        public   interface IAuthManager
    {
        Task<bool> ValidateUser(LoginDto loginDto);
        Task<string> CreateToken();
    }
}
