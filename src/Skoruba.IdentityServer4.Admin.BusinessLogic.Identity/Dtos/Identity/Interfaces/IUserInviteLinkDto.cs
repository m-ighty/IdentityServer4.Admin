using System;
using System.Collections.Generic;
using System.Text;

namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces
{
    public interface IUserInviteLinkDto
    {
        public int? RoleId { get; set; }
        public int? OrganizationId { get; set; }
    }
}
