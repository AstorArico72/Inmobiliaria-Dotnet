using System.ComponentModel.DataAnnotations;

namespace Principal.Models;

public class Propietario {
    [Key]
    public int ID {get; set;}
    public string Nombre {get; set;}
    public string? Contacto {get; set;}
    public string Clave {get; set;}
    public string DNI {get; set;}
    public string Rol {get; set;}

    public Propietario (string nombre, int id, string? contacto, string dni, string clave, string rol) {
        this.Nombre = nombre;
        this.ID = id;
        this.Contacto = contacto;
        this.DNI = dni;
        this.Clave = clave;
        this.Rol = rol;
    }

    [Obsolete("Éste constructor fué hecho antes de la unión de las tablas Usuario y Propietario.")]
    public Propietario (string nombre, int id, string? contacto, string dni) {
        this.Nombre = nombre;
        this.ID = id;
        this.Contacto = contacto;
        this.DNI = dni;
    }

    public Propietario () {}
}