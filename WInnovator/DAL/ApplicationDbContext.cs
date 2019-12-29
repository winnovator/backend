using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics.CodeAnalysis;
using WInnovator.Models;

namespace WInnovator.DAL
{
    [ExcludeFromCodeCoverage]
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<DesignShop> DesignShop { get; set; }
        public DbSet<DesignShopWorkingForm> DesignShopWorkingForm { get; set; }
        public DbSet<ImageStore> ImageStore { get; set; }
        public DbSet<WorkingForm> WorkingForm { get; set; }
        public DbSet<Phase> Phase { get; set; }
    }
}
