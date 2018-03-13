namespace CarDealer.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }

        public DbSet<Part> Parts { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<CarPart>()
                .HasKey(cp => new {cp.CarId, cp.PartId});

            builder
                .Entity<CarPart>()
                .HasOne(cp => cp.Car)
                .WithMany(c => c.Parts)
                .HasForeignKey(cp => cp.CarId);

            builder
                .Entity<CarPart>()
                .HasOne(cp => cp.Part)
                .WithMany(p => p.Cars)
                .HasForeignKey(cp => cp.PartId);

            builder
                .Entity<Part>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Parts)
                .HasForeignKey(p => p.SupplierId);

            builder
                .Entity<Sale>()
                .HasOne(s => s.Car)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.CarId);

            builder
                .Entity<Sale>()
                .HasOne(s => s.Customer)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.CustomerId);

            base.OnModelCreating(builder);
        }
    }
}
