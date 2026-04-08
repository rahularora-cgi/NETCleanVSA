using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Application
{
    public interface IJwtTokenProvider
    {
        Task<string> GenerateJwtToken<T>(User user, Func<User, Task<T>> GetRoles);
    }
}
