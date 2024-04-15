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
    private IRepo <Pago> repoPagos;

    public ContratoController(ILogger<ContratoController> logger, IRepo <Contrato> repo, IRepo <Inmueble> repoInmueble, IRepo <Propietario> repoPropietario, IRepo <Inquilino> repoInquilino, IRepo<Pago> repoPago) {
        _logger = logger;
        this.repo = repo;
        this.repoPropietarios = repoPropietario;
        this.repoInmuebles = repoInmueble;
        this.repoInquilinos = repoInquilino;
        this.repoPagos = repoPago;
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

    public IActionResult Detalles (int id) {
        ViewBag.Propietarios = repoPropietarios.ObtenerTodos ();
        ViewBag.Inmuebles = repoInmuebles.ObtenerTodos ();
        ViewBag.Inquilinos = repoInquilinos.ObtenerTodos ();
        ViewBag.Pagos = repoPagos.ObtenerTodos ();
        Contrato? resultado = repo.BuscarPorID (id);
        if (resultado == null) {
            return Error ();
        } else {
            return View (resultado);
        }
    }

    [HttpPost]
    public IActionResult Editar (int id, Contrato contrato) {
        int resultado = repo.Editar (id, contrato);
        if (resultado >= 0) {
            TempData ["Mensaje"] = "Contrato editado con éxito.";
            TempData ["ColorMensaje"] = "#00FF00";
            return RedirectToAction ("Index");
        } else if (resultado == -2) {
            TempData ["Mensaje"] = "Un contrato ocupa la fecha seleccionada.";
            TempData ["ColorMensaje"] = "#FFFF00";
            return RedirectToAction ("Editar");
        } else if (resultado == -3) {
            TempData ["Mensaje"] = "La fecha seleccionada es inválida. Debe ser al menos un día después de la fecha de inicio.";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Editar");
        } else {
            return RedirectToAction ("Editar");
        }
    }

    [Authorize (policy:"Admin")]
    public IActionResult Borrar (int id, Contrato contrato) {
        if (repo.Borrar (id, contrato) != -1) {
            TempData ["Mensaje"] = "Contrato borrado.";
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
    public IActionResult Nuevo (Contrato contrato) {
        int resultado = repo.Nuevo (contrato);
        if (resultado >= 0) {
            TempData ["Mensaje"] = "Nuevo contrato añadido.";
            TempData ["ColorMensaje"] = "#00FF00";
            return RedirectToAction ("Index");
        } else if (resultado == -2) {
            TempData ["Mensaje"] = "Un contrato ocupa la fecha seleccionada.";
            TempData ["ColorMensaje"] = "#FFFF00";
            return RedirectToAction ("Nuevo");
        } else if (resultado == -3) {
            TempData ["Mensaje"] = "La fecha seleccionada es inválida. Debe ser al menos un día después de la fecha de inicio.";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Nuevo");
        } else {
            return RedirectToAction ("Nuevo");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}