
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PmeMemberApi.Core.Model;


namespace PmeMemberApi.SecureAuth
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, long>
    {
       
       
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {
            }

            public DbSet<AppUser> AppUsers { get; set; }
            public DbSet<PmeMemberApi> PmeMemberApis { get; set; }
            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);
            }
        }
    }

