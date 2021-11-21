using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Skoruba.AuditLogging.EntityFramework.DbContexts;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.IdentityServer4.Admin.Configuration;
using Skoruba.IdentityServer4.Admin.Configuration.Interfaces;
using Skoruba.IdentityServer4.Admin.EntityFramework.Interfaces;

namespace Skoruba.IdentityServer4.Admin.Helpers
{
    public static class DbMigrationHelpers
    {
        /// <summary>
        /// Generate migrations before running this method, you can use these steps bellow:
        /// https://github.com/skoruba/IdentityServer4.Admin#ef-core--data-access
        /// </summary>
        /// <param name="host"></param>
        /// <param name="applyDbMigrationWithDataSeedFromProgramArguments"></param>
        /// <param name="seedConfiguration"></param>
        /// <param name="databaseMigrationsConfiguration"></param>
        public static async Task ApplyDbMigrationsWithDataSeedAsync<TIdentityServerDbContext, TIdentityDbContext,
            TPersistedGrantDbContext, TLogDbContext, TAuditLogDbContext, TDataProtectionDbContext, TUser, TRole>(
            IHost host, bool applyDbMigrationWithDataSeedFromProgramArguments, SeedConfiguration seedConfiguration,
            DatabaseMigrationsConfiguration databaseMigrationsConfiguration, bool forceProgramArgument)
            where TIdentityServerDbContext : DbContext, IAdminConfigurationDbContext
            where TIdentityDbContext : DbContext
            where TPersistedGrantDbContext : DbContext, IAdminPersistedGrantDbContext
            where TLogDbContext : DbContext, IAdminLogDbContext
            where TAuditLogDbContext : DbContext, IAuditLoggingDbContext<AuditLog>
            where TDataProtectionDbContext : DbContext, IDataProtectionKeyContext
            where TUser : IdentityUser, new()
            where TRole : IdentityRole, new()
        {
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                
                if ((databaseMigrationsConfiguration != null && databaseMigrationsConfiguration.ApplyDatabaseMigrations) 
                    || (applyDbMigrationWithDataSeedFromProgramArguments))
                {
                    await EnsureDatabasesMigratedAsync<TIdentityDbContext, TIdentityServerDbContext, TPersistedGrantDbContext, TLogDbContext, TAuditLogDbContext, TDataProtectionDbContext>(services);
                }

                if ((seedConfiguration != null && seedConfiguration.ApplySeed) 
                    || (applyDbMigrationWithDataSeedFromProgramArguments))
                {
                    await EnsureSeedDataAsync<TIdentityServerDbContext, TUser, TRole>(services, forceProgramArgument);
                }
            }
        }

