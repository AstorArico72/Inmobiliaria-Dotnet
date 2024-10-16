using System.ComponentModel.DataAnnotations;

namespace Principal.Models;

public class Propietario {
    [Key]
    public int ID {get; set;}
    public string? Nombre {get; set;}
    public string? Contacto {get; set;}
    public string Clave {get; set;}
    public string DNI {get; set;}

    public Propietario (string nombre, int id, string? contacto, string dni) {
        this.Nombre = nombre;
        this.ID = id;
        this.Contacto = contacto;
        this.DNI = dni;
    }

    public Propietario () {}
}