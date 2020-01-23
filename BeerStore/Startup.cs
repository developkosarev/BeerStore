using System.Globalization;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using BeerStore.DAL.EF;
using BeerStore.Models.Entities;

using BeerStore.Data;
using BeerStore.Services;
using BeerStore.Extensions;

namespace BeerStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public object AuthOptions { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
                
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<StoreContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.UseRowNumberForPaging()));            

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<StoreContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<CustomIdentityErrorDescriber>();

            services.AddCustomizedIdentity(Configuration);

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ITokenService, TokenService>();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddDataAnnotationsLocalization(options => { options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResource)); })
                .AddViewLocalization();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigin",
                        builder => builder.AllowAnyOrigin()
                                          .AllowAnyMethod()
                                          .AllowAnyHeader());
            });


            var provider = services.BuildServiceProvider();

            DbInitializer.UserManager = provider.GetService<UserManager<ApplicationUser>>();
            DbInitializer.RoleManager = provider.GetService<RoleManager<IdentityRole>>();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
                //app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();

                app.UseDeveloperExceptionPage();
                //app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                //{
                //    HotModuleReplacement = true
                //});
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            #region Localization

            var supportedCultures = new[]
            {                
                new CultureInfo("en"),
                new CultureInfo("ru")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {                
                DefaultRequestCulture = new RequestCulture("ru"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });            

            #endregion

            app.UseStaticFiles();

            app.UseCustomizedIdentity(); //app.UseAuthentication();

            app.UseMvc(routes =>
            {                
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                  name: "spa-fallback",
                  defaults: new { controller = "Home", action = "Index" });

                routes.MapRoute(
                    "api_default",
                    "api/{controller}/{action}/{id?}");
            });
        }
    }
}
