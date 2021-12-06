using Skoruba.IdentityServer4.Admin.BusinessLogic.Shared.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity
{
    public class UserOrganizationTreatmentTypeDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public int OrganizationId { get; set; }
        //public string OrganizationName { get; set; }

        public List<SelectItemDto> OrganizationTreatmentTypes { get; set; } // All the OrganizationTreatmentTypes in the DB
        public List<AssignedOrganizationTreatmentTypeDto> AssignedOrganizationTreatmentTypes { get; set; } // Already assigned OrganisationTreatmentTypes

        public int NewTreatmentTypeId { get; set; }
    }

    public class AssignedOrganizationTreatmentTypeDto
    {
        public int TreatmentTypeId { get; set; }
        public string Name { get; set; }

        public AssignedOrganizationTreatmentTypeDto(int treatmentTypeId, string name)
        {
            TreatmentTypeId = treatmentTypeId;
            Name = name;
        }
    }
}
