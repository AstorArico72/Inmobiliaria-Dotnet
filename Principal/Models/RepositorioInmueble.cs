namespace Principal.Models;
using MySql.Data.MySqlClient;

[Obsolete("Repositorios ADO.NET deprecados en función de la migración a Entity Framework. Usa la API en su lugar.")]
public class RepositorioInmueble : IRepo <Inmueble> {
    protected readonly string ConnectionString;

    //Los constructores pueden ir sin parámetros.
    public RepositorioInmueble () {
        //String para la conexión MySQL.
        this.ConnectionString = "Server=127.0.0.1; Database=Inmobiliaria_NET; Uid=root; Pwd=;";
    }

    [Obsolete("Reemplazado por \"TodosLosInmuebles ()\" en \"API/InmueblesController.cs\"")]
    public List<Inmueble> ObtenerTodos () {
        var resultado = new List<Inmueble> ();
        string SQLQuery = @"SELECT DISTINCT i.ID, i.Direccion, i.Superficie, i.Precio, i.Propietario, p.Nombre, p.ID, i.Tipo, i.Uso, p.Contacto, i.Ambientes, i.Disponible, p.DNI, i.CoordenadasX, i.CoordenadasY FROM Inmuebles i " +
        "LEFT JOIN Propietarios p ON p.ID = i.Propietario";

        using (var con = new MySqlConnection (ConnectionString)) {
            using (var com = new MySqlCommand (SQLQuery, con)) {
                con.Open ();
                var lector = com.ExecuteReader ();
                while (lector.Read ()) {
                    var NuevoItem = new Inmueble ();
                    NuevoItem.ID = lector.GetInt32 (0);
                    NuevoItem.Tipo = lector.GetString (7);
                    NuevoItem.Uso = lector.GetString (8);
                    NuevoItem.Ambientes = lector.GetByte (10);
                    NuevoItem.Disponible = lector.GetByte (11);
                    NuevoItem.CoordenadasX = lector.GetFloat (13);
                    NuevoItem.CoordenadasY = lector.GetFloat (14);
                    if (lector.IsDBNull (3)) {
                        NuevoItem.Precio = null;
                        NuevoItem.Propietario = lector.GetInt32 (4);
                    }
                    NuevoItem.Direccion = lector.GetString (1);
                    NuevoItem.Superficie = lector.GetInt16 (2);
                    resultado.Add (NuevoItem);
                }
                con.Close ();
            }
            return resultado;
        }
    }

