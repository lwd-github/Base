using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UserApi.IdentityServer;

namespace UserApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserApi", Version = "v1" });
            });

            //���IdentityServer4
            services.AddIdentityServer()
                //1������ǩ��֤��
                .AddDeveloperSigningCredential()

                  //identityserver4.entityframework
                  //Microsoft.AspNetCore.Identity.EntityFrameworkCore
                  //.AddConfigurationStore(options =>
                  //  {
                  //      options.ConfigureDbContext = builder =>
                  //      {
                  //          //builder.UseMySQL(Configuration.GetConnectionString("DefaultConnection"));
                  //      };
                  //  })

                  //.AddTestUsers(Config.Users().ToList())
                  .AddInMemoryIdentityResources(Config.GetIdentityResources()) //����ڴ�apiresource
                  .AddInMemoryApiResources(Config.GetApiResources())
                  .AddInMemoryApiScopes(Config.GetApiScopes())
                  .AddInMemoryClients(Config.GetClients()) //�������ļ���Client������Դ�ŵ��ڴ�
                  .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>(); // 2���Զ����û�У��

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserApi v1"));
            }

            app.UseHttpsRedirection();

            //ʹ��IdentityServer
            app.UseIdentityServer();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }



    public class Config
    {
        //����ids4��������install-package IdentityServer4  -version 2.1.1
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("AggregationApi", "��һ��api�ӿ�")
                {
                    //!!!��Ҫ
                    Scopes = { "scope1" }
                },
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("scope1"),
            };
        }

        public static IEnumerable<TestUser> Users()
        {
            return new[]
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "mail@qq.com",
                    Password = "123"
                }
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                 new Client
                {
                    ClientId = "client1",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowOfflineAccess = true,//���Ҫ��ȡrefresh_tokens ,�����AllowOfflineAccess����Ϊtrue��ͬʱ���StandardScopes.OfflineAccess ���Scopes
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,//���Ҫ��ȡrefresh_tokens ,������scopes�м���OfflineAccess
                        "scope1",
                    },

                    //�ڲ��������Ƿ��֣������� 60s �������ں�Token��Ȼ���á�������Ϊ��Դ����У�� Token ��ʱ�����һ��Ĭ�ϵ�ʱ��ƫ��������ƫ������Χ��Token�ᱻ��Ϊû����
                    //https://www.cnblogs.com/stulzq/p/8998274.html
                    AccessTokenLifetime = 60, //AccessToken��Чʱ�䣬��λ��
                    //AccessTokenType = AccessTokenType.Reference //reference token
                 }
            };
        }
    }

}
