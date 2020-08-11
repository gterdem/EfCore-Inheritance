using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EfInheritanceTest.Domain.Choices;

namespace EfInheritanceTest.Domain.Choosable
{
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
}
