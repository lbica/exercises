using webapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;


namespace webapi.Persistence
{
    public class AppDbContext : DbContext
    {
        private IConfiguration _configuration;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Customer> Customers { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // apply the EntityFramework configuration for all entities that implements IEntityTypeConfiguration interface inside the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //set here the metadata/audit fields for each entity
            //foreach (var entry in base.ChangeTracker.Entries<BaseEntity>()
            //    .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
            //{
            //    entry.Entity.UpdatedDate = DateTime.Now;
            //    if (entry.State == EntityState.Added)
            //        entry.Entity.InsertedDate = DateTime.Now;
            //}
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
