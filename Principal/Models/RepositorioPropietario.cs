namespace TP1_ASP.Models;
using MySql.Data.MySqlClient;

public class RepositorioPropietario : IRepo <Propietario> {
    protected readonly string ConnectionString;

    //Los constructores pueden ir sin parámetros.
    public RepositorioPropietario () {
        //String para la conexión MySQL.
        this.ConnectionString = "Server=127.0.0.1; Database=Inmobiliaria_NET; Uid=root; Pwd=;";
    }

    public List<Propietario> ObtenerTodos () {
        var resultado = new List<Propietario> ();
        string SQLQuery = @"SELECT * FROM Propietarios";
        using (var con = new MySqlConnection (ConnectionString)) {
            using (var com = new MySqlCommand (SQLQuery, con)) {
                con.Open ();
                var lector = com.ExecuteReader ();
                while (lector.Read ()) {
                    var NuevoItem = new Propietario (
                        lector.GetString ("Nombre"),
                        lector.GetInt32 ("ID")
                    );

                    resultado.Add (NuevoItem);
                }
            }
        }
        return resultado;
    }

    public int Nuevo (Propietario pr) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"INSERT INTO Propietarios (Nombre) VALUES (@Nombre); SELECT LAST_INSERT_ID ()";
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Nombre", pr.Nombre);
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

    public int Editar (int id, Propietario pr) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"UPDATE Propietarios SET Nombre = @Nombre WHERE ID = " + id;
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Nombre", pr.Nombre);
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

    public int Borrar (int id, Propietario pr) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"DELETE FROM Propietarios WHERE ID = " + id;
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

    public Propietario? BuscarPorID (int id) {
        var resultado = new Propietario ();
        string SQLQuery = @"SELECT * FROM Propietarios WHERE ID = " + id;
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
        }
        return resultado;
    }
}