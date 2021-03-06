﻿using EmployeeManagments.Models;
using EmployeeManagments.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using DNTCaptcha.Core;

namespace EmployeeManagments
{
    public class Startup

    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDNTCaptcha(options =>
                options.UseCookieStorageProvider()
                    .ShowThousandsSeparators(false)
            );


            services.AddAuthentication()
                .AddGoogle(options =>
            {
                options.ClientId = "XXXXX";
                options.ClientSecret = "YYYYY";
            })
            .AddFacebook(options =>
            {
                options.AppId = "XXXXX";
                options.AppSecret = "YYYYY";
            });



            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administraion/AccessDenied");
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("EditPolePolicy", policy =>
                    policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                //options.AddPolicy("EditPolePolicy", policy => policy.RequireAssertion(context =>
                //       context.User.IsInRole("Admin") &&
                //       context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
                //       context.User.IsInRole("Super Admin")));

                //options.AddPolicy("EditPolePolicy", policy =>
                //    policy
                //        .RequireRole("Admin")
                //        .RequireClaim("Edit Role", "true")
                //        .RequireRole("Super Admin"));

                options.AddPolicy("CreatePolePolicy", policy =>
                    policy.RequireClaim("Create Role", "true"));

                options.AddPolicy("DeletePolePolicy", policy =>
                    policy.RequireClaim("Delete Role", "true"));
            });

            services.AddDbContextPool<AppDbContext>(options =>
                options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //overwrite register model
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 3;
                options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

                //lockout account
                options.Lockout.MaxFailedAccessAttempts = 5;    //5 Times try
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

            }).AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<CustomEmailConfirmationTokenProvider
                    <ApplicationUser>>("CustomEmailConfirmation");

            // Changes token lifespan of all token types
            services.Configure<DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromHours(5));   //5 hours

            // Changes token lifespan of just the Email Confirmation Token type
            services.Configure<CustomEmailConfirmationTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromDays(3));  //3 days


            services.AddMvc(options =>
            {
                //to config Authorize
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();

            // Register the first handler
            services.AddSingleton<IAuthorizationHandler,
                CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            // Register the second handler
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();

            //for encrypt id param 
            services.AddSingleton<DataProtectionPurposeStrings>();

            //setup email address


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
