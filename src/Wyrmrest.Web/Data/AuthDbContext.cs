using Microsoft.EntityFrameworkCore;
using Wyrmrest.Web.Models;

namespace Wyrmrest.Web.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options) { }

#pragma warning disable IDE1006 // Naming Styles
        public DbSet<account> account { get; set; }
        public DbSet<account_banned> account_banned { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}
