using Microsoft.AspNetCore.Identity;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Organization;
using System.Collections.Generic;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity
{
	public class UserIdentity : IdentityUser
	{
		public int OrganizationId { get; set; }
		public virtual Organization.Organization Organization { get; set; }

		public readonly List<UserOrganizationTreatmentType> _userOrganizationTreatmentTypes;
		public virtual IReadOnlyCollection<UserOrganizationTreatmentType> UserOrganizationTreatmentTypes => _userOrganizationTreatmentTypes;

        public UserIdentity() : base()
        {
			_userOrganizationTreatmentTypes = new List<UserOrganizationTreatmentType>();

		}
	}
}