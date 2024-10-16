using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Models;

public class Contrato {
    [Key]
    public int ID {get; set;}
    [ForeignKey("Locatario")]
    public int Locatario {get; set;}
    [ForeignKey("Propiedad")]
    public int Propiedad {get; set;}
    public DateTime FechaLímite {get; set;}
    public DateTime FechaContrato {get; set;}
    public byte Vigente {get; set;}
    public int Monto {get; set;}

    public Contrato (int id, int inquilino, int inmueble, DateTime fechalimite, DateTime fechacontrato, byte vigente, int monto) {
        this.ID = id;
        this.Locatario = inquilino;
        this.Propiedad = inmueble;
        this.FechaLímite = fechalimite;
        this.FechaContrato = fechacontrato;
        this.Vigente = vigente;
        this.Monto = monto;
    }
    public Contrato () {

    }
}