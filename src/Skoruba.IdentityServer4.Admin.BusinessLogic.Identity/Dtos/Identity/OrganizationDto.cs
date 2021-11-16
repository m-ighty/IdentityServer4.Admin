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

        public List<OrganizationTreatmentTypeDto> OrganizationTreatmentTypes { get; set; }
        public List<SelectItemDto> TreatmentTypesList { get; set; } // List of all TreatmentTypes in the DB

        public int CurrentSelectedTreatmentTypeId { get; set; }

        public OrganizationDto() {}

        public OrganizationDto(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public OrganizationDto(int id, string name, List<SelectItemDto> treatmentTypesList, List<OrganizationTreatmentTypeDto> organizationTreatmentTypes)
        {
            Id = id;
            Name = name;
            TreatmentTypesList = treatmentTypesList;
            OrganizationTreatmentTypes = organizationTreatmentTypes;
        }
    }

    public class OrganizationTreatmentTypeDto
    {
        public int TreatmentTypeId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
