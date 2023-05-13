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

        public DbSet<Final.Models.Categoria> Categorias { get; set; } = default!;
        public DbSet<Final.Models.Cliente> Clientes { get; set; } = default!;
        public DbSet<Final.Models.Compra> Compras { get; set; } = default!;
        public DbSet<Final.Models.Localidad> Localidades { get; set; } = default!;
        public DbSet<Final.Models.Marca> Marcas { get; set; } = default!;
        public DbSet<Final.Models.Producto> Productos { get; set; } = default!;
        public DbSet<Final.Models.Proveedor> Proveedores { get; set; } = default!;
        public DbSet<Final.Models.Provincia> Provincias { get; set; } = default!;
        public DbSet<Final.Models.Usuario> Usuarios { get; set; } = default!;
        public DbSet<Final.Models.Venta> Ventas { get; set; } = default!;

    }

}
