using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.Web.Infrastructure;

namespace Sample.Web
{
    public class Startup
    {
        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
           // Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration);
            services.AddControllersWithViews();
            InitService(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
           
        }


        public static void ResolveAllTypes(IServiceCollection services, ServiceLifetime serviceLifetime, Type refType, string suffix)
        {
            var assembly = refType.GetTypeInfo().Assembly;

            var allServices = assembly.GetTypes().Where(t =>
                t.GetTypeInfo().IsClass &&
                !t.GetTypeInfo().IsAbstract &&
                //!t.Name.StartsWith("I") &&
                t.Name.EndsWith(suffix)
            );


            foreach (var type in allServices)
            {
                var allInterfaces = type.GetInterfaces();
                var mainInterfaces = allInterfaces.Except
                    (allInterfaces.SelectMany(t => t.GetInterfaces()));
                foreach (var itype in mainInterfaces)
                {
                    if (allServices.Any(x => !x.Equals(type) && itype.IsAssignableFrom(x)))
                    {
                        throw new Exception("The " + itype.Name +
                                            " type has more than one implementations, please change your filter");
                    }
                    services.Add(new ServiceDescriptor(itype, type, serviceLifetime));
                }
            }
        }

        public static void InitService(IServiceCollection services)
        {
            services.AddTransient(typeof(Service.IService<>), typeof(Service.Service<>));
            ResolveAllTypes(services, ServiceLifetime.Transient, typeof(Service.IService<>), "Service");


         
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHttpClient, StandardHttpClient>();
            services.AddHttpContextAccessor();
            services.AddControllersWithViews();

        }
    }
}
