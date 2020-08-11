using System;
using System.Collections.Generic;
using EfInheritanceTest.Domain.Choices;

namespace EfInheritanceTest.Domain.Choosable
{
    public interface IDrivable<TVehicle> : IDrivable
    {
        TVehicle AddDriver(Guid id, string title, bool hasLicense = false);
    }

    public interface IDrivable
    {
        ICollection<Driver> GetDrivers();
    }
}
