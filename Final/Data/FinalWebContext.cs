using Microsoft.EntityFrameworkCore;
using Final.Models;

namespace Final.Data
{
    public class FinalWebContext : DbContext
    {
        public FinalWebContext
            (DbContextOptions<FinalWebContext> options) : base(options)
        {

        }
        public DbSet<FinalWebContext.Models.Proveedor> Proveedores { get; set; } = default!;
        public DbSet<FinalWebContext.Models.Cliente> Clientes { get; set; } = default!;
        public DbSet<FinalWebContext.Models.Marca> Marcas { get; set; } = default!;
        public DbSet<FinalWebContext.Models.Categoria> Categorias { get; set; } = default!;
        public DbSet<FinalWebContext.Models.Producto> Productos { get; set; } = default!;
        public DbSet<FinalWebContext.Models.Usuario> Usuarios { get; set; } = default!;
        public DbSet<FinalWebContext.Models.Compra> Compras { get; set; } = default!;
        public DbSet<FinalWebContext.Models.Venta> Ventas { get; set; } = default!;

    }

}
