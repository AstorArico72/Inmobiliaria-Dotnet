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
                string SQLQuery = @"INSERT INTO Contratos (Locatario, Propiedad, FechaLímite, FechaContrato) VALUES (@Locatario, @Propiedad, @FechaLímite, @FechaContrato); SELECT LAST_INSERT_ID ()";
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Locatario", co.Locatario);
                    comm.Parameters.AddWithValue ("@Propiedad", co.Propiedad);
                    comm.Parameters.AddWithValue ("@FechaLímite", co.FechaLímite);
                    comm.Parameters.AddWithValue ("@FechaContrato", co.FechaContrato);
                    if (co.FechaLímite.CompareTo (co.FechaContrato) <= 0) {
                        resultado = -3;
                        return resultado;
                    }
                    bool EstaOcupado = PropiedadOcupada (co.Propiedad, co.FechaContrato, co.FechaLímite);
                    byte NoDisponible = InmuebleNoDisponible (co.Propiedad);
                    if (NoDisponible == 0) {
                        resultado = -4;
                        return resultado;
                    } else if (EstaOcupado) {
                        resultado = -2;
                        return resultado;
                    } else {
                        con.Open ();
                        resultado = Convert.ToInt32 (comm.ExecuteScalar ());
                        con.Close ();
                    }
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
                    if (co.FechaLímite.CompareTo (co.FechaContrato) <= 0) {
                        resultado = -3;
                        return resultado;
                    }
                    bool ocupado = PropiedadOcupada (co.Propiedad, co.FechaLímite);
                    byte NoDisponible = InmuebleNoDisponible (co.Propiedad);
                    if (NoDisponible == 0) {
                        resultado = -4;
                    } else if (ocupado) {
                        resultado = -2;
                    } else {
                        con.Open ();
                        resultado = Convert.ToInt32 (comm.ExecuteNonQuery ());
                        con.Close ();
                    }
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

    public Contrato? BuscarPorID (int id) {
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
                if (!lector.HasRows) {
                    con.Close ();
                    return null;
                }
            }
            con.Close ();
            return resultado;
        }
    }

    private bool PropiedadOcupada (int propiedad, DateTime fechaComienzo, DateTime fechaLimite) {
        string limite = fechaLimite.ToString ("yyyy-MM-dd");
        string comienzo = fechaComienzo.ToString ("yyyy-MM-dd");
        string SQLQuery = @"SELECT * FROM Contratos WHERE Propiedad = @Propiedad AND '" + limite + "' >= FechaContrato AND '" + comienzo + "' <= FechaLímite";
        int? resultado = null;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    con.Open ();
                    comm.Parameters.AddWithValue ("@Propiedad", propiedad);
                    //comm.Parameters.AddWithValue ("@FechaLimite", fechaLimite.ToString ("yyyy-MM-dd"));
                    //comm.Parameters.AddWithValue ("@FechaComienzo", fechaComienzo.ToString ("yyyy-MM-dd"));
                    var lector = comm.ExecuteReader ();
                    while (lector.Read ()) {
                        resultado = lector.GetInt32 ("ID");
                    }
                    con.Close ();
                }
            }
            if (resultado != null) {
                return true;
            } else {
                return false;
            }
        } catch (MySqlException ex) {
            Console.WriteLine (ex);
            return true;
        }
    }


    //Sobrecarga del anterior para cuando se edita un contrato.

    private bool PropiedadOcupada (int propiedad, DateTime fechaLimite) {
        bool ocupada;
        string SQLQuery = @"SELECT id FROM Contratos WHERE Propiedad = @propiedad AND @fechaLimite BETWEEN FechaLímite AND FechaContrato";
        int? resultado = null;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    con.Open ();
                    comm.Parameters.AddWithValue ("@propiedad", propiedad);
                    comm.Parameters.AddWithValue ("@fechaLimite", fechaLimite);
                    var lector = comm.ExecuteReader ();
                    while (lector.Read ()) {
                        resultado = lector.GetInt32 (0);
                    }
                    con.Close ();
                }
            }
            if (resultado != null) {
                ocupada = true;
            } else {
                ocupada = false;
            }
            return ocupada;
        } catch (MySqlException ex) {
            Console.WriteLine (ex);
            return true;
        }
    }

    private byte InmuebleNoDisponible (int propiedad) {
        byte disponible = 0;
        string SQLQuery = @"SELECT Disponible FROM Inmuebles WHERE ID = @propiedad";
        using (var con = new MySqlConnection (ConnectionString)) {
            using (var comm = new MySqlCommand (SQLQuery, con)) {
                con.Open ();
                comm.Parameters.AddWithValue ("@propiedad", propiedad);
                var lector = comm.ExecuteReader ();
                while (lector.Read ()) {
                    disponible = lector.GetByte (0);
                }
                con.Close ();
                return disponible;
            }
        }
    }
}