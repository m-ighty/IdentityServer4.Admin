﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity
{
    public class UserInvitation
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime ValidTill { get; set; }
        public DateTime? Visited { get; set; }
        public bool Used { get; set; }

        public string RoleId { get; set; }
        public virtual UserIdentityRole Role { get; set; }

        public int OrganizationId { get; set; }
        public virtual Organization.Organization Organization { get; set; }

        public UserInvitation(string roleId, int organizationId)
        {
            // Id generated by Database
            RoleId = roleId;
            OrganizationId = organizationId;
            Created = DateTime.UtcNow;
            ValidTill = DateTime.UtcNow.AddDays(2); // link will work for 48hours
        }

        public UserInvitation InvitationPageVisited()
        {
            Visited = DateTime.UtcNow;
            return this;
        }

        public UserInvitation InvitationIsUsed()
        {
            Used = true;
            return this;
        }

        /// When a user goes to the "invitation page" to create his/her account,
        /// the Visited prop is set, the invitation link will then remain valid for 5 minutes
    }
}
