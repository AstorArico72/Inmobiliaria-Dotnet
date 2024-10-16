namespace Principal.Models;

using System.Data;
using MySql.Data.MySqlClient;

[Obsolete("Repositorios ADO.NET deprecados en función de la migración a Entity Framework. Usa la API en su lugar.")]
public class RepositorioInquilino : IRepo <Inquilino> {
    protected readonly string ConnectionString;

    //Los constructores pueden ir sin parámetros.
    public RepositorioInquilino () {
        //String para la conexión MySQL.
        this.ConnectionString = "Server=127.0.0.1; Database=Inmobiliaria_NET; Uid=root; Pwd=;";
    }

    [Obsolete("Reemplazado por \"ConseguirInquilino (int)\" en \"API/InquilinosController.cs\"")]
    public List<Inquilino> ObtenerTodos () {
        var resultado = new List<Inquilino> ();
        string SQLQuery = @"SELECT * FROM Inquilinos";
        using (var con = new MySqlConnection (ConnectionString)) {
            using (var com = new MySqlCommand (SQLQuery, con)) {
                con.Open ();
                var lector = com.ExecuteReader ();
                while (lector.Read ()) {
                    var NuevoItem = new Inquilino (
                        lector.GetString ("Nombre"),
                        lector.GetInt32 ("ID"),
                        lector.GetString ("DNI"),
                        lector.GetString ("Contacto")
                    );

                    resultado.Add (NuevoItem);
                }
            }
        }
        return resultado;
    }

    [Obsolete("Reemplazado por \"NuevoInquilino (int)\" en \"API/InquilinosController.cs\"")]
    public int Nuevo (Inquilino iq) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"INSERT INTO Inquilinos (Nombre, DNI, Contacto) VALUES (@Nombre, @DNI, @Contacto); SELECT LAST_INSERT_ID ()";
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Nombre", iq.Nombre);
                    comm.Parameters.AddWithValue ("@DNI", iq.DNI);
                    comm.Parameters.AddWithValue ("@Contacto", iq.Contacto);
                    con.Open ();
                    resultado = Convert.ToInt32 (comm.ExecuteScalar ());
                    con.Close ();
                }
            }
        } catch (MySqlException ex) {
            Console.WriteLine (ex);
        }
        return resultado;
    }

    [Obsolete("Reemplazado por \"EditarInquilino (int)\" en \"API/InquilinosController.cs\"")]
    public int Editar (int id, Inquilino iq) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"UPDATE Inquilinos SET Nombre = @Nombre, DNI = @DNI, Contacto = @Contacto WHERE ID = " + id;
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Nombre", iq.Nombre);
                    comm.Parameters.AddWithValue ("@DNI", iq.DNI);
                    comm.Parameters.AddWithValue ("@Contacto", iq.Contacto);
                    con.Open ();
                    resultado = Convert.ToInt32 (comm.ExecuteScalar ());
                    con.Close ();
                }
            }
        } catch (MySqlException ex) {
            Console.WriteLine (ex);
        }
        return resultado;
    }

    [Obsolete("Reemplazado por \"BorrarInquilino (int)\" en \"API/InquilinosController.cs\"")]
    public int Borrar (int id, Inquilino iq) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"DELETE FROM Inquilinos WHERE ID = " + id;
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

    [Obsolete("Reemplazado por \"ConseguirInquilino (int)\" en \"API/InquilinosController.cs\"")]
    public Inquilino? BuscarPorID (int id) {
        var resultado = new Inquilino ();
        string SQLQuery = @"SELECT * FROM Inquilinos WHERE ID = " + id;
        using (var con = new MySqlConnection (ConnectionString)) {
            using (var com = new MySqlCommand (SQLQuery, con)) {
                con.Open ();
                var lector = com.ExecuteReader ();
                while (lector.Read ()) {
                    resultado.Nombre = lector.GetString ("Nombre");
                    resultado.ID = lector.GetInt32 ("ID");
                    resultado.DNI = lector.GetString ("DNI");
                    resultado.Contacto = lector.GetString ("Contacto");
                }
                if (!lector.HasRows) {
                    con.Close ();
                    return null;
                }
            }
            con.Close ();
            return resultado;
        }
    }
}