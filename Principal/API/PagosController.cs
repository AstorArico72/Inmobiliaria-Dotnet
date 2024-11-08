using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Principal.Models;
using System.Diagnostics.CodeAnalysis;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Principal.API;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("Api/Pagos")]
public class PagosController : ControllerBase {
    private readonly ContextoDb Contexto;

    public PagosController (ContextoDb contexto) {
        this.Contexto = contexto;
    }

    [HttpGet("{id}")]
    public IActionResult ConseguirPago (int id) {
        Pago? pago = Contexto.Pagos.Find (id);
        if (pago == null) {
            return StatusCode (404);
        } else {
            return Ok (pago);
        }
    }
    [HttpGet("Todos")]
    public IActionResult TodosLosPagos () {
        List <Pago> Pagos = Contexto.Pagos.ToList ();
        return Ok (Pagos);
    }

    [HttpGet("PorContrato/{id}")]
    public IActionResult PagosPorContrato ([FromRoute] int id) {
        List <Pago> Pagos = Contexto.Pagos.Where (pago => pago.IdContrato == id).ToList ();
        return Ok (Pagos);
    }
    
    [HttpPost("Nuevo")]
    public async Task<IActionResult> NuevoPago ([FromForm] Pago pago) {
        try {
            if (ModelState.IsValid) {
                await Contexto.Pagos.AddAsync (pago);
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
    public async Task<IActionResult> BorrarPago (int id) {
        try {
            Pago? PagoABorrar = await Contexto.Pagos.FindAsync (id);
            if (PagoABorrar == null) {
                return BadRequest ("No hay ningún pago con ése ID.");
            } else {
                Contexto.Pagos.Remove (PagoABorrar);
                await Contexto.SaveChangesAsync ();
                return Ok ("Pago borrado.");
            }
        } catch (MySqlException ex) {
            return StatusCode (500, ex);
        }
    }
    [HttpPut("Editar")]
    public async Task<IActionResult> EditarPago ([FromForm] Pago pago) {
        int IdPago = pago.ID;
        Pago? PagoAEditar = await Contexto.Pagos.FindAsync(IdPago);
        if (PagoAEditar == null) {
            return BadRequest ("No existe un pago con ése ID");
        } else {
            try {
                if (ModelState.IsValid) {
                    PagoAEditar.NumeroPago = pago.NumeroPago;
                    PagoAEditar.Monto = pago.Monto;
                    PagoAEditar.FechaPago = pago.FechaPago;
                    PagoAEditar.Pagado = pago.Pagado;
                    Contexto.Update (PagoAEditar);
                    await Contexto.SaveChangesAsync ();
                    return Ok ("El pago fué editado.");
                } else {
                    return BadRequest ("Un campo es inválido");
                }
            } catch (MySqlException ex) {
                return StatusCode (500, ex);
            }
        }
    }
}