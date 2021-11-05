using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity
{
    public class OrganizationsDto : IOrganizationsDto
    {
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<OrganizationDto> Organizations { get; set; }
    }
}