        public static async Task EnsureDatabasesMigratedAsync<TIdentityDbContext, TConfigurationDbContext, TPersistedGrantDbContext, TLogDbContext, TAuditLogDbContext, TDataProtectionDbContext>(IServiceProvider services)
            where TIdentityDbContext : DbContext
            where TPersistedGrantDbContext : DbContext
            where TConfigurationDbContext : DbContext
            where TLogDbContext : DbContext
            where TAuditLogDbContext : DbContext
            where TDataProtectionDbContext : DbContext
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<TPersistedGrantDbContext>())
                {
                    await context.Database.MigrateAsync();
                }

                using (var context = scope.ServiceProvider.GetRequiredService<TIdentityDbContext>())
                {
                    await context.Database.MigrateAsync();
                }

                using (var context = scope.ServiceProvider.GetRequiredService<TConfigurationDbContext>())
                {
                    await context.Database.MigrateAsync();
                }

                using (var context = scope.ServiceProvider.GetRequiredService<TLogDbContext>())
                {
                    await context.Database.MigrateAsync();
                }

                using (var context = scope.ServiceProvider.GetRequiredService<TAuditLogDbContext>())
                {
                    await context.Database.MigrateAsync();
                }

                using (var context = scope.ServiceProvider.GetRequiredService<TDataProtectionDbContext>())
                {
                    await context.Database.MigrateAsync();
                }
            }
        }

        public static async Task EnsureSeedDataAsync<TIdentityServerDbContext, TUser, TRole>(IServiceProvider serviceProvider, bool force)
        where TIdentityServerDbContext : DbContext, IAdminConfigurationDbContext
        where TUser : IdentityUser, new()
        where TRole : IdentityRole, new()
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TIdentityServerDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<TRole>>();
                var rootConfiguration = scope.ServiceProvider.GetRequiredService<IRootConfiguration>();



                
                await EnsureSeedIdentityServerData(context, rootConfiguration.IdentityServerDataConfiguration, force);
                await EnsureSeedIdentityData(userManager, roleManager, rootConfiguration.IdentityDataConfiguration, force);
            }
        }

        /// <summary>
        /// Generate default admin user / role
        /// </summary>
        private static async Task EnsureSeedIdentityData<TUser, TRole>(UserManager<TUser> userManager,
            RoleManager<TRole> roleManager, IdentityDataConfiguration identityDataConfiguration, bool force)
            where TUser : IdentityUser, new()
            where TRole : IdentityRole, new()
        {


            DumpDataIdentity(userManager, roleManager);





            var applyRoles = !await roleManager.Roles.AnyAsync();


            Log.Information($"DB SEEDING Must apply roles: {applyRoles}| ");
            if (applyRoles | force)
            {
                // adding roles from seed
                foreach (var r in identityDataConfiguration.Roles)
                {
                    if (!await roleManager.RoleExistsAsync(r.Name))
                    {
                        var role = new TRole
                        {
                            Name = r.Name
                        };

                        Log.Information($"DB SEEDING Must apply identity_roles {role.Name} ");
                        var result = await roleManager.CreateAsync(role);

                        if (result.Succeeded)
                        {
                            foreach (var claim in r.Claims)
                            {
                                Log.Information($"DB SEEDING Must apply identity_roles claims  { role.Name}, {claim.Type}, {claim.Value} ", role.Name, claim.Type, claim.Value);
                                await roleManager.AddClaimAsync(role, new System.Security.Claims.Claim(claim.Type, claim.Value));
                            }
                        }
                    }
                }
            }

            var applyUser = !await userManager.Users.AnyAsync();

            Log.Information($"DB SEEDING Must apply user roles: {applyUser} ");
            if (applyUser | force)
            {
                // adding users from seed
                foreach (var user in identityDataConfiguration.Users)
                {
                    Log.Information($"DB SEEDING adding user: '{user.Username}'");
                       var identityUser = new TUser
                    {
                        UserName = user.Username,
                        Email = user.Email,
                        EmailConfirmed = true
                    };

                    // if there is no password we create user without password
                    // user can reset password later, because accounts have EmailConfirmed set to true
                    var result = !string.IsNullOrEmpty(user.Password)
                        ? await userManager.CreateAsync(identityUser, user.Password)
                        : await userManager.CreateAsync(identityUser);

                    if (result.Succeeded)
                    {
                        
                        foreach (var claim in user.Claims)
                        {
                            Log.Information($"DB SEEDING adding claim: {user.Username}, {claim.Type}, {claim.Value}" );
                            await userManager.AddClaimAsync(identityUser, new System.Security.Claims.Claim(claim.Type, claim.Value));
                        }

                        
                        foreach (var role in user.Roles)
                        {
                            Log.Information($"DB SEEDING adding roles: {user.Username}, {role}" );
                            await userManager.AddToRoleAsync(identityUser, role);
                        }
                    }
                }
            }
        }

        private static void DumpDataIdentity<TUser, TRole>(UserManager<TUser> userManager, RoleManager<TRole> roleManager)
            where TUser : IdentityUser, new()
            where TRole : IdentityRole, new()
        {
            Log.Information(" DB SEEDING dump users for db");
            foreach (var user in userManager.Users.ToList())
            {
                Log.Information($"users defined in db: {user.Email} {user.UserName} ");
            }

            Log.Information(" DB SEEDING dump Roles for db");
            foreach (var role in roleManager.Roles.ToList())
            {
                Log.Information($"IdentityResources defined in db: {role.Name} ");
            }
        }

        /// <summary>
        /// Generate default clients, identity and api resources
        /// </summary>
        private static async Task EnsureSeedIdentityServerData<TIdentityServerDbContext>(TIdentityServerDbContext context, IdentityServerDataConfiguration identityServerDataConfiguration, bool force)
            where TIdentityServerDbContext : DbContext, IAdminConfigurationDbContext
        {
            DumpIdentityServerData(context);

            Log.Warning($"using database {context.Database.GetDbConnection().Database},{context.Database.GetDbConnection().DataSource}, waiting 10sec before apply ");
            var  applyIdentityResource = !await context.IdentityResources.AnyAsync();
            Log.Information($"DB SEEDING Must apply identityresources: {applyIdentityResource},  force : {force}") ;
            if (applyIdentityResource | force)
            {
                foreach (var resource in identityServerDataConfiguration.IdentityResources)
                {
                    await context.IdentityResources.AddAsync(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }

            var applyApiResources = ! await context.ApiResources.AnyAsync();

            Log.Information($"DB SEEDING Must apply api resources: {applyApiResources}, force : {force}");
            if (applyApiResources | force)
            {
                foreach (var resource in identityServerDataConfiguration.ApiResources)
                {
                    foreach (var s in resource.ApiSecrets)
                    {
                        s.Value = s.Value.ToSha256();
                    }

                    await context.ApiResources.AddAsync(resource.ToEntity());
                }

                await context.SaveChangesAsync();
            }

            var applyClients = ! await context.Clients.AnyAsync();
            Log.Information($"DB SEEDING Must apply Clients: {applyClients} | force : {force}");
            if (applyClients | force)
            {
                foreach (var client in identityServerDataConfiguration.Clients)
                {
                    foreach (var secret in client.ClientSecrets)
                    {
                        secret.Value = secret.Value.ToSha256();
                    }

                    client.Claims = client.ClientClaims
                        .Select(c => new System.Security.Claims.Claim(c.Type, c.Value))
                        .ToList();

                    await context.Clients.AddAsync(client.ToEntity());
                }

                await context.SaveChangesAsync();
            }
        }

        private static void DumpIdentityServerData<TIdentityServerDbContext>(TIdentityServerDbContext context) where TIdentityServerDbContext : DbContext, IAdminConfigurationDbContext
        {
            Log.Information("DB SEEDING dump Clients fro db");
            foreach (var client in context.Clients.ToList()) {
                Log.Information($"client defined in db: {client.ClientId} ");
            }

            Log.Information(" DB SEEDING dump IdentityResources fro db");
            foreach (var res in context.IdentityResources.ToList())
            {
                Log.Information($"IdentityResources defined in db: {res.Name} ");
            }
            Log.Information(" DB SEEDING dump ApiResources fro db");
            foreach (var res in context.ApiResources.ToList())
            {
                Log.Information($"IdentityResources defined in db: {res.Name} ");
            }


        }
    }
}
