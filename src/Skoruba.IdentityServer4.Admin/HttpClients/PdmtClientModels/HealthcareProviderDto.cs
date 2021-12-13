using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skoruba.IdentityServer4.Admin.HttpClients.PdmtClientModels
{
    public class HealthcareProviderDto
    {
        public string Id { get; set; } // We store this in "Identifiers" as "prov"+id --> Ex prov-1
        public string Name { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        public HealthcareProviderDto(string id, string name, string addressLine, string city, string postalCode)
        {
            Id = id;
            Name = name;
            AddressLine = addressLine;
            City = city;
            PostalCode = postalCode;
        }
    }
}
