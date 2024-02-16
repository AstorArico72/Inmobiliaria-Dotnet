namespace TP1_ASP.Models;
using MySql.Data.MySqlClient;

public class RepositorioInquilino : IRepo <Inquilino> {
    protected readonly string ConnectionString;

    //Los constructores pueden ir sin parámetros.
    public RepositorioInquilino () {
        //String para la conexión MySQL.
        this.ConnectionString = "Server=127.0.0.1; Database=Inmobiliaria_NET; Uid=root; Pwd=;";
    }

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
                        lector.GetInt32 ("ID")
                    );

                    resultado.Add (NuevoItem);
                }
            }
        }
        return resultado;
    }

    public int Nuevo (Inquilino iq) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"INSERT INTO Inquilinos (Nombre) VALUES (@Nombre); SELECT LAST_INSERT_ID ()";
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Nombre", iq.Nombre);
                    con.Open ();
                    resultado = Convert.ToInt32 (comm.ExecuteScalar ()); // <-- Aquí hay un error crítico que no sé cómo resolver.
                    con.Close ();
                }
            }
        } catch (MySqlException ex) {
            Console.WriteLine (ex);
        }
        return resultado;
    }

    public int Editar (int id, Inquilino iq) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"UPDATE Inquilinos SET Nombre = @Nombre WHERE ID = " + id;
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Nombre", iq.Nombre);
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