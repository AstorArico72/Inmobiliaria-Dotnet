using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http;
using System.Security.Claims;
using TP1_ASP.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authentication;

namespace TP1_ASP.Controllers;

[Authorize (policy:"Admin")]
public class UsuarioController : Controller
{
    private readonly ILogger<UsuarioController> _logger;
    private RepositorioUsuario repo;
    private readonly IConfiguration Config;

    public UsuarioController(ILogger<UsuarioController> logger, RepositorioUsuario repo, IConfiguration setup)
    {
        _logger = logger;
        this.repo = repo;
        this.Config = setup;
    }

    public IActionResult Index () {
        var lista = repo.ObtenerTodos ();

        return View (lista);
    }

    public IActionResult Detalles (int id) {
        return View (repo.BuscarPorID (id));
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login (string ReturnUrl) {
        TempData ["ReturnUrl"] = ReturnUrl;
        return View ();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task <IActionResult> Ingresar (LoginView data) {
        string ContraseñaConHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
			password: data.Clave,
			salt: System.Text.Encoding.ASCII.GetBytes(Config["Salt"]),
			prf: KeyDerivationPrf.HMACSHA256,
			iterationCount: 1000,
			numBytesRequested: 256 / 8
        ));
        string? ReturnURL = String.IsNullOrEmpty(TempData ["ReturnUrl"] as string) ? "/Home" : TempData ["ReturnUrl"].ToString();

        Usuario UsuarioSeleccionado = repo.BuscarPorNombre (data.NombreUsuario);

        if (UsuarioSeleccionado == null || UsuarioSeleccionado.Clave != ContraseñaConHash) {
            ViewBag.MensajeError = ("Usuario o clave incorrectas");
            return RedirectToAction ("Login");
        } else {
            var ClaimList = new List<Claim> {
                new Claim (ClaimTypes.Name, UsuarioSeleccionado.NombreUsuario),
                new Claim (ClaimTypes.Role, UsuarioSeleccionado.Rol)
            };
            var IdentityClaim = new ClaimsIdentity (ClaimList, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(IdentityClaim));
            if (String.IsNullOrEmpty (ReturnURL) || !Url.IsLocalUrl (ReturnURL)) {
                return RedirectToAction("Index", "Home");
            } else {
                TempData.Remove("ReturnUrl");
                return Redirect (ReturnURL);
            }
        }
    }

    [HttpGet]
    public IActionResult Nuevo () {
        return View ();
    }

    [HttpPost]
    public IActionResult Nuevo (Usuario usuario) {
        if (repo.Nuevo (usuario) != -1) {
            return RedirectToAction ("Index");
        }
        else {
            return View ();
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    public async Task <IActionResult> Logout () {
        await HttpContext.SignOutAsync (CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
