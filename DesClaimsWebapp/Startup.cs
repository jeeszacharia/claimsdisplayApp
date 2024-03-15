// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Threading.Tasks;
using WebApp_OpenIDConnect_DotNet.Models;


namespace WebApp_OpenIDConnect_DotNet
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                // Handling SameSite cookie according to https://learn.microsoft.com/aspnet/core/security/samesite?view=aspnetcore-3.1
                options.HandleSameSiteCookieCompatibility();
               
            });

            // Configuration to sign-in users with Azure AD B2C

            //services.AddMicrosoftIdentityWebAppAuthentication(Configuration, "AzureAdB2C");
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                        .AddMicrosoftIdentityWebApp(options =>
                        {
                            Configuration.Bind("AzureAdB2C", options);

                            options.Events.OnRemoteFailure = context =>
                            {
                                var message=context.Failure;
                                context.HttpContext.Items["Message"] = message;
                                context.Response.Redirect("/Home/Error");
                                return Task.CompletedTask;
                            };
                        }).EnableTokenAcquisitionToCallDownstreamApi()
            .AddInMemoryTokenCaches();


            services.AddControllersWithViews()
                .AddMicrosoftIdentityUI();

                 
    

            services.AddRazorPages();

            //Configuring appsettings section AzureAdB2C, into IOptions
            services.AddOptions();
            services.Configure<OpenIdConnectOptions>(Configuration.GetSection("AzureAdB2C"));
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Add the Microsoft Identity Web cookie policy
            app.UseCookiePolicy();
            app.UseRouting();
            // Add the ASP.NET Core authentication service
            app.UseAuthentication();
            app.UseAuthorization();

 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                // Add endpoints for Razor pages
                endpoints.MapRazorPages();
            });
           
        }
    }
}