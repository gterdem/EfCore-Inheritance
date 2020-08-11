using System;
using System.Collections.Generic;
using EfInheritanceTest.Domain.Choices;

namespace EfInheritanceTest.Domain.Choosable
{
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
}
