namespace TP1_ASP.Models;

public class Propietario {
    public int ID {get; set;}
    public string? Nombre {get; set;}
    //private List <Inmueble> ListaInmuebles;
    //Inplementar éso después.

    public Propietario (string nombre, int id) {
        this.Nombre = nombre;
        this.ID = id;
    }

    public Propietario () {}
}