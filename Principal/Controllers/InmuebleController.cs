using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Principal.Models;

namespace Principal.Controllers;

[Authorize]
public class InmuebleController : Controller
{
    private readonly ILogger<InmuebleController> _logger;
    private RepositorioInmueble repo;
    private IRepo <Propietario> repoPropietarios;
    private IRepo <Contrato> repoContratos;
    private IRepo <Inquilino> repoInquilinos;

    public InmuebleController(ILogger<InmuebleController> logger, RepositorioInmueble repoInmuebles, IRepo <Propietario> repoPropietario, IRepo <Contrato> repoContrato, IRepo <Inquilino> repoInquilino) {
        _logger = logger;
        this.repo = repoInmuebles;
        this.repoPropietarios = repoPropietario;
        this.repoContratos = repoContrato;
        this.repoInquilinos = repoInquilino;
    }

    [HttpGet]
    public IActionResult Index () {
        return View (repo.ObtenerTodos ());
    }

    [HttpGet]
    public IActionResult Nuevo() {
        return View (repoPropietarios.ObtenerTodos ());
    }

    [HttpGet]
    public IActionResult Editar (int id) {
        var InmuebleSeleccionado = repo.BuscarPorID (id);
        if (InmuebleSeleccionado == null) {
            TempData ["Mensaje"] = "El inmueble seleccionado no existe.";
            TempData ["ColorMensaje"] = "#FF0000";
            return StatusCode (400);
        } else {
            var resultados = new ConjuntoResultados {
                Propietarios = repoPropietarios.ObtenerTodos (),
                Propietario = repoPropietarios.BuscarPorID ((int)InmuebleSeleccionado.IDPropietario),
                Inmueble = InmuebleSeleccionado
            };
            return View (resultados);
        }
    }

    [HttpGet]
    public IActionResult Detalles (int id) {
        var InmuebleSeleccionado = repo.BuscarPorID (id);

        if (InmuebleSeleccionado == null) {
            return StatusCode (404);
        } else {
            var resultados = new ConjuntoResultados {
                Propietario = repoPropietarios.ObtenerTodos ().Where (item => item.ID == InmuebleSeleccionado.IDPropietario).First (),
                Contratos = repoContratos.ObtenerTodos ().Where (item => item.Propiedad == InmuebleSeleccionado.ID).ToList (),
                Inquilinos = repoInquilinos.ObtenerTodos (),
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
