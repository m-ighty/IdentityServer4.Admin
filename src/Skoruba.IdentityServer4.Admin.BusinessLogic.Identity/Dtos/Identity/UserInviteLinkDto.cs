using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity
{
    public class UserInviteLinkDto : IUserInviteLinkDto
    {
        public int? RoleId { get; set; }
        public int? OrganizationId { get; set; }
    }
}
