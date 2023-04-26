namespace TP1_ASP.Models;

public class Contrato {
    public int ID {get; set;}
    public int Locador {get; set;}
    public int Locatario {get; set;}
    public int Propiedad {get; set;}
    public DateTime FechaLímite {get; set;}
    public DateTime FechaContrato {get; set;}

    public Contrato (int id, int propietario, int inquilino, int inmueble, DateTime fechalimite, DateTime fechainicio) {
        this.ID = id;
        this.Locador = propietario;
        this.Locatario = inquilino;
        this.Propiedad = inmueble;
        this.FechaLímite = fechalimite;
        this.FechaContrato = fechainicio;
    }
    public Contrato () {

    }
}