using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Shared.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity
{
    public class OrganizationDto : IOrganizationDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        public OrganizationDto() {}

        public OrganizationDto(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public OrganizationDto(int id, string name, string addressLine, string city, string postalCode)
        {
            Id = id;
            Name = name;
            AddressLine = addressLine;
            City = city;
            PostalCode = postalCode;
        }
    }
}
