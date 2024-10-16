using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Principal.Models;

namespace Principal.Controllers;

[Authorize]
public class InquilinoController : Controller
{
    private readonly ILogger<InquilinoController> _logger;
    private IRepo <Inquilino> repo;
    private RepositorioContrato repoContratos;
    private RepositorioInmueble repoInmuebles;

    public InquilinoController(ILogger<InquilinoController> logger, IRepo <Inquilino> repo, RepositorioContrato repoContratos, RepositorioInmueble repoInmuebles) {
        _logger = logger;
        this.repo = repo;
        this.repoContratos = repoContratos;
        this.repoInmuebles = repoInmuebles;
    }

    [HttpGet]
    public IActionResult Index () {
        return View (repo.ObtenerTodos ());
    }

    [HttpGet]
    public IActionResult Nuevo() {
        return View ();
    }

    [HttpGet]
    public IActionResult Editar (int id) {
        return View (repo.BuscarPorID (id));
    }

    [HttpGet]
    public IActionResult Detalles (int id) {
        var InquilinoSeleccionado = repo.BuscarPorID (id);
        if (InquilinoSeleccionado == null) {
            return StatusCode (404);
        } else {
            var resultados = new ConjuntoResultados {
                Inquilino = InquilinoSeleccionado,
                Contratos = repoContratos.ObtenerTodos ().Where (item => item.Locatario == InquilinoSeleccionado.ID).ToList (),
                Inmuebles = repoInmuebles.ObtenerTodos (),
            };
            return View (resultados);
        }
    }

    [HttpPost]
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
