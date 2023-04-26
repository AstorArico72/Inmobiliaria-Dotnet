using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace TP1_ASP.Models;

public class Usuario {
    public int ID {get; set;}
    public string NombreUsuario {get; set;}
    public string Clave {get; set;}
    public string Rol {get; set;}

    public Usuario (int id, string nombre, string contraseña, string rol) {
        this.ID = id;
        this.NombreUsuario = nombre;
        this.Clave = contraseña;
        this.Rol = rol;
    }
    public Usuario () {

    }
}