namespace TP1_ASP.Models;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MySql.Data.MySqlClient;

public class RepositorioUsuario : IRepo <Usuario> {
    protected readonly string ConnectionString;

    //Los constructores pueden ir sin parámetros.
    public RepositorioUsuario () {
        //String para la conexión MySQL.
        this.ConnectionString = "Server=127.0.0.1; Database=Inmobiliaria_NET; Uid=root; Pwd=;";
    }

    public List<Usuario> ObtenerTodos () {
        var resultado = new List<Usuario> ();
        string SQLQuery = @"SELECT * FROM Usuarios";

        using (var con = new MySqlConnection (ConnectionString)) {
            using (var com = new MySqlCommand (SQLQuery, con)) {
                con.Open ();
                var lector = com.ExecuteReader ();
                while (lector.Read ()) {
                    var NuevoItem = new Usuario ();
                    NuevoItem.ID = lector.GetInt32 ("ID");
                    NuevoItem.Clave = lector.GetString ("Clave");
                    NuevoItem.NombreUsuario = lector.GetString ("Nombre");
                    NuevoItem.Rol = lector.GetString ("Rol");
                    if (lector.IsDBNull (4)) {
                        NuevoItem.URLFoto = "/medios/Nulo.png";
                    } else {
                        NuevoItem.URLFoto = lector.GetString ("UrlImagen");
                    }
                    resultado.Add (NuevoItem);
                }
                con.Close ();
            }
            return resultado;
        }
    }

    public int Nuevo (Usuario u) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"INSERT INTO Usuarios (Nombre, Clave, Rol) VALUES (@Nombre, @Clave, @Rol); SELECT LAST_INSERT_ID ()";
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Nombre", u.NombreUsuario);
                    u.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
			            password: u.Clave,
			            salt: System.Text.Encoding.ASCII.GetBytes("Salty-as-the-ocean"),
			            prf: KeyDerivationPrf.HMACSHA256,
			            iterationCount: 1000,
			            numBytesRequested: 256 / 8
                    ));
                    comm.Parameters.AddWithValue ("@Clave", u.Clave);
                    comm.Parameters.AddWithValue ("@Rol", u.Rol);
                    con.Open ();
                    resultado = Convert.ToInt32 (comm.ExecuteScalar ());
                    con.Close ();
                }
            }
        } catch (MySqlException ex) {
            throw ex;
        }
        return resultado;
    }

    public int CambiarClave (int id, Usuario u) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"UPDATE Usuarios SET Clave = @Clave WHERE ID = " + id;
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    u.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
			            password: u.Clave,
			            salt: System.Text.Encoding.ASCII.GetBytes("Salty-as-the-ocean"),
			            prf: KeyDerivationPrf.HMACSHA256,
			            iterationCount: 1000,
			            numBytesRequested: 256 / 8
                    ));
                    comm.Parameters.AddWithValue ("@Clave", u.Clave);
                    con.Open ();

                    resultado = Convert.ToInt32 (comm.ExecuteNonQuery ());
                    con.Close ();
                }
            }
        } catch (MySqlException ex) {
            throw ex;
        }
        return resultado;
    }

    public int Editar (int id, Usuario u) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"UPDATE Usuarios SET Nombre = @Nombre, Rol = @Rol, UrlImagen = @UrlImagen WHERE ID = " + id;
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Nombre", u.NombreUsuario);
                    comm.Parameters.AddWithValue ("@Rol", u.Rol);
                    comm.Parameters.AddWithValue ("@UrlImagen", u.URLFoto);
                    con.Open ();

                    resultado = Convert.ToInt32 (comm.ExecuteNonQuery ());
                    con.Close ();
                }
            }
        } catch (MySqlException ex) {
            throw ex;
        }
        return resultado;
    }

    public int Borrar (int id, Usuario u) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"DELETE FROM Usuarios WHERE ID = " + id + " AND Rol = 'Empleado'";
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    con.Open ();
                    resultado = Convert.ToInt32 (comm.ExecuteNonQuery ());
                    con.Close ();
                }
            }
        } catch (MySqlException ex) {
            Console.WriteLine (ex);
        }
        return resultado;
    }

    public Usuario? BuscarPorID (int id) {
        var NuevoItem = new Usuario ();
        string SQLQuery = @"SELECT ID, Clave, Nombre, Rol, UrlImagen FROM Usuarios WHERE ID = " + id;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    con.Open ();
                    var lector = comm.ExecuteReader ();
                    while (lector.Read ()) {
                        NuevoItem.ID = lector.GetInt32 ("ID");
                        NuevoItem.Clave = lector.GetString ("Clave");
                        NuevoItem.NombreUsuario = lector.GetString ("Nombre");
                        NuevoItem.Rol = lector.GetString ("Rol");
                        if (lector.IsDBNull (4)) {
                            NuevoItem.URLFoto = "/medios/Nulo.png";
                        } else {
                            NuevoItem.URLFoto = lector.GetString ("UrlImagen");
                        }
                    }
                    if (!lector.HasRows) {
                        con.Close ();
                        return null;
                    }
                con.Close ();
                }
            }
        } catch (MySqlException ex) {
            Console.WriteLine (ex);
        }
        return NuevoItem;
    }

    public Usuario BuscarPorNombre (string nombre) {
        var NuevoItem = new Usuario ();
        string SQLQuery = @"SELECT ID, Nombre, Clave, Rol, UrlImagen FROM Usuarios WHERE Nombre = '" + nombre + "'";
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    con.Open ();
                    var lector = comm.ExecuteReader ();
                    while (lector.Read ()) {
                        NuevoItem.ID = lector.GetInt32 ("ID");
                        NuevoItem.Clave = lector.GetString ("Clave");
                        NuevoItem.NombreUsuario = lector.GetString ("Nombre");
                        NuevoItem.Rol = lector.GetString ("Rol");
                        if (lector.IsDBNull (4)) {
                            NuevoItem.URLFoto = "/medios/Nulo.png";
                        } else {
                            NuevoItem.URLFoto = lector.GetString ("UrlImagen");
                        }
                    }
                    con.Close ();
                }
            }
        } catch (MySqlException ex) {
            Console.WriteLine (ex);
        }
        return NuevoItem;
    }
}