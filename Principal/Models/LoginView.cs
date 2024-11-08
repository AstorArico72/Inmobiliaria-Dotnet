using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace Principal.Models;

public class LoginView {
	[DataType(DataType.EmailAddress)]
	[Required (ErrorMessage = "Es necesario el nombre de usuario")]
	public string Correo {get; set;}

	[DataType(DataType.Password)]
	[Required (ErrorMessage = "Es necesaria la contraseña")]
	public string Clave {get; set;}
	public LoginView () {

	}
}