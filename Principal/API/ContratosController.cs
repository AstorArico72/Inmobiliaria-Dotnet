using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Principal.Models;
using Principal.Controllers;
using System.Diagnostics.CodeAnalysis;
using MySql.Data.MySqlClient;

namespace Principal.API;

[ApiController]
[Route("Api/Contratos")]
public class ContratosController : ControllerBase {
    private readonly ContextoDb Contexto;

    public ContratosController (ContextoDb contexto) {
        this.Contexto = contexto;
    }

    [HttpGet("{id}")]
    public IActionResult ConseguirContrato (int id) {
        Contrato? contrato = Contexto.Contratos.Find (id);
        if (contrato == null) {
            return StatusCode (404);
        } else {
            return Ok (contrato);
        }
    }
    [HttpGet("Todos")]
    public IActionResult TodosLosContratos () {
        List <Contrato> Contratos = Contexto.Contratos.ToList ();
        return Ok (Contratos);
    }
    [HttpPost("Nuevo")]
    public async Task<IActionResult> NuevoContrato ([FromForm] Contrato contrato) {
        try {
            if (ModelState.IsValid) {
                await Contexto.Contratos.AddAsync (contrato);
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
    public async Task<IActionResult> BorrarContrato (int id) {
        try {
            Contrato? ContratoABorrar = await Contexto.Contratos.FindAsync (id);
            if (ContratoABorrar == null) {
                return BadRequest ("No hay ningún contrato con ése ID.");
            } else {
                Contexto.Contratos.Remove (ContratoABorrar);
                await Contexto.SaveChangesAsync ();
                return Ok ("Contrato borrado.");
            }
        } catch (MySqlException ex) {
            return StatusCode (500, ex);
        }
    }
    [HttpPut("Editar")]
    public async Task<IActionResult> EditarContrato ([FromForm] Contrato contrato) {
        int IdContrato = contrato.ID;
        Contrato? ContratoAEditar = await Contexto.Contratos.FindAsync(IdContrato);
        if (ContratoAEditar == null) {
            return BadRequest ("No existe un contrato con ése ID");
        } else {
            try {
                if (ModelState.IsValid) {
                    ContratoAEditar.FechaLímite = contrato.FechaLímite;
                    ContratoAEditar.FechaContrato = contrato.FechaContrato;
                    ContratoAEditar.Vigente = contrato.Vigente;
                    ContratoAEditar.Monto = contrato.Monto;
                    Contexto.Update (ContratoAEditar);
                    await Contexto.SaveChangesAsync ();
                    return Ok ("El contrato fué editado.");
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