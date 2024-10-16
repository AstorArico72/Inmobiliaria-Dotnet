using Microsoft.EntityFrameworkCore;

namespace Principal.Models;
public class ContextoDb : DbContext {
    public ContextoDb (DbContextOptions<ContextoDb> options) : base (options) {
        // Constructor vacío.
    }

    public DbSet<Propietario> Propietarios {get; set;}
    public DbSet<Inmueble> Inmuebles {get; set;}
    public DbSet<Contrato> Contratos {get; set;}
    public DbSet<Inquilino> Inquilinos {get; set;}
    public DbSet<Pago> Pagos {get; set;}
}