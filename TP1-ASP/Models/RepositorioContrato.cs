namespace TP1_ASP.Models;
using MySql.Data.MySqlClient;

public class RepositorioContrato : IRepo <Contrato> {
    protected readonly string ConnectionString;

    //Los constructores pueden ir sin parámetros.
    public RepositorioContrato () {
        //String para la conexión MySQL.
        this.ConnectionString = "Server=127.0.0.1; Database=Inmobiliaria_NET; Uid=root; Pwd=;";
    }

    public List<Contrato> ObtenerTodos () {
        var resultado = new List<Contrato> ();
        string SQLQuery = @"SELECT * FROM Contratos";
        using (var con = new MySqlConnection (ConnectionString)) {
            using (var com = new MySqlCommand (SQLQuery, con)) {
                con.Open ();
                var lector = com.ExecuteReader ();
                while (lector.Read ()) {
                    var NuevoItem = new Contrato (
                        lector.GetInt32 ("ID"),
                        lector.GetInt32 ("Locador"),
                        lector.GetInt32 ("Locatario"),
                        lector.GetInt32 ("Propiedad"),
                        lector.GetDateTime ("FechaLímite"),
                        lector.GetDateTime ("FechaContrato")
                    );
                    resultado.Add (NuevoItem);
                }
                con.Close ();
            }
        }
        return resultado;
    }

    public int Nuevo (Contrato co) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"INSERT INTO Contratos (Locador, Locatario, Propiedad, FechaLímite) VALUES (@Locador, @Locatario, @Propiedad, @FechaLímite); SELECT LAST_INSERT_ID ()";
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Locador", co.Locador);
                    comm.Parameters.AddWithValue ("@Locatario", co.Locatario);
                    comm.Parameters.AddWithValue ("@Propiedad", co.Propiedad);
                    comm.Parameters.AddWithValue ("@FechaLímite", co.FechaLímite);
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

    public int Editar (int id, Contrato co) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"UPDATE Contratos SET Locador = @Locador, Locatario = @Locatario, Propiedad = @Propiedad, FechaLímite = @FechaLímite WHERE ID = " + id;
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Locador", co.Locador);
                    comm.Parameters.AddWithValue ("@Locatario", co.Locatario);
                    comm.Parameters.AddWithValue ("@Propiedad", co.Propiedad);
                    comm.Parameters.AddWithValue ("@FechaLímite", co.FechaLímite);
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

    public int Borrar (int id, Contrato co) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"DELETE FROM Contratos WHERE ID = " + id;
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

    public Contrato BuscarPorID (int id) {
        var resultado = new Contrato ();
        string SQLQuery = @"SELECT * FROM Contratos WHERE ID = " + id;
        using (var con = new MySqlConnection (ConnectionString)) {
            using (var com = new MySqlCommand (SQLQuery, con)) {
                con.Open ();
                var lector = com.ExecuteReader ();
                while (lector.Read ()) {
                        resultado.ID = lector.GetInt32 ("ID");
                        resultado.Locador = lector.GetInt32 ("Locador");
                        resultado.Locatario = lector.GetInt32 ("Locatario");
                        resultado.Propiedad = lector.GetInt32 ("Propiedad");
                        resultado.FechaLímite = lector.GetDateTime ("FechaLímite");
                    }
                }
            con.Close ();
            return resultado;
        }
    }
}