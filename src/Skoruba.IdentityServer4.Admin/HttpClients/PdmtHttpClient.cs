using Newtonsoft.Json;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Organization;
using Skoruba.IdentityServer4.Admin.HttpClients.PdmtClientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Skoruba.IdentityServer4.Admin.HttpClients
{
    public class PdmtHttpClient
    {
        private readonly HttpClient _client;

        public PdmtHttpClient(HttpClient client)
        {
            _client = client;
        }

        #region HealtcareProvider
        public async Task<HttpResponseMessage> AddOrganization(Organization organization)
        {
            var endpoint = "Organization/healthcareProvider";
            var content = CreateHealthCareProvider(organization);

            var json = JsonConvert.SerializeObject(content);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await _client.PostAsync(endpoint, stringContent);
            return response;
        }

        public async Task<HttpResponseMessage> UpdateOrganization(Organization organization)
        {
            var endpoint = "Organization/healthcareProvider";
            var content = CreateHealthCareProvider(organization);

            var json = JsonConvert.SerializeObject(content);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await _client.PutAsync(endpoint, stringContent);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteOrganization(Organization organization)
        {
            var endpoint = $"Organization/healthcareProvider/prov-{organization.Id}";

            var response = await _client.DeleteAsync(endpoint);
            return response;
        }
        #endregion

        #region Department
        public async Task<HttpResponseMessage> AddDepartment(OrganizationTreatmentType organizationTretmentType)
        {
            var endpoint = "Organization/department";
            var content = CreateDepartment(organizationTretmentType);

            var json = JsonConvert.SerializeObject(content);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await _client.PostAsync(endpoint, stringContent);
            return response;
        }

        public async Task<HttpResponseMessage> UpdateDepartment(OrganizationTreatmentType organizationTretmentType)
        {
            var endpoint = "Organization/department";
            var content = CreateDepartment(organizationTretmentType);

            var json = JsonConvert.SerializeObject(content);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var response = await _client.PutAsync(endpoint, stringContent);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteDepartment(OrganizationTreatmentType organizationTretmentType)
        {
            var endpoint = $"Organization/department/{organizationTretmentType.OrganizationCode}";

            var response = await _client.DeleteAsync(endpoint);
            return response;
        }
        #endregion

        private HealthcareProviderDto CreateHealthCareProvider(Organization organization)
        {
            return new HealthcareProviderDto($"prov-{organization.Id}", organization.Name, organization.AddressLine, organization.City, organization.PostalCode);
        }

        private DepartmentDto CreateDepartment(OrganizationTreatmentType organizationTretmentType)
        {
            return new DepartmentDto(organizationTretmentType.TreatmentType.Name, organizationTretmentType.OrganizationCode, $"prov-{organizationTretmentType.OrganizationId}");
        }
    }
}
