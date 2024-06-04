namespace TP1_ASP.Models;

public class Propietario {
    public int ID {get; set;}
    public string? Nombre {get; set;}
    public string? Contacto {get; set;}
    public string DNI {get; set;}

    public Propietario (string nombre, int id, string? contacto, string dni) {
        this.Nombre = nombre;
        this.ID = id;
        this.Contacto = contacto;
        this.DNI = dni;
    }

    public Propietario () {}
}