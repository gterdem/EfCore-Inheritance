# EfCore-Inheritance
EfCore Inheritance test project

### Db Creation-Migration

run command `dotnet ef migrations add InitialCreate --startup-project ../EfInheritance.API/EfInheritance.API.csproj --verbose` under **EfInheritance.Domain** folder for migrations.



run command `dotnet ef database update --startup-project ../EfInheritance.API/EfInheritance.API.csproj --verbose`  under **EfInheritance.Domain** folder to update database.



### Testing

Under **EfInheritance.Test** project, run the **Vehicles_Should_Have_Drivers** test.

### DbContext

```csharp
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
```

### Domain Inheritance

```csharp
public abstract class VehicleBase
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public VehicleBase(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
```

```csharp
public class Drone : VehicleBase
{
    public Drone(Guid id, string name) : base(id, name)
    {
    }
}
```

**Drivables:**

```csharp
public class Automobile : VehicleBase, IDrivable<Automobile>
{
    public ICollection<Driver> Drivers { get; private set; }

    public Automobile(Guid id, string name) : base(id, name)
    {
        Drivers = new Collection<Driver>();
    }

    public Automobile AddDriver(Guid id, string title, bool hasLicense = false)
    {
        Drivers.Add(new Driver(id, title, hasLicense));
        return this;
    }

    public ICollection<Driver> GetDrivers()
    {
        return Drivers;
    }
}
```

```csharp
public class Plane : VehicleBase, IDrivable<Plane>
{
    public ICollection<Driver> Drivers { get; private set; }

    public Plane(Guid id, string name) : base(id, name)
    {
        Drivers = new List<Driver>();
    }

    public Plane AddDriver(Guid id, string title, bool hasLicense = false)
    {
        Drivers.Add(new Driver(id, title, hasLicense));
        return this;
    }

    public ICollection<Driver> GetDrivers()
    {
        return Drivers;
    }
}
```

```csharp
public class Yact : VehicleBase, IDrivable<Yact>
{
    public ICollection<Driver> Drivers { get; private set; }

    public Yact(Guid id, string name) : base(id, name)
    {
        Drivers = new Collection<Driver>();
    }

    public Yact AddDriver(Guid id, string title, bool hasLicense = false)
    {
        Drivers.Add(new Driver(id, title, hasLicense));
        return this;
    }

    public ICollection<Driver> GetDrivers()
    {
        return Drivers;
    }
}
```

```csharp
public interface IDrivable<TVehicle> : IDrivable
{
    TVehicle AddDriver(Guid id, string title, bool hasLicense = false);
}

public interface IDrivable
{
    ICollection<Driver> GetDrivers();
}
```

**Driver:**

```csharp
public class Driver
{
    public Guid Id { get; private set; }
    public Guid VehicleId { get; set; }
    public string Title { get; set; }
    public bool HasLicense { get; set; }

    protected Driver()
    {

    }
    public Driver(Guid id, string title, bool hasLicense)
    {
        Id = id;
        Title = title;
        HasLicense = hasLicense;
    }
}
```

**Issue:**

Including sub collection causes n^2 added collection item repeated record return. 

```csharp
var vehiclesWithDrivers = await _context.Vehicles.AsNoTracking().Include("Drivers").ToListAsync();
```

