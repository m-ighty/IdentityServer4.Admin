using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.DbContexts;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Skoruba.IdentityServer4.STS.Identity.Services
{
    public class ProfileService<TUser, TKey> : DefaultProfileService
        where TUser : IdentityUser<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        private readonly UserManager<TUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<TUser> _claimsFactory;
        private readonly AdminIdentityDbContext _adminIdentityDbContext;

        public ProfileService(ILogger<DefaultProfileService> logger,
            UserManager<TUser> userManager,
            IUserClaimsPrincipalFactory<TUser> claimsFactory,
            AdminIdentityDbContext adminIdentityDbContext) : base(logger)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
            _adminIdentityDbContext = adminIdentityDbContext;
        }

        public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            // Add custom claims in token here based on user properties or any other source
            var treatmentTypeClaims = _adminIdentityDbContext.OrganizationTreatmentTypes
                .Where(ott => ott.OrganizationId == (user as UserIdentity).OrganizationId)
                .Select(ott => new Claim(ott.TreatmentType.Name, ott.OrganizationCode))
                .ToList();

            claims.AddRange(treatmentTypeClaims);

            //claims.Add(new Claim("CPAP", "123456789")); // Example of custom claim

            context.IssuedClaims = claims;
        }

        public override async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
