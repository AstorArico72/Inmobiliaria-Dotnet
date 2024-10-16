using System.ComponentModel.DataAnnotations;

namespace Principal.Models;

public class Inquilino {
    [Key]
    public int ID {get; set;}
    public string? Nombre {get; set;}
    public string DNI {get; set;}
    public string Contacto {get; set;}

    public Inquilino (string nombre, int id, string dni, string contacto) {
        this.Nombre = nombre;
        this.ID = id;
        this.DNI = dni;
        this.Contacto = contacto;
    }

    public Inquilino () {}
}