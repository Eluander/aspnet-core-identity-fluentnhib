using Eluander.Domain.Identity.Extends;
using Eluander.Presentation.MVC.Extensions;
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
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace Eluander.Presentation.MVC
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            #region Cookies config.
            services.Configure<CookiePolicyOptions>(opt =>
            {
                opt.CheckConsentNeeded = context => true;
                opt.MinimumSameSitePolicy = SameSiteMode.None;
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
                // password settings.
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;

                // lockout settings.
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.AllowedForNewUsers = true;

                // user settings.
                opt.User.RequireUniqueEmail = true;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZ-._@+";

                // signs settings.
                opt.SignIn.RequireConfirmedAccount = false;
                opt.SignIn.RequireConfirmedPhoneNumber = false;
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
                .AddCookie(opt =>
                {
                    opt.LoginPath = "/Account/Unnauthorized/";
                    opt.AccessDeniedPath = "/Account/Forbidden/";
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

            services.AddSwaggerGen(opt =>
            {
                var path = AppContext.BaseDirectory;
                var assemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                var file = System.IO.Path.GetFileName($"{assemblyName}.xml");

                opt.IncludeXmlComments(System.IO.Path.Combine(path, file));
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Eluander API",
                    Version = "v1",
                    Description = "O Swagger fornecerá uma documentação das suas API's.",
                    Contact = new OpenApiContact
                    {
                        Email = "eluander@gmail.com",
                        Name = "Eluander",
                        Url = new Uri("https://eluander.com.br")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://exemple.com/license")
                    }
                });
                opt.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "Cabeçalho de autorização JWT usando o esquema Bearer. Ex.: \"Autorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });
            });

            services.AddControllersWithViews();
            services.AddRazorPages();

            #region MVC Config.
            services.AddMvc()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.IgnoreNullValues = true;
                });
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
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

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.RoutePrefix = "doc";
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Eluander [API] - v1");
                opt.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });

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
