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
                    var NuevoItem = new Contrato ();
                    NuevoItem.ID = lector.GetInt32 (0);
                    NuevoItem.Locatario = lector.GetInt32 (1);
                    NuevoItem.Propiedad = lector.GetInt32 (2);
                    NuevoItem.FechaLímite = lector.GetDateTime (3);
                    NuevoItem.FechaContrato = lector.GetDateTime (4);
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
                string SQLQuery = @"INSERT INTO Contratos (Locatario, Propiedad, FechaLímite) VALUES (@Locatario, @Propiedad, @FechaLímite, @FechaContrato); SELECT LAST_INSERT_ID ()";
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Locatario", co.Locatario);
                    comm.Parameters.AddWithValue ("@Propiedad", co.Propiedad);
                    comm.Parameters.AddWithValue ("@FechaLímite", co.FechaLímite);
                    comm.Parameters.AddWithValue ("@FechaContrato", co.FechaContrato);
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
                string SQLQuery = @"UPDATE Contratos SET Locatario = @Locatario, Propiedad = @Propiedad, FechaLímite = @FechaLímite WHERE ID = " + id;
                using (var comm = new MySqlCommand (SQLQuery, con)) {
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
                        resultado.ID = lector.GetInt32 (0);
                        resultado.Locatario = lector.GetInt32 (1);
                        resultado.Propiedad = lector.GetInt32 (2);
                        resultado.FechaLímite = lector.GetDateTime (3);
                        resultado.FechaContrato = lector.GetDateTime (4);
                    }
                }
            con.Close ();
            return resultado;
        }
    }
}