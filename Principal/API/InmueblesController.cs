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
[Route("Api/Inmuebles")]
public class InmueblesController : ControllerBase {
    private readonly ContextoDb Contexto;
    private readonly IWebHostEnvironment WebHostEnvironment;

    public InmueblesController (ContextoDb contexto, IWebHostEnvironment environment) {
        this.Contexto = contexto;
        this.WebHostEnvironment = environment;
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
    [Authorize]
    [HttpGet("Todos")]
    public IActionResult TodosLosInmuebles () {
        string usuario = User.Claims.FirstOrDefault (claim => claim.Type == "IdUsuario").Value;
        int IdUsuario = int.Parse (usuario);
        List <Inmueble> inmuebles = Contexto.Inmuebles.Where (item => item.Propietario == IdUsuario).ToList ();
        return Ok (inmuebles);
    }
    [HttpPost("Nuevo")]
    public async Task<IActionResult> NuevoInmueble ([FromForm] Inmueble inmueble) {
        string usuario = User.Claims.FirstOrDefault (claim => claim.Type == "IdUsuario").Value;
        int IdUsuario = int.Parse (usuario);
        try {
            inmueble.Propietario = IdUsuario;
            if (ModelState.IsValid) {
                await Contexto.Inmuebles.AddAsync (inmueble);
                await Contexto.SaveChangesAsync ();
                SubirFoto (inmueble);
                return Ok (inmueble);
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
                    inmuebleAEditar.Direccion = inmueble.Direccion;
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

    [HttpPatch("CambiarDisponibilidad")]
    public async Task <IActionResult> CambiarDisponibilidad ([FromForm] int IdInmueble) {
        string usuario = User.Claims.FirstOrDefault (claim => claim.Type == "IdUsuario").Value;
        int IdUsuario = int.Parse (usuario);
        Inmueble? InmuebleAParchear = await Contexto.Inmuebles.FirstAsync (item => item.ID == IdInmueble && item.Propietario == IdUsuario);
        string? MensajeExito = "";

        try {
            if (InmuebleAParchear != null) {
                Contexto.Entry (InmuebleAParchear).State = EntityState.Modified;
                if (InmuebleAParchear.Disponible == 0) {
                    InmuebleAParchear.Disponible = 1;
                    MensajeExito = "El inmueble ahora está disponible.";
                } else if (InmuebleAParchear.Disponible == 1) {
                    InmuebleAParchear.Disponible = 0;
                    MensajeExito = "El inmueble ahora no está disponible.";
                }
                Contexto.Update (InmuebleAParchear);
                await Contexto.SaveChangesAsync ();
            }
            return Ok (MensajeExito);
        } catch (Exception ex) {
            return StatusCode (500, ex);
        }
    }

    private void SubirFoto (Inmueble inmueble) {
        if (inmueble.Foto != null) {
            string Camino = Path.Combine (WebHostEnvironment.WebRootPath, "Inmuebles");
            if (!Directory.Exists (Camino)) {
                Directory.CreateDirectory (Camino);
            }
            string NombreArchivo = "Inmueble_" + inmueble.ID + Path.GetExtension (inmueble.Foto.FileName);
            inmueble.UrlFoto = "/Inmuebles/" + NombreArchivo;
            string RutaFoto = Path.Combine (Camino, NombreArchivo);
            using (FileStream stream = new FileStream (RutaFoto, FileMode.Create)) {
                inmueble.Foto.CopyTo (stream);
            }
            Contexto.Update (inmueble);
            Contexto.SaveChanges ();
        }
    }
}