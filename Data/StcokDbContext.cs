using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using stockapplocation.Models;

namespace stockapplocation.Data
{
    public class StcokDbContext : IdentityDbContext<AppUser>
    {
        public StcokDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Stock> stocks { get; set; }
        public DbSet<Comments> comments { get; set; }
        public DbSet<Portfolio> portfolios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));

            builder.Entity<Portfolio>()
            .HasOne(u => u.AppUser)
            .WithMany(p => p.Portfolios)
            .HasForeignKey(p => p.AppUserId);

            builder.Entity<Portfolio>()
            .HasOne(u => u.Stock)
            .WithMany(p => p.Portfolios)
           .HasForeignKey(p => p.StockId);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                 new IdentityRole
        {
            Id = "a8d5a1f3-7c1a-4f98-bd2f-9e3b7c1d5e1a", // Fixed GUID
            Name = "Admin",
            NormalizedName = "ADMIN"
        },
        new IdentityRole
        {
            Id = "b1f6e8c7-4e9a-499d-931a-6f7c2a8b5d3b", // Fixed GUID
            Name = "User",
            NormalizedName = "USER"
        }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
