using System;

namespace EfInheritanceTest.Domain
{
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
}
