using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Principal.Models;

namespace Principal.Controllers;

[Authorize]
public class ContratoController : Controller {
    private readonly ILogger<ContratoController> _logger;
    private RepositorioContrato repo;
    private IRepo <Propietario> repoPropietarios;
    private RepositorioInmueble repoInmuebles;
    private IRepo <Inquilino> repoInquilinos;
    private IRepo <Pago> repoPagos;

    public ContratoController(ILogger<ContratoController> logger, RepositorioContrato repo, RepositorioInmueble repoInmueble, IRepo <Propietario> repoPropietario, IRepo <Inquilino> repoInquilino, IRepo<Pago> repoPago) {
        _logger = logger;
        this.repo = repo;
        this.repoPropietarios = repoPropietario;
        this.repoInmuebles = repoInmueble;
        this.repoInquilinos = repoInquilino;
        this.repoPagos = repoPago;
    }

    [HttpGet]
    public IActionResult Index () {
        var resultados = new ConjuntoResultados {
            Contratos = repo.ObtenerTodos (),
            Inmuebles = repoInmuebles.ObtenerTodos ()
        };
        return View (resultados);
    }

    [HttpGet]
    public IActionResult BuscarPorFecha () {
        return View ();
    }

    [HttpGet]
    public IActionResult Busqueda (DateTime FechaInicio, DateTime FechaFin) {
        var resultados = new ConjuntoResultados {
            Contratos = repo.BuscarPorFecha (FechaInicio, FechaFin),
            Inmuebles = repoInmuebles.ObtenerTodos ()
        };
        return View (resultados);
    }

    [HttpGet]
    public IActionResult Nuevo() {
        var resultados = new ConjuntoResultados {
            Contratos = repo.ObtenerTodos (),
            Inmuebles = repoInmuebles.ObtenerTodos (),
            Inquilinos = repoInquilinos.ObtenerTodos ()
        };
        return View (resultados);
    }

    [HttpGet]
    public IActionResult Editar (int id) {
        var resultados = new ConjuntoResultados {
            Propietarios = repoPropietarios.ObtenerTodos (),
            Inmuebles = repoInmuebles.ObtenerTodos (),
            Inquilinos = repoInquilinos.ObtenerTodos (),
            Contrato = repo.BuscarPorID (id),
            Inmueble = repoInmuebles.BuscarPorID (id)
        };
        if (resultados.Contrato == null) {
            return StatusCode (404);
        } else {
            return View (resultados);
        }
    }

    [HttpGet]
    public IActionResult Detalles (int id) {
        var ContratoSeleccionado = repo.BuscarPorID (id);
        var InmuebleSeleccionado = repoInmuebles.ObtenerTodos ().Where (item => item.ID == ContratoSeleccionado.Propiedad).First ();
        
        var resultados = new ConjuntoResultados {
            Propietario = repoPropietarios.ObtenerTodos ().Where(item => item.ID == InmuebleSeleccionado.IDPropietario).First (),
            Inmueble = InmuebleSeleccionado,
            Inquilino = repoInquilinos.ObtenerTodos ().Where (item => item.ID == ContratoSeleccionado.Locatario).First (),
            Pagos = repoPagos.ObtenerTodos ().FindAll (item => item.IdContrato == id),
            Contrato = ContratoSeleccionado
        };
        if (resultados.Contrato == null) {
            return StatusCode (404);
        } else {
            return View (resultados);
        }
    }

    [HttpPost]
    public IActionResult Editar (int id, Contrato contrato) {
        int resultado = repo.Editar (id, contrato);
        switch (resultado) {
            case > 0:
                TempData ["Mensaje"] = "Contrato terminado. Multa a pagar: $" + resultado;
                TempData ["ColorMensaje"] = "#00FF00";
                return RedirectToAction ("Index");
            case 0:
                TempData ["Mensaje"] = "Contrato editado con éxito.";
                TempData ["ColorMensaje"] = "#00FF00";
                return RedirectToAction ("Index");
            case -2:
                TempData ["Mensaje"] = "Un contrato ocupa la fecha seleccionada.";
                TempData ["ColorMensaje"] = "#FFFF00";
                return RedirectToAction ("Editar");
            case -3:
                TempData ["Mensaje"] = "La fecha seleccionada es inválida. Debe ser al menos un día después de la fecha de inicio.";
                TempData ["ColorMensaje"] = "#FF0000";
                return RedirectToAction ("Editar");
            case -4:
                TempData ["Mensaje"] = "El inmueble seleccionado no está disponible.";
                TempData ["ColorMensaje"] = "#FF0000";
                return RedirectToAction ("Editar");
            case -5:
                TempData ["Mensaje"] = "El contrato no puede terminarse, aún no comenzó.";
                TempData ["ColorMensaje"] = "#FFFF00";
                return RedirectToAction ("Index");
            case -6:
                TempData ["Mensaje"] = "Hay pagos pendientes, no puede terminarse el contrato.";
                TempData ["ColorMensaje"] = "#FFFF00";
                return RedirectToAction ("Index");
            default:
                return StatusCode (500);
        }
    }

    [Authorize (policy:"Admin")]
    [HttpGet]
    public IActionResult Borrar (int id, Contrato contrato) {
        if (repo.Borrar (id, contrato) != -1) {
            TempData ["Mensaje"] = "Contrato borrado.";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Index");
        }
        else {
            return StatusCode (500);
        }
    }

    [HttpPost]
    public IActionResult Nuevo (Contrato contrato) {
        int resultado = repo.Nuevo (contrato);
        switch (resultado) {
            case >= 0:
                TempData ["Mensaje"] = "Contrato registrado con éxito.";
                TempData ["ColorMensaje"] = "#00FF00";
                return RedirectToAction ("Index");
            case -2:
                TempData ["Mensaje"] = "Un contrato ocupa la fecha seleccionada.";
                TempData ["ColorMensaje"] = "#FFFF00";
                return RedirectToAction ("Nuevo");
            case -3:
                TempData ["Mensaje"] = "La fecha seleccionada es inválida. Debe ser al menos un día después de la fecha de inicio.";
                TempData ["ColorMensaje"] = "#FF0000";
                return RedirectToAction ("Nuevo");
            case -4:
                TempData ["Mensaje"] = "El inmueble seleccionado no está disponible.";
                TempData ["ColorMensaje"] = "#FF0000";
                return RedirectToAction ("Nuevo");
            default:
                return StatusCode (500);
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