using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        public async Task<dynamic> AddOrganization(OrganizationDto organizationDto)
        {
            var endpoint = "/api/Organization";
        }
    }
}
