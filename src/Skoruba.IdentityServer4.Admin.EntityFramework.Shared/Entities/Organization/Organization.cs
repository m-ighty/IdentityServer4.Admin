using System;
using System.Collections.Generic;
using System.Text;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Organization
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Organization() {}

        public Organization(string name)
        {
            Name = name;
        }

        public Organization UpdateName(string name)
        {
            Name = name;
            return this;
        }
    }
}
