using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TP1_ASP.Models;

namespace TP1_ASP.Controllers;

[Authorize]
public class PagoController : Controller
{
    private readonly ILogger<PagoController> _logger;
    private IRepo <Pago> repo;
    private IRepo <Inmueble> repoInmuebles;
    private IRepo <Contrato> repoContratos;

    public PagoController(ILogger<PagoController> logger, IRepo <Pago> repo, IRepo <Inmueble> repoinmuebles, IRepo <Contrato> repocontratos) {
        _logger = logger;
        this.repo = repo;
        this.repoInmuebles = repoinmuebles;
        this.repoContratos = repocontratos;
    }

    public IActionResult Index () {
        var lista = repo.ObtenerTodos ();

        return View (lista);
    }

    public IActionResult Nuevo() {
        ViewBag.Inmuebles = repoInmuebles.ObtenerTodos ();
        ViewBag.Contratos = repoContratos.ObtenerTodos ();
        return View (repo.ObtenerTodos ());
    }

    public IActionResult Editar (int id) {
        return View (repo.BuscarPorID (id));
    }

    [HttpPost]
    public IActionResult Editar (int id, Pago pago) {
        if (repo.Editar (id, pago) != -1) {
            return RedirectToAction ("Index");
        }
        else {
            return Error ();
        }
    }

    public IActionResult Borrar (int id, Pago pago) {
        if (repo.Borrar (id, pago) != -1) {
            return RedirectToAction ("Index");
        }
        else {
            return Error ();
        }
    }

    [HttpPost]
    public IActionResult Nuevo (Pago pago) {
        if (repo.Nuevo (pago) != -1) {
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
