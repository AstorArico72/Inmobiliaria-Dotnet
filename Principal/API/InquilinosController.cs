using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Principal.Models;
using Principal.Controllers;
using System.Diagnostics.CodeAnalysis;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Principal.API;

[Authorize(JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("Api/Inquilinos")]
public class InquilinosController : ControllerBase {
    private readonly ContextoDb Contexto;

    public InquilinosController (ContextoDb contexto) {
        this.Contexto = contexto;
    }

    [HttpGet("{id}")]
    public IActionResult ConseguirInquilino (int id) {
        Inquilino? inquilino = Contexto.Inquilinos.Find (id);
        if (inquilino == null) {
            return StatusCode (404);
        } else {
            return Ok (inquilino);
        }
    }
    [HttpGet("Todos")]
    public IActionResult TodosLosInquilinos () {
        List <Inquilino> Inquilinos = Contexto.Inquilinos.ToList ();
        return Ok (Inquilinos);
    }
    [HttpPost("Nuevo")]
    public async Task<IActionResult> NuevoInquilino ([FromForm] Inquilino inquilino) {
        try {
            if (ModelState.IsValid) {
                await Contexto.Inquilinos.AddAsync (inquilino);
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
    public async Task<IActionResult> BorrarInquilino (int id) {
        try {
            Inquilino? InquilinoABorrar = await Contexto.Inquilinos.FindAsync (id);
            if (InquilinoABorrar == null) {
                return BadRequest ("No hay ningún inquilino con ése ID.");
            } else {
                Contexto.Inquilinos.Remove (InquilinoABorrar);
                await Contexto.SaveChangesAsync ();
                return Ok ("Inquilino borrado.");
            }
        } catch (MySqlException ex) {
            return StatusCode (500, ex);
        }
    }
    [HttpPut("Editar")]
    public async Task<IActionResult> EditarInquilino ([FromForm] Inquilino inquilino) {
        int IdInquilino = inquilino.ID;
        Inquilino? InquilinoAEditar = await Contexto.Inquilinos.FindAsync(IdInquilino);
        if (InquilinoAEditar == null) {
            return BadRequest ("No existe un inquilino con ése ID");
        } else {
            try {
                if (ModelState.IsValid) {
                    InquilinoAEditar.DNI = inquilino.DNI;
                    InquilinoAEditar.Nombre = inquilino.Nombre;
                    InquilinoAEditar.Contacto = inquilino.Contacto;
                    Contexto.Update (InquilinoAEditar);
                    await Contexto.SaveChangesAsync ();
                    return Ok ("El inquilino fué editado.");
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