using Skoruba.IdentityServer4.Admin.BusinessLogic.Shared.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces
{
    public interface IUserInviteLinkDto
    {
        List<SelectItemDto> RoleList { get; set; }
        string RoleId { get; set; }

        List<SelectItemDto> OrganizationList { get; set; }
        string OrganizationId { get; set; }
    }
}
