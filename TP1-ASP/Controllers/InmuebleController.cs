using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TP1_ASP.Models;

namespace TP1_ASP.Controllers;

[Authorize]
public class InmuebleController : Controller
{
    private readonly ILogger<InmuebleController> _logger;
    private IRepo <Inmueble> repo;
    private IRepo <Propietario> repoPropietarios;

    public InmuebleController(ILogger<InmuebleController> logger, IRepo <Inmueble> repoInmuebles, IRepo <Propietario> repoPropietario) {
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

    [HttpPost]
    public IActionResult Editar (int id, Inmueble inmueble) {
        if (repo.Editar (id, inmueble) != -1) {
            return RedirectToAction ("Index");
        }
        else {
            return Error ();
        }
    }

    public IActionResult Borrar (int id, Inmueble inmueble) {
        if (repo.Borrar (id, inmueble) != -1) {
            return RedirectToAction ("Index");
        }
        else {
            return Error ();
        }
    }

    [HttpPost]
    public IActionResult Nuevo (Inmueble inmueble) {
        if (repo.Nuevo (inmueble) != -1) {
            return RedirectToAction ("Index");
        }
        else {
            return View ();
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
