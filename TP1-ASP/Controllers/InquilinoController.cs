using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TP1_ASP.Models;

namespace TP1_ASP.Controllers;

[Authorize]
public class InquilinoController : Controller
{
    private readonly ILogger<InquilinoController> _logger;
    private IRepo <Inquilino> repo;

    public InquilinoController(ILogger<InquilinoController> logger, IRepo <Inquilino> repo) {
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

    public IActionResult Editar (int id) {
        return View (repo.BuscarPorID (id));
    }

    [HttpPost]
    public IActionResult Editar (int id, Inquilino inquilino) {
        if (repo.Editar (id, inquilino) != -1) {
            return RedirectToAction ("Index");
        }
        else {
            return Error ();
        }
    }

    [HttpPost]
    public IActionResult Nuevo (Inquilino inquilino) {
        var repo = new RepositorioInquilino ();
        if (repo.Nuevo (inquilino) != -1) {
            return RedirectToAction ("Index");
        }
        else {
            return View ();
        }
    }

    public IActionResult Borrar (int id, Inquilino inquilino) {
        if (repo.Borrar (id, inquilino) != -1) {
            return RedirectToAction ("Index");
        }
        else {
            return Error ();
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
