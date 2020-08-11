using EfInheritance.Shared;
using EfInheritanceTest.Domain;
using EfInheritanceTest.Domain.Choices;
using EfInheritanceTest.Domain.Choosable;
using EfInheritanceTest.Domain.NoneDrivable;
using Microsoft.EntityFrameworkCore;

namespace EfInheritanceTest
{
    public class AppDbContext : DbContext
    {
        public DbSet<VehicleBase> Vehicles { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Automobile> Automobiles { get; set; }
        public DbSet<Plane> Planes { get; set; }
        public DbSet<Yact> Yacts { get; set; }
        public DbSet<Drone> Drones { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VehicleBase>(vb =>
            {
                vb.ToTable("Vehicles");
                vb.HasDiscriminator<VehicleTypes>("Type")
                    .HasValue<Drone>(VehicleTypes.Drone)
                    .HasValue<Automobile>(VehicleTypes.Automobile)
                    .HasValue<Plane>(VehicleTypes.Plane)
                    .HasValue<Yact>(VehicleTypes.Yact);
            });

            modelBuilder.Entity<Driver>(c =>
            {
                c.ToTable("Drivers");
                c.HasIndex(x => new {x.Id, x.VehicleId});
            });

            modelBuilder.Entity<Automobile>(a =>
            {
                a.HasMany(v => v.Drivers)
                    .WithOne()
                    .HasForeignKey(q => q.VehicleId)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .IsRequired();
            });

            modelBuilder.Entity<Plane>(a =>
            {
                a.HasMany(v => v.Drivers)
                    .WithOne()
                    .HasForeignKey(q => q.VehicleId)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .IsRequired();
            });

            modelBuilder.Entity<Yact>(a =>
            {
                a.HasMany(v => v.Drivers)
                    .WithOne()
                    .HasForeignKey(q => q.VehicleId)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .IsRequired();
            });
        }
    }
}
