using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TP1_ASP.Models;

namespace TP1_ASP.Controllers;

[Authorize]
public class PropietarioController : Controller
{
    private readonly ILogger<PropietarioController> _logger;
    private IRepo <Propietario> repo;

    public PropietarioController(ILogger<PropietarioController> logger, IRepo <Propietario> repo) {
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

    public IActionResult Detalles (int id) {
        var RepoInmuebles = new RepositorioInmueble ();
        var RepoContratos = new RepositorioContrato ();
        ViewBag.ListaContratos = RepoContratos.ObtenerTodos ();
        ViewBag.ListaInmuebles = RepoInmuebles.ObtenerTodos ();
        return View (repo.BuscarPorID (id));
    }

    [HttpPost]
    public IActionResult Editar (int id, Propietario propietario) {
        if (repo.Editar (id, propietario) != -1) {
            TempData ["Mensaje"] = "Propietario añadido con éxito.";
            TempData ["ColorMensaje"] = "#00FF00";
            return RedirectToAction ("Index");
        }
        else {
            TempData ["Mensaje"] = "Un error ha ocurrido. Algún campo es inválido.";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Editar");
        }
    }

    [Authorize (policy:"Admin")]
    public IActionResult Borrar (int id, Propietario propietario) {
        if (repo.Borrar (id, propietario) != -1) {
            TempData ["Mensaje"] = "Propietario borrado.";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Index");
        }
        else {
            TempData ["Mensaje"] = "Un error ha ocurrido. Algún campo es inválido.";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Index");
        }
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
