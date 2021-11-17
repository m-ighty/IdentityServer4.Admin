using System;
using System.Collections.Generic;
using System.Text;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Organization
{
    public class OrganizationTreatmentType
    {
        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        public int TreatmentTypeId { get; set; }
        public virtual TreatmentType TreatmentType { get; set; }

        public string OrganizationCode { get; set; } // Unique code used by linde to link/invoice the hospital

        public OrganizationTreatmentType() { }

        public OrganizationTreatmentType(int organizationId, int treatmentTypeId, string organizationCode)
        {
            OrganizationId = organizationId;
            TreatmentTypeId = treatmentTypeId;
            OrganizationCode = organizationCode;
        }

        public void UpdateValue(string newCode)
        {
            OrganizationCode = newCode;
        }
    }
}
