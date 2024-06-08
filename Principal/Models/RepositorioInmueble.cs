namespace TP1_ASP.Models;
using MySql.Data.MySqlClient;

public class RepositorioInmueble : IRepo <Inmueble> {
    protected readonly string ConnectionString;

    //Los constructores pueden ir sin parámetros.
    public RepositorioInmueble () {
        //String para la conexión MySQL.
        this.ConnectionString = "Server=127.0.0.1; Database=Inmobiliaria_NET; Uid=root; Pwd=;";
    }

    public List<Inmueble> ObtenerTodos () {
        var resultado = new List<Inmueble> ();
        string SQLQuery = @"SELECT DISTINCT i.ID, i.Dirección, i.Superficie, i.Precio, i.Propietario, p.Nombre, p.ID, i.Tipo, i.Uso, p.Contacto, i.Ambientes, i.Disponible, p.DNI, i.CoordenadasX, i.CoordenadasY FROM Inmuebles i " +
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
                    if (lector.IsDBNull (3) || lector.IsDBNull (4)) {
                        if (lector.IsDBNull (3) && !lector.IsDBNull (4)) {
                            //Sin precio pero con propietario
                            NuevoItem.Precio = null;
                            NuevoItem.IDPropietario = lector.GetInt32 (4);
                            NuevoItem.Dueño = new Propietario (
                                lector.GetString (5),
                                lector.GetInt32 (4),
                                lector.GetString (9),
                                lector.GetString (12)
                            );
                        } else if (lector.IsDBNull (4) && !lector.IsDBNull (3)) {
                            //Con precio pero sin propietario (es decir, propiedad de la inmobiliaria)
                            NuevoItem.Precio = lector.GetInt32 (3);
                            NuevoItem.IDPropietario = null;
                            NuevoItem.Dueño = null;
                        } else if (lector.IsDBNull (4) && lector.IsDBNull (3)) {
                            //Los dos nulos
                            NuevoItem.IDPropietario = null;
                            NuevoItem.Dueño = null;
                            NuevoItem.Precio = null;
                        }
                    } else {
                        //Ninguno nulo
                        NuevoItem.Precio = lector.GetInt32 (3);
                        NuevoItem.IDPropietario = lector.GetInt32 (4);
                        NuevoItem.Dueño = new Propietario ();
                        NuevoItem.Dueño.ID = lector.GetInt32 (4);
                        NuevoItem.Dueño.Nombre = lector.GetString (5);
                    }
                    NuevoItem.Dirección = lector.GetString (1);
                    NuevoItem.Superficie = lector.GetInt16 (2);
                    resultado.Add (NuevoItem);
                }
                con.Close ();
            }
            return resultado;
        }
    }

    public int Nuevo (Inmueble im) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"INSERT INTO Inmuebles (Dirección, Superficie, Precio, Propietario, Tipo, Uso, Ambientes, Disponible) VALUES (@Dirección, @Superficie, @Precio, @Propietario, @Tipo, @Uso, @Ambientes, 1, @X, @Y); SELECT LAST_INSERT_ID ()";
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Dirección", im.Dirección);
                    comm.Parameters.AddWithValue ("@Superficie", im.Superficie);
                    comm.Parameters.AddWithValue ("@Precio", im.Precio);
                    comm.Parameters.AddWithValue ("@Propietario", im.IDPropietario);
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

    public int Editar (int id, Inmueble im) {
        int resultado = -1;
        try {
            using (var con = new MySqlConnection (ConnectionString)) {
                string SQLQuery = @"UPDATE Inmuebles SET Dirección = @Dirección, Superficie = @Superficie, Precio = @Precio, Propietario = @Propietario, Tipo = @Tipo, Uso = @Uso, Ambientes = @Ambientes, Disponible = @Disponible, CoordenadasX = @X, CoordenadasY = @Y WHERE ID = " + id;
                using (var comm = new MySqlCommand (SQLQuery, con)) {
                    comm.Parameters.AddWithValue ("@Dirección", im.Dirección);
                    comm.Parameters.AddWithValue ("@Superficie", im.Superficie);
                    comm.Parameters.AddWithValue ("@Precio", im.Precio);
                    comm.Parameters.AddWithValue ("@Propietario", im.IDPropietario);
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

    public Inmueble? BuscarPorID (int id) {
        var NuevoItem = new Inmueble ();
        string SQLQuery = @"SELECT ID, Dirección, Superficie, Precio, Propietario, Tipo, Uso, Ambientes, Disponible, CoordenadasX, CoordenadasY FROM Inmuebles WHERE ID = " + id;
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
                    if (lector.IsDBNull (3) || lector.IsDBNull (4)) {
                        if (lector.IsDBNull (3) && !lector.IsDBNull (4)) {
                            NuevoItem.Precio = null;
                            NuevoItem.IDPropietario = lector.GetInt32 (4);
                        } else if (lector.IsDBNull (4) && !lector.IsDBNull (3)) {
                            NuevoItem.Precio = lector.GetInt32 (3);
                            NuevoItem.IDPropietario = null;
                        } else if (lector.IsDBNull (4) && lector.IsDBNull (3)) {
                            NuevoItem.IDPropietario = null;
                            NuevoItem.Precio = null;
                        }
                    } else {
                        NuevoItem.Precio = lector.GetInt32 (3);
                        NuevoItem.IDPropietario = lector.GetInt32 (4);
                    }
                    NuevoItem.Dirección = lector.GetString (1);
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