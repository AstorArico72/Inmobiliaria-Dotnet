using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Models;

public class Pago {
    [Key]
    public int ID {get; set;}
    public int NumeroPago {get; set;}
    [ForeignKey("IdContrato")]
    public int IdContrato {get; set;}
    public int Monto {get; set;}
    public DateTime FechaPago {get; set;}
    public byte Pagado {get; set;}

    public Pago (int id, int numpago, int contrato, int monto, DateTime fecha, byte pagado) {
        this.ID = id;
        this.NumeroPago = numpago;
        this.IdContrato = contrato;
        this.Monto = monto;
        this.FechaPago = fecha;
        this.Pagado = pagado;
    }
    public Pago () {
        //Nada aquí.
    }
}