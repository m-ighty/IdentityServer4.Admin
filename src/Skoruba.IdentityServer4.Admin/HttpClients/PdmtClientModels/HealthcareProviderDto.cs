using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skoruba.IdentityServer4.Admin.HttpClients.PdmtClientModels
{
    public class HealthcareProviderDto
    {
        public string Id { get; set; } // We store this in "Identifiers" as "prov"+id --> Ex prov-1
        public string RizivNumber { get; set; }
        public string Name { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        public HealthcareProviderDto(string id, string name, string rizivNumber, string addressLine, string city, string postalCode)
        {
            Id = id;
            RizivNumber = rizivNumber;
            Name = name;
            AddressLine = addressLine;
            City = city;
            PostalCode = postalCode;
        }
    }
}
