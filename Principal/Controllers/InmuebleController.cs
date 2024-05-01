using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TP1_ASP.Models;

namespace TP1_ASP.Controllers;

[Authorize]
public class InmuebleController : Controller
{
    private readonly ILogger<InmuebleController> _logger;
    private RepositorioInmueble repo;
    private IRepo <Propietario> repoPropietarios;

    public InmuebleController(ILogger<InmuebleController> logger, RepositorioInmueble repoInmuebles, IRepo <Propietario> repoPropietario) {
        _logger = logger;
        this.repo = repoInmuebles;
        this.repoPropietarios = repoPropietario;
    }

    public IActionResult Index () {
        var lista = repo.ObtenerTodos ();

        return View (lista);
    }

    public IActionResult Nuevo() {
        ViewBag.Propietarios = repoPropietarios.ObtenerTodos ();
        return View (repo.ObtenerTodos ());
    }

    public IActionResult Editar (int id) {
        ViewBag.Propietarios = repoPropietarios.ObtenerTodos ();
        return View (repo.BuscarPorID (id));
    }

    public IActionResult Detalles (int id) {
        ViewBag.Propietarios = repoPropietarios.ObtenerTodos ();
        Inmueble? resultado = repo.BuscarPorID (id);
        if (resultado == null) {
            return Error ();
        } else {
            return View (resultado);
        }
    }

    public IActionResult BuscarPorFecha () {
        return View ();
    }

    [HttpGet]
    public IActionResult Busqueda (DateTime FechaInicio, DateTime FechaFin) {
        List <Inmueble>? Lista = repo.BuscarPorFecha (FechaInicio, FechaFin);
        return View (Lista);
    }

    [HttpPost]
    public IActionResult Editar (int id, Inmueble inmueble) {
        if (repo.Editar (id, inmueble) != -1) {
            TempData ["Mensaje"] = "Inmueble editado con éxito";
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
    public IActionResult Borrar (int id, Inmueble inmueble) {
        if (repo.Borrar (id, inmueble) != -1) {
            TempData ["Mensaje"] = "Inmueble borrado.";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Index");
        }
        else {
            TempData ["Mensaje"] = "Un error ha ocurrido.";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Index");
        }
    }

    [HttpPost]
    public IActionResult Nuevo (Inmueble inmueble) {
        if (repo.Nuevo (inmueble) != -1) {
            TempData ["Mensaje"] = "Inmueble añadido con éxito.";
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
