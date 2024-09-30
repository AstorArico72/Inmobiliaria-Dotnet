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

    public PagoController(ILogger<PagoController> logger, IRepo <Pago> repo, RepositorioInmueble repoinmuebles, IRepo <Contrato> repocontratos) {
        _logger = logger;
        this.repo = repo;
        this.repoInmuebles = repoinmuebles;
        this.repoContratos = repocontratos;
    }

    [HttpGet]
    public IActionResult Index () {
        var lista = repo.ObtenerTodos ();

        return View (lista);
    }

    [HttpGet]
    public IActionResult Nuevo() {
        var resultados = new ConjuntoResultados {
            Inmuebles = repoInmuebles.ObtenerTodos (),
            Contratos = repoContratos.ObtenerTodos ()
        };
        return View (resultados);
    }

    [HttpGet]
    public IActionResult Editar (int id) {
        var resultado = repo.BuscarPorID (id);
        if (resultado == null) {
            return StatusCode (404);
        } else {
            return View (resultado);
        }
    }

    [HttpGet]
    public IActionResult Detalles (int id) {
        var resultado = repo.BuscarPorID (id);
        if (resultado == null) {
            return StatusCode (404);
        } else {
            return View (resultado);
        }
    }

    [HttpPost]
    public IActionResult Editar (int id, Pago pago) {
        if (repo.Editar (id, pago) != -1) {
            TempData ["Mensaje"] = "Pago editado con éxito.";
            TempData ["ColorMensaje"] = "#00FF00";
            return RedirectToAction ("Detalles", "Contrato", new {id= TempData["RedirigirAContrato"]});
        }
        else {
            TempData ["Mensaje"] = "Un error ha ocurrido. Algún campo es inválido.";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Editar");
        }
    }

    [Authorize (policy:"Admin")]
    [HttpGet]
    public IActionResult Borrar (int id, Pago pago) {
        if (repo.Borrar (id, pago) != -1) {
            TempData ["Mensaje"] = "Pago borrado.";
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
    public IActionResult Nuevo (Pago pago) {
        if (repo.Nuevo (pago) != -1) {
            TempData ["Mensaje"] = "Pago añadido con éxito";
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
    public IActionResult Error(string excepcion)
    {
        ErrorViewModel ErrorVM =new ErrorViewModel (excepcion);
        ErrorVM.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        return View(ErrorVM);
    }
}
