using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Constants;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Organization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Skoruba.IdentityServer4.Admin.EntityFramework.Shared.DbContexts
{
    public class AdminIdentityDbContext : IdentityDbContext<UserIdentity, UserIdentityRole, string, UserIdentityUserClaim, UserIdentityUserRole, UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken>
    {
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<UserInvitation> UserInvitations { get; set; }

        public AdminIdentityDbContext(DbContextOptions<AdminIdentityDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureIdentityContext(builder);
            ConfigureOrganizationContext(builder);
        }

        private void ConfigureIdentityContext(ModelBuilder builder)
        {
            builder.Entity<UserIdentityRole>().ToTable(TableConsts.IdentityRoles);
            builder.Entity<UserIdentityRoleClaim>().ToTable(TableConsts.IdentityRoleClaims);
            builder.Entity<UserIdentityUserRole>().ToTable(TableConsts.IdentityUserRoles);

            builder.Entity<UserIdentity>().ToTable(TableConsts.IdentityUsers);
            builder.Entity<UserIdentityUserLogin>().ToTable(TableConsts.IdentityUserLogins);
            builder.Entity<UserIdentityUserClaim>().ToTable(TableConsts.IdentityUserClaims);
            builder.Entity<UserIdentityUserToken>().ToTable(TableConsts.IdentityUserTokens);

            // UserInvitations
            builder.Entity<UserInvitation>().HasKey(o => o.Id);
            builder.Entity<UserInvitation>().Property(o => o.Id).ValueGeneratedOnAdd().IsRequired();

        }

        private void ConfigureOrganizationContext(ModelBuilder builder)
        {
            builder.Entity<Organization>().HasKey(o => o.Id);

            builder.Entity<Organization>().Property(o => o.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Entity<UserIdentity>().HasOne(ui => ui.Organization)
                .WithMany()
                .HasForeignKey(ui => ui.OrganizationId);

            builder.Entity<Organization>().ToTable(TableConsts.Organizations);
        }
    }
}