using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Principal.Models;

namespace Principal.Controllers;

[Authorize]
public class InmuebleController : Controller
{
    private readonly ILogger<InmuebleController> _logger;
    private readonly ContextoDb Database;
    [Obsolete("Deprecado en función de la migración a Entity Framework.")]
    private RepositorioInmueble repo;
    [Obsolete("Deprecado en función de la migración a Entity Framework.")]
    private IRepo <Propietario> repoPropietarios;
    [Obsolete("Deprecado en función de la migración a Entity Framework.")]
    private IRepo <Contrato> repoContratos;
    [Obsolete("Deprecado en función de la migración a Entity Framework.")]
    private IRepo <Inquilino> repoInquilinos;

    [Obsolete("Constructor con repositorios deprecado en función de la migración a Entity Framework.")]
    public InmuebleController(ILogger<InmuebleController> logger, RepositorioInmueble repoInmuebles, IRepo <Propietario> repoPropietario, IRepo <Contrato> repoContrato, IRepo <Inquilino> repoInquilino) {
        _logger = logger;
        this.repo = repoInmuebles;
        this.repoPropietarios = repoPropietario;
        this.repoContratos = repoContrato;
        this.repoInquilinos = repoInquilino;
    }

    public InmuebleController (ILogger<InmuebleController> logger, ContextoDb contextoDb) {
        this.Database = contextoDb;
        this._logger = logger;
    }

    [HttpGet]
    public IActionResult Index () {
        return View (Database.Inmuebles.ToList ());
    }

    [HttpGet]
    public IActionResult Nuevo() {
        return View (Database.Propietarios.ToList ());
    }

    [HttpGet]
    public IActionResult Editar (int id) {
        var InmuebleSeleccionado = Database.Inmuebles.Find (id);
        if (InmuebleSeleccionado == null) {
            TempData ["Mensaje"] = "El inmueble seleccionado no existe.";
            TempData ["ColorMensaje"] = "#FF0000";
            return StatusCode (400);
        } else {
            var resultados = new ConjuntoResultados {
                Propietarios = Database.Propietarios.ToList (),
                Propietario = Database.Propietarios.Find (InmuebleSeleccionado.Propietario),
                Inmueble = InmuebleSeleccionado
            };
            return View (resultados);
        }
    }

    [HttpGet]
    public IActionResult Detalles (int id) {
        var InmuebleSeleccionado = Database.Inmuebles.Find (id);

        if (InmuebleSeleccionado == null) {
            return StatusCode (404);
        } else {
            var resultados = new ConjuntoResultados {
                Propietario = Database.Propietarios.Find (InmuebleSeleccionado.Propietario),
                Contratos = Database.Contratos.Where (item => item.Propiedad == InmuebleSeleccionado.ID).ToList (),
                Inquilinos = Database.Inquilinos.ToList (),
                Inmueble = InmuebleSeleccionado
            };
            return View (resultados);
        }
    }

    [HttpGet]
    public IActionResult BuscarPorFecha () {
        return View ();
    }

    [HttpGet]
    [Obsolete ("Deprecado en función de la migración a Entity Framework.")]
    public IActionResult Busqueda (DateTime FechaInicio, DateTime FechaFin) {
        List <Inmueble>? Lista = repo.BuscarPorFecha (FechaInicio, FechaFin);
        return View (Lista);
    }

    [HttpPost]
    [Obsolete("Deprecado en función de la migración a Entity Framework. Usa el API en su lugar.")]
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

    [Obsolete("Deprecado en función de la migración a Entity Framework. Usa el API en su lugar.")]
    [Authorize (policy:"Admin")]
    [HttpGet]
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
    [Obsolete("Deprecado en función de la migración a Entity Framework. Usa el API en su lugar.")]
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
    public IActionResult Error(string? excepcion)
    {
        ErrorViewModel ErrorVM =new ErrorViewModel (excepcion);
        ErrorVM.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        return View(ErrorVM);
    }
}
