using Principal.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens; //Pendiente: Abandonar Microsoft.IdentityModel - está deprecado.
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using System.Text.Json.Serialization;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc ();
builder.Services.AddTransient <RepositorioInmueble, RepositorioInmueble> ();
builder.Services.AddTransient <RepositorioContrato, RepositorioContrato> ();
builder.Services.AddTransient <IRepo <Propietario>, RepositorioPropietario> ();
builder.Services.AddTransient <IRepo <Inquilino>, RepositorioInquilino> ();
builder.Services.AddTransient <IRepo <Contrato>, RepositorioContrato> ();
builder.Services.AddTransient <IRepo <Pago>, RepositorioPago> ();
builder.Services.AddTransient <RepositorioUsuario, RepositorioUsuario> ();

builder.WebHost.UseUrls("http://127.0.0.1:8011", "http://192.168.1.150:8011");

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configuración para ignorar referencias cíclicas y evitar incluir $id/$ref
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;  // Opcional, para formato más legible
    });

builder.Services.AddDbContext<ContextoDb> (
    options => options.UseMySql (
            builder.Configuration ["ConnectionStrings:DefaultConnection"], new MariaDbServerVersion (new Version (10, 4, 21))
        )
    );

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
        //policy.RequireRole ("Admin");
        policy.RequireClaim (ClaimTypes.Role, "Admin");
    });
    options.AddPolicy ("Usuario", policy => {
        policy.RequireRole ("Usuario");
        policy.RequireClaim (ClaimTypes.Role, "Empleado");
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseExceptionHandler("/Home/Error");
app.UseStatusCodePagesWithReExecute ("/Home/Error", "?StatusCode={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization ();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();