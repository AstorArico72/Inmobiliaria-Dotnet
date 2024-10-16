using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Principal.Models;
using Principal.Controllers;
using System.Diagnostics.CodeAnalysis;
using MySql.Data.MySqlClient;

namespace Principal.API;

[ApiController]
[Route("Api/Propietarios")]
public class PropietariosController : ControllerBase {
    private readonly ContextoDb Contexto;

    public PropietariosController (ContextoDb contexto) {
        this.Contexto = contexto;
    }

    [HttpGet("{id}")]
    public IActionResult ConseguirPropietario (int id) {
        Propietario? propietario = Contexto.Propietarios.Find (id);
        if (propietario == null) {
            return StatusCode (404);
        } else {
            return Ok (propietario);
        }
    }
    [HttpGet("Todos")]
    public IActionResult TodosLosPropietarios () {
        List <Propietario> propietarios = Contexto.Propietarios.ToList ();
        return Ok (propietarios);
    }
    [HttpPost("Nuevo")]
    public async Task<IActionResult> NuevoPropietario ([FromForm] Propietario propietario) {
        try {
            if (ModelState.IsValid) {
                await Contexto.Propietarios.AddAsync (propietario);
                await Contexto.SaveChangesAsync ();
                return Created ();
            } else {
                return BadRequest ("Un campo es inválido.");
            }
        } catch (MySqlException ex) {
            return StatusCode (500, ex);
        }
    }
    [HttpDelete("Borrar/{id}")]
    public async Task<IActionResult> BorrarPropietario (int id) {
        try {
            Propietario? PropietarioABorrar = await Contexto.Propietarios.FindAsync (id);
            if (PropietarioABorrar == null) {
                return BadRequest ("No hay ningún propietario con ése ID.");
            } else {
                Contexto.Propietarios.Remove (PropietarioABorrar);
                await Contexto.SaveChangesAsync ();
                return Ok ("Propietario borrado.");
            }
        } catch (MySqlException ex) {
            return StatusCode (500, ex);
        }
    }
    [HttpPut("Editar")]
    public async Task<IActionResult> EditarPropietario ([FromForm] Propietario propietario) {
        int IdPropietario = propietario.ID;
        Propietario? PropietarioAEditar = await Contexto.Propietarios.FindAsync(IdPropietario);
        if (PropietarioAEditar == null) {
            return BadRequest ("No existe un propietario con ése ID");
        } else {
            try {
                if (ModelState.IsValid) {
                    PropietarioAEditar.DNI = propietario.DNI;
                    PropietarioAEditar.Nombre = propietario.Nombre;
                    PropietarioAEditar.Contacto = propietario.Contacto;
                    Contexto.Update (PropietarioAEditar);
                    await Contexto.SaveChangesAsync ();
                    return Ok ("El propietario fué editado.");
                } else {
                    return BadRequest ("Un campo es inválido");
                }
            } catch (MySqlException ex) {
                return StatusCode (500, ex);
            }
        }
    }

    //Después implementar un método para cambiar la clave.
}