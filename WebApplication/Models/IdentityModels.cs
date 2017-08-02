using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace WebApplication.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class user : IdentityUser
    {


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<user> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class AppRole : IdentityRole
{
    public AppRole() : base() { }
    public AppRole(string name) : base(name) { }
    // extra properties here 
}

    public class MyDbContext : IdentityDbContext<user>
    {
        public MyDbContext()
            : base("bsadashiDataBaseEntities")
        {
        }
        
        
    }
}