namespace TP1_ASP.Models;
using MySql.Data.MySqlClient;

public class RepositorioPago : IRepo <Pago> {
    protected readonly string ConnectionString;

    //Los constructores pueden ir sin parámetros.
    public RepositorioPago () {
        //String para la conexión MySQL.
        this.ConnectionString = "Server=127.0.0.1; Database=Inmobiliaria_NET; Uid=root; Pwd=;";
    }

    public List<Pago> ObtenerTodos () {
        var resultado = new List<Pago> ();
        string SQLQuery = @"SELECT * FROM Pagos";

        using (var con = new MySqlConnection (ConnectionString)) {
            using (var com = new MySqlCommand (SQLQuery, con)) {
                con.Open ();
                var lector = com.ExecuteReader ();
                while (lector.Read ()) {
                    var NuevoItem = new Pago ();
                    NuevoItem.ID = lector.GetInt32 (0);
                    NuevoItem.NumeroPago = lector.GetInt32 (1);
                    NuevoItem.IdContrato = lector.GetInt32 (2);
                    NuevoItem.Monto = lector.GetInt32 (3);
                    resultado.Add (NuevoItem);
                }
                con.Close ();
            }
            return resultado;
        }
    }

    public int Nuevo (Pago pa) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"INSERT INTO Pagos (NumeroPago, IdContrato, Monto) VALUES (@NumPago, @NumContrato, @Importe); SELECT LAST_INSERT_ID ()";
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@NumPago", pa.NumeroPago);
                    comm.Parameters.AddWithValue ("@NumContrato", pa.IdContrato);
                    comm.Parameters.AddWithValue ("@Importe", pa.Monto);
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

    public int Editar (int id, Pago pa) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"UPDATE Pagos SET NumeroPago = @NumPago, IdContrato = @Contrato, Monto = @Importe WHERE ID = " + id;
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@NumPago", pa.NumeroPago);
                    comm.Parameters.AddWithValue ("@Contrato", pa.IdContrato);
                    comm.Parameters.AddWithValue ("@Importe", pa.Monto);
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

    public int Borrar (int id, Pago pa) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"DELETE FROM Pagos WHERE ID = " + id;
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

    public Pago BuscarPorID (int id) {
        var NuevoItem = new Pago ();
        string SQLQuery = @"SELECT ID, NumeroPago, IdContrato, Monto FROM Pagos WHERE ID = " + id;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    con.Open ();
                var lector = comm.ExecuteReader ();
                while (lector.Read ()) {
                    NuevoItem.ID = lector.GetInt32 (0);
                    NuevoItem.NumeroPago = lector.GetInt32 (1);
                    NuevoItem.IdContrato = lector.GetInt32 (2);
                    NuevoItem.Monto = lector.GetInt32 (3);
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