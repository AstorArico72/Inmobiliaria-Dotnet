using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Principal.Models;
[Table ("Inmuebles")]
public class Inmueble {
    [Key]
    public int ID {get; set;}
    public string? Dirección {get; set;}
    public short Superficie {get; set;}
    [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "No está en alquiler", ApplyFormatInEditMode=true)]
    public int? Precio {get; set;}
    [ForeignKey("Propietario")]
    public int? IDPropietario {get; set;}
    public Propietario Dueño {get; set;}
    [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "Sin especificar", ApplyFormatInEditMode=true)]
    public string? Tipo {get; set;}
    public string? Uso {get; set;}
    public byte Ambientes {get; set;}
    public byte Disponible {get; set;}
    public float CoordenadasX {get; set;}
    public float CoordenadasY {get; set;}
    public Inmueble (int id, string? direccion, short area, int? precio, int? numDueño, Propietario dueño, string? tipo, string? uso, byte ambientes, byte disponible, float x, float y) {
        this.ID = id;
        this.Dirección = direccion;
        this.Superficie = area;
        this.Precio = precio;
        this.IDPropietario = numDueño;
        this.Dueño = dueño;
        this.Tipo = tipo;
        this.Uso = uso;
        this.Ambientes = ambientes;
        this.Disponible = disponible;
        this.CoordenadasX = x;
        this.CoordenadasY = y;
    }

    //Sin parámetros
    public Inmueble () {
    }
}