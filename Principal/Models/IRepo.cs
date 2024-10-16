namespace Principal.Models;

public interface IRepo <T> {
    List <T> ObtenerTodos ();
    int Nuevo (T parametro);
    int Editar (int id, T parametro);
    int Borrar (int id, T parametro);
    T? BuscarPorID (int id);
}