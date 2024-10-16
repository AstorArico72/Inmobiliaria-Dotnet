namespace Principal.Models;

[Obsolete("Repositorios ADO.NET deprecados en función de la migración a Entity Framework. Usa la API en su lugar.")]
public interface IRepo <T> {
    List <T> ObtenerTodos ();
    int Nuevo (T parametro);
    int Editar (int id, T parametro);
    int Borrar (int id, T parametro);
    T? BuscarPorID (int id);
}