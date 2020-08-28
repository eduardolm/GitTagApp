using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GitTagApp
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
            services.AddRazorPages();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = "Github";
                })
                .AddCookie()
                .AddOAuth("Github", options =>
                {
                    options.ClientId = Configuration["GithubOauth:ClientId"];
                    options.ClientSecret = Configuration["GithuOauth:ClientSecret"];
                    options.CallbackPath = new PathString("/signin-github");
                    options.AuthorizationEndpoint = Configuration["GithubOauth:AuthorizationEndpoint"];
                    options.TokenEndpoint = Configuration["GithubOauth:TokenEndpoint"];
                    options.UserInformationEndpoint = Configuration["GithubOauth:UserInformationEndpoint"];

                    options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                    options.ClaimActions.MapJsonKey("urn:github:login", "login");
                    options.ClaimActions.MapJsonKey("urn:github:url", "html_url");

                    options.Events = new OAuthEvents
                    {
                        OnCreatingTicket = async context =>
                        {
                            var request = new HttpRequestMessage(
                                HttpMethod.Get, context.Options.UserInformationEndpoint);
                            
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            
                            request.Headers.Authorization = new AuthenticationHeaderValue(
                                "Bearer", context.AccessToken);

                            var response = await context.Backchannel.SendAsync(
                                request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                            
                            response.EnsureSuccessStatusCode();

                            var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

                            context.RunClaimActions(json.RootElement);
                        }
                    };

                    options.SaveTokens = true;
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    "default",
                    "{controller}/{action=Index}/{id}");
            });
        }
    }
}
