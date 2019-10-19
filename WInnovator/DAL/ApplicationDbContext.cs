using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics.CodeAnalysis;
using WInnovator.Models;

namespace WInnovator.Data
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            //This will singularize all table names
            foreach (IMutableEntityType entityType in builder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.DisplayName());
            }

            // Specify specific binding between DesignShop, WorkingForm and DesignShopWorkingForm
            
            builder.Entity<DesignShop>()
                .HasOne<DesignShopWorkingForm>(ds => ds.CurrentDesignShopWorkingForm)
                .WithOne(dswf => dswf.IsCurrentWorkingForm);

            builder.Entity<DesignShopWorkingForm>()
                .HasOne<DesignShop>(dswf => dswf.DesignShop)
                .WithMany(ds => ds.DesignShopWorkingForms);

            builder.Entity<DesignShopWorkingForm>()
                .HasOne<WorkingForm>(dswf => dswf.WorkingForm)
                .WithMany(wf => wf.DesignShopWorkingForms);
        }
    }
}
