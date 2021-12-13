using System;
using System.Collections.Generic;
using System.Text;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Organization
{
    public class TreatmentType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TreatmentType() {}
        public TreatmentType(string name)
        {
            Name = name;
        }
    }
}
