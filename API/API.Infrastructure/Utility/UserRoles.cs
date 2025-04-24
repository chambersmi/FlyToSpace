using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Utility
{
    public class UserRoles : IdentityRole
    {
        public const string Role_User = "User";
        public const string Role_Admin = "Admin";
    }
}
