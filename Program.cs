using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using tpInmobliaria.Models;

var builder = WebApplication.CreateBuilder(args);


// Configurar la URL
builder.WebHost.UseUrls("http://localhost:5000","http://localhost:5001", "http://*:5000","http://*:5001");
var configuration = builder.Configuration;
// Agregar servicios al contenedor
 builder.Services.AddControllersWithViews();
 builder.Services.AddRazorPages();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
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
                       ValidIssuer = configuration["TokenAuthentication:Issuer"],
                       ValidAudience = configuration["TokenAuthentication:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
                           configuration["TokenAuthentication:SecretKey"])),
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
                          /*    OnMessageReceived = context =>
             {
                // Leer el token desde el query string
                var accessToken = context.Request.Query["access_token"];
                // Si el request es para el Hub u otra ruta seleccionada...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/chatsegurohub") ||
                    path.StartsWithSegments("/api/Propietario/reset") ||
                    path.StartsWithSegments("/api/Propietario/token")))
                {//reemplazar las urls por las necesarias ruta ⬆
                    context.Token = accessToken;
                }*/
                           return Task.CompletedTask;
                       }
                   };
               });


            builder.Services.AddAuthorization(options =>
            {
               options.AddPolicy("Empleado", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador", "Empleado"));
                options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
            });
      builder.Services.AddMvc();
            builder.Services.AddSignalR();
            //builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();
           builder.Services.AddTransient<RepositorioUsuario>();
            builder.Services.AddTransient<RepositorioPropietario>();
           builder.Services.AddTransient<RepositorioInmueble>();
           builder.Services.AddTransient<RepositorioContrato>();
            builder.Services.AddTransient<RepositorioPago>();
          builder.Services.AddTransient<RepositorioInquilino>();


     
           builder.Services.AddDbContext<DataContext>(options =>
                options.UseMySql(configuration.GetConnectionString("DatabaseConnectionString"),
                new MySqlServerVersion(new Version(8, 0, 2))));


            builder.Services.AddHttpContextAccessor();
           
       


// Configurar la autenticación


// Configurar el inicio de la aplicación
var app = builder.Build();
//app.UseHttpsRedirection();
// Middleware de la aplicación
// Permitir CORS
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseStaticFiles();








// Habilitar el enrutamiento
app.UseRouting();
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
});
// Habilitar la autenticación y la autorización
app.UseAuthentication();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


// Configurar los endpoints
//app.MapControllerRoute(
 //   name: "default",
   // pattern: "{controller=Home}/{action=Index}/{id?}");
   app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute("login", "entrar/{**accion}", new { controller = "Usuarios", action = "Login" });
app.MapControllerRoute("rutaFija", "ruteo/{valor}", new { controller = "Home", action = "Ruta", valor = "defecto" });
app.MapControllerRoute("fechas", "{controller=Home}/{action=Fecha}/{anio}/{mes}/{dia}");
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
  /* app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });*/
app.MapRazorPages();


// Ejecutar la aplicación
app.Run();

/*
using Microsoft.AspNetCore;

using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace tpInmobliaria
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

*/