using Microsoft.AspNetCore.Identity;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity
{
	public class UserIdentity : IdentityUser
	{
		public int OrganizationId { get; set; }
		public virtual Organization.Organization Organization { get; set; }
	}
}