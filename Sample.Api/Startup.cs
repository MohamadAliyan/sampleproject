using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Sample.Api.Infrastructure;
using Sample.Service;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sample.Api
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
            //            TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "CorsPolicy";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            DI.ConfigureServices(services, Configuration.GetConnectionString("DefaultConnection"));
            InitService(services);
            services.Configure<AppSettings>(Configuration);
            services.AddSingleton<IHttpClient, StandardHttpClient>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddCookie()
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateActor = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Issuer"],
                        ValidAudience = Configuration["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                            (Configuration["SigningKey"]))
                    };
                });

            //services.AddSingleton<IExceptionFilter, CustomExceptionFilterAttribute>();
            //services.AddMvc();

            services.AddMvc(config =>
            {
                //config.Filters.Add(new CustomExceptionFilterAttribute());
                //config.Filters.AddService(typeof(IExceptionFilter));

            });

            services.AddSignalR();
            services.AddMemoryCache();
            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseCors(corsPolicyBuilder =>
                corsPolicyBuilder

                    .WithOrigins()
               .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapControllerRoute("pagination", "api/{controller}/{action}/page/{pageNumber}");
                endpoints.MapControllerRoute("default", "api/{controller=Home}/{action=Index}/{id?}");
            });


        }

        public static void InitService(IServiceCollection services)
        {
          
       
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }

        public static void ResolveAllTypes(IServiceCollection services, ServiceLifetime serviceLifetime, Type refType, string suffix)
        {
            var assembly = refType.GetTypeInfo().Assembly;

            var allServices = assembly.GetTypes().Where(t =>
                t.GetTypeInfo().IsClass &&
                !t.GetTypeInfo().IsAbstract &&
                !t.Name.StartsWith("I") &&
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




    }
}
