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
        public DbSet<TreatmentType> TreatmentTypes { get; set; }
        public DbSet<OrganizationTreatmentType> OrganizationTreatmentTypes { get; set; }

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

            builder.Entity<UserInvitation>().HasOne(ui => ui.Organization)
                .WithMany()
                .HasForeignKey(ui => ui.OrganizationId);

            builder.Entity<UserInvitation>().HasOne(ui => ui.Role)
                .WithMany()
                .HasForeignKey(ui => ui.RoleId);

            builder.Entity<UserInvitation>().ToTable(TableConsts.UserInvitations);
        }

        private void ConfigureOrganizationContext(ModelBuilder builder)
        {
            // Organization
            builder.Entity<Organization>().HasKey(o => o.Id);

            builder.Entity<Organization>().Property(o => o.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Entity<UserIdentity>().HasOne(ui => ui.Organization)
                .WithMany()
                .HasForeignKey(ui => ui.OrganizationId);

            builder.Entity<Organization>().ToTable(TableConsts.Organizations);


            // TreatmentType
            builder.Entity<TreatmentType>().HasKey(tt => tt.Id);

            builder.Entity<TreatmentType>().Property(tt => tt.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Entity<TreatmentType>().Property(tt => tt.Name).IsRequired();

            builder.Entity<TreatmentType>().ToTable(TableConsts.TreatmentTypes);


            // OrganizationTreatmentType
            builder.Entity<OrganizationTreatmentType>().HasKey(ott => new { ott.OrganizationId, ott.TreatmentTypeId });

            builder.Entity<OrganizationTreatmentType>().HasIndex(ott => ott.OrganizationCode).IsUnique();

            builder.Entity<OrganizationTreatmentType>().HasOne(ott => ott.Organization)
                .WithMany(o => o.OrganizationTreatmentTypes)
                .HasForeignKey(ott => ott.OrganizationId);

            builder.Entity<OrganizationTreatmentType>().HasOne(ott => ott.TreatmentType)
                .WithMany()
                .HasForeignKey(ott => ott.TreatmentTypeId);

            builder.Entity<OrganizationTreatmentType>().ToTable(TableConsts.OrganizationTreatmentTypes);
        }
    }
}