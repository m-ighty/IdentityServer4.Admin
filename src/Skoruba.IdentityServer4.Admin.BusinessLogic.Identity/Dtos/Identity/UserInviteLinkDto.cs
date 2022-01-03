using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Shared.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity
{
    public class UserInviteLinkDto : IUserInviteLinkDto
    {
        public List<SelectItemDto> RoleList { get; set; }
        public string RoleId { get; set; }

        public List<SelectItemDto> OrganizationList { get; set; }

        public List<SelectInvitationTreatmentTypeDto> TreatmentTypes { get; set; }

        public int[] SelectedTreatmentTypes { get; set; }

        public string OrganizationId { get; set; }
    }
}
