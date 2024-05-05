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
                    NuevoItem.Vigente = lector.GetByte (5);
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
                        if (resultado >= 0) {
                            GenerarPagos (co);
                        }
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
        int multa = 0;
        DateTime hoy = DateTime.Now;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"UPDATE Contratos SET Locatario = @Locatario, Propiedad = @Propiedad, FechaLímite = @FechaLímite, Vigente = @Vigente WHERE ID = " + id;
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Locatario", co.Locatario);
                    comm.Parameters.AddWithValue ("@Propiedad", co.Propiedad);
                    if (co.Vigente == 0) {
                        if (hoy > co.FechaContrato) {
                            comm.Parameters.AddWithValue ("@Vigente", co.Vigente);
                            comm.Parameters.AddWithValue ("@FechaLímite", hoy);

                            //Por dónde informo de la multa!?
                            multa = CalcularMulta (co);
                        } else {
                            resultado = -5;
                            return resultado;
                        }
                    } else {
                        comm.Parameters.AddWithValue ("@Vigente", co.Vigente);
                        comm.Parameters.AddWithValue ("@FechaLímite", co.FechaLímite);
                    }
                    if (co.FechaLímite.CompareTo (co.FechaContrato) <= 0) {
                        resultado = -3;
                        return resultado;
                    }
                    bool ocupado = PropiedadOcupada (co.Propiedad, co.FechaLímite);
                    byte NoDisponible = InmuebleNoDisponible (co.Propiedad);
                    if (NoDisponible == 0) {
                        resultado = -4;
                        return resultado;
                    } else if (ocupado) {
                        resultado = -2;
                        return resultado;
                    } else {
                        con.Open ();
                        if (multa > 0) {
                            comm.ExecuteNonQuery ();
                            resultado = multa;
                        } else {
                            comm.ExecuteNonQuery ();
                            resultado = 0;
                        }
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
                        resultado.Vigente = lector.GetByte (5);
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

    public bool PropiedadOcupada (int propiedad, DateTime fechaComienzo, DateTime fechaLimite) {
        string limite = fechaLimite.ToString ("yyyy-MM-dd");
        string comienzo = fechaComienzo.ToString ("yyyy-MM-dd");
        if (fechaComienzo > fechaLimite) {
            throw new ArgumentException ("Error: La fecha de inicio es posterior a la fecha de fin. La fecha de inicio debe ser al menos un día anterior a la fecha de fin.");
        } else if (fechaComienzo == fechaLimite) {
            throw new ArgumentException ("Error: La fecha de inicio es igual a la fecha de fin. La fecha de inicio debe ser al menos un día anterior a la fecha de fin.");
        }
        string SQLQuery = @"SELECT * FROM Contratos WHERE Propiedad = @Propiedad AND '" + limite + "' >= FechaContrato AND '" + comienzo + "' <= FechaLímite AND Vigente = 1";
        int? resultado = null;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    con.Open ();
                    comm.Parameters.AddWithValue ("@Propiedad", propiedad);
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
        string SQLQuery = @"SELECT id FROM Contratos WHERE Propiedad = @propiedad AND Vigente = 1 AND @fechaLimite BETWEEN FechaLímite AND FechaContrato";
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

    private void GenerarPagos (Contrato co) {
        string SQLQuery2 = @"SELECT Precio FROM Inmuebles WHERE ID = @id";
        int Precio = 0;
        int Meses = ((co.FechaLímite.Year - co.FechaContrato.Year) *12) + co.FechaLímite.Month - co.FechaContrato.Month;
        var con2 = new MySqlConnection (ConnectionString);
        var con3 = new MySqlConnection (ConnectionString);
        var con4 = new MySqlConnection (ConnectionString);
        using (var com2 = new MySqlCommand (SQLQuery2, con2)) {
            con2.Open ();
            com2.Parameters.AddWithValue ("@id", co.Propiedad);
            var lector = com2.ExecuteReader ();
            while (lector.Read ()) {
                Precio = lector.GetInt32 (0);
            }
            con2.Close ();
        }
        int UltimoId = -1;
        string SQLQuery4 = @"SELECT MAX(id) FROM Contratos";
        using (var com4 = new MySqlCommand (SQLQuery4, con4)) {
            con4.Open ();
            var lector = com4.ExecuteReader ();
            while (lector.Read ()) {
                UltimoId = lector.GetInt32 (0);
            }
            con4.Close ();
        }
        string SQLQuery3 = @"INSERT INTO Pagos (NumeroPago, IdContrato, Monto, FechaPago) VALUES (@NumPago, @NumContrato, @Importe, @Fecha); SELECT LAST_INSERT_ID ()";
        for (int i = 1; i <= Meses; i++) {
            using (var com3 = new MySqlCommand (SQLQuery3, con3)) {
                con3.Open ();
                com3.Parameters.AddWithValue ("@NumPago", i);
                com3.Parameters.AddWithValue ("@NumContrato", UltimoId);
                com3.Parameters.AddWithValue ("@Importe", Precio);
                com3.Parameters.AddWithValue ("@Fecha", co.FechaContrato.AddMonths (i -1));
                com3.ExecuteScalar ();
                con3.Close ();
            }
        }
    }

    private int CalcularMulta (Contrato co) {
        DateTime Hoy = DateTime.Now;
        int MesesContrato = ((co.FechaLímite.Year - co.FechaContrato.Year) *12) + co.FechaLímite.Month - co.FechaContrato.Month;
        int MesesMulta = ((co.FechaLímite.Year - Hoy.Year) *12) + co.FechaLímite.Month - Hoy.Month;
        int Precio = 0;

        var con = new MySqlConnection (ConnectionString);
        string SQLQuery = @"SELECT Precio FROM Inmuebles WHERE ID = @id";

        using (var com = new MySqlCommand (SQLQuery, con)) {
            con.Open ();
            com.Parameters.AddWithValue ("@id", co.Propiedad);
            var lector = com.ExecuteReader ();
            while (lector.Read ()) {
                Precio = lector.GetInt32 (0);
            }
            con.Close ();
        }

        if (MesesMulta >= MesesContrato/2) {
            return Precio * 2;
        } else if (MesesMulta <= 0) {
            return 0;
        } else {
            return Precio;
        }
    }
}