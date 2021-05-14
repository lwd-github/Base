using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.IdentityServer
{
    public static class IdentityServerExtension
    {
        public static void AddIdentity1(this IServiceCollection services)
        {
            services.AddIdentityServer().AddDeveloperSigningCredential();
        }
    }
}
