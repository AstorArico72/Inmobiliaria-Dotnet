using TP1_ASP.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient <RepositorioInmueble, RepositorioInmueble> ();
builder.Services.AddTransient <RepositorioContrato, RepositorioContrato> ();
builder.Services.AddTransient <IRepo <Propietario>, RepositorioPropietario> ();
builder.Services.AddTransient <IRepo <Inquilino>, RepositorioInquilino> ();
builder.Services.AddTransient <IRepo <Contrato>, RepositorioContrato> ();
builder.Services.AddTransient <IRepo <Pago>, RepositorioPago> ();
builder.Services.AddTransient <RepositorioUsuario, RepositorioUsuario> ();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication (CookieAuthenticationDefaults.AuthenticationScheme).AddCookie (options => {
    options.LoginPath = "/Usuario/Login";
    options.LogoutPath = "/Usuario/Logout";
    options.AccessDeniedPath = "/Home/AccesoDenegado";
    options.ExpireTimeSpan = TimeSpan.FromHours (1);
}).AddJwtBearer (options => {
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration ["TokenAuthentication:Issuer"],
        ValidAudience = builder.Configuration ["TokenAuthentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey (System.Text.Encoding.ASCII.GetBytes (builder.Configuration ["TokenAuthentication:SecretKey"]))
    };
});

builder.Services.AddAuthorization (options => {
    options.AddPolicy ("Admin", policy => {
        policy.RequireRole ("Admin");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseExceptionHandler("/Home/Error");
app.UseStatusCodePagesWithReExecute ("/Home/Error", "?StatusCode={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting ();
app.UseAuthentication ();
app.UseAuthorization ();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();