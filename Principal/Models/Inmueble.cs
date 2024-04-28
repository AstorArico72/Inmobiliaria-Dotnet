using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TP1_ASP.Models {
[Table ("Inmuebles")]
public class Inmueble {
        public int ID {get; set;}
        public string? Dirección {get; set;}
        public short Superficie {get; set;}
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "No está en alquiler", ApplyFormatInEditMode=true)]
        public int? Precio {get; set;}
        public int? IDPropietario {get; set;}
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "Dueño no especificado", ApplyFormatInEditMode=true)]
        public Propietario? Dueño {get; set;}
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "Sin especificar", ApplyFormatInEditMode=true)]
        public string? Tipo {get; set;}
        public string? Uso {get; set;}
        public byte Ambientes {get; set;}
        public byte Disponible {get; set;}
        public Inmueble (int id, string? direccion, short area, int? precio, int? numDueño, Propietario? dueño, string? tipo, string? uso, byte ambientes, byte disponible) {
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
        }
    
        //Sin parámetros
        public Inmueble () {
        }
    }
}