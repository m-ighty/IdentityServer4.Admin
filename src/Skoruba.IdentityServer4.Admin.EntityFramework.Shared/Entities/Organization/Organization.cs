using System;
using System.Collections.Generic;
using System.Text;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Organization
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        public readonly List<OrganizationTreatmentType> _organizationTreatmentTypes;
        public virtual IReadOnlyCollection<OrganizationTreatmentType> OrganizationTreatmentTypes => _organizationTreatmentTypes;

        public Organization() {}

        public Organization(string name, string addressLine, string city, string postalCode)
        {
            Name = name;
            AddressLine = addressLine;
            City = city;
            PostalCode = postalCode;
        }

        public Organization UpdateName(string name, string addressLine, string city, string postalCode)
        {
            Name = name;
            AddressLine = addressLine;
            City = city;
            PostalCode = postalCode;

            return this;
        }
    }
}