    [Obsolete("Reemplazado por \"NuevoInmueble (Inmueble)\" en \"API/InmueblesController.cs\"")]
    public int Nuevo (Inmueble im) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"INSERT INTO Inmuebles (Direccion, Superficie, Precio, Propietario, Tipo, Uso, Ambientes, Disponible, CoordenadasX, CoordenadasY) VALUES (@Direccion, @Superficie, @Precio, @Propietario, @Tipo, @Uso, @Ambientes, 1, @X, @Y); SELECT LAST_INSERT_ID ()";
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Direccion", im.Direccion);
                    comm.Parameters.AddWithValue ("@Superficie", im.Superficie);
                    comm.Parameters.AddWithValue ("@Precio", im.Precio);
                    comm.Parameters.AddWithValue ("@Propietario", im.Propietario);
                    comm.Parameters.AddWithValue ("@Tipo", im.Tipo);
                    comm.Parameters.AddWithValue ("@Uso", im.Uso);
                    comm.Parameters.AddWithValue ("@Ambientes", im.Ambientes);
                    comm.Parameters.AddWithValue ("@X", im.CoordenadasX);
                    comm.Parameters.AddWithValue ("@Y", im.CoordenadasY);
                    con.Open ();
                    resultado = Convert.ToInt32 (comm.ExecuteScalar ());
                    con.Close ();
                }
            }
            if (im.CoordenadasX < -180 || im.CoordenadasX > 180 || im.CoordenadasY < -90 || im.CoordenadasY > 90) {
                resultado = -1;
                return resultado;
            }
        } catch (MySqlException ex) {
            Console.WriteLine (ex);
        }
        return resultado;
    }

    [Obsolete("Reemplazado por \"EditarInmueble (Inmueble)\" en \"API/InmueblesController.cs\"")]
    public int Editar (int id, Inmueble im) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"UPDATE Inmuebles SET Direccion = @Direccion, Superficie = @Superficie, Precio = @Precio, Propietario = @Propietario, Tipo = @Tipo, Uso = @Uso, Ambientes = @Ambientes, Disponible = @Disponible, CoordenadasX = @X, CoordenadasY = @Y WHERE ID = " + id;
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Direccion", im.Direccion);
                    comm.Parameters.AddWithValue ("@Superficie", im.Superficie);
                    comm.Parameters.AddWithValue ("@Precio", im.Precio);
                    comm.Parameters.AddWithValue ("@Propietario", im.Propietario);
                    comm.Parameters.AddWithValue ("@Tipo", im.Tipo);
                    comm.Parameters.AddWithValue ("@Uso", im.Uso);
                    comm.Parameters.AddWithValue ("@Ambientes", im.Ambientes);
                    comm.Parameters.AddWithValue ("@Disponible", im.Disponible);
                    comm.Parameters.AddWithValue ("@X", im.CoordenadasX);
                    comm.Parameters.AddWithValue ("@Y", im.CoordenadasY);
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

    [Obsolete("Reemplazado por \"BorrarInmueble (int)\" en \"API/InmueblesController.cs\"")]
    public int Borrar (int id, Inmueble im) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"DELETE FROM Inmuebles WHERE ID = " + id;
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

    [Obsolete("Reemplazado por \"ConseguirInmueble (int)\" en \"API/InmueblesController.cs\"")]
    public Inmueble? BuscarPorID (int id) {
        var NuevoItem = new Inmueble ();
        string SQLQuery = @"SELECT ID, Direccion, Superficie, Precio, Propietario, Tipo, Uso, Ambientes, Disponible, CoordenadasX, CoordenadasY FROM Inmuebles WHERE ID = " + id;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    con.Open ();
                var lector = comm.ExecuteReader ();
                while (lector.Read ()) {
                    NuevoItem.ID = lector.GetInt32 (0);
                    NuevoItem.Tipo = lector.GetString (5);
                    NuevoItem.Uso = lector.GetString (6);
                    NuevoItem.Ambientes = lector.GetByte (7);
                    NuevoItem.Disponible = lector.GetByte (8);
                    NuevoItem.CoordenadasX = lector.GetFloat (9);
                    NuevoItem.CoordenadasY = lector.GetFloat (10);
                    NuevoItem.Propietario = lector.GetInt32 (4);
                    if (lector.IsDBNull (3)) {
                        NuevoItem.Precio = null;
                    } else {
                        NuevoItem.Precio = lector.GetInt32 (3);
                    }
                    NuevoItem.Direccion = lector.GetString (1);
                    NuevoItem.Superficie = lector.GetInt16 (2);
                }
                if (!lector.HasRows) {
                    con.Close ();
                    return null;
                }
                con.Close ();
                }
            }
        } catch (MySqlException ex) {
            Console.WriteLine (ex);
        }
        return NuevoItem;
    }

    [Obsolete("Deprecado en función de la migración a Entity Framework.")]
    public List <Inmueble> BuscarPorFecha (DateTime FechaInicio, DateTime FechaFin) {
        RepositorioContrato repo = new RepositorioContrato ();
        List <Inmueble> Resultado = new List<Inmueble> ();

        List <Inmueble> Todos = ObtenerTodos ().Where (item => item.Disponible == 1).ToList ();

        foreach (var inmueble in Todos) {
            int id = inmueble.ID;
            bool ocupada = repo.PropiedadOcupada (id, FechaInicio, FechaFin);

            if (!ocupada) {
                Resultado.Add (inmueble);
            }
        }
        return Resultado;
    }
}