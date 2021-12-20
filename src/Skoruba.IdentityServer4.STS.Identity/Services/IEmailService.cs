using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Razor.Templating.Core;
using Skoruba.IdentityServer4.STS.Identity.ViewModels.Emails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skoruba.IdentityServer4.STS.Identity.Services
{
    public interface IEmailService
    {
        Task SendForgotPasswordEmailAsync<TUser, TKey>(TUser user, string callbackUrl) where TUser: IdentityUser<TKey> where TKey: IEquatable<TKey>;
    }

    public class EmailService : IEmailService
    {
        private readonly IEmailSender _sender;

        public EmailService(IEmailSender sender) => _sender = sender;

        public async Task SendForgotPasswordEmailAsync<TUser, TKey>(TUser user, string callbackUrl) where TUser : IdentityUser<TKey> where TKey : IEquatable<TKey>
        {
            await _sender.SendEmailAsync(user.Email, "Reset password", await RazorTemplateEngine.RenderAsync("~/Views/Emails/ForgotPasswordEmail.cshtml", new ForgotPasswordEmailDto() { CallbackUrl = callbackUrl, UserName = user.UserName }));
        }
    }
}
