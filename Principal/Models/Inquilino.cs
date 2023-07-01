namespace TP1_ASP.Models;

public class Inquilino {
    public int ID {get; set;}
    public string? Nombre {get; set;}/*
    public Inmueble Vivienda {get; set;}
    public List <Contrato> Contratos;*/
    //Inplementar éso después.
    // hice la primera netrega - Alguien.

    public Inquilino (string nombre, int id) {
        this.Nombre = nombre;
        this.ID = id;
    }

    public Inquilino () {}
}