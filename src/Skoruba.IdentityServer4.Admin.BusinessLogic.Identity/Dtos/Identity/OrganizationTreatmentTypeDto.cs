using Skoruba.IdentityServer4.Admin.BusinessLogic.Shared.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity
{
    public class OrganizationTreatmentTypeDto
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<TreatmentTypeDto> TreatmentTypes { get; set; }
        public List<SelectItemDto> TreatmentTypesList { get; set; } // List of all TreatmentTypes in the DB

        public int NewTreatmentTypeId { get; set; }
        public string NewTreatmentTypeValue { get; set; }


        public OrganizationTreatmentTypeDto()
        {

        }
        public OrganizationTreatmentTypeDto(int organizationId, string organizationName, List<TreatmentTypeDto> treatmentTypes, List<SelectItemDto> treatmentTypesList)
        {
            OrganizationId = organizationId;
            OrganizationName = organizationName;
            TreatmentTypes = treatmentTypes;
            TreatmentTypesList = treatmentTypesList;
        }
    }

    public class TreatmentTypeDto
    {
        public int TreatmentTypeId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
