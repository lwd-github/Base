using IdentityModel;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UserApi.IdentityServer
{
    /// <summary>
    /// 自定义资源持有者验证
    /// </summary>
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var userName = context.UserName;
            var password = context.Password;

            context.Result = new GrantValidationResult(
                        subject: "1",
                        authenticationMethod: "mail@qq.com",
                        claims: GetUserClaims()
                        );
            await Task.CompletedTask;
        }

        // 用户身份声明
        public Claim[] GetUserClaims()
        {
            return new Claim[]
            {
                new Claim(JwtClaimTypes.Subject, "1"),
                new Claim(JwtClaimTypes.Id, "1"),
                new Claim(JwtClaimTypes.Name, "mail@qq.com")
            };
        }

    }
}
