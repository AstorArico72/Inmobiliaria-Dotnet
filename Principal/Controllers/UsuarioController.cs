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
    private readonly IWebHostEnvironment ENV;

    public UsuarioController(ILogger<UsuarioController> logger, RepositorioUsuario repo, IConfiguration setup, IWebHostEnvironment environment)
    {
        _logger = logger;
        this.repo = repo;
        this.Config = setup;
        this.ENV = environment;
    }

    public IActionResult Index () {
        var lista = repo.ObtenerTodos ();

        return View (lista);
    }

    public IActionResult Borrar (int id, Usuario usuario) {
        if (repo.Borrar (id, usuario) != -1) {
            TempData ["Mensaje"] = "Cuenta borrada.";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Index");
        }
        else {
            TempData ["Mensaje"] = "Un error ha ocurrido.";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Index");
        }
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
            TempData ["Mensaje"] = "Usuario o clave incorrectas";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Login");
        } else {
            string URLFoto;
            if (UsuarioSeleccionado.URLFoto == null) {
                    URLFoto = "/medios/Nulo.png";
                } else {
                    URLFoto = UsuarioSeleccionado.URLFoto;
                }
            var ClaimList = new List<Claim> {
                new Claim (ClaimTypes.Name, UsuarioSeleccionado.NombreUsuario),
                new Claim (ClaimTypes.Role, UsuarioSeleccionado.Rol),
                new Claim ("IdUsuario", UsuarioSeleccionado.ID.ToString ()),
                new Claim ("FotoUsuario", URLFoto)
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
            SubirFoto (usuario, ENV, repo);
            TempData ["Mensaje"] = "Cuenta creada con éxito. Ahora puedes iniciar sesión con ésa cuenta.";
            TempData ["ColorMensaje"] = "#00FF00";
            return RedirectToAction ("Index");
        }
        else {
            TempData ["Mensaje"] = "Un error ha ocurrido. Algún campo es inválido.";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Nuevo");
        }
    }

    [HttpGet]
    [Authorize]
    public IActionResult Editar (int id) {
        int? IdUsuario;
        var ClaimUsuario = User.Claims.FirstOrDefault (claim => claim.Type == "IdUsuario");
        if (ClaimUsuario != null) {
            IdUsuario = int.Parse (ClaimUsuario.Value);
            if (IdUsuario == id && IdUsuario != null) {
                return View (repo.BuscarPorID (id));
            } else {
                TempData ["Mensaje"] = "No está permitido editar perfiles de otros usuarios.";
                TempData ["ColorMensaje"] = "#FFFF00";
                return RedirectToAction ("Login");
            }
        } else {
            return RedirectToAction ("Login");
        }
    }

    [HttpPost]
    [Authorize]
    public IActionResult Editar (int id, Usuario usuario) {
        var ClaimUsuario = User.Claims.FirstOrDefault (claim => claim.Type == "IdUsuario");
        int? IdUsuario = int.Parse (ClaimUsuario.Value);
        if (repo.Editar (id, usuario) != -1 && IdUsuario == id && IdUsuario != null) {
            SubirFoto (usuario, ENV, repo);
            TempData ["Mensaje"] = "Cuenta editada con éxito.";
            TempData ["ColorMensaje"] = "#00FF00";
            return RedirectToAction ("Index", "Home");
        }
        else {
            TempData ["Mensaje"] = "Un error ha ocurrido. Algún campo es inválido.";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Login");
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

private void SubirFoto (Usuario usuario, IWebHostEnvironment ENV, IRepo <Usuario> repo) {
    if (usuario.Foto != null && usuario.ID != 0) {
            var NombreGuid = Guid.NewGuid ();
            string Camino = Path.Combine (ENV.WebRootPath, "medios");
            if (!Directory.Exists (Camino)) {
                Directory.CreateDirectory (Camino);
            }
            string NombreArchivo = "Foto_" + NombreGuid;
            string NombreArchivoCompleto = NombreArchivo + Path.GetExtension (usuario.Foto.FileName);
            usuario.URLFoto = "/medios/" + NombreArchivoCompleto;
            string ruta = Path.Combine (Camino, NombreArchivoCompleto);
            using (FileStream stream = new FileStream (ruta, FileMode.Create)) {
                usuario.Foto.CopyTo (stream);
            }
            repo.Editar (usuario.ID, usuario);
        }
    }
}