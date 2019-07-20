using System.Threading;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using TicoCinema.WebApplication.ViewModels;

[assembly: OwinStartup(typeof(TicoCinema.WebApplication.Startup))]
namespace TicoCinema.WebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {       
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            ConfigureAuth(app);
            CreateRoles();
        }

        private void CreateRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            IdentityRole role;
            if (!roleManager.RoleExists("Admin"))
            {
                role = new IdentityRole { Name = "Admin" };
                roleManager.Create(role);
                CreateDefaultAdmin(context);
            }

            if (!roleManager.RoleExists("Guest"))
            {
                role = new IdentityRole { Name = "Guest" };
                roleManager.Create(role);
            }
        }

        private void CreateDefaultAdmin(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = new ApplicationUser
            {
                UserName = "admin1@gmail.com",
                Email = "admin1@gmail.com",
                EmailConfirmed = true
            };
            string defaultPassword = "admin1";

            var chkUser = userManager.Create(user, defaultPassword); 
            if (chkUser.Succeeded)
            {
                var result1 = userManager.AddToRole(user.Id, "Admin");
            }

            //guest1@gmail.com
            //Admin123*
        }
    }
}
