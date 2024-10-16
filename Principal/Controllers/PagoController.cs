using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Principal.Models;

namespace Principal.Controllers;

[Authorize]
public class PagoController : Controller
{
    private readonly ILogger<PagoController> _logger;
    private readonly ContextoDb Database;
    [Obsolete("Deprecado en función de la migración a Entity Framework.")]
    private IRepo <Pago> repo;
    [Obsolete("Deprecado en función de la migración a Entity Framework.")]
    private IRepo <Inmueble> repoInmuebles;
    [Obsolete("Deprecado en función de la migración a Entity Framework.")]
    private IRepo <Contrato> repoContratos;

    [Obsolete("Constructor con repositorios deprecado en función de la migración a Entity Framework.")]
    public PagoController(ILogger<PagoController> logger, IRepo <Pago> repo, RepositorioInmueble repoinmuebles, IRepo <Contrato> repocontratos) {
        _logger = logger;
        this.repo = repo;
        this.repoInmuebles = repoinmuebles;
        this.repoContratos = repocontratos;
    }

    public PagoController (ILogger <PagoController> logger, ContextoDb contextoDb) {
        this._logger = logger;
        this.Database = contextoDb;
    }

    [HttpGet]
    public IActionResult Index () {
        var lista = Database.Pagos.ToList ();

        return View (lista);
    }

    [HttpGet]
    public IActionResult Nuevo() {
        var resultados = new ConjuntoResultados {
            Inmuebles = Database.Inmuebles.ToList (),
            Contratos = Database.Contratos.ToList ()
        };
        return View (resultados);
    }

    [HttpGet]
    public IActionResult Editar (int id) {
        var resultado = Database.Inmuebles.Find (id);
        if (resultado == null) {
            return StatusCode (404);
        } else {
            return View (resultado);
        }
    }

    [HttpGet]
    public IActionResult Detalles (int id) {
        var resultado = Database.Inmuebles.Find (id);
        if (resultado == null) {
            return StatusCode (404);
        } else {
            return View (resultado);
        }
    }

    [HttpPost]
    [Obsolete ("Deprecado en función de la migración a Entity Framework. Usa el API en su lugar.")]
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

    [Obsolete ("Deprecado en función de la migración a Entity Framework. Usa el API en su lugar.")]
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
    [Obsolete ("Deprecado en función de la migración a Entity Framework. Usa el API en su lugar.")]
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
