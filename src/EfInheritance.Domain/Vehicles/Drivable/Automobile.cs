using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EfInheritanceTest.Domain.Choices;

namespace EfInheritanceTest.Domain.Choosable
{
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
}
