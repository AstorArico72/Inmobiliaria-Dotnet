namespace TP1_ASP.Models;

public class ConjuntoResultados {
    public List <Contrato>? Contratos {get; set;}
    public List <Inmueble>? Inmuebles {get; set;}
    public List <Inquilino>? Inquilinos {get; set;}
    public List <Propietario>? Propietarios {get; set;}
    public List <Pago>? Pagos {get; set;}
    public Contrato? Contrato {get; set;}
    public Inmueble? Inmueble {get; set;}
    public Inquilino? Inquilino {get; set;}
    public Propietario? Propietario {get; set;}
}