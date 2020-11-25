using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EfInheritanceTest;
using EfInheritanceTest.Domain;
using EfInheritanceTest.Domain.Choosable;
using EfInheritanceTest.Domain.NoneDrivable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace EfInheritance.Test
{
    public class Vehicle_Tests : IClassFixture<DbFixture>
    {
        private readonly ServiceProvider _serviceProvider;

        public Vehicle_Tests(DbFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        [Fact]
        public async Task Vehicles_Should_Have_Drivers()
        {
            if (!await IsSeeded())
            {
                await SeedVehicleData();
            }

            await using (var _context = _serviceProvider.GetService<AppDbContext>())
            {
                var vehicles = await _context.Vehicles.ToListAsync();
                vehicles.Count.ShouldBe(4);

                // Problem with include -> returns repeating duplicate data
                var vehiclesWithDrivers = await _context.Vehicles.AsNoTracking().Include("Drivers").ToListAsync();

                var automobile = vehiclesWithDrivers.First(q => q.Id == TestData.automobileId);
                automobile.Name.ShouldBe(TestData.automobileName);
                (automobile as Automobile)?.Drivers.Count.ShouldBe(2);

                var plane = vehiclesWithDrivers.First(q => q.Id == TestData.planeId);
                plane.Name.ShouldBe(TestData.planeName);
                (plane as Plane)?.Drivers.Count.ShouldBe(4);

                vehiclesWithDrivers.Count.ShouldBe(4); //but was 15 -> 9 yacts, 1 automobile, 4 plane, 1 drone
            }
        }

        private async Task SeedVehicleData()
        {
            await using (var _context = _serviceProvider.GetService<AppDbContext>())
            {
                await _context.Drones.AddAsync(new Drone(Guid.NewGuid(), "DroneX12"));

                var superCar = new Automobile(TestData.automobileId, TestData.automobileName);
                superCar.AddDriver(Guid.NewGuid(), "1st Capt.", true)
                    .AddDriver(Guid.NewGuid(), "2nd Capt.", false);
                await _context.Automobiles.AddAsync(superCar);

                var planeX = new Plane(TestData.planeId, TestData.planeName);
                planeX.AddDriver(Guid.NewGuid(), "1st Capt.", true)
                    .AddDriver(Guid.NewGuid(), "2nd Capt.", false)
                    .AddDriver(Guid.NewGuid(), "3rd Capt.", false)
                    .AddDriver(Guid.NewGuid(), "4th Capt.", false);
                await _context.Planes.AddAsync(planeX);

                var yactFx = new Yact(TestData.yactId, "YactFx");
                yactFx.AddDriver(Guid.NewGuid(), "1st Capt.", true)
                    .AddDriver(Guid.NewGuid(), "2nd Capt.", false)
                    .AddDriver(Guid.NewGuid(), "3rd Capt.", false);
                await _context.Yacts.AddAsync(yactFx);

                await _context.SaveChangesAsync();
            }
        }

        private async Task<List<VehicleBase>> GetVehiclesAsync()
        {
            await using (var _context = _serviceProvider.GetService<AppDbContext>())
            {
                return await _context.Vehicles.ToListAsync();
            }
        }

        private async Task<bool> IsSeeded()
        {
            await using (var _context = _serviceProvider.GetService<AppDbContext>())
            {
                return (await _context.Vehicles.ToListAsync()).Count > 0;
            }
        }
    }
}