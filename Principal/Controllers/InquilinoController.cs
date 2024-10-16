using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Principal.Models;

namespace Principal.Controllers;

[Authorize]
public class InquilinoController : Controller
{
    private readonly ILogger<InquilinoController> _logger;
    private readonly ContextoDb Database;
    private IRepo <Inquilino> repo;
    [Obsolete("Deprecado en función de la migración a Entity Framework.")]
    private RepositorioContrato repoContratos;
    [Obsolete("Deprecado en función de la migración a Entity Framework.")]
    private RepositorioInmueble repoInmuebles;

    [Obsolete("Constructor con repositorios deprecado en función de la migración a Entity Framework.")]
    public InquilinoController(ILogger<InquilinoController> logger, IRepo <Inquilino> repo, RepositorioContrato repoContratos, RepositorioInmueble repoInmuebles) {
        _logger = logger;
        this.repo = repo;
        this.repoContratos = repoContratos;
        this.repoInmuebles = repoInmuebles;
    }
    public InquilinoController (ILogger<InquilinoController> logger, ContextoDb contextoDb) {
        this._logger = logger;
        this.Database = contextoDb;
    }

    [HttpGet]
    public IActionResult Index () {
        return View (Database.Inquilinos.ToList ());
    }

    [HttpGet]
    public IActionResult Nuevo() {
        return View ();
    }

    [HttpGet]
    public IActionResult Editar (int id) {
        return View (Database.Inmuebles.Find (id));
    }

    [HttpGet]
    public IActionResult Detalles (int id) {
        var InquilinoSeleccionado = repo.BuscarPorID (id);
        if (InquilinoSeleccionado == null) {
            return StatusCode (404);
        } else {
            var resultados = new ConjuntoResultados {
                Inquilino = InquilinoSeleccionado,
                Contratos = Database.Contratos.Where (item => item.Locatario == InquilinoSeleccionado.ID).ToList (),
                Inmuebles = Database.Inmuebles.ToList (),
            };
            return View (resultados);
        }
    }

    [HttpPost]
    [Obsolete ("Deprecado en función de la migración a Entity Framework. Usa el API en su lugar.")]
    public IActionResult Editar (int id, Inquilino inquilino) {
        if (repo.Editar (id, inquilino) != -1) {
            TempData ["Mensaje"] = "Inquilino editado con éxito.";
            TempData ["ColorMensaje"] = "#00FF00";
            return RedirectToAction ("Index");
        }
        else {
            TempData ["Mensaje"] = "Un error ha ocurrido. Algún campo es inválido.";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Editar");
        }
    }

    [HttpPost]
    [Obsolete ("Deprecado en función de la migración a Entity Framework. Usa el API en su lugar.")]
    public IActionResult Nuevo (Inquilino inquilino) {
        var repo = new RepositorioInquilino ();
        if (repo.Nuevo (inquilino) != -1) {
            TempData ["Mensaje"] = "Inquilino añadido con éxito.";
            TempData ["ColorMensaje"] = "#00FF00";
            return RedirectToAction ("Index");
        }
        else {
            TempData ["Mensaje"] = "Un error ha ocurrido. Algún campo es inválido.";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Nuevo");
        }
    }

    [Obsolete ("Deprecado en función de la migración a Entity Framework. Usa el API en su lugar.")]
    [Authorize (policy:"Admin")]
    [HttpGet]
    public IActionResult Borrar (int id, Inquilino inquilino) {
        if (repo.Borrar (id, inquilino) != -1) {
            TempData ["Mensaje"] = "Inquilino borrado";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Index");
        }
        else {
            TempData ["Mensaje"] = "Un error ha ocurrido.";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Index");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(string? excepcion)
    {
        ErrorViewModel ErrorVM =new ErrorViewModel (excepcion);
        ErrorVM.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        return View(ErrorVM);
    }
}
