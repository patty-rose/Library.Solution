using Microsoft.AspNetCore.Authorization;//for using authorization and roles
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;//for startup configuration and configure services methods
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models;

namespace Library
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
          services.AddControllersWithViews();

          //AddEntityFrameworkMySql().
          services.AddDbContext<LibraryContext>(options => options
          .UseMySql(Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(Configuration["ConnectionStrings:DefaultConnection"])));

          //below and chained lines set up ASP.NET core identity as a service and tells Identity we are using ApplicationUser as a model. .AddRoles<> adds role services to Identity. addEntityFrameworkStores method specifies that identity should use EF core adn the LibraryContext class. AddDefaultTokenProviders method adds the default token providers used to generate tokens for reset passwords, change email, etc. and two factor authentication token generation.
          services.AddIdentity<ApplicationUser, IdentityRole>()
          .AddRoles<IdentityRole>()
          .AddEntityFrameworkStores<LibraryContext>()
          .AddDefaultTokenProviders();
          
          // services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>,ApplicationUserClaimsPrincipalFactory>();

          // services.AddAuthorization(options =>
          // {
          //   options.FallbackPolicy = new AuthorizationPolicyBuilder()
          //     .RequireAuthenticatedUser()
          //     .Build();
          // });//Set the fallback authentication policy to require ALL users to be authenticated except for Razor Pages, controllers, or action methods with an authentication attribute.For example, Razor Pages, controllers, or action methods with [AllowAnonymous] or [Authorize(PolicyName="MyPolicy").

          services.Configure<IdentityOptions>(options =>
          {//development password settings -- now can use 1 letter passwords.
          options.Password.RequireDigit = false;
          options.Password.RequiredLength = 0;
          options.Password.RequireLowercase = false;
          options.Password.RequireNonAlphanumeric = false;
          options.Password.RequireUppercase = false;
          options.Password.RequiredUniqueChars = 0;
          });
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
            app.UseAuthentication();//on every http request, the user's 
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseStaticFiles(); //use static files like images and CSS

      app.Run(async (context) =>
      {
        await context.Response.WriteAsync("awaiting..");
      });
        }
    }
}
