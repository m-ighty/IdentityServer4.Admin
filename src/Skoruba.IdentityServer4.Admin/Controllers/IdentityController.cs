﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Services.Interfaces;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Shared.Dtos.Common;
using Skoruba.IdentityServer4.Admin.Configuration.Constants;
using Skoruba.IdentityServer4.Admin.Configuration.Interfaces;
using Skoruba.IdentityServer4.Admin.EntityFramework.Extensions.Extensions;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.DbContexts;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity;
using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Organization;
using Skoruba.IdentityServer4.Admin.ExceptionHandling;
using Skoruba.IdentityServer4.Admin.Helpers.Localization;

namespace Skoruba.IdentityServer4.Admin.Controllers
{
    [Authorize(Policy = AuthorizationConsts.AdministrationPolicy)]
    [TypeFilter(typeof(ControllerExceptionFilterAttribute))]
    public class IdentityController<TUserDto, TRoleDto, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
            TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
            TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto, TUserClaimDto, TRoleClaimDto> : BaseController
        where TUserDto : UserDto<TKey>, new()
        where TRoleDto : RoleDto<TKey>, new()
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
        where TUserToken : IdentityUserToken<TKey>
        where TUsersDto : UsersDto<TUserDto, TKey>
        where TRolesDto : RolesDto<TRoleDto, TKey>
        where TUserRolesDto : UserRolesDto<TRoleDto, TKey>
        where TUserClaimsDto : UserClaimsDto<TUserClaimDto, TKey>
        where TUserProviderDto : UserProviderDto<TKey>
        where TUserProvidersDto : UserProvidersDto<TUserProviderDto, TKey>
        where TUserChangePasswordDto : UserChangePasswordDto<TKey>
        where TRoleClaimsDto : RoleClaimsDto<TRoleClaimDto, TKey>
        where TUserClaimDto : UserClaimDto<TKey>
        where TRoleClaimDto : RoleClaimDto<TKey>
    {
        private readonly IIdentityService<TUserDto, TRoleDto, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
            TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
            TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto, TUserClaimDto, TRoleClaimDto> _identityService;
        private readonly IGenericControllerLocalizer<IdentityController<TUserDto, TRoleDto, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
            TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
            TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto, TUserClaimDto, TRoleClaimDto>> _localizer;
        private readonly AdminIdentityDbContext _adminIdentityDbContext;
        private readonly IRootConfiguration _configuration;

        public IdentityController(IIdentityService<TUserDto, TRoleDto, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
                TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
                TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto, TUserClaimDto, TRoleClaimDto> identityService,
            ILogger<ConfigurationController> logger,
            IGenericControllerLocalizer<IdentityController<TUserDto, TRoleDto, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
                TUsersDto, TRolesDto, TUserRolesDto, TUserClaimsDto,
                TUserProviderDto, TUserProvidersDto, TUserChangePasswordDto, TRoleClaimsDto, TUserClaimDto, TRoleClaimDto>> localizer,
            AdminIdentityDbContext adminIdentityDbContext, IRootConfiguration configuration) : base(logger)
        {
            _identityService = identityService;
            _localizer = localizer;
            _adminIdentityDbContext = adminIdentityDbContext;
            _configuration = configuration;
        }

        #region Organization
        [HttpGet]
        public async Task<IActionResult> Organizations(int? page, string search)
        {
            ViewBag.Search = search;
            page = page ?? 1;
            var organizations = await _adminIdentityDbContext.Organizations.PageBy(x => x.Id, page.Value, 10).ToListAsync();

            // TODO: Use automapper
            var result = new OrganizationsDto()
            {
                PageSize = 10,
                TotalCount = _adminIdentityDbContext.Organizations.Count(),
                Organizations = organizations.Select(o => new OrganizationDto(o.Id, o.Name)).ToList()
            };

            return View(result);
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Organization(int? id)
        {
            if (!id.HasValue)
            {
                return View(new OrganizationDto());
            }

            var organization = await _adminIdentityDbContext.Organizations.FirstOrDefaultAsync(o => o.Id == id);
            if (organization == null) return NotFound();

            var organizationTreatmentTypes = new List<OrganizationTreatmentTypeDto>()
            {
                new OrganizationTreatmentTypeDto()
                {
                    Name = "CPAP",
                    Value = "4512457895"
                }
            }; // TODO: get from DB
            
            var result = new OrganizationDto(organization.Id, organization.Name, await GetTreatmentTypesList(), organizationTreatmentTypes);

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Organization(OrganizationDto organization)
        {
            if (!ModelState.IsValid)
            {
                return View(organization);
            }

            int organizationId;
            Organization newOrganization = new Organization();

            if (!organization.Id.HasValue)
            {
                newOrganization = new Organization(organization.Name);
                await _adminIdentityDbContext.Organizations.AddAsync(newOrganization);
            }
            else
            {
                var existingOrganization = await _adminIdentityDbContext.Organizations.FirstOrDefaultAsync(o => o.Id == organization.Id);
                existingOrganization.UpdateName(organization.Name);
            }

            await _adminIdentityDbContext.SaveChangesAsync();

            organizationId = organization.Id.HasValue ? organization.Id.Value : newOrganization.Id;

            SuccessNotification($"Organization '{organization.Name}' created successfully", "Success" );

            return RedirectToAction(nameof(Organization), new { Id = organizationId });
        }

        [HttpGet]
        public async Task<IActionResult> AddOrganizationTreatmentType(int treatmentTypeId, int organizationId)
        {
            return RedirectToAction(nameof(Organization), new { Id = organizationId });
        }

        [HttpGet]
        public async Task<IActionResult> OrganizationDelete(int id)
        {
            var organization = await _adminIdentityDbContext.Organizations.FirstOrDefaultAsync(o => o.Id == id);
            if (organization == null) return NotFound();

            var result = new OrganizationDto(organization.Id, organization.Name);

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrganizationDelete(OrganizationDto organization)
        {
            var organizationToRemove = await _adminIdentityDbContext.Organizations.FirstOrDefaultAsync(o => o.Id == organization.Id);
            _adminIdentityDbContext.Organizations.Remove(organizationToRemove);
            await _adminIdentityDbContext.SaveChangesAsync();

            SuccessNotification("Organization is successfully deleted!", "Success");

            return RedirectToAction(nameof(Organizations));
        }
        #endregion

        #region UserInviteLink
        [HttpGet]
        public async Task<IActionResult> UserInviteLink()
        {
            var newUserInviteLink = new UserInviteLinkDto();
            newUserInviteLink.OrganizationList = await GetOrganizationList();
            newUserInviteLink.RoleList = await GetRoleList();

            return View(newUserInviteLink);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserInviteLink(UserInviteLinkDto userInvite)
        {
            var userInvitation = new UserInvitation(userInvite.RoleId, Int32.Parse(userInvite.OrganizationId));
            await _adminIdentityDbContext.UserInvitations.AddAsync(userInvitation);
            await _adminIdentityDbContext.SaveChangesAsync();

            var link = $"{_configuration.AdminConfiguration.IdentityServerBaseUrl}/Account/RegisterByInvitation?token={userInvitation.Id}";
            ViewBag.Link = link;

            SuccessNotification($"User invite link successfully created: {link}", "Success");

            userInvite.OrganizationList = await GetOrganizationList();
            userInvite.RoleList = await GetRoleList();

            return View(userInvite);
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> Roles(int? page, string search)
        {
            ViewBag.Search = search;
            var roles = await _identityService.GetRolesAsync(search, page ?? 1);

            return View(roles);
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Role(TKey id)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default))
            {
                return View(new TRoleDto());
            }

            var role = await _identityService.GetRoleAsync(id.ToString());

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Role(TRoleDto role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }

            TKey roleId;

            if (EqualityComparer<TKey>.Default.Equals(role.Id, default))
            {
                var roleData = await _identityService.CreateRoleAsync(role);
                roleId = roleData.roleId;
            }
            else
            {
                var roleData = await _identityService.UpdateRoleAsync(role);
                roleId = roleData.roleId;
            }

            SuccessNotification(string.Format(_localizer["SuccessCreateRole"], role.Name), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(Role), new { Id = roleId });
        }

        [HttpGet]
        public async Task<IActionResult> Users(int? page, string search)
        {
            ViewBag.Search = search;
            var usersDto = await _identityService.GetUsersAsync(search, page ?? 1);

            return View(usersDto);
        }

        [HttpGet]
        public async Task<IActionResult> RoleUsers(string id, int? page, string search)
        {
            ViewBag.Search = search;
            var roleUsers = await _identityService.GetRoleUsersAsync(id, search, page ?? 1);

            var roleDto = await _identityService.GetRoleAsync(id);
            ViewData["RoleName"] = roleDto.Name;

            return View(roleUsers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserProfile(TUserDto user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            TKey userId;

            if (EqualityComparer<TKey>.Default.Equals(user.Id, default))
            {
                var userData = await _identityService.CreateUserAsync(user);
                userId = userData.userId;
            }
            else
            {
                var userData = await _identityService.UpdateUserAsync(user);
                userId = userData.userId;
            }

            SuccessNotification(string.Format(_localizer["SuccessCreateUser"], user.UserName), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(UserProfile), new { Id = userId });
        }

        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            var newUser = new TUserDto();
            newUser.OrganizationList = await GetOrganizationList();


            return View("UserProfile", newUser);
        }

        [HttpGet]
        [Route("[controller]/UserProfile/{id}")]
        public async Task<IActionResult> UserProfile(TKey id)
        {
            var user = await _identityService.GetUserAsync(id.ToString());
            if (user == null) return NotFound();
            user.OrganizationList = await GetOrganizationList();

            return View("UserProfile", user);
        }

        [HttpGet]
        public async Task<IActionResult> UserRoles(TKey id, int? page)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default)) return NotFound();

            var userRoles = await _identityService.BuildUserRolesViewModel(id, page);

            return View(userRoles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserRoles(TUserRolesDto role)
        {
            await _identityService.CreateUserRoleAsync(role);
            SuccessNotification(_localizer["SuccessCreateUserRole"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(UserRoles), new { Id = role.UserId });
        }

        [HttpGet]
        public async Task<IActionResult> UserRolesDelete(TKey id, TKey roleId)
        {
            await _identityService.ExistsUserAsync(id.ToString());
            await _identityService.ExistsRoleAsync(roleId.ToString());

            var userDto = await _identityService.GetUserAsync(id.ToString());
            var roles = await _identityService.GetRolesAsync();

            var rolesDto = new UserRolesDto<TRoleDto, TKey>
            {
                UserId = id,
                RolesList = roles.Select(x => new SelectItemDto(x.Id.ToString(), x.Name)).ToList(),
                RoleId = roleId,
                UserName = userDto.UserName
            };

            return View(rolesDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserRolesDelete(TUserRolesDto role)
        {
            await _identityService.DeleteUserRoleAsync(role);
            SuccessNotification(_localizer["SuccessDeleteUserRole"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(UserRoles), new { Id = role.UserId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserClaims(TUserClaimsDto claim)
        {
            if (!ModelState.IsValid)
            {
                return View(claim);
            }

            await _identityService.CreateUserClaimsAsync(claim);
            SuccessNotification(string.Format(_localizer["SuccessCreateUserClaims"], claim.ClaimType, claim.ClaimValue), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(UserClaims), new { Id = claim.UserId });
        }

        [HttpGet]
        public async Task<IActionResult> UserClaims(TKey id, int? page)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default)) return NotFound();

            var claims = await _identityService.GetUserClaimsAsync(id.ToString(), page ?? 1);
            claims.UserId = id;

            return View(claims);
        }

        [HttpGet]
        public async Task<IActionResult> UserClaimsDelete(TKey id, int claimId)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default)
            || EqualityComparer<int>.Default.Equals(claimId, default)) return NotFound();

            var claim = await _identityService.GetUserClaimAsync(id.ToString(), claimId);
            if (claim == null) return NotFound();

            var userDto = await _identityService.GetUserAsync(id.ToString());
            claim.UserName = userDto.UserName;

            return View(claim);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserClaimsDelete(TUserClaimsDto claim)
        {
            await _identityService.DeleteUserClaimAsync(claim);
            SuccessNotification(_localizer["SuccessDeleteUserClaims"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(UserClaims), new { Id = claim.UserId });
        }

        [HttpGet]
        public async Task<IActionResult> UserProviders(TKey id)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default)) return NotFound();

            var providers = await _identityService.GetUserProvidersAsync(id.ToString());

            return View(providers);
        }

        [HttpGet]
        public async Task<IActionResult> UserProvidersDelete(TKey id, string providerKey)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default) || string.IsNullOrEmpty(providerKey)) return NotFound();

            var provider = await _identityService.GetUserProviderAsync(id.ToString(), providerKey);
            if (provider == null) return NotFound();

            return View(provider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserProvidersDelete(TUserProviderDto provider)
        {
            await _identityService.DeleteUserProvidersAsync(provider);
            SuccessNotification(_localizer["SuccessDeleteUserProviders"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(UserProviders), new { Id = provider.UserId });
        }

        [HttpGet]
        public async Task<IActionResult> UserChangePassword(TKey id)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default)) return NotFound();

            var user = await _identityService.GetUserAsync(id.ToString());
            var userDto = new UserChangePasswordDto<TKey> { UserId = id, UserName = user.UserName };

            return View(userDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserChangePassword(TUserChangePasswordDto userPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(userPassword);
            }

            var identityResult = await _identityService.UserChangePasswordAsync(userPassword);

            if (!identityResult.Errors.Any())
            {
                SuccessNotification(_localizer["SuccessUserChangePassword"], _localizer["SuccessTitle"]);

                return RedirectToAction("UserProfile", new { Id = userPassword.UserId });
            }

            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(userPassword);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RoleClaims(TRoleClaimsDto claim)
        {
            if (!ModelState.IsValid)
            {
                return View(claim);
            }

            await _identityService.CreateRoleClaimsAsync(claim);
            SuccessNotification(string.Format(_localizer["SuccessCreateRoleClaims"], claim.ClaimType, claim.ClaimValue), _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(RoleClaims), new { Id = claim.RoleId });
        }

        [HttpGet]
        public async Task<IActionResult> RoleClaims(TKey id, int? page)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default)) return NotFound();

            var claims = await _identityService.GetRoleClaimsAsync(id.ToString(), page ?? 1);
            claims.RoleId = id;

            return View(claims);
        }

