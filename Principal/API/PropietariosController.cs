using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Principal.Models;
using System.Diagnostics.CodeAnalysis;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MimeKit;
using MailKit.Net.Smtp;

namespace Principal.API;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    [AllowAnonymous]
    [HttpPost("Nuevo")]
    public async Task<IActionResult> NuevoPropietario ([FromForm] Propietario propietario) {
        try {
            if (ModelState.IsValid) {
                propietario.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
			        password: propietario.Clave,
			        salt: System.Text.Encoding.UTF8.GetBytes(Config["Salt"]),
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
			salt: System.Text.Encoding.UTF8.GetBytes(Config["Salt"]),
			prf: KeyDerivationPrf.HMACSHA256,
			iterationCount: 1000,
			numBytesRequested: 256 / 8
        ));
        var Llave = new SymmetricSecurityKey (System.Text.Encoding.UTF8.GetBytes(Config["TokenAuthentication:SecretKey"]));
        var Credenciales = new SigningCredentials (Llave, SecurityAlgorithms.HmacSha256);
        Propietario UsuarioSeleccionado = await Contexto.Propietarios.FirstOrDefaultAsync (item => item.Correo == LoginData.Correo);

        if (UsuarioSeleccionado == null || ContraseñaConHash != UsuarioSeleccionado.Clave) {
            return BadRequest ("Usuario o clave incorrectos.");
        } else {
            var ClaimList = new List<Claim> {
                new Claim (ClaimTypes.Name, UsuarioSeleccionado.Nombre),
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

    [HttpGet("Perfil")]
    public async Task <IActionResult> Perfil () {
        string usuario = User.Claims.FirstOrDefault (claim => claim.Type == "IdUsuario").Value;
        int IdUsuario = int.Parse (usuario);
        Propietario perfil = await Contexto.Propietarios.FindAsync (IdUsuario);
        if (perfil != null) {
            return Ok (perfil);
        } else {
            return Unauthorized ("No autenticado.");
        }
    }

    [HttpPut("EditarPerfil")]
    public async Task<IActionResult> EditarPerfil ([FromForm] string Nombre, [FromForm] string Contacto, [FromForm] string Dni) {
        string usuario = User.Claims.FirstOrDefault (claim => claim.Type == "IdUsuario").Value;
        int IdUsuario = int.Parse (usuario);
        Propietario PropietarioAEditar = await Contexto.Propietarios.FindAsync (IdUsuario);
        if (PropietarioAEditar == null) {
            return Unauthorized ("No autenticado.");            
        } else {
            try {
                if (ModelState.IsValid) {
                    PropietarioAEditar.Nombre = Nombre;
                    PropietarioAEditar.Contacto = Contacto;
                    PropietarioAEditar.DNI = Dni;
                    Contexto.Update (PropietarioAEditar);
                    await Contexto.SaveChangesAsync ();
                    return Ok (PropietarioAEditar);
                } else {
                    return BadRequest ("Un campo es inválido.");
                }
            } catch (MySqlException ex) {
                return StatusCode (500, ex);
            }
        }
    }

    [HttpGet("ReiniciarClave")]
    public async Task <IActionResult> ReiniciarClave () {
        string? usuario = User.Claims.FirstOrDefault (claim => claim.Type == "IdUsuario").Value;
        int IdUsuario = int.Parse (usuario);
        try {
            Propietario? UsuarioSeleccionado = Contexto.Propietarios.Find (IdUsuario);
            Console.WriteLine (UsuarioSeleccionado.Nombre);

            if (UsuarioSeleccionado == null) {
                return Unauthorized ("Función limitada a usuarios con cuenta.");
            }

            Random rand = new Random ();
            string NuevaClave = "";
            for (int i = 0; i<6; i++) {
                NuevaClave += rand.Next (0, 9);
            }
            Console.WriteLine ("Clave nueva: " + NuevaClave);
            //Ésto convierte la OTP a un hash.
            UsuarioSeleccionado.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
		    	password: NuevaClave,
		    	salt: System.Text.Encoding.UTF8.GetBytes(Config["Salt"]),
		    	prf: KeyDerivationPrf.HMACSHA256,
		    	iterationCount: 1000,
		    	numBytesRequested: 256 / 8
            ));

            var Mensaje = new MimeKit.MimeMessage ();
            Mensaje.To.Add (new MailboxAddress (UsuarioSeleccionado.Nombre, UsuarioSeleccionado.Correo));
            Mensaje.From.Add (new MailboxAddress ("Inmobiliaria Dotnet", Config["Correo:UsuarioSMTP"]));
            Mensaje.Subject = "Recuperación de cuenta (prueba)";
            TextPart HtmlMensaje = new TextPart ("html"){
                Text = @$"<h1>Saludos</h1> <p>{UsuarioSeleccionado.Nombre}, éste correo fue enviado porque has aceptado reiniciar tu clave. <br> <h3>Tu nueva clave es: {NuevaClave}</h3> </p>"
                };
            Mensaje.Body = HtmlMensaje;
            SmtpClient ClienteSMTP = new SmtpClient ();
            ClienteSMTP.Connect ("sandbox.smtp.mailtrap.io", 25, false);
            ClienteSMTP.Authenticate (Config["Correo:UsuarioSMTP"], Config["Correo:ClaveSMTP"]);
            await ClienteSMTP.SendAsync (Mensaje);

            //Ésto guarda el OTP convertido a hash, sobre-escribiendo la clave del usuario.
            Contexto.Propietarios.Update (UsuarioSeleccionado);
            await Contexto.SaveChangesAsync ();
            return Ok ("Revisa tu correo, allí recibirás tu nueva contraseña.");
        } catch (Exception ex) {
            return StatusCode (500, ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("ClaveOlvidada")]
    public async Task <IActionResult> ClaveOlvidada ([FromForm] string correo) {
        try {

        Propietario usuario = await Contexto.Propietarios.FirstOrDefaultAsync (item => item.Correo == correo);
        if (usuario == null) {
            return BadRequest ("La cuenta pedida no existe.");
        }
        var ClaimList = new List<Claim> {
                new Claim (ClaimTypes.Name, usuario.Nombre),
                new Claim ("IdUsuario", usuario.ID.ToString ())
            };
        var ClaimPropietario = new Claim ("IdUsuario", usuario.ID.ToString ());
        var Llave = new SymmetricSecurityKey (System.Text.Encoding.UTF8.GetBytes(Config["TokenAuthentication:SecretKey"]));
        var Credenciales = new SigningCredentials (Llave, SecurityAlgorithms.HmacSha256);
        var Token = new JwtSecurityToken (
            issuer: Config["TokenAuthentication:Issuer"],
            audience: Config["TokenAuthentication:Audience"],
            expires: DateTime.Now.AddMinutes (10),
            signingCredentials: Credenciales,
            claims: ClaimList
        );

        var TokenRecuperación = new JwtSecurityTokenHandler ().WriteToken (Token);

        //Ésto genera y envía un correo de recuperación, con la OTP.

        var Mensaje = new MimeKit.MimeMessage ();
        Mensaje.To.Add (new MailboxAddress (usuario.Nombre, usuario.Correo));
        Mensaje.From.Add (new MailboxAddress ("Inmobiliaria Dotnet", Config["Correo:UsuarioSMTP"]));
        Mensaje.Subject = "Recuperación de cuenta (prueba)";
        TextPart HtmlMensaje = new TextPart ("html"){
            Text = @$"<h1>Saludos</h1> <p>{usuario.Nombre}, éste correo fue enviado porque hubo una solicitud para recuperar tu cuenta. <br> <a href='http://192.168.1.150:8011/Api/Propietarios/ReiniciarClave?TokenRecuperación={TokenRecuperación}'><h2>Si tú hiciste el pedido, toca aquí.</h2></a> <h3>Ése enlace es válido por 10 minutos.</h3> </p>"
            };
        Mensaje.Body = HtmlMensaje;
        SmtpClient ClienteSMTP = new SmtpClient ();
        ClienteSMTP.Connect ("sandbox.smtp.mailtrap.io", 25, false);
        ClienteSMTP.Authenticate (Config["Correo:UsuarioSMTP"], Config["Correo:ClaveSMTP"]);
        await ClienteSMTP.SendAsync (Mensaje);
        return Ok ();
        } catch (Exception ex) {
            return StatusCode (500, ex);
        }
    }
}