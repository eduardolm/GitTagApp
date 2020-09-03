using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using GitTagApp.Interfaces;
using GitTagApp.Repositories;
using GitTagApp.Repositories.Context;
using GitTagApp.Repositories.GenericRepository;
using GitTagApp.Services;
using GitTagApp.Services.GenericService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;

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
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                }
            };
            var client = new SecretClient(new Uri("https://gittagapp.vault.azure.net/"), new DefaultAzureCredential(),options);

            KeyVaultSecret clientId = client.GetSecret("ClientId");
            KeyVaultSecret clientSecret = client.GetSecret("ClientSecret");
            KeyVaultSecret connectionString = client.GetSecret("ConnectionString");
            KeyVaultSecret authorizationEndpoint = client.GetSecret("AuthorizationEndpoint");
            KeyVaultSecret tokenEndpoint = client.GetSecret("TokenEndpoint");
            KeyVaultSecret userInformationEndpoint = client.GetSecret("UserInformationEndpoint");

            services.AddDbContext<MainContext>(opt => opt
                .UseSqlServer(connectionString.Value));
    
            
            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IGitRepoRepository, GitRepoRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IGitRepoService, GitRepoService>();
            services.AddScoped<IGitRepoTagRepository, GitRepoTagRepository>();
            services.AddScoped<IGitRepoTagService, GitRepoTagService>();
            
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
                    options.ClientId = clientId.Value;
                    options.ClientSecret = clientSecret.Value;
                    options.CallbackPath = new PathString("/signin-github");
                    options.AuthorizationEndpoint = authorizationEndpoint.Value;
                    options.TokenEndpoint = tokenEndpoint.Value;
                    options.UserInformationEndpoint = userInformationEndpoint.Value;

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
