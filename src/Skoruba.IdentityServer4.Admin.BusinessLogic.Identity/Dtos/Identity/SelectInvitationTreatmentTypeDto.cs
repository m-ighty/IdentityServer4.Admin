using System;
using System.Collections.Generic;
using System.Text;

namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity
{
    public class SelectInvitationTreatmentTypeDto
    {
        public int TreatmentTypeId { get; set; }

        public string TreatmentTypeName { get; set; }

        public bool Selected { get; set; }
    }
}
