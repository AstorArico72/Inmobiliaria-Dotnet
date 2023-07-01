using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TP1_ASP.Models {
[Table ("Inmuebles")]
public class Inmueble {
        public int ID {get; set;}
        public string? Dirección {get; set;}
        public short Superficie {get; set;}
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "No está en alquiler", ApplyFormatInEditMode=true)]
        public Nullable<Int32> Precio {get; set;}
        public Nullable<Int32> IDPropietario {get; set;}
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "Propiedad de la inmobiliaria", ApplyFormatInEditMode=true)]
        public Propietario? Dueño {get; set;}
        /* Inplementar otro día
        public Contrato ContratoAlquiler {get; set;}
        */
        public Inmueble (int id, string? direccion, short area, Nullable <Int32> precio, Nullable <Int32> numDueño, Propietario? dueño) {
            this.ID = id;
            this.Dirección = direccion;
            this.Superficie = area;
            this.Precio = precio;
            this.IDPropietario = numDueño;
            this.Dueño = dueño;
        }
    
        //Sin parámetros
        public Inmueble () {
        }
    }
}