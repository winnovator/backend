using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WInnovator.Data;
using WInnovator.Helper;
using WInnovator.Interfaces;

namespace WInnovator
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly bool isProduction;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            isProduction = env.IsProduction();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // TODO: UPDATE IDENTITYUSER TO OUR OWN USE
            // See https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-3.0
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddHealthChecks();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<IUserIdentityHelper, UserIdentityHelper>();

            // Add authentication via Google and Twitter
            services.AddAuthentication(options =>
                {
                    //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            services.AddAuthorization();

            // We currently don't have a valid usecase for Google and/or Twitter authentication, so we're gonna disable it for this moment.

                //// Google authentication per https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-3.0
                //.AddGoogle(googleOptions =>
                //{
                //    IConfigurationSection googleAuthNSection =
                //        Configuration.GetSection("Authentication:Google");

                //    googleOptions.ClientId = googleAuthNSection["ClientId"];
                //    googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
                //})
                //// Twitter authentication per https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/twitter-logins?view=aspnetcore-3.0
                //.AddTwitter(twitterOptions =>
                //{
                //    IConfigurationSection twitterAuthNSection =
                //        Configuration.GetSection("Authentication:Twitter");

                //    twitterOptions.ConsumerKey = twitterAuthNSection["ConsumerKey"];
                //    twitterOptions.ConsumerSecret = twitterAuthNSection["ConsumerSecret"];

                //});

            services.AddRazorPages();
            services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            // per https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.0&tabs=visual-studio
            // We'll hide the service if it runs in production, but this must always be done to prevent build errors.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference 
                            { 
                                Type = ReferenceType.SecurityScheme, 
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
 
                    }
                });            
            });

            // See https://medium.com/it-dead-inside/implementing-health-checks-for-asp-net-core-a-deep-dive-85a327be9a75 for adding additional checks
            // https://github.com/xabaril/AspNetCore.Diagnostics.HealthChecks
            services.AddHealthChecks()
                .AddSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, IUserIdentityHelper userIdentityHelper)
        {
            if (!isProduction)
            {
                // If we're NOT in a production environment, use the Developer Exception page, Database Error page and swagger

                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    // we want to serve the Swagger UI at the app's root (http://localhost:<port>/), so the RoutePrefix property has to be set to an empty string
                    c.RoutePrefix = "swagger";
                });
                
                // In development we'll accept any request from all origins
                app.UseCors(builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                    );
            }
            else
            {
                app.UseExceptionHandler("/Error");
                
                // In production, we only accept request from the same origin
                app.UseCors(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                );
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapHealthChecks("/health");
            });

            CreateRoles(userIdentityHelper).Wait();
            createUsersIfNonexisting(userIdentityHelper).Wait();
            addRolesToDefaultUsers(serviceProvider, userIdentityHelper).Wait();
        }

        private async Task CreateRoles(IUserIdentityHelper userIdentityHelper)
        {
            foreach (string roleName in DefaultUsersAndRoles.getRoles())
            {
                await userIdentityHelper.CreateRoleIfNonExistent(roleName);
            }
        }

        private async Task createUsersIfNonexisting(IUserIdentityHelper userIdentityHelper)
        {
            foreach (UserData userData in DefaultUsersAndRoles.getDefaultUsers())
            {
                await userIdentityHelper.CreateConfirmedUserIfNonExistent(userData.email, userData.password);
            }
        }

        private async Task addRolesToDefaultUsers(IServiceProvider serviceProvider, IUserIdentityHelper userIdentityHelper)
        {
            var _logger = serviceProvider.GetRequiredService<ILogger<Startup>>();

            foreach (UserData userData in DefaultUsersAndRoles.getDefaultUsers())
            {
                // Does the user exist?
                if((await userIdentityHelper.SearchUser(userData.email)).Exists())
                {
                    foreach (string roleName in userData.defaultRoles)
                    {
                        await userIdentityHelper.AddRoleToUser(userData.email, roleName);
                    }
                } else
                {
                    // No, create error in log!
                    _logger.LogError($"Error, cannot add roles to user { userData.email }, user doesn't exist!");
                }
            }
        }
    }
}
