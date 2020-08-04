using Eluander.Domain.Identity.Extends;
using Eluander.Presentation.MVC.Extensions;
using Eluander.Presentation.MVC.Models;
using Eluander.Presentation.MVC.Repositories;
using Eluander.Presentation.MVC.Repositories.Interfaces;
using Eluander.Shared.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Eluander.Presentation.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            #region Cookies config.
            services.Configure<CookiePolicyOptions>(
               options => {
                   options.CheckConsentNeeded = context => true;
                   options.MinimumSameSitePolicy = SameSiteMode.None;
               });
            #endregion

            #region Referencies and repositories
            // NHibernate.
            services.AddDependencyInjection();

            // System.
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();
            #endregion

            #region Identity Config.
            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;

                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
                opt.SignIn.RequireConfirmedAccount = false;
            })
                .AddDefaultTokenProviders()
                .AddHibernateStores();

            services.AddAuthentication()
                .AddGoogle("google", opt =>
                {
                    var googleAuth = Configuration.GetSection("Authentication:Google");

                    opt.ClientId = googleAuth["ClientId"];
                    opt.ClientSecret = googleAuth["ClientSecret"];
                    opt.SignInScheme = IdentityConstants.ExternalScheme;
                });
            #endregion

            #region Routing config.
            services.AddRouting(opts => {
                opts.LowercaseUrls = true;
            });
            #endregion

            #region API Config.
            services.Configure<ApiBehaviorOptions>(options => {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
            });
            #endregion

            #region TempData Config.
            // detalhes em: https://docs.microsoft.com/pt-br/aspnet/core/fundamentals/app-state?view=aspnetcore-3.1

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".AdventureWorks.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.IsEssential = true;
            });
            #endregion

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Routing config.
            app.UseRouting();

            // Identity Config.
            app.UseAuthentication();
            app.UseAuthorization();

            // TempData Config.
            app.UseSession();

            // Using Route Default.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
