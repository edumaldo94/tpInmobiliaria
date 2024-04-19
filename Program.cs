using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using tpInmobliaria.Models;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = "/Usuario/Login";
		options.LogoutPath = "/Usuario/Logout";
		options.AccessDeniedPath = "/Home/Restringido";
	});
    
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador"));
});
builder.Services.AddTransient<RepositorioUsuario>();
builder.Services.AddTransient<RepositorioPropietario>();
builder.Services.AddTransient<RepositorioInmueble>();
builder.Services.AddTransient<RepositorioContrato>();
builder.Services.AddTransient<RepositorioPago>();
builder.Services.AddTransient<RepositorioInquilino>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
