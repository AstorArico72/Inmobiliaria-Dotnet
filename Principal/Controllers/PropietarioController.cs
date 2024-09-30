using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TP1_ASP.Models;

namespace TP1_ASP.Controllers;

[Authorize]
public class PropietarioController : Controller
{
    private readonly ILogger<PropietarioController> _logger;
    private IRepo <Propietario> repo;
    private RepositorioInmueble repoInmuebles;
    private RepositorioContrato repoContratos;

    public PropietarioController(ILogger<PropietarioController> logger, IRepo <Propietario> repo, RepositorioContrato repoContrato, RepositorioInmueble repoInmueble) {
        _logger = logger;
        this.repo = repo;
        this.repoContratos = repoContrato;
        this.repoInmuebles = repoInmueble;
    }

    [HttpGet]
    public IActionResult Index () {
        var lista = repo.ObtenerTodos ();

        return View (lista);
    }

    [HttpGet]
    public IActionResult Nuevo() {
        return View ();
    }

    [HttpGet]
    public IActionResult Detalles (int id) {
        var PropietarioSeleccionado = repo.BuscarPorID (id);

        if (PropietarioSeleccionado == null) {
            return StatusCode (404);
        } else {
            var resultados = new ConjuntoResultados {
                //Ése predicado debajo debería devolver todos los inmuebles del propietario seleccionado, pero por alguna razón no lo hace.
                Inmuebles = repoInmuebles.ObtenerTodos ().FindAll (item => item.IDPropietario == PropietarioSeleccionado.ID),
                Propietario = repo.BuscarPorID (id)
            };
            //Añadí ésto como control, pero "resultados.Inmuebles.Count" es 0.
            Console.WriteLine ("Inmuebles encontrados: " + resultados.Inmuebles.Count);
            return View (resultados);
        }
    }

    [HttpGet]
    public IActionResult Editar (int id) {
        return View (repo.BuscarPorID (id));
    }

    [HttpPost]
    public IActionResult Editar (int id, Propietario propietario) {
        if (repo.Editar (id, propietario) != -1) {
            TempData ["Mensaje"] = "Propietario editado con éxito.";
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
    public IActionResult Borrar (int id, Propietario propietario) {
        if (repo.Borrar (id, propietario) != -1) {
            TempData ["Mensaje"] = "Propietario borrado.";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Index");
        }
        else {
            TempData ["Mensaje"] = "Un error ha ocurrido. Algún campo es inválido.";
            TempData ["ColorMensaje"] = "#FF0000";
            return RedirectToAction ("Index");
        }
    }

    [HttpPost]
    public IActionResult Nuevo (Propietario propietario) {
        if (repo.Nuevo (propietario) != -1) {
            TempData ["Mensaje"] = "Propietario añadido con éxito.";
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
