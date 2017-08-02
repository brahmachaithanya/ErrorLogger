using System;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication
{
    internal class ApplicationUserManager
    {
        internal static Func<ApplicationUserManager> Create;

        internal Task<user> FindAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}