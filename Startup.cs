/*using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using tpInmobliaria.Models;

namespace tpInmobliaria
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

            services.AddControllersWithViews();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.LoginPath = "/Usuario/Login";
                   options.LogoutPath = "/Usuario/Logout";
                   options.AccessDeniedPath = "/Home/Restringido";
               })
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = Configuration["TokenAuthentication:Issuer"],
                       ValidAudience = Configuration["TokenAuthentication:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
                           Configuration["TokenAuthentication:SecretKey"])),
                   };
                   options.Events = new JwtBearerEvents
                   {
                       OnMessageReceived = context =>
                       {
                           var accessToken = context.Request.Query["access_token"];
                           var path = context.HttpContext.Request.Path;
                           if (!string.IsNullOrEmpty(accessToken) && 
                           path.StartsWithSegments("/chatsegurohub"))
                           {
                               context.Token = accessToken;
                           }
                           return Task.CompletedTask;
                       }
                   };
               });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Empleado", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador", "Empleado"));
                options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
            });

            services.AddTransient<RepositorioUsuario>();
            services.AddTransient<RepositorioPropietario>();
            services.AddTransient<RepositorioInmueble>();
            services.AddTransient<RepositorioContrato>();
            services.AddTransient<RepositorioPago>();
            services.AddTransient<RepositorioInquilino>();

            services.AddMvc();
            services.AddSignalR();
            services.AddDbContext<DataContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 2))));

            services.AddHttpContextAccessor();
            
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
          //  app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
            });

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



//////////////////////////////////////////////////////////////////////////////////////////////
namespace tpInmobliaria
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>//el sitio web valida con cookie
                {
              	options.LoginPath = "/Usuario/Login";
		options.LogoutPath = "/Usuario/Logout";
		options.AccessDeniedPath = "/Home/Restringido";
               })
               .AddJwtBearer(options =>//la api web valida con token
               {
                   options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = Configuration["TokenAuthentication:Issuer"],
                       ValidAudience = Configuration["TokenAuthentication:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
                           Configuration["TokenAuthentication:SecretKey"])),
                   };
                   // opci�n extra para usar el token el hub
                   options.Events = new JwtBearerEvents
                   {
                       OnMessageReceived = context =>
                       {
                           // Read the token out of the query string
                           var accessToken = context.Request.Query["access_token"];
                           // If the request is for our hub...
                           var path = context.HttpContext.Request.Path;
                           if (!string.IsNullOrEmpty(accessToken) &&
                               path.StartsWithSegments("/chatsegurohub"))
                           {//reemplazar la url por la usada en la ruta ?
                               context.Token = accessToken;
                           }
                           return Task.CompletedTask;
                       }
                   };
               })
            

               ;

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Empleado", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador", "Empleado"));
                options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador", "SuperAdministrador"));
            });
           services.AddTransient<RepositorioUsuario>();
services.AddTransient<RepositorioPropietario>();
services.AddTransient<RepositorioInmueble>();
services.AddTransient<RepositorioContrato>();
services.AddTransient<RepositorioPago>();
services.AddTransient<RepositorioInquilino>();
            services.AddMvc();
            services.AddSignalR();
       services.AddDbContext<DataContext>(options =>
    options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 2))));

            services.AddHttpContextAccessor();
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
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
            });

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
*/

