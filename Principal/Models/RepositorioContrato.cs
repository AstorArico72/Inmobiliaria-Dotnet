namespace Principal.Models;
using MySql.Data.MySqlClient;

[Obsolete("Repositorios ADO.NET deprecados en función de la migración a Entity Framework. Usa la API en su lugar.")]
public class RepositorioContrato : IRepo <Contrato> {
    protected readonly string ConnectionString;

    //Los constructores pueden ir sin parámetros.
    public RepositorioContrato () {
        //String para la conexión MySQL.
        this.ConnectionString = "Server=127.0.0.1; Database=Inmobiliaria_NET; Uid=root; Pwd=;";
    }

    [Obsolete("Reemplazado por \"TodosLosContratos ()\" en \"API/ContratosController.cs\"")]
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

    [Obsolete("Repositorios ADO.NET deprecados en función de la migración a Entity Framework. Está pendiente hacer una función que reemplace ésta.")]
    public List<Contrato>? BuscarPorFecha (DateTime FechaInicio, DateTime FechaFin) {
        var resultado = new List<Contrato> ();
        string limite = FechaFin.ToString ("yyyy-MM-dd");
        string comienzo = FechaInicio.ToString ("yyyy-MM-dd");
        if (FechaInicio > FechaFin) {
            throw new ArgumentException ("Error: La fecha de inicio es posterior a la fecha de fin. La fecha de inicio debe ser al menos un día anterior a la fecha de fin.");
        } else if (FechaInicio == FechaFin) {
            throw new ArgumentException ("Error: La fecha de inicio es igual a la fecha de fin. La fecha de inicio debe ser al menos un día anterior a la fecha de fin.");
        }
        string SQLQuery = @"SELECT * FROM Contratos WHERE FechaContrato >= '" + comienzo + "' AND FechaLímite <= '" + limite + "' AND Vigente = 1";
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

    [Obsolete("Reemplazado por \"NuevoContrato (Contrato)\" en \"API/ContratosController.cs\"")]
    public int Nuevo (Contrato co) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"INSERT INTO Contratos (Locatario, Propiedad, FechaLímite, FechaContrato, Monto) VALUES (@Locatario, @Propiedad, @FechaLímite, @FechaContrato, @Monto); SELECT LAST_INSERT_ID ()";
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Locatario", co.Locatario);
                    comm.Parameters.AddWithValue ("@Propiedad", co.Propiedad);
                    comm.Parameters.AddWithValue ("@FechaLímite", co.FechaLímite);
                    comm.Parameters.AddWithValue ("@FechaContrato", co.FechaContrato);
                    comm.Parameters.AddWithValue ("@Monto", co.Monto);
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

    [Obsolete("Reemplazado por \"EditarContrato (Contrato)\" en\"API/ContratosController.cs\"")]
    public int Editar (int id, Contrato co) {
        RepositorioPago pagos = new RepositorioPago ();
        List <Pago> ListaPagos = pagos.ObtenerTodos ().Where (item => item.IdContrato == id).ToList ();
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
                    IEnumerable <Pago> PagosPendientes = ListaPagos.Where (pago => pago.Pagado == 2);
                    if (PagosPendientes.Any ()) {
                        resultado = -6;
                        return resultado;
                    }
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

    [Obsolete("Reemplazado por \"BorrarContrato (int)\" en \"API/ContratosController.cs\"")]
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

    [Obsolete("Reemplazado por \"ConseguirContrato (int)\" en \"API/ContratosController.cs\"")]
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
                    resultado.Monto = lector.GetInt32 (6);
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
        int Precio = co.Monto;
        int Meses = ((co.FechaLímite.Year - co.FechaContrato.Year) *12) + co.FechaLímite.Month - co.FechaContrato.Month;
        var con1 = new MySqlConnection (ConnectionString);
        var con2 = new MySqlConnection (ConnectionString);
        int UltimoId = -1;
        string SQLQuery = @"SELECT MAX(id) FROM Contratos";
        using (var com1 = new MySqlCommand (SQLQuery, con1)) {
            con1.Open ();
            var lector = com1.ExecuteReader ();
            while (lector.Read ()) {
                UltimoId = lector.GetInt32 (0);
            }
            con1.Close ();
        }
        string SQLQuery2 = @"INSERT INTO Pagos (NumeroPago, IdContrato, Monto, FechaPago) VALUES (@NumPago, @NumContrato, @Importe, @Fecha); SELECT LAST_INSERT_ID ()";
        for (int i = 1; i <= Meses; i++) {
            using (var com2 = new MySqlCommand (SQLQuery2, con2)) {
                con2.Open ();
                com2.Parameters.AddWithValue ("@NumPago", i);
                com2.Parameters.AddWithValue ("@NumContrato", UltimoId);
                com2.Parameters.AddWithValue ("@Importe", Precio);
                com2.Parameters.AddWithValue ("@Fecha", co.FechaContrato.AddMonths (i -1));
                com2.ExecuteScalar ();
                con2.Close ();
            }
        }
    }

    private int CalcularMulta (Contrato co) {
        DateTime Hoy = DateTime.Now;
        int MesesContrato = ((co.FechaLímite.Year - co.FechaContrato.Year) *12) + co.FechaLímite.Month - co.FechaContrato.Month;
        int MesesMulta = ((co.FechaLímite.Year - Hoy.Year) *12) + co.FechaLímite.Month - Hoy.Month;
        int Precio = co.Monto;

        if (MesesMulta >= MesesContrato/2) {
            return Precio * 2;
        } else if (MesesMulta <= 0) {
            return 0;
        } else {
            return Precio;
        }
    }
}