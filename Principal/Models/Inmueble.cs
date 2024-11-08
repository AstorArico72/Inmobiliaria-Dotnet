using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Principal.Models;
[Table ("Inmuebles")]
public class Inmueble {
    [Key]
    public int ID {get; set;}
    public string? Direccion {get; set;}
    public short Superficie {get; set;}
    [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "No está en alquiler", ApplyFormatInEditMode=true)]
    public int? Precio {get; set;}
    [ForeignKey("Propietario")]
    public int? Propietario {get; set;}
    [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "Sin especificar", ApplyFormatInEditMode=true)]
    public string? Tipo {get; set;}
    public string? Uso {get; set;}
    public byte Ambientes {get; set;}
    public byte Disponible {get; set;}
    public float CoordenadasX {get; set;}
    public float CoordenadasY {get; set;}
    [NotMapped]
    public IFormFile? Foto {get; set;}
    public string? UrlFoto {get; set;}
    public Inmueble (int id, string? direccion, short area, int? precio, int? numDueño, string? tipo, string? uso, byte ambientes, byte disponible, float x, float y, IFormFile? foto) {
        this.ID = id;
        this.Direccion = direccion;
        this.Superficie = area;
        this.Precio = precio;
        this.Propietario = numDueño;
        this.Tipo = tipo;
        this.Uso = uso;
        this.Ambientes = ambientes;
        this.Disponible = disponible;
        this.CoordenadasX = x;
        this.CoordenadasY = y;
        this.Foto = foto;
    }

    //Sin parámetros
    public Inmueble () {
    }
}