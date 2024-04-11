using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TP1_ASP.Models;

namespace TP1_ASP.Controllers;

[Authorize]
public class InquilinoController : Controller
{
    private readonly ILogger<InquilinoController> _logger;
    private IRepo <Inquilino> repo;

    public InquilinoController(ILogger<InquilinoController> logger, IRepo <Inquilino> repo) {
        _logger = logger;
        this.repo = repo;
    }

    public IActionResult Index () {
        var lista = repo.ObtenerTodos ();

        return View (lista);
    }

    public IActionResult Nuevo() {
        return View ();
    }

    public IActionResult Editar (int id) {
        return View (repo.BuscarPorID (id));
    }

    public IActionResult Detalles (int id) {
        var RepoContratos = new RepositorioContrato ();
        ViewBag.Contratos = RepoContratos.ObtenerTodos ();
        var RepoInmuebles = new RepositorioInmueble ();
        ViewBag.Inmuebles = RepoInmuebles.ObtenerTodos ();
        return View (repo.BuscarPorID (id));
    }

    [HttpPost]
    public IActionResult Editar (int id, Inquilino inquilino) {
        if (repo.Editar (id, inquilino) != -1) {
            TempData ["Mensaje"] = "Inquilino añadido con éxito.";
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
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
