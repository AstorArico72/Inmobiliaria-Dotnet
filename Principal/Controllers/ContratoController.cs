using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Principal.Models;

namespace Principal.Controllers;

[Authorize]
public class ContratoController : Controller {
    private readonly ILogger<ContratoController> _logger;
    private readonly ContextoDb DataBase;
    [Obsolete("Deprecado en función de la migración a Entity Framework.")]
    private RepositorioContrato repo;
    [Obsolete("Deprecado en función de la migración a Entity Framework.")]
    private IRepo<Inmueble> repoInmuebles;

    public ContratoController (ILogger<ContratoController> logger, ContextoDb contexto) {
        _logger = logger;
        this.DataBase = contexto;
    }

    [Obsolete ("Constructor con repositorios deprecado en función de la migración a Entity Framework.")]
    public ContratoController(ILogger<ContratoController> logger, RepositorioContrato repo, IRepo<Inmueble> repoInmueble) {
        _logger = logger;
        this.repo = repo;
        this.repoInmuebles = repoInmueble;
    }

    [HttpGet]
    public IActionResult Index () {
        var resultados = new ConjuntoResultados {
            Contratos = DataBase.Contratos.ToList (),
            Inmuebles = DataBase.Inmuebles.ToList ()
        };
        return View (resultados);
    }

    [HttpGet]
    public IActionResult BuscarPorFecha () {
        return View ();
    }

    [HttpGet]
    [Obsolete ("Deprecado en función de la migración a Entity Framework.")]
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
            Contratos = DataBase.Contratos.ToList (),
            Inmuebles = DataBase.Inmuebles.ToList (),
            Inquilinos = DataBase.Inquilinos.ToList (),
            Propietarios = DataBase.Propietarios.ToList ()
        };
        return View (resultados);
    }

    [HttpGet]
    public IActionResult Editar (int id) {
        var resultados = new ConjuntoResultados {
            Propietarios = DataBase.Propietarios.ToList (),
            Inmuebles = DataBase.Inmuebles.ToList (),
            Inquilinos = DataBase.Inquilinos.ToList (),
            Contrato = DataBase.Contratos.Find (id),
            Inmueble = DataBase.Inmuebles.Find (id)
        };
        if (resultados.Contrato == null) {
            return StatusCode (404);
        } else {
            return View (resultados);
        }
    }

    [HttpGet]
    public IActionResult Detalles (int id) {
        Contrato? ContratoSeleccionado = DataBase.Contratos.Find (id);
        if (ContratoSeleccionado == null) {
            return StatusCode (404);
        } else {
            var InmuebleSeleccionado = DataBase.Inmuebles.Find (ContratoSeleccionado.ID);
            var resultados = new ConjuntoResultados {
                Propietario = DataBase.Propietarios.Find (InmuebleSeleccionado.Propietario),
                Inmueble = InmuebleSeleccionado,
                Inquilino = DataBase.Inquilinos.Find (ContratoSeleccionado.Locatario),
                Pagos = DataBase.Pagos.Where (item => item.IdContrato == id).ToList (),
                Contrato = ContratoSeleccionado
            };
            return View (resultados);
        }
    }

    [HttpPost]
    [Obsolete("Deprecado en función de la migración a Entity Framework. Usa el API en su lugar.")]
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

    [Obsolete("Deprecado en función de la migración a Entity Framework. Usa el API en su lugar.")]
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
    [Obsolete ("Deprecado en función de la migración a Entity Framework. Usa el API en su lugar.")]
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