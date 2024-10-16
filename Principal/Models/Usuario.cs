using System.ComponentModel.DataAnnotations;

namespace Principal.Models;

public class Usuario {
    [Key]
    public int ID {get; set;}
    public string NombreUsuario {get; set;}
    public string Clave {get; set;}
    public string Rol {get; set;}
    public string? URLFoto {get; set;}
    public IFormFile? Foto {get; set;}

    public Usuario (int id, string nombre, string contraseña, string rol, IFormFile? foto, string? ruta) {
        this.ID = id;
        this.NombreUsuario = nombre;
        this.Clave = contraseña;
        this.Rol = rol;
        this.Foto = foto;
        this.URLFoto = ruta;
    }
    public Usuario () {

    }
}