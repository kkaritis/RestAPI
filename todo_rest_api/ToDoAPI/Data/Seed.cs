using System.Linq;
using TodoAPI.Helpers;
using TodoAPI.Models;

namespace TodoAPI.Data
{
    public static class Seed
    {
        public static void EnsureDatabaseSeeded(this TodoContext context)
        {
            context.Database.EnsureCreated();

            SeedAdmin(context);

            context.SaveChanges();
        }

        private static void SeedAdmin(TodoContext context)
        {
            var email = "admin@devlix.de";
            var password = "admin";

            var admin = context.Users.FirstOrDefault(u => u.Email == email);

            if (admin == null)
            {
                (var securePassword, var salt) = PasswordHelper.CreateSecurePassword(password);
                admin = new User
                {
                    FirstName = "Admin",
                    LastName = "Devlix",
                    Email = email,
                    IsAdmin = true,
                    Password = securePassword,
                    Salt = salt
                };

                context.Users.Add(admin);
            }
        }
    }
}
