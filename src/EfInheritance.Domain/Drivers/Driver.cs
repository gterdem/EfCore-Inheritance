using System;

namespace EfInheritanceTest.Domain.Choices
{
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
}
