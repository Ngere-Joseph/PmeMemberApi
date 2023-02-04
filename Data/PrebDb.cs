using Microsoft.EntityFrameworkCore;
using PmeMemberApi.SecureAuth;

namespace PmeMemberApi.Data
{
    public static class PrebDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            Console.WriteLine("--> Attempting to apply migrations ...");
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"--> Could not run migrations: {ex.Message}");
            }
            
            //
            if (!context.Roles.Any())
            {
                Console.WriteLine("--> Seeding Data");

                context.Roles.AddRange(
                    
                );

                context.SaveChanges();
            }

            if (!context.AppUsers.Any())
            {

            }
            else
            {
                Console.WriteLine("---> We already have data");
            }
        }
    }
}
