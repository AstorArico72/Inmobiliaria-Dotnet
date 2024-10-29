using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Principal.Models;

namespace Principal.Controllers;

[Authorize]
public class PropietarioController : Controller
{
    private readonly ILogger<PropietarioController> _logger;
    private IRepo <Propietario> repo;
    private RepositorioInmueble repoInmuebles;
    private RepositorioContrato repoContratos;

    public PropietarioController(ILogger<PropietarioController> logger, IRepo <Propietario> repo, RepositorioContrato repoContrato, RepositorioInmueble repoInmueble) {
        _logger = logger;
        this.repo = repo;
        this.repoContratos = repoContrato;
        this.repoInmuebles = repoInmueble;
    }

    [HttpGet]
    public IActionResult Index () {
        var lista = repo.ObtenerTodos ();

        return View (lista);
    }

    [HttpGet]
    public IActionResult Nuevo() {
        return View ();
    }

    [HttpGet]
    public IActionResult Detalles (int id) {
        var PropietarioSeleccionado = repo.BuscarPorID (id);

        if (PropietarioSeleccionado == null) {
            return StatusCode (404);
        } else {
            var resultados = new ConjuntoResultados {
                //Ése predicado debajo debería devolver todos los inmuebles del propietario seleccionado, pero por alguna razón no lo hace.
                Inmuebles = repoInmuebles.ObtenerTodos ().FindAll (item => item.Propietario == PropietarioSeleccionado.ID),
                Propietario = repo.BuscarPorID (id)
            };
            //Añadí ésto como control, pero "resultados.Inmuebles.Count" es 0.
            Console.WriteLine ("Inmuebles encontrados: " + resultados.Inmuebles.Count);
            return View (resultados);
        }
    }

    [HttpGet]
    public IActionResult Editar (int id) {
        return View (repo.BuscarPorID (id));
    }

    [HttpPost]
    public IActionResult Nuevo (Propietario propietario) {
        if (repo.Nuevo (propietario) != -1) {
            TempData ["Mensaje"] = "Propietario añadido con éxito.";
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
    public IActionResult CambiarClave (int id) {
        var ClaimUsuario = User.Claims.FirstOrDefault (claim => claim.Type == "IdUsuario");
        if (ClaimUsuario != null) {
            return View (repo.BuscarPorID (id));
        } else {
            return RedirectToAction ("Login");
        }
    }
/*
    [HttpPost]
    [Authorize (policy:"Admin")]
    public IActionResult Editar (int id, Propietario usuario) {
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
    }*/
/*
    [HttpPost]
    [Authorize (policy:"Admin")]
    public IActionResult CambiarClave (int id, Propietario usuario) {
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

    public async Task <IActionResult> Logout () {
        await HttpContext.SignOutAsync (CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
    */

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
