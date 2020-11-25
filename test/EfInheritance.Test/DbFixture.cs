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
    }
}
