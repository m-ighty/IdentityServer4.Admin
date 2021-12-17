using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Organization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity
{
    public class UserInvitationTreatmentType
    {
        public Guid UserInvitationId { get; set; }

        public int TreatmentTypeId { get; set; }

        public TreatmentType TreatmentType { get; set; }

        public UserInvitation UserInvitation { get; set; }
    }
}
