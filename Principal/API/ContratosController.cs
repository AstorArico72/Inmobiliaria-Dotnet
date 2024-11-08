using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Principal.Models;
using Principal.Controllers;
using System.Diagnostics.CodeAnalysis;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Principal.API;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    public async Task <IActionResult> TodosLosContratos () {
        string usuario = User.Claims.FirstOrDefault (claim => claim.Type == "IdUsuario").Value;
        int IdUsuario = int.Parse (usuario);

        List <Inmueble> Inmuebles = Contexto.Inmuebles.Where (item => item.Propietario == IdUsuario).ToList ();
        Console.WriteLine ("Inmuebles del propietario " + "#" + IdUsuario + ":");
        Inmuebles.ForEach (item => Console.WriteLine (item.ID));
        List<ResultadoContrato> ContratosSeleccionados = new List<ResultadoContrato> ();

        List <Contrato> Contratos = Contexto.Contratos.ToList ();
        foreach (Contrato contrato in Contratos) {
            if (Inmuebles.Any (inmueble => inmueble.ID == contrato.Propiedad)) {
                Inquilino InquilinoSeleccionado = await Contexto.Inquilinos.FindAsync(contrato.Locatario);
                Inmueble InmuebleSeleccionado = await Contexto.Inmuebles.FindAsync (contrato.Propiedad);
                //List <Pago> PagosDelContrato = Contexto.Pagos.Where (pago => pago.IdContrato == contrato.ID).ToList ();
                ResultadoContrato NuevoItem = new ResultadoContrato ();
                NuevoItem.DireccionInmueble = InmuebleSeleccionado.Direccion;
                NuevoItem.IdContrato = contrato.ID;
                NuevoItem.IdInquilino = InquilinoSeleccionado.ID;
                //NuevoItem.ListaPagos = PagosDelContrato;
                NuevoItem.FechaContrato = contrato.FechaContrato;
                NuevoItem.FechaLímite = contrato.FechaLímite;
                NuevoItem.Vigente = contrato.Vigente;
                NuevoItem.Monto = contrato.Monto;
                NuevoItem.IdInmueble = InmuebleSeleccionado.ID;
                NuevoItem.NombreInquilino = InquilinoSeleccionado.Nombre;

                ContratosSeleccionados.Add (NuevoItem);
            }
        }
        return Ok (ContratosSeleccionados);
    }
    [HttpPost("Nuevo")]
    public async Task<IActionResult> NuevoContrato ([FromForm] Contrato contrato) {
        try {
            if (ModelState.IsValid) {
                contrato.Vigente = 1;
                await Contexto.Contratos.AddAsync (contrato);
                await Contexto.SaveChangesAsync ();
                GenerarPagos (contrato);
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
    private void GenerarPagos (Contrato co) {
        int Meses = ((co.FechaLímite.Year - co.FechaContrato.Year) *12) + co.FechaLímite.Month - co.FechaContrato.Month;
        for (int i = 1; i <= Meses; i++) {
            Contexto.Pagos.Add (new Pago {
                NumeroPago = i,
                IdContrato = co.ID,
                Monto = co.Monto,
                FechaPago = co.FechaContrato.AddMonths (i),
                Pagado = 0
            });
        }
        Contexto.SaveChangesAsync ();
    }
}