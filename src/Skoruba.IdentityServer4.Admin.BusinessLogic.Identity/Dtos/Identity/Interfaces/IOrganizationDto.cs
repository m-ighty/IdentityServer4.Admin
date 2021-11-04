using System;
using System.Collections.Generic;
using System.Text;

namespace Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity.Interfaces
{
    public interface IOrganizationDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}
