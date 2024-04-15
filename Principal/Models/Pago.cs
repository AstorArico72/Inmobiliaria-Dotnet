namespace TP1_ASP.Models;

public class Pago {
    public int ID {get; set;}
    public int NumeroPago {get; set;}
    public int IdContrato {get; set;}
    public int Monto {get; set;}
    public DateTime FechaPago {get; set;}

    public Pago (int id, int numpago, int contrato, int monto, DateTime fecha) {
        this.ID = id;
        this.NumeroPago = numpago;
        this.IdContrato = contrato;
        this.Monto = monto;
        this.FechaPago = fecha;
    }
    public Pago () {
        //Nada aquí.
    }
}