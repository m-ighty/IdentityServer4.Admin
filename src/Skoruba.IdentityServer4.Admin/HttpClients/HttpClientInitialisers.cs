using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Skoruba.IdentityServer4.Admin.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skoruba.IdentityServer4.Admin.HttpClients
{
    public static class HttpClientInitialisers
    {
        public static IServiceCollection AddPdmtHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            var pdmtConfiguration = configuration.GetSection(nameof(PdmtConfiguration)).Get<PdmtConfiguration>();

            services.AddHttpClient<PdmtHttpClient>(c =>
            {
                c.BaseAddress = new Uri(pdmtConfiguration.PdmtBaseUrl);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            return services;
        }
    }
}
