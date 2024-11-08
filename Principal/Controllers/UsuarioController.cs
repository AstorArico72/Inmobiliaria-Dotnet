using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http;
using System.Security.Claims;
using Principal.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authentication;

namespace Principal.Controllers;

[Obsolete("Controlador deprecado en función de la unión de las tablas Usuarios y Propietarios.")]
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

    [HttpGet]
    public IActionResult Index () {
        var lista = repo.ObtenerTodos ();

        return View (lista);
    }

    [HttpGet]
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

    [HttpGet]
    public IActionResult Detalles (int id) {
        return View (repo.BuscarPorID (id));
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login (string ReturnUrl) {
        TempData ["ReturnUrl"] = ReturnUrl;
        return View ();
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
    public IActionResult Editar (int id) {
        var ClaimUsuario = User.Claims.FirstOrDefault (claim => claim.Type == "IdUsuario");
        if (ClaimUsuario != null) {
            return View (repo.BuscarPorID (id));
        } else {
            return RedirectToAction ("Login");
        }
    }

    [HttpGet]
    public IActionResult CambiarClave (int id) {
        var ClaimUsuario = User.Claims.FirstOrDefault (claim => claim.Type == "IdUsuario");
        if (ClaimUsuario != null) {
            return View (repo.BuscarPorID (id));
        } else {
            return RedirectToAction ("Login");
        }
    }

    [HttpPost]
    [Authorize (policy:"Admin")]
    public IActionResult Editar (int id, Usuario usuario) {
        var ClaimUsuario = User.Claims.FirstOrDefault (claim => claim.Type == "IdUsuario");
        int? IdUsuario = int.Parse (ClaimUsuario.Value);
        if (repo.Editar (id, usuario) != -1 && IdUsuario != null) {
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

    [HttpPost]
    [Authorize (policy:"Admin")]
    public IActionResult CambiarClave (int id, Usuario usuario) {
        var ClaimUsuario = User.Claims.FirstOrDefault (claim => claim.Type == "IdUsuario");
        int? IdUsuario = int.Parse (ClaimUsuario.Value);
        if (repo.CambiarClave (id, usuario) != -1 && IdUsuario != null) {
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

    [HttpGet]
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
    public IActionResult Error(string? excepcion)
    {
        ErrorViewModel ErrorVM =new ErrorViewModel (excepcion);
        ErrorVM.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        return View(ErrorVM);
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