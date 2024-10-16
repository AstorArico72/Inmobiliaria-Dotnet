namespace Principal.Models;
using MySql.Data.MySqlClient;

[Obsolete("Repositorios ADO.NET deprecados en función de la migración a Entity Framework. Usa la API en su lugar.")]
public class RepositorioPropietario : IRepo <Propietario> {
    protected readonly string ConnectionString;

    //Los constructores pueden ir sin parámetros.
    public RepositorioPropietario () {
        //String para la conexión MySQL.
        this.ConnectionString = "Server=127.0.0.1; Database=Inmobiliaria_NET; Uid=root; Pwd=;";
    }

    [Obsolete("Reemplazado por \"TodosLosPropietarios ()\" en \"API/PropietariosController.cs\"")]
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
                        lector.GetInt32 ("ID"),
                        lector.GetString ("Contacto"),
                        lector.GetString ("DNI")
                    );

                    resultado.Add (NuevoItem);
                }
            }
        }
        return resultado;
    }

    [Obsolete("Reemplazado por \"NuevoPropietario (Propietario)\" en \"API/PropietariosController.cs\"")]
    public int Nuevo (Propietario pr) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"INSERT INTO Propietarios (Nombre, Contacto, DNI) VALUES (@Nombre, @Contacto, @DNI); SELECT LAST_INSERT_ID ()";
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Nombre", pr.Nombre);
                    comm.Parameters.AddWithValue ("@Contacto", pr.Contacto);
                    comm.Parameters.AddWithValue ("@DNI", pr.DNI);
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

    [Obsolete("Reemplazado por \"EditarPropietario (Propietario)\" en \"API/PropietariosController.cs\"")]
    public int Editar (int id, Propietario pr) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"UPDATE Propietarios SET Nombre = @Nombre, Contacto = @Contacto, DNI = @DNI WHERE ID = " + id;
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Nombre", pr.Nombre);
                    comm.Parameters.AddWithValue ("@Contacto", pr.Contacto);
                    comm.Parameters.AddWithValue ("@DNI", pr.DNI);
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

    [Obsolete("Reemplazado por \"BorrarPropietario (int)\" en \"API/PropietariosController.cs\"")]
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

    [Obsolete("Reemplazado por \"ConseguirPropietario (int)\" en \"API/PropietariosController.cs\"")]
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
                    resultado.Contacto = lector.GetString ("Contacto");
                    resultado.DNI = lector.GetString ("DNI");
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