namespace TP1_ASP.Models;

public class Contrato {
    public int ID {get; set;}
    public int Locatario {get; set;}
    public int Propiedad {get; set;}
    public DateTime FechaLímite {get; set;}
    public DateTime FechaContrato {get; set;}
    public byte Vigente {get; set;}

    public Contrato (int id, int inquilino, int inmueble, DateTime fechalimite, DateTime fechacontrato, byte vigente) {
        this.ID = id;
        this.Locatario = inquilino;
        this.Propiedad = inmueble;
        this.FechaLímite = fechalimite;
        this.FechaContrato = fechacontrato;
        this.Vigente = vigente;
    }
    public Contrato () {

    }
}