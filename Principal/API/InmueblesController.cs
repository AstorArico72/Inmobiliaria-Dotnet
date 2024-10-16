using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Principal.Models;
using Principal.Controllers;
using System.Diagnostics.CodeAnalysis;
using MySql.Data.MySqlClient;

namespace Principal.API;

[ApiController]
[Route("Api/Inmuebles")]
public class InmueblesController : ControllerBase {
    private readonly ContextoDb Contexto;

    public InmueblesController (ContextoDb contexto) {
        this.Contexto = contexto;
    }

    [HttpGet("{id}")]
    public IActionResult ConseguirInmueble (int id) {
        Inmueble? inmueble = Contexto.Inmuebles.Find (id);
        if (inmueble == null) {
            return StatusCode (404);
        } else {
            return Ok (inmueble);
        }
    }
    [HttpGet("Todos")]
    public IActionResult TodosLosInmuebles () {
        List <Inmueble> inmuebles = Contexto.Inmuebles.ToList ();
        return Ok (inmuebles);
    }
    [HttpPost("Nuevo")]
    public async Task<IActionResult> NuevoInmueble ([FromForm] Inmueble inmueble) {
        try {
            if (ModelState.IsValid) {
                await Contexto.Inmuebles.AddAsync (inmueble);
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
    public async Task<IActionResult> BorrarInmueble (int id) {
        try {
            Inmueble? inmuebleABorrar = await Contexto.Inmuebles.FindAsync (id);
            if (inmuebleABorrar == null) {
                return BadRequest ("No hay ningún inmueble con ése ID.");
            } else {
                Contexto.Inmuebles.Remove (inmuebleABorrar);
                await Contexto.SaveChangesAsync ();
                return Ok ("inmueble borrado.");
            }
        } catch (MySqlException ex) {
            return StatusCode (500, ex);
        }
    }
    [HttpPut("Editar")]
    public async Task<IActionResult> EditarInmueble ([FromForm] Inmueble inmueble) {
        int IdInmueble = inmueble.ID;
        Inmueble? inmuebleAEditar = await Contexto.Inmuebles.FindAsync(IdInmueble);
        if (inmuebleAEditar == null) {
            return BadRequest ("No existe un inmueble con ése ID");
        } else {
            try {
                if (ModelState.IsValid) {
                    inmuebleAEditar.Dirección = inmueble.Dirección;
                    inmuebleAEditar.Superficie = inmueble.Superficie;
                    inmuebleAEditar.Precio = inmueble.Precio;
                    inmuebleAEditar.Tipo = inmueble.Tipo;
                    inmuebleAEditar.Uso = inmueble.Uso;
                    inmuebleAEditar.Ambientes = inmueble.Ambientes;
                    inmuebleAEditar.CoordenadasX = inmueble.CoordenadasX;
                    inmuebleAEditar.CoordenadasY = inmueble.CoordenadasY;
                    Contexto.Update (inmuebleAEditar);
                    await Contexto.SaveChangesAsync ();
                    return Ok ("El inmueble fué editado.");
                } else {
                    return BadRequest ("Un campo es inválido");
                }
            } catch (MySqlException ex) {
                return StatusCode (500, ex);
            }
        }
    }
}