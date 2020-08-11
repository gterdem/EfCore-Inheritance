using System;

namespace EfInheritanceTest.Domain.NoneDrivable
{
    public class Drone : VehicleBase
    {
        public Drone(Guid id, string name) : base(id, name)
        {
        }
    }
}
