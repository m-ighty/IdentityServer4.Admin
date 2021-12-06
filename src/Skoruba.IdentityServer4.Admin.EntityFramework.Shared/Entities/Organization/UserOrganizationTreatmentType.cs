using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Organization
{
    public class UserOrganizationTreatmentType
    {
        public string UserId { get; set; }
        public int OrganizationTreatmentTypeId { get; set; }

        public virtual UserIdentity User { get; set; }
        public virtual OrganizationTreatmentType OrganizationTreatmentType { get; set; }

        public UserOrganizationTreatmentType() { }

        public UserOrganizationTreatmentType(string userId, int organizationTreatmentTypeId)
        {
            UserId = userId;
            OrganizationTreatmentTypeId = organizationTreatmentTypeId;
        }
    }
}
