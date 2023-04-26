using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TP1_ASP.Models;

namespace TP1_ASP.Controllers;

[Authorize]
public class ContratoController : Controller {
    private readonly ILogger<ContratoController> _logger;
    private IRepo <Contrato> repo;
    private IRepo <Propietario> repoPropietarios;
    private IRepo <Inmueble> repoInmuebles;
    private IRepo <Inquilino> repoInquilinos;

    public ContratoController(ILogger<ContratoController> logger, IRepo <Contrato> repo, IRepo <Inmueble> repoInmueble, IRepo <Propietario> repoPropietario, IRepo <Inquilino> repoInquilino) {
        _logger = logger;
        this.repo = repo;
        this.repoPropietarios = repoPropietario;
        this.repoInmuebles = repoInmueble;
        this.repoInquilinos = repoInquilino;
    }

    public IActionResult Index () {
        var lista = repo.ObtenerTodos ();
        ViewBag.Propietarios = repoPropietarios.ObtenerTodos ();
        ViewBag.Inmuebles = repoInmuebles.ObtenerTodos ();
        ViewBag.Inquilinos = repoInquilinos.ObtenerTodos ();
        return View (lista);
    }

    public IActionResult Nuevo() {
        ViewBag.Propietarios = repoPropietarios.ObtenerTodos ();
        ViewBag.Inmuebles = repoInmuebles.ObtenerTodos ();
        ViewBag.Inquilinos = repoInquilinos.ObtenerTodos ();
        return View (repo.ObtenerTodos ());
    }

    public IActionResult Editar (int id) {
        ViewBag.Propietarios = repoPropietarios.ObtenerTodos ();
        ViewBag.Inmuebles = repoInmuebles.ObtenerTodos ();
        ViewBag.Inquilinos = repoInquilinos.ObtenerTodos ();
        return View (repo.BuscarPorID (id));
    }

    [HttpPost]
    public IActionResult Editar (int id, Contrato contrato) {
        if (repo.Editar (id, contrato) != -1) {
            return RedirectToAction ("Index");
        }
        else {
            return Error ();
        }
    }

    public IActionResult Borrar (int id, Contrato contrato) {
        if (repo.Borrar (id, contrato) != -1) {
            return RedirectToAction ("Index");
        }
        else {
            return Error ();
        }
    }

    [HttpPost]
    public IActionResult Nuevo (Contrato contrato) {
        if (repo.Nuevo (contrato) != -1) {
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