using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skoruba.IdentityServer4.Admin.HttpClients.PdmtClientModels
{
    public class DepartmentDto
    {
        public string TreatmentTypeName { get; set; } //Ex: CPAP
        public string DepartmentIdentifier { get; set; } //EX: 78.2215.1515

        public string HealthcareProviderIdentifier { get; set; } // Ex: prov-1

        public DepartmentDto(string treatmentTypeName, string departmentIdentifier, string healthcareProviderIdentifier)
        {
            TreatmentTypeName = treatmentTypeName;
            DepartmentIdentifier = departmentIdentifier;
            HealthcareProviderIdentifier = healthcareProviderIdentifier;
        }
    }
}