        [HttpGet]
        public async Task<IActionResult> RoleClaimsDelete(TKey id, int claimId)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default) ||
                EqualityComparer<int>.Default.Equals(claimId, default)) return NotFound();

            var claim = await _identityService.GetRoleClaimAsync(id.ToString(), claimId);

            return View(claim);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RoleClaimsDelete(TRoleClaimsDto claim)
        {
            await _identityService.DeleteRoleClaimAsync(claim);
            SuccessNotification(_localizer["SuccessDeleteRoleClaims"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(RoleClaims), new { Id = claim.RoleId });
        }

        [HttpGet]
        public async Task<IActionResult> RoleDelete(TKey id)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default)) return NotFound();

            var roleDto = await _identityService.GetRoleAsync(id.ToString());
            if (roleDto == null) return NotFound();

            return View(roleDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RoleDelete(TRoleDto role)
        {
            await _identityService.DeleteRoleAsync(role);
            SuccessNotification(_localizer["SuccessDeleteRole"], _localizer["SuccessTitle"]);

            return RedirectToAction(nameof(Roles));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserDelete(TUserDto user)
        {
            var currentUserId = User.GetSubjectId();
            if (user.Id.ToString() == currentUserId)
            {
                CreateNotification(Helpers.NotificationHelpers.AlertType.Warning, _localizer["ErrorDeleteUser_CannotSelfDelete"]);
                return RedirectToAction(nameof(UserDelete), user.Id);
            }
            else
            {
                await _identityService.DeleteUserAsync(user.Id.ToString(), user);
                SuccessNotification(_localizer["SuccessDeleteUser"], _localizer["SuccessTitle"]);

                return RedirectToAction(nameof(Users));
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserDelete(TKey id)
        {
            if (EqualityComparer<TKey>.Default.Equals(id, default)) return NotFound();

            var user = await _identityService.GetUserAsync(id.ToString());
            if (user == null) return NotFound();

            return View(user);
        }

        private async Task<List<SelectItemDto>> GetOrganizationList()
        {
            return await _adminIdentityDbContext.Organizations.Select(o => new SelectItemDto(o.Id.ToString(), o.Name)).ToListAsync();
        }

        private async Task<List<SelectItemDto>> GetRoleList()
        {
            return await _adminIdentityDbContext.Roles.Select(o => new SelectItemDto(o.Id.ToString(), o.Name)).ToListAsync();
        }

        private async Task<List<SelectItemDto>> GetTreatmentTypesList()
        {
            return await _adminIdentityDbContext.TreatmentTypes.Select(o => new SelectItemDto(o.Id.ToString(), o.Name)).ToListAsync();
        }
    }
}