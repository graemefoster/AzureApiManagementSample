using System;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel.AspNetCore.AccessTokenManagement;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TestOidcWebApp.EmployeeApi;

[assembly: AspMvcViewLocationFormat(@"~\Features\{1}\{0}.cshtml")]
[assembly: AspMvcViewLocationFormat(@"~\Features\{0}.cshtml")]
[assembly: AspMvcViewLocationFormat(@"~\Features\Shared\{0}.cshtml")]

[assembly: AspMvcAreaViewLocationFormat(@"~\Areas\{2}\{1}\{0}.cshtml")]
[assembly: AspMvcAreaViewLocationFormat(@"~\Areas\{2}\Features\{1}\{0}.cshtml")]
[assembly: AspMvcAreaViewLocationFormat(@"~\Areas\{2}\{0}.cshtml")]
[assembly: AspMvcAreaViewLocationFormat(@"~\Areas\{2}\Shared\{0}.cshtml")]


namespace TestOidcWebApp
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
            services.AddApplicationInsightsTelemetry();
            
            var authServerSettings = new IdentitySettings();
            Configuration.GetSection("AuthServer").Bind(authServerSettings);

            services.Configure<AppSettings>(Configuration.GetSection("SampleApp"));
            
            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.Configure<RazorViewEngineOptions>(x =>
            {
                x.ViewLocationExpanders.Add(new FeatureLocationExpander());
            });
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = authServerSettings.Url;
                    options.ClientId = authServerSettings.ClientId;
                    options.ClientSecret = authServerSettings.ClientSecret;
                    options.ResponseType = "code";
                    options.UsePkce = true;
                    options.Scope.Add("employee-apis");
                    options.Scope.Add("offline_access");

                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        NameClaimType = "name"
                    };
                    options.SaveTokens = true;
                });

            services.AddHttpClient<EmployeeApis>()
                .ConfigureHttpClient((sp, c) =>
                {
                    var appSettings = sp.GetRequiredService<IOptions<AppSettings>>().Value;
                    c.BaseAddress = new Uri(appSettings.ApiManagementUrl);
                    c.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", appSettings.ApiManagementSubscriptionKey);
                }).AddUserAccessTokenHandler();

            services.AddHttpContextAccessor();
            services.AddAccessTokenManagement();

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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
    
}
