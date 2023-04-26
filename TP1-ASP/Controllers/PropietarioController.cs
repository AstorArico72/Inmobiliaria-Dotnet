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

    public PropietarioController(ILogger<PropietarioController> logger, IRepo <Propietario> repo) {
        _logger = logger;
        this.repo = repo;
    }

    public IActionResult Index () {
        var lista = repo.ObtenerTodos ();

        return View (lista);
    }

    public IActionResult Nuevo() {
        return View ();
    }

    [HttpPost]
    public IActionResult Editar (int id, Propietario propietario) {
        if (repo.Editar (id, propietario) != -1) {
            return RedirectToAction ("Index");
        }
        else {
            return Error ();
        }
    }

//Por ahora, queda así.
    public IActionResult Borrar (int id, Propietario propietario) {
        if (repo.Borrar (id, propietario) != -1) {
            return RedirectToAction ("Index");
        }
        else {
            return Error ();
        }
    }

    [HttpPost]
    public IActionResult Nuevo (Propietario propietario) {
        if (repo.Nuevo (propietario) != -1) {
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
