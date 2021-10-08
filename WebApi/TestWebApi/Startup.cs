using DTO.Constant;
using Helper.Extensions.Swagger;
using Helper.Middleware;
using Framework.IOC;
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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Framework.Common.Currency;

namespace TestWebApi
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
            ////ע��Redis
            //services.AddSingleton(typeof(Cache.Redis.RedisCache), sp =>
            //{
            //    var content = new Cache.Redis.RedisCache(Config.ConfigAgent.Value<Cache.Redis.Config.RedisConfig>());
            //    return content;
            //});

            ServiceStack.Text.JsConfig<DateTime>.SerializeFn = time => time.ToString("yyyy/MM/dd HH");
            ServiceStack.Text.JsConfig<CNY>.SerializeFn = cny => cny.ToString();
            ServiceStack.Text.JsConfig<float>.SerializeFn = num => string.Format("{0:N2}", num);
            ServiceStack.Text.JsConfig<double>.SerializeFn = num => string.Format("{0:N2}", num);
            ServiceStack.Text.JsConfig<Double>.SerializeFn = num => string.Format("{0:N2}", num);
            ServiceStack.Text.JsConfig<decimal>.SerializeFn = num => string.Format("{0:N2}", num);

            services.AddControllers();

            //net5.0 �Դ�swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestWebApi", Version = "v1" });
                c.AddXmlComments("TestWebApi.xml"); //���xml�ĵ�ע��
            });

            services.AddMvc(o =>
            {
                //ȫ��ע���쳣������
                o.Filters.Add(typeof(ExceptionHandler));
                //o.ModelBinderProviders.Add(new ModelBinder.CustomModelBinderProvider());
                o.ModelBinderProviders.Insert(0, new ModelBinder.CustomModelBinderProvider());
            }).AddMvcOptions(t =>
            {
                t.OutputFormatters.Clear();
                t.OutputFormatters.Add(new JsonOutputFormater());
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
        }

        /*
        /// <summary>
        /// �����÷���������IOCע���������
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(Autofac.ContainerBuilder builder)
        {
            IocManager.Init(builder, MQConstant.IOCAssemblies.Split(';'));
            CommonService.Registration.CustomRegistration.Register(builder);//�Զ���ӿ�ע��
        }
        */


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestWebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseMiddleware<HttpRequestBodyEnableBuffering>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
