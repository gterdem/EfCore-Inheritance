using System;
using System.Threading.Tasks;
using EfInheritanceTest;
using EfInheritanceTest.Domain.Choosable;
using EfInheritanceTest.Domain.NoneDrivable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EfInheritance.Test
{
    public class DbFixture
    {
        public ServiceProvider ServiceProvider { get; private set; }

        public DbFixture()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(
                        "Server=(localdb)\\mssqllocaldb;Database=MyEfCoreTestDb;Trusted_Connection=True;MultipleActiveResultSets=true"),
                ServiceLifetime.Transient);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public async Task SeedVehicleData()
        {
            await using (var _context = ServiceProvider.GetService<AppDbContext>())
            {
                await _context.Drones.AddAsync(new Drone(Guid.NewGuid(), "DroneX12"));

                var superCar = new Automobile(TestData.automobileId, "SuperCar");
                superCar.AddDriver(Guid.NewGuid(), "1st Capt.", true)
                    .AddDriver(Guid.NewGuid(), "2nd Capt.", false);

                await _context.Automobiles.AddAsync(superCar);

                var planeX = new Plane(TestData.planeId, "PlaneXtr15");
                planeX.AddDriver(Guid.NewGuid(), "1st Capt.", true)
                    .AddDriver(Guid.NewGuid(), "2nd Capt.", false)
                    .AddDriver(Guid.NewGuid(), "3rd Capt.", false)
                    .AddDriver(Guid.NewGuid(), "4th Capt.", false);

                await _context.Planes.AddAsync(new Plane(TestData.planeId, "SuperPlane"));

                var yactFx = new Yact(TestData.yactId, "YactFx");
                yactFx.AddDriver(Guid.NewGuid(), "1st Capt.", true)
                    .AddDriver(Guid.NewGuid(), "2nd Capt.", false)
                    .AddDriver(Guid.NewGuid(), "3rd Capt.", false);

                await _context.Yacts.AddAsync(yactFx);
                await _context.SaveChangesAsync();
            }
        }
    }
}
