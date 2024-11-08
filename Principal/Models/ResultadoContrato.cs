namespace Principal.Models;

public class ResultadoContrato {
    public int IdContrato {get; set;}
    public int IdInquilino {get; set;}
    public int IdInmueble {get; set;}
    public DateTime FechaLímite {get; set;}
    public DateTime FechaContrato {get; set;}
    public byte Vigente {get; set;}
    public int Monto {get; set;}
    public string NombreInquilino {get; set;}
    public string DireccionInmueble {get; set;}
    //public List<Pago> ListaPagos {get; set;}

    public ResultadoContrato () {
        //Constructor vacío.
    }
}