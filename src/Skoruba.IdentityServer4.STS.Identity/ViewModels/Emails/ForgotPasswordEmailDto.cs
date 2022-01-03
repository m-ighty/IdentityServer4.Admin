using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skoruba.IdentityServer4.STS.Identity.ViewModels.Emails
{
    public class ForgotPasswordEmailDto
    {
        public string CallbackUrl { get; set; }

        public string UserName { get; set; }
    }
}
