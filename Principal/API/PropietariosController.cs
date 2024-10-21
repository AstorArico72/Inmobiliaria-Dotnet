using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Principal.Models;
using Principal.Controllers;
using System.Diagnostics.CodeAnalysis;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace Principal.API;

[ApiController]
[Route("Api/Propietarios")]
public class PropietariosController : ControllerBase {
    private readonly ContextoDb Contexto;
    private readonly IConfiguration Config;

    public PropietariosController (ContextoDb contexto, IConfiguration config) {
        this.Contexto = contexto;
        this.Config = config;
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
                propietario.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
			        password: propietario.Clave,
			        salt: System.Text.Encoding.ASCII.GetBytes(Config["Salt"]),
			        prf: KeyDerivationPrf.HMACSHA256,
			        iterationCount: 1000,
			        numBytesRequested: 256 / 8
                ));
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

    [AllowAnonymous]
    [HttpPost("Ingresar")]
    public async Task <IActionResult> Ingresar ([FromForm] LoginView LoginData) {
        string ContraseñaConHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
			password: LoginData.Clave,
			salt: System.Text.Encoding.ASCII.GetBytes(Config["Salt"]),
			prf: KeyDerivationPrf.HMACSHA256,
			iterationCount: 1000,
			numBytesRequested: 256 / 8
        ));
        Console.WriteLine ("Hash: " + ContraseñaConHash);
        var Llave = new SymmetricSecurityKey (System.Text.Encoding.ASCII.GetBytes(Config["TokenAuthentication:SecretKey"]));
        var Credenciales = new SigningCredentials (Llave, SecurityAlgorithms.HmacSha256);
        Propietario UsuarioSeleccionado = await Contexto.Propietarios.FirstOrDefaultAsync (item => item.Nombre == LoginData.NombreUsuario);

        if (UsuarioSeleccionado == null || ContraseñaConHash != UsuarioSeleccionado.Clave) {
            return BadRequest ("Usuario o clave incorrectos.");
        } else {
            var ClaimList = new List<Claim> {
                new Claim (ClaimTypes.Name, UsuarioSeleccionado.Nombre),
                new Claim (ClaimTypes.Role, UsuarioSeleccionado.Rol),
                new Claim ("IdUsuario", UsuarioSeleccionado.ID.ToString ())
            };
            
            var Token = new JwtSecurityToken (
                issuer: Config["TokenAuthentication:Issuer"],
                audience: Config["TokenAuthentication:Audience"],
                claims: ClaimList,
                expires: DateTime.Now.AddHours (24),
                signingCredentials: Credenciales
            );
            return Ok (new JwtSecurityTokenHandler ().WriteToken (Token));
        }
    }
}