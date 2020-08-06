using Eluander.Domain.Identity.Extends;
using Eluander.Presentation.MVC.Extensions;
using Eluander.Presentation.MVC.Models;
using Eluander.Presentation.MVC.Models.AppSettingsModels;
using Eluander.Presentation.MVC.Repositories;
using Eluander.Presentation.MVC.Repositories.Interfaces;
using Eluander.Shared.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

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
               options =>
               {
                   options.CheckConsentNeeded = context => true;
                   options.MinimumSameSitePolicy = SameSiteMode.None;
               });
            #endregion

            services.AddCors();

            #region Referencies and repositories
            // NHibernate.
            services.AddDependencyInjection();

            // token
            services.AddSingleton<ITokenService, TokenService>();

            // System.
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();
            #endregion

            #region Auth Config.
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

            // JWT Bearer Security.
            var jwtBearerSecret = Configuration.GetSection("Authentication:JwtBearer");
            var key = Encoding.ASCII.GetBytes(jwtBearerSecret["Secret"]);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = false;
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                })
                .AddGoogle("google", opt =>
                {
                    var googleAuth = Configuration.GetSection("Authentication:Google");

                    opt.ClientId = googleAuth["ClientId"];
                    opt.ClientSecret = googleAuth["ClientSecret"];
                    opt.SignInScheme = IdentityConstants.ExternalScheme;
                });
            #endregion

            #region Routing config.
            services.AddRouting(opts =>
            {
                opts.LowercaseUrls = true;
            });
            #endregion

            #region API Config.
            services.Configure<ApiBehaviorOptions>(options =>
            {
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

            app.UseCors(opt => opt
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

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
